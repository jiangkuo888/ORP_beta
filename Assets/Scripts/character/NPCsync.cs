using UnityEngine;
using System.Collections;

public class NPCsync : MonoBehaviour {


	public string syncEvent;
	public PlayMakerFSM myFSM;
	// Use this for initialization
	void Start () {
	
		PhotonView photonView = this.gameObject.GetPhotonView();
		
		
		photonView.RPC ("syncNPCstate",PhotonTargets.OthersBuffered);


	}
	
	// Update is called once per frame
	void Update () {
	
	}


	 
	
	[RPC]
	void syncNPCstate(){
		print ("111");
		myFSM.Fsm.Event(syncEvent);
		
	}
}
