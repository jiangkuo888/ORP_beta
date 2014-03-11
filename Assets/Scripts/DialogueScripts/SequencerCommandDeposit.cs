



using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.SequencerCommands;

public class SequencerCommandDeposit : SequencerCommand {

	private GameObject actor;
	private GameObject onUsedObj;

	public void Start() {
		// Add your initialization code here. You can use the GetParameter***() and GetSubject()
		// functions to get information from the command's parameters. You can also use the
		// Sequencer property to access the SequencerCamera, CameraAngle, and other properties
		// on the sequencer.
//		if(PlayerPrefs.GetString("LocalActor") != null)
//			actor = GameObject.Find(PlayerPrefs.GetString("LocalActor"));
//
//		if(PlayerPrefs.GetString("OnUsedObj") != null)
//			onUsedObj = GameObject.Find (PlayerPrefs.GetString("OnUsedObj"));
		if(GameObject.Find ("InventoryObj").GetComponent<inventory>().inventoryObject != null)
		{
			GameObject.Find ("Dialogue Manager").GetComponent<DialogueSystemController>().ShowAlert("Deposit "+ GameObject.Find ("InventoryObj").GetComponent<inventory>().inventoryObject.name +" successful");

			GameObject.Find ("InventoryObj").GetComponent<inventory>().clearInventory();

			DialogueLua.SetVariable("Has_Document",false);
			}



	}
	
	public void Update() {


		//set lock to false so others can interact with this object
		//transform.GetComponent<DetectObjects>().photonView.RPC("setInteractLock",PhotonTargets.AllBuffered, false);
		
		//delete the useless script generated by dunilog

		

//
//		if( GameObject.Find("InventoryObj").GetComponent<inventory>().inventoryObject == null)
//		{
//			PhotonView photonView = onUsedObj.GetPhotonView();
//			
//			photonView.RPC("disableRenderer",PhotonTargets.AllBuffered);
//			photonView.RPC("disableCollider",PhotonTargets.AllBuffered);
//			photonView.RPC("disableRigidbody",PhotonTargets.AllBuffered);
//			
//			
//			moveToPlayerPosition(onUsedObj.name);
//			disableRender(onUsedObj.name);
//			disableCollider(onUsedObj.name);
//			disableRigidbody(onUsedObj.name);
//			enableInventory(onUsedObj);
//		}
		Stop ();
		// Add your update code here. When the command is done, call Stop().

	}



	
	public void OnDestroy() {
		// Add your finalization code here. This is critical. If the sequence is cancelled and this
		// command is marked as "required", then only Start() and OnDestroy() will be called.

	}

	void moveToPlayerPosition(string objName){
		
		GameObject obj  = GameObject.Find (objName);
		
		GameObject.Find ("InventoryObj").GetComponent<inventory>().inventoryObjectOriginalScale = obj.transform.lossyScale;
		
		obj.transform.position = actor.transform.position;
		obj.transform.parent = actor.transform;
		
	}
	
	GameObject findGrabObjWithTag( Transform parent){
		
		if (parent.tag == "pickable")
			return parent.gameObject;
		else foreach (Transform child in parent){
			if(child.gameObject.tag == "pickable"){
				print (child.gameObject.name);
				return child.gameObject;
			}
		}
		
		return null;
		
	}
	
	
	
	void disableCollider(string objName){
		
		GameObject obj  = GameObject.Find (objName);
		Collider[] colliders = obj.GetComponentsInChildren<Collider>();
		
		foreach ( Collider c in colliders)
		{
			if(c.enabled == true)
				c.enabled = false;
		}
		
	}
	
	
	void disableRender(string objName)
	{
		
		GameObject obj  = GameObject.Find (objName);
		Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
		
		foreach ( Renderer r in renderers)
		{
			if(r.enabled == true)
				r.enabled = false;
		}
	}
	
	
	void disableRigidbody(string objName)
	{
		
		GameObject obj  = GameObject.Find (objName);
		if (obj.GetComponent<Rigidbody>()!=null)
			Destroy(obj.GetComponent<Rigidbody>());
	}
	void enableInventory(GameObject obj){
		
		GameObject.Find ("InventoryObj").GetComponent<GUITexture>().enabled = true;



		GameObject.Find ("InventoryObj").GetComponent<inventory>().updateInventoryObject(obj);
		
	}
	
}
 
 

/**/