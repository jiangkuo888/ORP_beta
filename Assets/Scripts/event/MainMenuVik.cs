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


public class MainMenuVik : MonoBehaviour
{
	public string fileList;

    void Awake()
    {
        //PhotonNetwork.logLevel = NetworkLogLevel.Full;

        //Connect to the main photon server. This is the only IP and port we ever need to set(!)
        if (!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings("v1.0"); // version of the game/demo. used to separate older clients from newer ones (e.g. if incompatible)

        //Load name from PlayerPrefs
        PhotonNetwork.playerName = PlayerPrefs.GetString("playerName", "Guest" + Random.Range(1, 9999));

        //Set camera clipping for nicer "main menu" background
        Camera.main.farClipPlane = Camera.main.nearClipPlane + 0.1f;

		//get list of games from server
		//get list of directory
		WWWForm sendForm = new WWWForm();
		sendForm.AddField("load", "LIST");
		WWW w = new WWW("http://www.sgi-singapore.com/projects/ORILE/loadFiles.php", sendForm);
		StartCoroutine(WaitForList(w));
		//fileList = w.data;
		//Debug.Log(fileList);
    }
	public GUISkin customSkin;
	public Texture2D background;
	public bool isLogin = true;
	public bool isCreate = false;
	public bool isMessage = false;
	public bool isPlaybackList = false;
	public bool isPlayback = false;
	public bool isChoose = false;
	public string message = "";
	public string playerName = "";
	public string password = "";
	public string reEnter = "";
    public string roomName = "jkroom";
    private Vector2 scrollPos = Vector2.zero;

    void OnGUI()
    {

		GUI.skin = customSkin;


        if (!PhotonNetwork.connected)
        {
            ShowConnectingGUI();
            return;   //Wait for a connection
        }
		if (isPlayback)
		{
			return;   //Wait for a connection
		}

		GameObject gameManager = GameObject.Find("GameManager");  
		GameManagerVik vikky = gameManager.GetComponent<GameManagerVik>();
		bool playback = vikky.playback;

		if (PhotonNetwork.room != null || playback)
            return; //Only when we're not in a Room

		if (isMessage) {

			GUILayout.BeginArea (new Rect ((Screen.width - 400) / 2, (Screen.height - 300) / 2, 600, 300));
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),background);


			GUILayout.Box (message, GUILayout.Width (300), GUILayout.Height (200));
			if (GUILayout.Button ("OK", GUILayout.Width (80))) {
					isMessage = false;
			}

			GUILayout.EndArea ();


		} else if (isLogin) {

			//login page call at start of game

			GUILayout.BeginArea (new Rect(0,0,Screen.width,Screen.height));
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),background);

			GUILayout.BeginArea (new Rect (Screen.width/4, Screen.height*.1f, Screen.width/2, Screen.height*.8f));

			
			
			
			
			
			
			GUILayout.Label ("ORILE","Title");
			
			GUILayout.Space (45);

			GUILayout.Label ("Login Menu");




			GUILayout.BeginArea (new Rect (0, Screen.height*.275f, Screen.width/2, Screen.height*.5f));

			GUILayout.BeginHorizontal();
			GUILayout.Label ("Player Name:", "labelText");
			playerName = GUILayout.TextField (playerName, 25, GUILayout.Width (200));
			GUILayout.EndHorizontal();

            


			GUILayout.BeginHorizontal();
			GUILayout.Label ("Password:", "labelText");
			password = GUILayout.PasswordField (password, "*" [0], 25, GUILayout.Width (200));

			GUILayout.EndHorizontal();

			GUILayout.Space(20);


			GUILayout.BeginHorizontal();
			GUILayout.Label("",GUILayout.Width (200));
			//if 'login' button is pressed, see if can login into game
			if (GUILayout.Button ("Login", GUILayout.Width (100))) {

				if (playerName != "" && password != "") {
						//add to db dbClass 
					dbClass db = new dbClass ();
					db.addFunction ("playerLogin");
					db.addValues ("playerName", playerName);
					db.addValues ("password", password);
					string dbReturn = db.connectToDb ();
					//print (dbReturn);
					//end add to db

					//if successful;, means login success
					if (dbReturn == "SUCCESS NO RETURN") {
						isLogin = false;

						//add playerName
						//GameObject gameManager = GameObject.Find("GameManager");  
						//GameManagerVik vikky = gameManager.GetComponent<GameManagerVik>();
						vikky.loginName = playerName;
					}
					//if not successful print error string
					else {
						isMessage = true;
						message = dbReturn;
					}

				} else {

						isMessage = true;
						message = "Please type in your playerName or password.";

				}
		
			}
			if (GUILayout.Button ("Create", GUILayout.Width (100))) {
					isCreate = true;
					isLogin = false;
			}
			GUILayout.EndHorizontal();



			GUILayout.Space(20);

			GUILayout.BeginHorizontal();
			// create account





			GUILayout.EndHorizontal();

			GUILayout.EndArea();
			GUILayout.EndArea();
			GUILayout.EndArea ();

				
		} else if (isCreate) {



			GUILayout.BeginArea (new Rect(0,0,Screen.width,Screen.height));
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),background);

			//create player page
			GUILayout.BeginArea (new Rect (Screen.width/4, Screen.height*.1f, Screen.width/2, Screen.height*.8f));

			
			
			
			
			
			
			GUILayout.Label ("ORILE","Title");
			
			GUILayout.Space (45);

			GUILayout.Label ("Create new player");
			GUILayout.Space (15);

			GUILayout.Label ("Player Name:", GUILayout.Width (150));
			playerName = GUILayout.TextField (playerName, GUILayout.Width (150));
			GUILayout.Label ("Password:", GUILayout.Width (150));
			password = GUILayout.PasswordField (password, "*" [0], GUILayout.Width (150));
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
							isCreate = false;
							isLogin = true;
							isMessage = true;
							message = "player created!";
						}
						//if not successful print error string
						else {
	
							isMessage = true;
							message = dbReturn;
						}

					} else {

						isMessage = true;
						message = "Password does not sync up.\n Please type it in again.";
					}

				} else {

					isMessage = true;
					message = "Please type in your playerName or password.";

				}
			}

			if (GUILayout.Button ("back", GUILayout.Width (80))) {
					isCreate = false;
					isLogin = true;
			}

			GUILayout.EndArea ();
			GUILayout.EndArea();

		} else if (isPlaybackList) {	


			GUILayout.BeginArea (new Rect(0,0,Screen.width,Screen.height));
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),background);
			//bring them to the playback menu
			GUILayout.BeginArea (new Rect ((Screen.width - 400) / 2, (Screen.height - 300) / 2, 600, 300));




			GUILayout.Label ("Playback Menu");
			GUILayout.Space (15);
			GUILayout.Label ("Select the one you want to playback");
			GUILayout.Space (15);

			//parse the json string
			string[] listArray = this.fileList.Split(',');
			foreach (string capture in listArray)
			{	
				string fileName = "";
				Debug.Log(capture);

				if (capture.IndexOf("[") == 0)
				{
					fileName = capture.Substring(2,capture.Length-3);
					Debug.Log(fileName);
				}
				else if (capture.IndexOf("]") == capture.Length-1)
				{
					fileName = capture.Substring(1,capture.Length-3);
					Debug.Log(fileName);
				}
				else
				{
					fileName = capture.Substring(1,capture.Length-2);
					Debug.Log(fileName);
				}

				if (fileName != "." && fileName != "..")
				{
					if (GUILayout.Button (fileName, GUILayout.Width (80))) {

						isPlayback = true;
						isPlaybackList = false;

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

			if (GUILayout.Button ("back", GUILayout.Width (80))) {
				isPlaybackList = false;
			}

			GUILayout.EndArea ();
			GUILayout.EndArea();

		} else {


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
			if (GUILayout.Button ("Join",GUILayout.Width(100))) {
				PhotonNetwork.JoinRoom (roomName);
			}
			GUILayout.EndHorizontal ();
			GUILayout.Space (15);





			//Create a room (fails if exist!)
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Create Room:", "labelText");
			roomName = GUILayout.TextField (roomName,GUILayout.Width(Screen.width/5));
			if (GUILayout.Button ("Start",GUILayout.Width(100))) {
				PhotonNetwork.CreateRoom (roomName, true, true, 10);

				//add to db
				dbClass db = new dbClass();
				db.addFunction("sessionCreate");
				db.addValues("roomName", roomName);
				string dbReturn = db.connectToDb();

				if (dbReturn != "SUCCESS") {
					print (dbReturn);
				}

				//add roomID
				//GameObject gameManager = GameObject.Find("GameManager");  
				//GameManagerVik vikky = gameManager.GetComponent<GameManagerVik>();
				vikky.sessionID = db.getReturnValueInt("sessionID");
				//end add to db
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

			if (PhotonNetwork.GetRoomList ().Length == 0) {
				GUILayout.Label ("..no games available..");
			} else {
				//Room listing: simply call GetRoomList: no need to fetch/poll whatever!


				scrollPos = GUILayout.BeginScrollView (scrollPos);
				foreach (RoomInfo game in PhotonNetwork.GetRoomList()) {
					GUILayout.BeginHorizontal ();
					GUILayout.Label (game.name + " " + game.playerCount + "/" + game.maxPlayers,GUILayout.Width(Screen.width/5+200));
					if (GUILayout.Button ("Join",GUILayout.Width(100))) {
						PhotonNetwork.JoinRoom (game.name);
					}
					GUILayout.EndHorizontal ();
				}
				GUILayout.EndScrollView ();
			}



			//video playback
			GUILayout.Space (45);
			GUILayout.Label ("Watch Replay");
			GUILayout.BeginHorizontal ();

			if (GUILayout.Button ("Watch")) {

				isPlaybackList = true;

			}
			GUILayout.EndHorizontal ();
			
			GUILayout.EndArea ();
			GUILayout.EndArea();

		}
    }

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
			Debug.Log (currentDir + "\\playback\\" + fileName);
			Directory.CreateDirectory (currentDir + "\\playback");
			File.WriteAllBytes(currentDir + "\\playback\\" + fileName, www.bytes);

			string filePath = "playback/" + fileName;
			EZReplayManager.get.loadFromFile(filePath);

		} else {
			Debug.Log("WWW Error: "+ www.error);
		}    
	}

	IEnumerator WaitForList(WWW www)
	{
		yield return www;
		
		// check for errors
		if (www.error == null)
		{

			this.fileList = www.data;
			Debug.Log(this.fileList);
		} else {
			Debug.Log("WWW Error: "+ www.error);
		}    
	}
	
	void ShowConnectingGUI()
	{
		GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));

        GUILayout.Label("Connecting to Photon server.");
        GUILayout.Label("Hint: This demo uses a settings file and logs the server address to the console.");

        GUILayout.EndArea();
    }
}
