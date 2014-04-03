using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
using System.Collections.Generic;
using dbConnect;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;

public class GameManagerVikMain : Photon.MonoBehaviour {

    // this is a object name (must be in any Resources folder) of the prefab to spawn as player avatar.
    // read the documentation for info how to spawn dynamically loaded game objects at runtime (not using Resources folders)
	public GUISkin customSkin;
	public Texture2D background;
	//public GameObject GameEndScreen;

	public GameObject[] playerPrefabList;
	public string[] playerList;
	private Vector3 spawnPosition;
	public HashSet<string> selectedPlayerList = new HashSet<string>();
	bool roleSelected = false;
	public int sessionID = -1;
	public string loginName = "";
	public bool isAdmin; 
	public bool startGame = false;
	public bool gameStarted = false;
	public bool playback = false;
	public bool isLobby;
	public string[] dots = new string[] {".", "..", "..."};
	public int dotInt = 0;
	//public GameObject mainCam;
	
	Vector3 randomSpawnPosition(Transform[] positionList){
		Transform tr = positionList[Random.Range(0,positionList.Length)];
		return tr.position;

	}

	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;
		
		// check for errors
		if (www.error == null)
		{
			//Debug.Log("WWW Ok!: " + www.data);
		} else {
			//Debug.Log("WWW Error: "+ www.error);
		}    
	}

	void OnGUI(){
	
	}
   

	/*void StartGameAdmin()
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
	}*/

	//set objects for recording

	void WaitingLobby(string playerName)
	{

	}



}
