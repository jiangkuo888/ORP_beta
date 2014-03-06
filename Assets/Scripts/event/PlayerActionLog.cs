using UnityEngine;
using System.Collections;
using dbConnect;
using System;

public class PlayerActionLog : MonoBehaviour {
	
	// Update is called once per frame
	public void  addToPlayerActionLog(string playerActionType, string outcome, string actionInfo) {

		//get roomID & playerName
		GameObject gameManager = GameObject.Find("GameManager");  
		GameManagerVik vikky = gameManager.GetComponent<GameManagerVik>();
		string roomID = vikky.roomID.ToString ();
		string playerName = vikky.loginName;
		string playerRole = PlayerPrefs.GetString("playerName");

		print (roomID);
		print (playerName);
		print (playerRole);
		//add to db
		dbClass db = new dbClass();
		db.addFunction("playerActionLog");
		db.addValues("playerName", playerName);
		db.addValues("playerRole", playerRole);
		db.addValues("roomID", roomID);
		db.addValues("playerActionType", playerActionType);
		db.addValues("outcome", outcome);
		db.addValues("actionInfo", actionInfo);
		string dbReturn = db.connectToDb();

		if (dbReturn != "SUCCESS NO RETURN") {
			print (dbReturn);
		}

		//end add to db

	}
}
