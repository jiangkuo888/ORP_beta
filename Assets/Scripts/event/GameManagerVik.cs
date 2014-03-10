using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
using System.Collections.Generic;
using dbConnect;

public class GameManagerVik : Photon.MonoBehaviour {

    // this is a object name (must be in any Resources folder) of the prefab to spawn as player avatar.
    // read the documentation for info how to spawn dynamically loaded game objects at runtime (not using Resources folders)
	public GameObject[] playerPrefabList;
	public string[] playerList;
	public Transform[] SMSpawnPositionList;
	public Transform[] LOSpawnPositionList;
	public Transform[] LMSpawnPositionList;
	public Transform[] CRSpawnPositionList;
	private Vector3 spawnPosition;
	HashSet<string> selectedPlayerList = new HashSet<string>();
	bool roleSelected = false;
	public PlayMakerFSM EventManager;
	public int sessionID = -1;
	public string loginName = "";

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

	}

	void OnGUI(){

		if (PhotonNetwork.room == null) return;


		// quit button GUI
		if (GUILayout.Button("Leave& QUIT"))
		{
			SaveAndQuit();

		}


		// if role selection not completed, draw GUI
		if(!roleSelected)
		{

		GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 600, 300));
		GUILayout.BeginHorizontal();
		GUILayout.Label("Chose a role:", GUILayout.Width(150));

		PhotonView photonView = this.gameObject.GetPhotonView();

			for(int i =0;i<playerList.Length;i++)
			{
				if (!selectedPlayerList.Contains(playerList[i]))
				{
					if(GUILayout.Button(playerList[i],GUILayout.Width(100)) )
					{
						PhotonNetwork.playerName = playerList[i];
						PlayerPrefs.SetString("playerName", playerList[i]);
						roleSelected = true;

						// broadcast role selected
						photonView.RPC ("setRoleUnavailable",PhotonTargets.AllBuffered,playerList[i]);
						
						StartGame();
					}
				}
			}

		GUILayout.EndHorizontal();
		GUILayout.EndArea();
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


		string playerName = PlayerPrefs.GetString("playerName");


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




    }
	[RPC]

	void setRoleUnavailable(string role){
		selectedPlayerList.Add(role);
	}

	[RPC]
	void setRoleAvailable(string role){
		selectedPlayerList.Remove(role);
	}

	

    void OnDisconnectedFromPhoton()
    {
        Debug.LogWarning("OnDisconnectedFromPhoton");
    }    
}
