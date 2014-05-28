using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.SequencerCommands;
using PixelCrushers.DialogueSystem.Examples;

public class NPCsync: MonoBehaviour {

	public PlayMakerFSM targetFSM;

	private GameObject target_usable;
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
		print ("send conversation");
		
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
	public void safeOtherLockRPC(){
		print (" I locked, left others");

		PhotonView photonView = this.gameObject.GetPhotonView ();
		photonView.RPC ("OtherLockRPC",PhotonTargets.OthersBuffered);
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

			print (gameObject.name);

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

	[RPC]
	public void OtherLockRPC()
	{
		DialogueLua.SetVariable("OtherUnlocked",false);
	}

	public void addUsable(GameObject target){


		target_usable= target;

		StartCoroutine(WaitForAddUsable());
	}


	IEnumerator WaitForAddUsable()
	{
		if(target_usable.GetComponent<Usable>())
			Destroy(target_usable.GetComponent<Usable>());

		yield return new WaitForSeconds(1);

		if(target_usable.GetComponent<Usable>()==null)
		target_usable.AddComponent<Usable>();
	}

}
