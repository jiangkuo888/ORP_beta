using System;
using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

public class NetworkTime : MonoBehaviour {
	
	
	
	public int GMT;
	public bool isMaster,isFreezed;
	public GUISkin customSkin;
	
	
	
	
	PlayMakerFSM EventManager;
	
	public int startTimeInSec;
	int currentTimeInSec;
	
	float FreezePoint;
	
	
	// Use this for initialization
	void Start () {
		startTimeInSec = 0;
		currentTimeInSec = 0;
		EventManager = GameObject.Find ("EventManager").GetComponent<PlayMakerFSM>();
		
		startTimeInSec = PhotonNetwork.networkingPeer.ServerTimeInMilliSeconds/1000;
		
		isFreezed = false;
		//print (formatAsTime(startTimeInSec));
	}
	
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.F1))
		{
			if(!isFreezed)
			{
				PhotonView timeView = this.gameObject.GetPhotonView();
				timeView.RPC ("Freeze",PhotonTargets.AllBuffered);
			}
			else
			{
				PhotonView timeView = this.gameObject.GetPhotonView();
				timeView.RPC ("unFreeze",PhotonTargets.AllBuffered);
			}
			
			
		}
		
		if(!isFreezed)
		{
			currentTimeInSec = PhotonNetwork.networkingPeer.ServerTimeInMilliSeconds/1000;
		}
		else{
			
			currentTimeInSec = (int)FreezePoint;
		}
		
		
		EventManager.FsmVariables.GetFsmBool("isMaster").Value = true;
		EventManager.FsmVariables.GetFsmFloat("TimeSinceCreate").Value = currentTimeInSec - startTimeInSec;
		
		
		
	}
	[RPC]
	void Freeze(){

	



		FreezePoint = PhotonNetwork.networkingPeer.ServerTimeInMilliSeconds/1000;



		
		isFreezed = true;
	}
	[RPC]
	void unFreeze(){
	
		


		currentTimeInSec = PhotonNetwork.networkingPeer.ServerTimeInMilliSeconds/1000;

	

		int freezeTime = currentTimeInSec - (int) FreezePoint;




	

		startTimeInSec = startTimeInSec + freezeTime;


		


		isFreezed = false;



		

		

	}
	
	
	
	
	void OnGUI(){
		
		
		
		
		
		
		GUILayout.BeginArea(new Rect(Screen.width/6+30, 0, 200,60));
		GUILayout.Label(formatAsTime(PhotonNetwork.networkingPeer.ServerTimeInMilliSeconds/1000),customSkin.customStyles[2]);
		
		//debug
		GUILayout.Label((currentTimeInSec - startTimeInSec).ToString(),customSkin.customStyles[2]);
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
