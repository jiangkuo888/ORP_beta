using System;
using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

public class NetworkTime : MonoBehaviour {



	public int GMT;
	public bool isMaster;
	public GUISkin customSkin;




	PlayMakerFSM EventManager;

	int startTimeInSec;
	int currentTimeInSec;


	// Use this for initialization
	void Start () {
		startTimeInSec = 0;
		currentTimeInSec = 0;
		EventManager = GameObject.Find ("EventManager").GetComponent<PlayMakerFSM>();

		startTimeInSec = PhotonNetwork.networkingPeer.ServerTimeInMilliSeconds/1000;

		print (formatAsTime(startTimeInSec));
	}

	
	// Update is called once per frame
	void Update () {
	


		 


		currentTimeInSec = PhotonNetwork.networkingPeer.ServerTimeInMilliSeconds/1000;


			EventManager.FsmVariables.GetFsmBool("isMaster").Value = true;
			EventManager.FsmVariables.GetFsmFloat("TimeSinceCreate").Value = currentTimeInSec - startTimeInSec;



	}

	void OnGUI(){




		GUILayout.BeginArea(new Rect(Screen.width/6, 0, 200,60));
		GUILayout.Label(formatAsTime(PhotonNetwork.networkingPeer.ServerTimeInMilliSeconds/1000),customSkin.customStyles[2]);

		GUILayout.EndArea();
	}


	string formatAsTime(int timeInFloat){


		int sec,min,hrs;
		DateTime date;

		sec = timeInFloat%60;
		min = timeInFloat/60%60;
		hrs = (timeInFloat/60/60)%24;
		date = System.DateTime.Now;
		return string.Format("{0:dd-MM-yyyy}  {1:D2}:{2:D2}:{3:D2}",date, hrs,min,sec);
	}
}
