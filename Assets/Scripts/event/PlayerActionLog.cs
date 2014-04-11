using UnityEngine;
using System.Collections;
using dbConnect;
using System;

public class PlayerActionLog : MonoBehaviour {
	
	// Update is called once per frame
	public void  addToPlayerActionLog(string actionRefID, string actionInfo) {

		//get roomID & playerName
		GameObject gameManager = GameObject.Find("GameManager");  
		GameManagerVik vikky = gameManager.GetComponent<GameManagerVik>();
		string sessionID = vikky.sessionID.ToString ();
		string playerName = vikky.loginName;
		string playerRole = vikky.characterName;

		//add to db
		dbClass db = new dbClass();
		db.addFunction("playerActionLog");
		db.addValues("playerName", playerName);
		db.addValues("playerRole", playerRole);
		db.addValues("sessionID", sessionID);
		db.addValues("actionRefID", actionRefID);
		db.addValues("actionInfo", actionInfo);
		string dbReturn = db.connectToDb();

		if (dbReturn != "SUCCESS NO RETURN") {
			print (dbReturn);
		}

		//end add to db

	}
}
