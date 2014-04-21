using UnityEngine;
using System.Collections;
using dbConnect;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SimpleJSON;
//using System;


public class MainMenuVik : Photon.MonoBehaviour
{
	public string fileList;
	public GUISkin customSkin;
	public Texture2D background;

	//------------------------------------------------------------------
	// boolean variables for different menu scenes; toggle on and off 
	// for each scene
	//------------------------------------------------------------------
	public bool isLogin = true; //login page
	public bool isCreate = false; //create player page
	public bool isMessage = false; //error/confirm message page
	public bool isPlaybackList = false; //page where you choose from a list of playbacks
	public bool isMain = false; //main page
	public bool isTutorial = false; //choose tutorial stage
	//public bool isPlayback = false; //playback mode
	public bool isChoose = false; //choose character page
	public bool isTrainer = false; //player is trainer
	public bool isLobby = false; //lobby screen
	public bool gameIsReadyToLoad = false; //whether the GameEnd is starting
	public bool isSkipLobby = false; //whether to skip lobby
	public string sceneLinkage = "Fire_event_merged"; //scene to link to 
	//------------------------------------------------------------------

	public HashSet<string> selectedPlayerList = new HashSet<string>();
	public string[] playerList;
	public string[] dots = new string[] {".", "..", "..."};
	public int dotInt = 0;

	public string message = "";
	public string playerName = "";
	public string password = "";
	public string reEnter = "";
	public string roomName = "jkroom";
	private Vector2 scrollPos = Vector2.zero;
	public int totalPlayers = 4;


	//RUN AT START OF GAME
    void Awake()
    {
		PlayerPrefs.DeleteKey ("sessionID");
		PlayerPrefs.DeleteKey ("roomName");
		PlayerPrefs.DeleteKey ("isTutorial");
		PlayerPrefs.DeleteKey("isTrainer");
		PlayerPrefs.DeleteKey("isPlayback");
		PlayerPrefs.DeleteKey("filePath");

		PlayerPrefs.DeleteKey ("isMaster");

		gameIsReadyToLoad = false;
		//Debug.Log ("main yo");
        //PhotonNetwork.logLevel = NetworkLogLevel.Full;

		//-----------------------------------------------------------------------------------------------------
		//					Photon network initialisation
		//-----------------------------------------------------------------------------------------------------

        // Connect to the main photon server. This is the only IP and port we ever need to set(!)
        if (!PhotonNetwork.connected)
           PhotonNetwork.ConnectUsingSettings("v1.0"); // version of the game/demo. used to separate older clients from newer ones (e.g. if incompatible)

		//-----------------------------------------------------------------------------------------------------

        //Load name from PlayerPrefs
        PhotonNetwork.playerName = PlayerPrefs.GetString("playerName", "Guest" + Random.Range(1, 9999));

        //Set camera clipping for nicer "main menu" background
        Camera.main.farClipPlane = Camera.main.nearClipPlane + 0.1f;


		//Also, get the list of replays in the server

		//get list of directory
		WWWForm sendForm = new WWWForm();
		sendForm.AddField("load", "LIST");
		WWW w = new WWW("http://www.sgi-singapore.com/projects/ORILE/loadFiles.php", sendForm);

		StartCoroutine(WaitForList(w));
		//fileList = w.data;
		//Debug.Log(fileList);

		//if come back do not go to login menu
		if (PlayerPrefs.GetString("playerLoginName") != "")
		{
			//---------------------------------------
			//	 TOGGLE isLogin false / isMain true
			//--------------------------------------
			isMain = true;
			isLogin = false;
		}

    }

    void OnGUI()
    {
		//set to custom skin
		GUI.skin = customSkin;

		//--------------------------------------------------------------------------------------------------
		//				IF SHOWING CONNECTING SCREEN
		//--------------------------------------------------------------------------------------------------
        if (!PhotonNetwork.connected)
        {
			GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));
			
			GUILayout.Label("Connecting to Photon server.");
			GUILayout.Label("Hint: This demo uses a settings file and logs the server address to the console.");
			
			GUILayout.EndArea();
            return;   //Wait for a connection
        }


		//--------------------------------------------------------------------------------------------------
		//				IF SHOWING ERROR MESSAGE
		//--------------------------------------------------------------------------------------------------
		if (isMessage) {

			GUILayout.BeginArea (new Rect ((Screen.width - 400) / 2, (Screen.height - 300) / 2, 600, 300));
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),background);


			GUILayout.Box (message, GUILayout.Width (300), GUILayout.Height (200));
			//ok button
			if (GUILayout.Button ("OK", GUILayout.Width (80))) {
					
				//---------------------------------
				//	 TOGGLE isMessage false
				//---------------------------------
					isMessage = false;

			}

			GUILayout.EndArea ();


		//--------------------------------------------------------------------------------------------------
		//				IF LOGGING INTO THE GAME
		//--------------------------------------------------------------------------------------------------
		} else if (isLogin) {


			GUILayout.BeginArea (new Rect(0,0,Screen.width,Screen.height));
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),background);


			GUILayout.BeginArea (new Rect (Screen.width/4, Screen.height*.1f, Screen.width/2, Screen.height*.8f));

			//title
			GUILayout.Label ("ORILE","Title");
			GUILayout.Space (45);
			GUILayout.Label ("Login Menu");


			GUILayout.BeginArea (new Rect (0, Screen.height*.275f, Screen.width/2, Screen.height*.5f));

			//player name entry
			GUILayout.BeginHorizontal();
			GUILayout.Label ("Player Name:", "labelText");
			playerName = GUILayout.TextField (playerName, 25, GUILayout.Width (200));
			GUILayout.EndHorizontal();

    		//password entry
			GUILayout.BeginHorizontal();
			GUILayout.Label ("Password:", "labelText");
			password = GUILayout.PasswordField (password, "*" [0], 25, GUILayout.Width (200));
			GUILayout.EndHorizontal();


			GUILayout.Space(20);

			//login button
			GUILayout.BeginHorizontal();
			GUILayout.Label("",GUILayout.Width (200));
			//if 'login' button is pressed, see if can login into game
			if (GUILayout.Button ("Login", GUILayout.Width (100))) {

				if (playerName != "" && password != "") {

					//connect to db
					dbClass db = new dbClass ();
					db.addFunction ("playerLogin");
					db.addValues ("playerName", playerName);
					db.addValues ("password", password);
					string dbReturn = db.connectToDb ();
					//print (dbReturn);
					//end add to db

					//if successful;, means login success
					if (dbReturn == "SUCCESS") {

						//-------------------------------------------
						//	 TOGGLE isLogin false/ isTutorial true
						//-------------------------------------------
						isLogin = false;
						isTutorial = true;

						//see if admin
						string playerType = db.getReturnValue("playerType");
						PlayerPrefs.SetString("playerLoginName", playerName);
						if (playerType == "ADMIN")
						{
							isTrainer = true;
						}


					}
					//if not successful print error string
					else {

						//---------------------------------
						//	 TOGGLE isMessage true
						//---------------------------------
						isMessage = true;
						message = dbReturn;
					}

				} 
				//send error msg for empty input
				else {
						
						//---------------------------------
						//	 TOGGLE isMessage true
						//---------------------------------
						isMessage = true;
						message = "Please type in your playerName or password.";

				}
		
			}

			//press create button, means create new players
			if (GUILayout.Button ("Create", GUILayout.Width (100))) {

					//-----------------------------------------
					//	 TOGGLE isCreate true/ isLogin false
					//-----------------------------------------
					isCreate = true;
					isLogin = false;
			}
			GUILayout.EndHorizontal();


			GUILayout.Space(20);


			GUILayout.EndArea();
			GUILayout.EndArea();
			GUILayout.EndArea ();


		//--------------------------------------------------------------------------------------------------
		//				IF CHOOSING TUTORIAL
		//--------------------------------------------------------------------------------------------------
		} else if (isTutorial) {


			// if role selection not completed, draw GUI
			GUILayout.BeginArea(new Rect((Screen.width - 600) / 2, (Screen.height - 300) / 2, 960, 600));
			GUILayout.BeginHorizontal();
			GUILayout.Label("Do you want to play the tutorial?", GUILayout.Width(200));
			GUILayout.Space (45);

			if(GUILayout.Button("Yes",GUILayout.Width(150)) )
			{
				//-------------------------------------------
				//	 TOGGLE isLogin false/ isTutorial true
				//-------------------------------------------
				isTutorial = false;
				//isMain = true;
				
				//load tutorial
				PlayerPrefs.SetString("isTutorial", "true");
				Application.LoadLevel("Fire_event_tutorial");
			}

			if(GUILayout.Button("No",GUILayout.Width(150)) )
			{

				//-------------------------------------------
				//	 TOGGLE isLogin false/ isTutorial true
				//-------------------------------------------
				isTutorial = false;
				isMain = true;
			}
			
			GUILayout.EndHorizontal();
			GUILayout.EndArea();


		//--------------------------------------------------------------------------------------------------
		//				IF CREATING PLAYER
		//--------------------------------------------------------------------------------------------------
		} else if (isCreate) {


			GUILayout.BeginArea (new Rect(0,0,Screen.width,Screen.height));
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),background);


			GUILayout.BeginArea (new Rect (Screen.width/4, Screen.height*.1f, Screen.width/2, Screen.height*.8f));


			GUILayout.Label ("ORILE","Title");
			GUILayout.Space (45);
			GUILayout.Label ("Create new player");
			GUILayout.Space (15);

			//player login
			GUILayout.Label ("Player Name:", GUILayout.Width (150));
			playerName = GUILayout.TextField (playerName, GUILayout.Width (150));
			//password entry
			GUILayout.Label ("Password:", GUILayout.Width (150));
			password = GUILayout.PasswordField (password, "*" [0], GUILayout.Width (150));
			//password re-entry
			GUILayout.Label ("Re-enter Password:", GUILayout.Width (150));
			reEnter = GUILayout.PasswordField (reEnter, "*" [0], GUILayout.Width (150));


			//if 'login' button is pressed, see if can login into game
			if (GUILayout.Button ("CREATE", GUILayout.Width (80))) {

				if (playerName != "" && password != "") {

					if (password.Equals (reEnter)) {

						//add to db dbClass 
						dbClass db = new dbClass ();
						db.addFunction ("playerCreate");
						db.addValues ("playerName", playerName);
						db.addValues ("password", password);
						string dbReturn = db.connectToDb ();
						//print (dbReturn);
						//end add to db

						//if successful;, means login success
						if (dbReturn == "SUCCESS NO RETURN") {

							//-----------------------------------------------------------
							//	 TOGGLE isCreate false/ isLogin true/ isMessage true
							//------------------------------------------------------------
							isCreate = false;
							isLogin = true;
							isMessage = true;
							message = "player created!";
						}
						//if not successful print error string
						else {

							//---------------------------------
							//	 TOGGLE isMessage true
							//---------------------------------
							isMessage = true;
							message = dbReturn;
						}

					}
					//password not equal to re-enter password
					else 
					{

						//---------------------------------
						//	 TOGGLE isMessage true
						//---------------------------------
						isMessage = true;
						message = "Password does not sync up.\n Please type it in again.";
					}

				} 
				//empty input
				else 
				{

					//---------------------------------
					//	 TOGGLE isMessage true
					//---------------------------------
					isMessage = true;
					message = "Please type in your playerName or password.";

				}
			}


			//back button
			if (GUILayout.Button ("back", GUILayout.Width (80))) {

				//-----------------------------------------
				//	 TOGGLE isCreate false/ isLogin true
				//-----------------------------------------
				isCreate = false;
				isLogin = true;
			}

			GUILayout.EndArea ();
			GUILayout.EndArea();
		
		
		//--------------------------------------------------------------------------------------------------
		//				IF ACCESSING PLAYBACK LIST
		//--------------------------------------------------------------------------------------------------
		} else if (isPlaybackList) {	


			GUILayout.BeginArea (new Rect(0,0,Screen.width,Screen.height));
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),background);


			GUILayout.BeginArea (new Rect ((Screen.width - 400) / 2, (Screen.height - 300) / 2, 600, 300));


			GUILayout.Label ("Playback Menu");
			GUILayout.Space (15);
			GUILayout.Label ("Select the one you want to playback");
			GUILayout.Space (15);


			string[] listArray = this.fileList.Split(',');
			foreach (string capture in listArray)
			{	
				string fileName = "";
				//Debug.Log(capture);


				//PARSE THE JSON STRING
				if (capture.IndexOf("[") == 0)
				{
					fileName = capture.Substring(2,capture.Length-3);
					//Debug.Log(fileName);
				}
				else if (capture.IndexOf("]") == capture.Length-1)
				{
					fileName = capture.Substring(1,capture.Length-3);
					//Debug.Log(fileName);
				}
				else
				{
					fileName = capture.Substring(1,capture.Length-2);
					//Debug.Log(fileName);
				}

				//this if statement is still for parsing the info
				if (fileName != "." && fileName != "..")
				{
					//create a button for each playback
					if (GUILayout.Button (fileName, GUILayout.Width (80))) {

						//prepare playbackdialogue for playback
						/*PlaybackDialogue diaggy = GameObject.Find("Main Camera").GetComponent<PlaybackDialogue>();
						diaggy.convoTitle = "";
						diaggy.dialogueNum = -1;
						diaggy.currNum = -1;
						diaggy.currTitle = "";*/

						//download files
						WWWForm sendForm = new WWWForm();
						sendForm.AddField("fileName", fileName);
						sendForm.AddField("load", "DOWN");
						WWW w = new WWW("http://www.sgi-singapore.com/projects/ORILE/loadFiles.php", sendForm);
						StartCoroutine(WaitForDownload(w, fileName));

					}
				}

			}


			GUILayout.Space (15);

			//back button
			if (GUILayout.Button ("back", GUILayout.Width (80))) {

				//----------------------------------------------------
				//	 TOGGLE isPlaybackList false /isMain true
				//----------------------------------------------------
				isPlaybackList = false;
				isMain = true;
			}


			GUILayout.EndArea();
			GUILayout.EndArea();
		

		//--------------------------------------------------------------------------------------------------
		//				IF MAIN PAGE
		//--------------------------------------------------------------------------------------------------
		} else if (isMain) {


			GUILayout.BeginArea (new Rect(0,0,Screen.width,Screen.height));
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),background);


			GUILayout.BeginArea (new Rect (Screen.width/4, Screen.height*.1f, Screen.width/2, Screen.height*.8f));


			GUILayout.Label ("ORILE","Title");
			GUILayout.Space (45);
			GUILayout.Label ("Main Menu");
			GUILayout.Space (15);
			


			//Join room by title
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Join Room:", "labelText");
			roomName = GUILayout.TextField (roomName,GUILayout.Width(Screen.width/5));
//			if (GUILayout.Button ("Join",GUILayout.Width(100))) {
//				PhotonNetwork.JoinRoom (roomName);
//
//				//-----------------------------------------
//				//	 TOGGLE isChoose true / isMain false
//				//-----------------------------------------
//				isChoose = true;
//				isMain = false;
//
//				//get from db
//				dbClass db = new dbClass();
//				db.addFunction("getSessionID");
//				db.addValues("roomName", roomName);
//				string dbReturn = db.connectToDb();
//				
//				if (dbReturn != "SUCCESS") {
//					print (dbReturn);
//				}
//				PlayerPrefs.SetInt("sessionID", db.getReturnValueInt("sessionID"));
//				PlayerPrefs.SetString("roomName", roomName);
//
//			}
			GUILayout.EndHorizontal ();
			GUILayout.Space (15);



			//Create a room (fails if exist!)
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Create Room:", "labelText");
			roomName = GUILayout.TextField (roomName,GUILayout.Width(Screen.width/5));
			if (GUILayout.Button ("Start",GUILayout.Width(100))) {




				//-----------------------------------------
				//	 TOGGLE isChoose true / isMain false
				//-----------------------------------------
				isChoose = true;
				isMain = false;

				//add to db
				dbClass db = new dbClass();
				db.addFunction("sessionCreate");
				db.addValues("roomName", roomName);
				string dbReturn = db.connectToDb();

				if (dbReturn != "SUCCESS") {
					print (dbReturn);
				}
				PlayerPrefs.SetInt("sessionID", db.getReturnValueInt("sessionID"));
				PlayerPrefs.SetString("roomName", roomName);
				PlayerPrefs.SetString ("isMaster","true");
				//add roomID
				//GameObject gameManager = GameObject.Find("GameManager");  
				//GameManagerVik vikky = gameManager.GetComponent<GameManagerVik>();
				//vikky.sessionID = db.getReturnValueInt("sessionID");
				//end add to db


				Application.LoadLevel(sceneLinkage);
			}
			GUILayout.EndHorizontal ();


			GUILayout.Space (15);


			//Join random room
//			GUILayout.BeginHorizontal ();
//			GUILayout.Label ("Join Random Room:", "labelText");
//			if (PhotonNetwork.GetRoomList ().Length == 0) {
//				GUILayout.Label ("No rooms available...");
//			} else {
//				if (GUILayout.Button ("Join")) {
//					PhotonNetwork.JoinRandomRoom ();
//				}
//			}
//			GUILayout.EndHorizontal ();
			GUILayout.Space (15);


			GUILayout.Space (30);
			GUILayout.Label ("Room Listing");
			GUILayout.Space (15);

			//room listing
			if (PhotonNetwork.GetRoomList ().Length == 0) {
				GUILayout.Label ("..no games available..");
			} else {
				//Room listing: simply call GetRoomList: no need to fetch/poll whatever!
				scrollPos = GUILayout.BeginScrollView (scrollPos);
				foreach (RoomInfo game in PhotonNetwork.GetRoomList()) {
					GUILayout.BeginHorizontal ();
					GUILayout.Label (game.name + " " + game.playerCount + "/" + game.maxPlayers,GUILayout.Width(Screen.width/5+200));
					//join button
					if (GUILayout.Button ("Join",GUILayout.Width(100))) {







						//-----------------------------------------
						//	 TOGGLE isChoose true / isMain false
						//-----------------------------------------
						isChoose = true;
						isMain = false;

						//get from db
						dbClass db = new dbClass();
						db.addFunction("getSessionID");
						db.addValues("roomName", game.name);
						string dbReturn = db.connectToDb();
						
						if (dbReturn != "SUCCESS") {
							print (dbReturn);
						}
						PlayerPrefs.SetInt("sessionID", db.getReturnValueInt("sessionID"));
						PlayerPrefs.SetString("roomName", game.name);
						PlayerPrefs.SetString ("isMaster","false");



						Application.LoadLevel(sceneLinkage);
					}
					GUILayout.EndHorizontal ();
				}
				GUILayout.EndScrollView ();
			}


			//video playback
			GUILayout.Space (45);
			GUILayout.Label ("Watch Replay");
			GUILayout.BeginHorizontal ();
			//watch button
			if (GUILayout.Button ("Watch")) {

				//----------------------------------------------
				//	 TOGGLE isPlaybackList true / isMain false
				//----------------------------------------------
				isPlaybackList = true;
				isMain = false;

			}
			GUILayout.EndHorizontal ();

			//log out
			GUILayout.Space (45);
			GUILayout.BeginHorizontal ();
			//watch button
			if (GUILayout.Button ("Log out")) {
				
				//----------------------------------------------
				//	 TOGGLE isLogin true / isMain false
				//----------------------------------------------
				isMain = false;
				isLogin = true;
				PlayerPrefs.DeleteKey("playerLoginName");
				
			}
			GUILayout.EndHorizontal ();
			
			GUILayout.EndArea ();
			GUILayout.EndArea();

		}

		//--------------------------------------------------------------------------------------------------
		//				IF CHOOSING CHARACTER
		//--------------------------------------------------------------------------------------------------
		else if (isChoose)
		{
			//choose character if player not trainer
			if (!isTrainer)
			{
				// if role selection not completed, draw GUI

			}
			else 
			{
				//if player trainer, do not need to go lobby or choose player; go directly to game
				PlayerPrefs.SetString("isTrainer", "true");
				Application.LoadLevel(sceneLinkage);
			}

		}

		//--------------------------------------------------------------------------------------------------
		//				IF WAITING LOBBY
		//--------------------------------------------------------------------------------------------------

    }




	//***********************************************************************************************************************************
	//				YIELD FUNCTIONS
	//***********************************************************************************************************************************

	//yield for downloading the playback file
	IEnumerator WaitForDownload(WWW www, string fileName)
	{
		yield return www;
			
		// check for errors
		if (www.error == null)
		{
			//put the data into a file
			//Debug.Log(www.bytes.Length);
			//Debug.Log(www.data);
			string currentDir = Directory.GetCurrentDirectory ();
			//Debug.Log (currentDir + "\\playback\\" + fileName);
			Directory.CreateDirectory (currentDir + "\\playback");
			File.WriteAllBytes(currentDir + "\\playback\\" + fileName, www.bytes);

			string filePath = "playback/" + fileName;
			//restore all to previous setting

			//EZReplayManager.get.loadFromFile(filePath);


			//---------------------------------
			//	 TOGGLE isPlaybackList false
			//--------------------------------
			PlayerPrefs.SetString("isTrainer", "false");
			PlayerPrefs.SetString("isPlayback", "true");
			PlayerPrefs.SetString("filePath", filePath);
			Application.LoadLevel(sceneLinkage);

			
		} else {
			//Debug.Log("WWW Error: "+ www.error);
		}    
	}

	//yield for waiting or list to download
	IEnumerator WaitForList(WWW www)
	{
		yield return www;
		
		// check for errors
		if (www.error == null)
		{

			this.fileList = www.data;
			//Debug.Log(this.fileList);
		} else {
			//Debug.Log("WWW Error: "+ www.error);
		}    
	}




	//***********************************************************************************************************************************



	//***********************************************************************************************************************************
	//				PHOTON NETWORK / RPC FUNCTIONS
	//***********************************************************************************************************************************

	[RPC]
	void levelLoaded(){
		

	}


	[RPC]
	
	void checkGameStatusFromMaster(){
		

		
	}

	[RPC]
	void allStartGame()
	{
		gameIsReadyToLoad = true;
		
//		Debug.LogError("slave start game");
	}





	[RPC]
	
	void setRoleUnavailable(string role){
		selectedPlayerList.Add(role);
	}
	
	[RPC]
	void setRoleAvailable(string role){
		selectedPlayerList.Remove(role);
	}
	
	[RPC]
	void allLoadGame()
	{
		gameIsReadyToLoad = true;
	}

	void OnDisconnectedFromPhoton()
	{
		Debug.LogWarning("OnDisconnectedFromPhoton");
	} 
	
	void OnPhotonPlayerConnected(){
		//print ("Now we have: "+PhotonNetwork.playerList.Length+" players in total.");
		//print (EventManager.FsmVariables.GetFsmInt ("playerNum").Value);
		print(PhotonNetwork.playerList.Length);
		//EventManager.FsmVariables.GetFsmInt("playerNum").Value = PhotonNetwork.playerList.Length;
		
	}
	
	void OnPhotonPlayerDisconnected(){
		print ("Now we have: "+PhotonNetwork.playerList.Length+" players in total.");
		//EventManager.FsmVariables.GetFsmInt("playerNum").Value = PhotonNetwork.playerList.Length;
		
	}

	//***********************************************************************************************************************************
	
}
