using UnityEngine;
using System.Collections;

public class NPCsync: MonoBehaviour {

	public PlayMakerFSM targetFSM;


	// Use this for initialization
	void Start () {
	



	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void sendRPC(string syncEvent)
	{

		PhotonView photonView = this.gameObject.GetPhotonView();
		
		if(photonView == null)
			print ("no photonView on" + this.gameObject.name);
		else
		{
		photonView.RPC ("syncNPCstate",PhotonTargets.OthersBuffered,syncEvent);
		print ("111");
		}
		
	}
	 
	
	[RPC]
	void syncNPCstate(string myEvent){


		targetFSM.Fsm.Event(myEvent);
		
	}
}
