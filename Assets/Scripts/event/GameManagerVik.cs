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
	public GameObject GameEndScreen;
	
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
	public bool isTrainer = false;
	public bool isPlayBack = false;
	public bool isTutorial = false;
	public bool connected = false;
	public bool startGameNow = false;
	
	//sync boolean
	public int syncNum = 0;
	public int syncTotal = 4;
	
	//debugging variables
	public string characterName = "";
	public string roomName = "Room";
	public bool noLogin = false;
	
	
	
	//***********************************************************************************************************************************
	//		 start the game either using values from login screen or with predefined values
	//***********************************************************************************************************************************
	void Start()
	{
		this.isPlayBack = false;
		//get player name
		string tempName = PlayerPrefs.GetString ("playerName");
		if (tempName != "" && !noLogin) 
		{
			characterName = tempName;
			sessionID = PlayerPrefs.GetInt ("sessionID");
			loginName = PlayerPrefs.GetString ("playerLoginName");
			roomName = PlayerPrefs.GetString ("roomName");
			
			if (PlayerPrefs.GetString ("isTrainer") == "true")
			{
				isTrainer = true;
			}
			else
			{
				isTrainer = false;
			}
		} 
		else if (PlayerPrefs.GetString("isTutorial") == "true")
		{
			isTutorial = true;
		}
		else
		{
			//-----------------------------------------------------------------------------------------------------
			//					Photon network initialisation
			//-----------------------------------------------------------------------------------------------------
			
			// Connect to the main photon server. This is the only IP and port we ever need to set(!)
			//if (!PhotonNetwork.connected)
			if (!PhotonNetwork.connected)
				PhotonNetwork.ConnectUsingSettings("v1.0"); // version of the game/demo. used to separate older clients from newer ones (e.g. if incompatible)
			PhotonNetwork.playerName = PlayerPrefs.GetString("playerName", "Guest" + Random.Range(1, 9999));
			//-----------------------------------------------------------------------------------------------------
		}
		
		
		if(isTutorial)
			startTutorial();
	}
	
	void OnLevelWasLoaded(int level) {
		
		
		
		if (level == 1)
		{
			print("Woohoo");
			photonView.RPC ("levelLoaded",PhotonTargets.AllBuffered);
		}
		
	}
	
	
	
	void Update()
	{
		if(!isTutorial)
		
		{
			if (PlayerPrefs.GetString ("isPlayback") == "true" && !this.isPlayBack)
			{
				EZReplayManager.get.loadFromFile(PlayerPrefs.GetString ("filePath"));
				this.isPlayBack = true;
			}
			
			
			if (noLogin && !connected && !this.isPlayBack)
			{
				if (PhotonNetwork.connected)
				{
					StartCoroutine(WaitForConnect());
					connected = true;
				}
				
			}
			
			if (!noLogin && !connected && !this.isPlayBack && this.syncNum >= this.syncTotal)
			{
				photonView.RPC ("allStartGame",PhotonTargets.AllBuffered);
				connected = true;
				
			}
			
			if (startGameNow)
			{
				startGame();
				startGameNow = false;
			}
			
		}
		
	}
	
	//yield the saving of binary files
	IEnumerator WaitForConnect()
	{
		yield return new WaitForSeconds(1);
		
		//-----------------------------------------------------------------------------------------------------
		//					create room
		//-----------------------------------------------------------------------------------------------------
		PhotonNetwork.CreateRoom (roomName, true, true, 10);
		//PhotonNetwork.JoinRoom (roomName);
		
		//Debug.Log (PhotonNetwork.countOfPlayersOnMaster);
		//Debug.Log (PhotonNetwork.countOfRooms);
	//	Debug.Log (PhotonNetwork.countOfPlayersInRooms);
		//Debug.Log (PhotonNetwork.playerName);
		
		PhotonNetwork.playerName = characterName;
		//PlayerPrefs.SetString("playerName", playerList[i]);
		
		// broadcast role selected
		photonView.RPC ("setRoleUnavailable",PhotonTargets.AllBuffered,characterName);
		
		//add to db
		dbClass db = new dbClass();
		db.addFunction("sessionCreate");
		db.addValues("roomName", roomName);
		string dbReturn = db.connectToDb();
		
		if (dbReturn != "SUCCESS") {
			print (dbReturn);
		}
		sessionID =  db.getReturnValueInt("sessionID");
		
		yield return new WaitForSeconds(1);
		startGame();
	}


	// start tutorial game
	void startTutorial(){
		print ("Now we are in tutorial mode.");

		EventManager.FsmVariables.GetFsmInt("playerNum").Value = 1;
		Camera.main.farClipPlane = 1000; //Main menu set this to 0.4 for a nicer BG    

		string playerName = "LPU Officer";
		PhotonNetwork.playerName = playerName;
		GameObject.Find ("QuestLogButton").GetComponent<GUITexture>().enabled = true;
		GameObject.Find ("phoneButton").GetComponent<GUITexture>().enabled = true;
		GameObject.Find ("InventoryContainer").GetComponent<GUITexture>().enabled = true;
		GameObject.Find ("InventoryButton1").GetComponent<GUITexture>().enabled = true;
		GameObject.Find ("InventoryButton2").GetComponent<GUITexture>().enabled = false;


		GameObject playa = null;


		for(int i = 0 ; i < playerPrefabList.Length; i++)
		{
			
			if(playerName == playerPrefabList[i].name)
			{
				switch(playerName)
				{
				case "Sales Manager":

					break;
				case "LPU Officer":
					spawnPosition = randomSpawnPosition(LOSpawnPositionList);
					playa = Instantiate(playerPrefabList[i], spawnPosition, Quaternion.identity) as GameObject;
					playa.name = "LPU Officer";

					//playa.GetComponent<CharacterController>().detectCollisions = false;
					break;
				case "LPU Manager":

					break;
				case "Credit Risk":

					break;
				default:
					break;
					
				}
				
				GameObject.Find ("phoneButton").GetComponent<phoneButton>().loadSmallButtonCharacter();
				
			}
			
		}
		
		
		EZReplayManager.get.record();
		if (playa != null)
		{
			CameraChange (playa);
		}



	}


	//start the game
	void startGame()
	{
		//set name
		//PhotonNetwork.playerName = characterName;
		
		print ("Now we have: "+PhotonNetwork.playerList.Length+" players in total.");
		
		//		print (EventManager.FsmVariables.GetFsmInt ("playerNum").Value);
		//	print(PhotonNetwork.playerList.Length);
		
		//if not trainer
		if (!isTrainer)
		{
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
			//GameObject.Find ("InventoryContainer").GetComponent<GUITexture>().enabled = true;
		//	GameObject.Find ("InventoryButton1").GetComponent<GUITexture>().enabled = true;
		//	GameObject.Find ("InventoryButton2").GetComponent<GUITexture>().enabled = false;
			
			// start timer 
			if(!isTutorial)
			{
				GameObject.Find ("EventManager").GetComponent<NetworkTime>().enabled = true;
				this.GetComponent<ChatVik>().enabled = true;
			}
			
			
			// instantiate prefab based on the name
			GameObject playa = null;
			for(int i = 0 ; i < playerPrefabList.Length; i++)
			{
				
				if(playerName == playerPrefabList[i].name)
				{
					switch(playerName)
					{
					case "Sales Manager":
						spawnPosition = randomSpawnPosition(SMSpawnPositionList);
						playa = PhotonNetwork.Instantiate(playerPrefabList[i].name, spawnPosition, Quaternion.identity, 0, objs);
						
						playa.name = "Sales Manager";
						//playa.GetComponent<CharacterController>().detectCollisions = false;
						
						if(!isTutorial)
						{
							if(GameObject.Find (playa.name+" Table").gameObject.transform.Find ("DocumentHolder").GetComponent<documentData>().enabled == false)
								GameObject.Find (playa.name+" Table").gameObject.transform.Find ("DocumentHolder").GetComponent<documentData>().enabled = true;
						}
						else{
							GameObject.Find ("EventManager-Tutorial").GetComponent<PlayMakerFSM>().FsmVariables.GetFsmBool("isSM").Value = true;
						}
						
						break;
					case "LPU Officer":
						spawnPosition = randomSpawnPosition(LOSpawnPositionList);
						playa = PhotonNetwork.Instantiate(playerPrefabList[i].name, spawnPosition, Quaternion.identity, 0, objs);
						playa.name = "LPU Officer";
						//playa.GetComponent<CharacterController>().detectCollisions = false;
						
						if(!isTutorial)
						{
							if(GameObject.Find (playa.name+" Table").gameObject.transform.Find ("DocumentHolder").GetComponent<documentData>().enabled == false)
								GameObject.Find (playa.name+" Table").gameObject.transform.Find ("DocumentHolder").GetComponent<documentData>().enabled = true;
						}
						break;
					case "LPU Manager":
						spawnPosition = randomSpawnPosition(LMSpawnPositionList);
						playa = PhotonNetwork.Instantiate(playerPrefabList[i].name, spawnPosition, Quaternion.identity, 0, objs);
						playa.name = "LPU Manager";
						//playa.GetComponent<CharacterController>().detectCollisions = false;
						
						if(!isTutorial)
						{
							if(GameObject.Find (playa.name+" Table").gameObject.transform.Find ("DocumentHolder").GetComponent<documentData>().enabled == false)
								GameObject.Find (playa.name+" Table").gameObject.transform.Find ("DocumentHolder").GetComponent<documentData>().enabled = true;
						}
						break;
					case "Credit Risk":
						spawnPosition = randomSpawnPosition(CRSpawnPositionList);
						playa = PhotonNetwork.Instantiate(playerPrefabList[i].name, spawnPosition, Quaternion.identity, 0, objs);
						playa.name = "Credit Risk";
						//playa.GetComponent<CharacterController>().detectCollisions = false;
						
						if(!isTutorial)
						{
							if(GameObject.Find (playa.name+" Table").gameObject.transform.Find ("DocumentHolder").GetComponent<documentData>().enabled == false)
								GameObject.Find (playa.name+" Table").gameObject.transform.Find ("DocumentHolder").GetComponent<documentData>().enabled = true;
						}
						break;
					default:
						break;
						
					}
					
					GameObject.Find ("phoneButton").GetComponent<phoneButton>().loadSmallButtonCharacter();
					
				}
				
			}
			
			
			EZReplayManager.get.record();
			if (playa != null)
			{
				CameraChange (playa);
			}
		}
		// if is trainer
		else
		{
			GameObject a = PhotonNetwork.Instantiate("Admin", new Vector3(-19.0f, 3.5f, 57.0f), Quaternion.identity, 0);
		}
		
		
//		Debug.Log (PhotonNetwork.countOfPlayersOnMaster);
//		Debug.Log (PhotonNetwork.countOfRooms);
//		Debug.Log (PhotonNetwork.countOfPlayersInRooms);
//		Debug.Log (PhotonNetwork.playerName);
		
	}
	
	//spawn positions at start
	Vector3 randomSpawnPosition(Transform[] positionList){
		Transform tr = positionList[Random.Range(0,positionList.Length)];
		return tr.position;
		
	}
	
	public void CameraChange(GameObject ch)
	{
		Camera.main.transform.parent = ch.transform;
		Camera.main.transform.localPosition =  new Vector3(0,1.257728f, 0);
		Camera.main.transform.localEulerAngles = new Vector3(0.6651921f, 90, 0);
		
		//update the camera state for playback
		PlaybackCamera script = Camera.main.GetComponent<PlaybackCamera>();
		script.isMainCameraChild = true;
	}
	
	//***********************************************************************************************************************************
	
	
	//GUI
	
	void OnGUI()
	{
		//set to custom skin
		GUI.skin = customSkin;
		
		// quit button GUI
		if (GUILayout.Button ("Leave & Quit")) {
			SaveAndQuit ();
		}
		
	}
	
	//***********************************************************************************************************************************
	//				QUIT AND SAVE REPLAY FILE
	//***********************************************************************************************************************************
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
			//Debug.Log (currentDir);
			Directory.CreateDirectory (currentDir + "\\playback");
			string filePath = "playback/" + sessionID.ToString();
			EZReplayManager.get.saveToFile(filePath);
			
			//upload files
			WWWForm sendForm = new WWWForm();
			string fileDir = currentDir + "\\playback\\" + sessionID.ToString();
			//Debug.Log(fileDir);
			byte[] something = File.ReadAllBytes(fileDir);
			//Debug.Log(something.Length);
			sendForm.AddBinaryData("capture", something, sessionID.ToString());
			sendForm.AddField("load", "UP");
			WWW w = new WWW("http://www.sgi-singapore.com/projects/ORILE/loadFiles.php", sendForm);
			
			StartCoroutine(WaitForRequest(w));
		}
		
	}
	
	//yield the saving of binary files
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
		//PlayerPrefs.DeleteAll();
		
		yield return new WaitForSeconds (1);
		
		Application.LoadLevel("login scene");
	}
	
	//***********************************************************************************************************************************
	
	//***********************************************************************************************************************************
	//				END GAME
	//***********************************************************************************************************************************
	public void EndGame(){
		GameObject MainCamera = Camera.main.gameObject;
		
		if(MainCamera.gameObject != null)
		{
			if(MainCamera.transform.parent != null)
			{
				
				MainCamera.transform.parent.GetComponent<ClickMove>().gameOn = false;
				MainCamera.transform.parent.GetComponent<DetectObjects>().gameOn = false;
			}
			
			MainCamera.SetActive(false);
			//GameEndScreen.SetActive(true);
			
			
			GameObject.Find ("QuestLogButton").GetComponent<GUITexture>().enabled = false;
			GameObject.Find ("phoneButton").GetComponent<GUITexture>().enabled = false;
			GameObject.Find ("InventoryContainer").GetComponent<GUITexture>().enabled = false;
			GameObject.Find ("InventoryButton1").GetComponent<GUITexture>().enabled = false;
			GameObject.Find ("InventoryButton2").GetComponent<GUITexture>().enabled = false;
			GameObject.Find ("GameManager").GetComponent<ChatVik>().enabled = false;
			
			GameObject.Find ("EventManager").GetComponent<NetworkTime>().enabled = true;
			
			
		}
		
	}
	
	//***********************************************************************************************************************************
	
	[RPC]
	
	void setRoleUnavailable(string role){
		selectedPlayerList.Add(role);
	}
	
	[RPC]
	void setRoleAvailable(string role){
		selectedPlayerList.Remove(role);
	}
	
	[RPC]
	void levelLoaded(){
		
		this.syncNum++;
	}
	
	[RPC]
	void allStartGame()
	{
		startGameNow = true;
	}
	
	void OnDisconnectedFromPhoton()
	{
		Debug.LogWarning("OnDisconnectedFromPhoton");
	} 
	
	IEnumerator OnLeftRoom()
	{
		//Easy way to reset the level: Otherwise we'd manually reset the camera
		
		//Wait untill Photon is properly disconnected (empty room, and connected back to main server)
		while(PhotonNetwork.room!=null || PhotonNetwork.connected==false)
			yield return 0;
		//Application.LoadLevel(Application.loadedLevel);
		
		
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
	
	
}
