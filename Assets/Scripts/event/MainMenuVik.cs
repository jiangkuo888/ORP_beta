using UnityEngine;
using System.Collections;
using dbConnect;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
//using System;


public class MainMenuVik : MonoBehaviour
{

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

    }

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

			GUILayout.Box (message, GUILayout.Width (300), GUILayout.Height (200));
			if (GUILayout.Button ("OK", GUILayout.Width (80))) {
					isMessage = false;
			}

			GUILayout.EndArea ();


		} else if (isLogin) {

			//login page call at start of game

			GUILayout.BeginArea (new Rect ((Screen.width - 400) / 2, (Screen.height - 300) / 2, 600, 300));

			GUILayout.Label ("Login Menu");
			GUILayout.Space (15);

			GUILayout.Label ("Welcome to ORILE! Hope you have a great time. :)");
			GUILayout.Space (15);

			GUILayout.Label ("Player Name:", GUILayout.Width (150));
			playerName = GUILayout.TextField (playerName, 25, GUILayout.Width (150));
			GUILayout.Label ("Password:", GUILayout.Width (150));
			password = GUILayout.PasswordField (password, "*" [0], 25, GUILayout.Width (150));

			//if 'login' button is pressed, see if can login into game
			if (GUILayout.Button ("LOGIN", GUILayout.Width (80))) {

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

			// create account
			GUILayout.Label ("Do not have a account yet? Register here.");
			if (GUILayout.Button ("CREATE", GUILayout.Width (80))) {
					isCreate = true;
					isLogin = false;
			}
			GUILayout.EndArea ();

				
		} else if (isCreate) {

			//create player page
			GUILayout.BeginArea (new Rect ((Screen.width - 400) / 2, (Screen.height - 300) / 2, 600, 300));

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

		} else if (isPlaybackList) {	

			//bring them to the playback menu
			GUILayout.BeginArea (new Rect ((Screen.width - 400) / 2, (Screen.height - 300) / 2, 600, 300));

			GUILayout.Label ("Playback Menu");
			GUILayout.Space (15);
			GUILayout.Label ("Select the one you want to playback");
			GUILayout.Space (15);
			//GUILayout.Box ("", GUILayout.Width (300), GUILayout.Height (200));
			//if (GUILayout.Button ("OK", GUILayout.Width (80))) {
			//	isMessage = false;
			//}

			DirectoryInfo dir = new DirectoryInfo(@"C:\Users\srinivas\Documents\GitHub\ORP_beta\playback\");
			FileInfo[] fileinfo = dir.GetFiles();
			//Debug.Log(fileinfo.ToString());
			foreach (FileInfo getfile in fileinfo) {
				if (GUILayout.Button (getfile.Name, GUILayout.Width (80))) {
					isPlayback = true;
					isPlaybackList = false;
					string filePath = "playback/" + getfile.Name;
					EZReplayManager.get.loadFromFile(filePath);
				}
			}
			GUILayout.Space (15);

			if (GUILayout.Button ("back", GUILayout.Width (80))) {
				isPlaybackList = false;
			}

			GUILayout.EndArea ();

		} else {

			GUILayout.BeginArea (new Rect ((Screen.width - 400) / 2, (Screen.height - 300) / 2, 600, 300));
			
			GUILayout.Label ("Main Menu");
			
			GUILayout.Space (15);
			
			
			//Join room by title
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("JOIN ROOM:", GUILayout.Width (150));
			roomName = GUILayout.TextField (roomName);
			if (GUILayout.Button ("JOIN")) {
				PhotonNetwork.JoinRoom (roomName);
			}
			GUILayout.EndHorizontal ();
			
			//Create a room (fails if exist!)
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("CREATE ROOM:", GUILayout.Width (150));
			roomName = GUILayout.TextField (roomName);
			if (GUILayout.Button ("START")) {
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
			
			//Join random room
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("JOIN RANDOM ROOM:", GUILayout.Width (150));
			if (PhotonNetwork.GetRoomList ().Length == 0) {
				GUILayout.Label ("..no games available...");
			} else {
				if (GUILayout.Button ("JOIN")) {
					PhotonNetwork.JoinRandomRoom ();
				}
			}
			GUILayout.EndHorizontal ();
			
			GUILayout.Space (30);
			GUILayout.Label ("ROOM LISTING:");
			if (PhotonNetwork.GetRoomList ().Length == 0) {
				GUILayout.Label ("..no games available..");
			} else {
				//Room listing: simply call GetRoomList: no need to fetch/poll whatever!
				scrollPos = GUILayout.BeginScrollView (scrollPos);
				foreach (RoomInfo game in PhotonNetwork.GetRoomList()) {
					GUILayout.BeginHorizontal ();
					GUILayout.Label (game.name + " " + game.playerCount + "/" + game.maxPlayers);
					if (GUILayout.Button ("JOIN")) {
						PhotonNetwork.JoinRoom (game.name);
					}
					GUILayout.EndHorizontal ();
				}
				GUILayout.EndScrollView ();
			}

			//video playback
			GUILayout.Space (20);
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("See previous replay?", GUILayout.Width (150));
			if (GUILayout.Button ("Go Here")) {

				isPlaybackList = true;

			}
			GUILayout.EndHorizontal ();
			
			GUILayout.EndArea ();

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
