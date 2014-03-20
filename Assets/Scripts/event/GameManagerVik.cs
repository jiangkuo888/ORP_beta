using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
using System.Collections.Generic;
using dbConnect;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;

public class GameManagerVik : Photon.MonoBehaviour {

    // this is a object name (must be in any Resources folder) of the prefab to spawn as player avatar.
    // read the documentation for info how to spawn dynamically loaded game objects at runtime (not using Resources folders)
	public GUISkin customSkin;
	public Texture2D background;

	public GameObject[] playerPrefabList;
	public string[] playerList;
	public Transform[] SMSpawnPositionList;
	public Transform[] LOSpawnPositionList;
	public Transform[] LMSpawnPositionList;
	public Transform[] CRSpawnPositionList;
	private Vector3 spawnPosition;
	public HashSet<string> selectedPlayerList = new HashSet<string>();
	bool roleSelected = false;
	public PlayMakerFSM EventManager;
	public int sessionID = -1;
	public string loginName = "";
	public bool isAdmin; 
	public bool startGame = false;
	public bool gameStarted = false;
	public bool playback = false;
	public bool isLobby;
	public string[] dots = new string[] {".", "..", "..."};
	public int dotInt = 0;

    void OnJoinedRoom()
    {


    }

	Vector3 randomSpawnPosition(Transform[] positionList){
		Transform tr = positionList[Random.Range(0,positionList.Length)];
		return tr.position;

	}
	public void SaveAndQuit(){
		PhotonView photonView = this.gameObject.GetPhotonView();
		
		
		photonView.RPC ("setRoleAvailable",PhotonTargets.AllBuffered,PlayerPrefs.GetString("playerName"));
		PhotonNetwork.LeaveRoom();
	
		//replay footage saving
		EZReplayManager.get.stop ();

		//create folder for game
		if (this.sessionID != -1)
		{
			string currentDir = Directory.GetCurrentDirectory ();
			Debug.Log (currentDir);
			Directory.CreateDirectory (currentDir + "\\playback");
			string filePath = "playback/" + sessionID.ToString();
			EZReplayManager.get.saveToFile(filePath);
			
			//upload files
			WWWForm sendForm = new WWWForm();
			string fileDir = currentDir + "\\playback\\" + sessionID.ToString();
			Debug.Log(fileDir);
			byte[] something = File.ReadAllBytes(fileDir);
			Debug.Log(something.Length);
			sendForm.AddBinaryData("capture", something, sessionID.ToString());
			sendForm.AddField("load", "UP");
			WWW w = new WWW("http://www.sgi-singapore.com/projects/ORILE/loadFiles.php", sendForm);

			StartCoroutine(WaitForRequest(w));
		}

	}

	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;
		
		// check for errors
		if (www.error == null)
		{
			Debug.Log("WWW Ok!: " + www.data);
		} else {
			Debug.Log("WWW Error: "+ www.error);
		}    
	}

	void OnGUI(){
		//Add GUISkin
		GUI.skin = customSkin;
		//GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),background);

		if (PhotonNetwork.room == null || playback) return;


		// quit button GUI
		if (GUILayout.Button("Leave & Quit"))
		{
			SaveAndQuit();

		}

		if (isAdmin && !roleSelected)
		{
			StartGameAdmin();
			roleSelected = true;
			//return;
		}

		// if role selection not completed, draw GUI
		if(!roleSelected)
		{

		GUILayout.BeginArea(new Rect((Screen.width - 600) / 2, (Screen.height - 300) / 2, 960, 600));
		GUILayout.BeginHorizontal();
		GUILayout.Label("Choose a role:", GUILayout.Width(200));

		PhotonView photonView = this.gameObject.GetPhotonView();

			for(int i =0;i<playerList.Length;i++)
			{
				if (!selectedPlayerList.Contains(playerList[i]))
				{
					if(GUILayout.Button(playerList[i],GUILayout.Width(150)) )
					{
						PhotonNetwork.playerName = playerList[i];
						PlayerPrefs.SetString("playerName", playerList[i]);
						roleSelected = true;

						// broadcast role selected
						photonView.RPC ("setRoleUnavailable",PhotonTargets.AllBuffered,playerList[i]);
						
						//StartGame();
					}
				}
			}

		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		}

		if (roleSelected && !gameStarted)
		{
			WaitingLobby(PhotonNetwork.playerName);
		}

		// if number of  players reach maximum, player cannot select role
//		if(PhotonNetwork.playerList.Length < playerList.Length)
//			GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 600, 300));
	}
    IEnumerator OnLeftRoom()
    {
        //Easy way to reset the level: Otherwise we'd manually reset the camera

        //Wait untill Photon is properly disconnected (empty room, and connected back to main server)
        while(PhotonNetwork.room!=null || PhotonNetwork.connected==false)
            yield return 0;
		Application.LoadLevel(Application.loadedLevel);


    }


	void OnPhotonPlayerConnected(){
		print ("Now we have: "+PhotonNetwork.playerList.Length+" players in total.");
		EventManager.FsmVariables.GetFsmInt("playerNum").Value = PhotonNetwork.playerList.Length;

	}

	void OnPhotonPlayerDisconnected(){
		print ("Now we have: "+PhotonNetwork.playerList.Length+" players in total.");
		EventManager.FsmVariables.GetFsmInt("playerNum").Value = PhotonNetwork.playerList.Length;

	}

	void StartGameAdmin()
	{
		GameObject a = PhotonNetwork.Instantiate("Admin", new Vector3(-19.0f, 3.5f, 57.0f), Quaternion.identity, 0);

		//update roomID if needed
		if (this.sessionID == -1)
		{
			//add to db
			dbClass db = new dbClass();
			db.addFunction("getSessionID");
			db.addValues("roomName", PhotonNetwork.room.name);
			string dbReturn = db.connectToDb();
			
			if (dbReturn != "SUCCESS") {
				print (dbReturn);
			}
			
			//add roomID
			this.sessionID = db.getReturnValueInt("sessionID");
			//end add to db
			
		}
		this.gameStarted = true;
	}

	//set objects for recording
	void set4Recording()
	{
		//find the objects for recording
		//GameObject[] npc = GameObject.FindGameObjectsWithTag ("NPC");
		//foreach (GameObject npcSingle in npc)
		//	EZReplayManager.get.mark4Recording (npcSingle);

		//GameObject[] door = GameObject.FindGameObjectsWithTag ("doorSwing");
		//foreach (GameObject doorSingle in door)
		//	EZReplayManager.get.mark4Recording (doorSingle);

		//GameObject[] pickable = GameObject.FindGameObjectsWithTag ("pickable");
		//foreach (GameObject pickableSingle in pickable)
		//	EZReplayManager.get.mark4Recording (pickableSingle);

		//GameObject[] interactive = GameObject.FindGameObjectsWithTag ("interactive");
		//foreach (GameObject interactiveSingle in interactive)
		//	EZReplayManager.get.mark4Recording (interactiveSingle);

	//	GameObject[] maincamera = GameObject.FindGameObjectsWithTag ("MainCamera");
	//	foreach (GameObject maincameraSingle in maincamera)
	//		EZReplayManager.get.mark4Recording (maincameraSingle);


	}

	void WaitingLobby(string playerName)
	{
		GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 600, 300));
		//GUILayout.BeginHorizontal();

		GUILayout.Label("YOU HAVE CHOSEN: " + playerName);
		GUILayout.Space(20);

		//wait till there are four people in the room
		if (selectedPlayerList.Count < 4 && isLobby) {

			GUILayout.Label ("PLEASE WAIT TILL THERE ARE ENOUGH PLAYERS IN THE ROOM");
			GUILayout.Space (20);

			GUILayout.Label ("Number of players in the room now: " + selectedPlayerList.Count);
			GUILayout.Space (20);

			int dotNum = dotInt / 10;
			GUILayout.Label ("WAITING" + dots [dotNum]);
			dotInt++;
			if (dotInt >= 30)
			{
				dotInt = 0;
			}


		} 
		else 
		{
			if(GUILayout.Button("StartGame",GUILayout.Width(100)) )
			{	
				photonView.RPC ("allStartGame", PhotonTargets.AllBuffered);
			}
		}

		//GUILayout.EndHorizontal();
		GUILayout.EndArea();

		if (startGame)
		{
			StartGame();
			startGame = false;
		}
	}

    void StartGame()
    {

		print ("Now we have: "+PhotonNetwork.playerList.Length+" players in total.");


		EventManager.FsmVariables.GetFsmInt("playerNum").Value = PhotonNetwork.playerList.Length;
        Camera.main.farClipPlane = 1000; //Main menu set this to 0.4 for a nicer BG    

        //prepare instantiation data for the viking: Randomly diable the axe and/or shield
        bool[] enabledRenderers = new bool[2];
        enabledRenderers[0] = Random.Range(0,2)==0;//Axe
        enabledRenderers[1] = Random.Range(0, 2) == 0; ;//Shield
        
        object[] objs = new object[1]; // Put our bool data in an object array, to send
        objs[0] = enabledRenderers;

		//Debug.Log (PlayerPrefs.GetString("playerName"));
		string playerName = PhotonNetwork.playerName;


		// start drawing GUI elements
		GameObject.Find ("QuestLogButton").GetComponent<GUITexture>().enabled = true;
		GameObject.Find ("phoneButton").GetComponent<GUITexture>().enabled = true;
		GameObject.Find ("InventoryContainer").GetComponent<GUITexture>().enabled = true;
		GameObject.Find ("InventoryButton1").GetComponent<GUITexture>().enabled = true;
		GameObject.Find ("InventoryButton2").GetComponent<GUITexture>().enabled = true;


		// instantiate prefab based on the name
		for(int i = 0 ; i < playerPrefabList.Length; i++)
		{
			if(playerName == playerPrefabList[i].name)
			{
				switch(playerName)
				{
				case "Sales Manager":
					spawnPosition = randomSpawnPosition(SMSpawnPositionList);
					GameObject a = PhotonNetwork.Instantiate(playerPrefabList[i].name, spawnPosition, Quaternion.identity, 0, objs);

					a.name = "Sales Manager";
					if(GameObject.Find (a.name+" Table").gameObject.transform.Find ("DocumentHolder").GetComponent<documentData>().enabled == false)
						GameObject.Find (a.name+" Table").gameObject.transform.Find ("DocumentHolder").GetComponent<documentData>().enabled = true;


					break;
				case "LPU Officer":
					spawnPosition = randomSpawnPosition(LOSpawnPositionList);
					GameObject b = PhotonNetwork.Instantiate(playerPrefabList[i].name, spawnPosition, Quaternion.identity, 0, objs);
					b.name = "LPU Officer";
					if(GameObject.Find (b.name+" Table").gameObject.transform.Find ("DocumentHolder").GetComponent<documentData>().enabled == false)
						GameObject.Find (b.name+" Table").gameObject.transform.Find ("DocumentHolder").GetComponent<documentData>().enabled = true;

					break;
				case "LPU Manager":
					spawnPosition = randomSpawnPosition(LMSpawnPositionList);
					GameObject c = PhotonNetwork.Instantiate(playerPrefabList[i].name, spawnPosition, Quaternion.identity, 0, objs);
					c.name = "LPU Manager";
					if(GameObject.Find (c.name+" Table").gameObject.transform.Find ("DocumentHolder").GetComponent<documentData>().enabled == false)
						GameObject.Find (c.name+" Table").gameObject.transform.Find ("DocumentHolder").GetComponent<documentData>().enabled = true;

					break;
				case "Credit Risk":
					spawnPosition = randomSpawnPosition(CRSpawnPositionList);
					GameObject d = PhotonNetwork.Instantiate(playerPrefabList[i].name, spawnPosition, Quaternion.identity, 0, objs);
					d.name = "Credit Risk";

					if(GameObject.Find (d.name+" Table").gameObject.transform.Find ("DocumentHolder").GetComponent<documentData>().enabled == false)
						GameObject.Find (d.name+" Table").gameObject.transform.Find ("DocumentHolder").GetComponent<documentData>().enabled = true;

					break;
				default:
					break;

				}
				
			}
		
			//update roomID if needed
			if (this.sessionID == -1)
			{
				//add to db
				dbClass db = new dbClass();
				db.addFunction("getSessionID");
				db.addValues("roomName", PhotonNetwork.room.name);
				string dbReturn = db.connectToDb();

				if (dbReturn != "SUCCESS") {
					print (dbReturn);
				}
				
				//add roomID
				this.sessionID = db.getReturnValueInt("sessionID");
				//end add to db
	       		
			}


		}

		set4Recording ();
		EZReplayManager.get.record();

		gameStarted = true;
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
	void allStartGame()
	{
		startGame = true;
	}

    void OnDisconnectedFromPhoton()
    {
        Debug.LogWarning("OnDisconnectedFromPhoton");
    } 

}
