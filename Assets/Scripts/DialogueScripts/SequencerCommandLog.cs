using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.SequencerCommands;
using dbConnect;


public class SequencerCommandLog : SequencerCommand  {

	private string actionRefID; 
	private string actionInfo;

	void initialize() {
	}

	// Use this for initialization
	void Start () {

		this.actionRefID = GetParameter(0);
		this.actionInfo = GetParameter(1);
		
		addToPlayerActionLog (this.actionRefID, this.actionInfo);
	
		Stop ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void onDestroy () {

	}
	
	public void  addToPlayerActionLog(string actionRefID, string actionInfo) {
		
		//get roomID & playerName
		GameObject gameManager = GameObject.Find("GameManager");  
		GameManagerVik vikky = gameManager.GetComponent<GameManagerVik>();
		string sessionID = vikky.sessionID.ToString ();
		string playerName = vikky.loginName;
		string playerRole = PlayerPrefs.GetString("playerName");
		
		//add to db
		dbClass db = new dbClass();
		db.addFunction("playerActionLog");
		db.addValues("playerName", playerName);
		db.addValues("playerRole", playerRole);
		db.addValues("sessionID", sessionID);
		db.addValues("actionRefID", actionRefID);
		db.addValues("actionInfo", actionInfo);
		string dbReturn = db.connectToDb();
		//print (dbReturn);
		if (dbReturn != "SUCCESS NO RETURN") {
			print (dbReturn);
			print (sessionID);
			print (playerName);
			print (playerRole);
		}
		
		//end add to db
		
	}
}
