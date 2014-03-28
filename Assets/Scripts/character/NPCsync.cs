using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.SequencerCommands;

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
	 
	public void sendMessageRPC(string senderName,string receiverName,string messageName)
	{

		Debug.Log ("Sending RPC...");
		PhotonView photonView = this.gameObject.GetPhotonView();
		photonView.RPC ("SendMessage",PhotonTargets.OthersBuffered,senderName,receiverName,messageName);
		Debug.Log (senderName);
		Debug.Log (receiverName);
		Debug.Log (messageName);
	}


	public void safeOtherUnlockRPC(){
		print (" me unlocked, tell others");



		PhotonView photonView = this.gameObject.GetPhotonView ();
		photonView.RPC ("OtherUnlockRPC",PhotonTargets.OthersBuffered);


	}



	[RPC]
	void syncNPCstate(string myEvent){


		targetFSM.Fsm.Event(myEvent);
		
	}


	[RPC]
	public void SendMessage(string sender,string receiver,string message)
	{
		
		
		if(PhotonNetwork.playerName == receiver)
		{
			
			print ("RPC received "+sender+receiver+message);
			// show inbox red dot
			GameObject.Find ("RedDot").GetComponent<GUIresponsive>().addRedDot();

			
			// set conversation number
			GameObject.Find ("EmailIcon").GetComponent<phoneShowInbox>().enableReceiverInboxMessage(message);
			
			// start inbox conversation
			//GameObject.Find ("EmailIcon").GetComponent<phoneShowInbox>().startConversation();
			
			
			
			
			// enable response conversation
			
			
			
			
		}
		
		
	}

	[RPC]
	public void OtherUnlockRPC()
	{
		DialogueLua.SetVariable("OtherUnlocked",true);

	}

}
