using System;
using UnityEngine;
using System.Collections;

public class NetworkTime : MonoBehaviour {
	public int GMT;
	int sec,min,hrs;
	public GUISkin customSkin;

	DateTime date;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
		 sec = PhotonNetwork.networkingPeer.ServerTimeInMilliSeconds/1000%60;
		 min = PhotonNetwork.networkingPeer.ServerTimeInMilliSeconds/1000/60%60;
		hrs = (PhotonNetwork.networkingPeer.ServerTimeInMilliSeconds/1000/60/60 + GMT - 6)%24;
		date = System.DateTime.Now;

	}

	void OnGUI(){




		GUILayout.BeginArea(new Rect(Screen.width/2-100, 0, 200,60));
		GUILayout.Label(string.Format("{0:dd-MM-yyyy}  {1:D2}:{2:D2}:{3:D2}",date, hrs,min,sec),customSkin.customStyles[2]);

		GUILayout.EndArea();
	}
}
