// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class InventoryNew : MonoBehaviour {
	//This is the central piece of the Inventory System.
	
	public Transform[] Contents; //The content of the Inventory
	public int MaxContent = 12; //The maximum number of items the Player can carry.
	
	bool DebugMode= false; //If this is turned on the Inventory script will output the base of what it's doing to the Console window.
	public bool isFull = false;
	//private Component playersInvDisplay; //Keep track of the InventoryDisplayCSharp script.

	public static Transform itemHolderObject; //The object the unactive items are going to be parented to. In most cases this is going to be the Inventory object itself.
	
	//@script AddComponentMenu ("Inventory/Inventory")
		
		//Handle components and assign the itemHolderObject.
	void  Awake (){
		itemHolderObject = gameObject.transform;
		

		if (this.GetComponent<InventoryDisplayCSharp>() == null)
		{
			Debug.LogError("No Inventory Display script was found on " + transform.name + " but an Inventory script was.");
			Debug.LogError("Unless a Inventory Display script is added the Inventory won't show. Add it to the same gameobject as the Inventory for maximum performance");
		}
	}


	void Update()
	{

		if(Contents.Length >= MaxContent)
			isFull = true;
		else
			isFull = false;





	}
	//Add an item to the inventory.
	public void  AddItem ( Transform Item  ){
		System.Collections.Generic.List<Transform> list = new System.Collections.Generic.List<Transform>(Contents);
		list.Add(Item);
		Contents = list.ToArray();

		
		if (DebugMode)
		{
			Debug.Log(Item.name+" has been added to inventroy");
		}

		//Tell the InventoryDisplayCSharp to update the list.
		if (this.GetComponent<InventoryDisplayCSharp>() != null)
		{
			this.GetComponent<InventoryDisplayCSharp>().UpdateInventoryList();
		}
	}
	
	//Removed an item from the inventory (IT DOESN'T DROP IT).
	public void RemoveItem ( Transform Item  ){
		System.Collections.Generic.List<Transform> newContents = new System.Collections.Generic.List<Transform>(Contents);

		int index=0;
		bool shouldend=false;

		foreach(Transform i in newContents) //Loop through the Items in the Inventory:
		{
			if(i == Item) //When a match is found, remove the Item.
			{
				newContents.RemoveAt(index);
				shouldend=true;
				//No need to continue running through the loop since we found our item.
			}
			index++;
			
			if(shouldend) //Exit the loop
			{
				Contents=newContents.ToArray();
				if (DebugMode)
				{
					Debug.Log(Item.name+" has been removed from inventroy");
				}
				if (this.GetComponent<InventoryDisplayCSharp>() != null)
				{
					this.GetComponent<InventoryDisplayCSharp>().UpdateInventoryList();
				}
				return;
			}
		}
	}
	
	//Dropping an Item from the Inventory
	public void  DropItem (Transform item){

		print ("droping");
		GameObject inventoryObject = item.gameObject;

		
		//item.DropMeFromThePlayer(makeDuplicate); //Calling the drop function + telling it if the object is stacked or not.
		
		if(inventoryObject != null&& inventoryObject.GetComponent<Collider>()!=null && inventoryObject.tag != "document")
		{
			// Distance from your player    
			//float distance   = 3;     
			
			// Transforms a forward position relative to your player into the world space  
			
			RemoveItem(item.transform);
			
			
			Transform player = GameObject.Find (PhotonNetwork.playerName).transform;
			
			
			Vector3 throwPos = player.position +Camera.main.transform.forward;
			
			//Vector3 throwPos = new Vector3(GameObject.Find ("ClickArrow(Clone)").transform.position.x,GameObject.Find ("ClickArrow(Clone)").transform.position.y+ 1f,GameObject.Find ("ClickArrow(Clone)").transform.position.z);
			if(GameObject.Find ("DropArea_"+inventoryObject.name))
				GameObject.Find ("DropArea_"+inventoryObject.name).GetComponent<DropAreaController>().AreaActivated = false;


			Vector3 originalScale = inventoryObject.transform.localScale;

			inventoryObject.transform.parent = null;
			inventoryObject.transform.position = throwPos;
			inventoryObject.transform.localScale = originalScale;




			//inventoryObject.transform.localScale = inventoryObjectOriginalScale;
			
			if(inventoryObject.GetComponent<Rigidbody>() == null)
				inventoryObject.AddComponent<Rigidbody> ();
			
			enableCollider (inventoryObject);
			enableRender (inventoryObject);
			
			PhotonView photonView = inventoryObject.GetPhotonView();
			
			photonView.RPC ("enableRenderer",PhotonTargets.AllBuffered);
			photonView.RPC ("enableCollider",PhotonTargets.AllBuffered);
			photonView.RPC ("enableRigidbody",PhotonTargets.AllBuffered);
			photonView.RPC ("updateAllInfo",PhotonTargets.AllBuffered,throwPos);
			
			
			
		
			
		}
		else {
			if(DebugMode)
			print ("nothing to drop");
			
		}
	}

	void enableCollider(GameObject obj){
		
		
		Collider collider = obj.GetComponent<Collider> ();
		
		if (collider.enabled == false)
			collider.enabled = true;
		
		
		
		Collider[] colliders = obj.GetComponentsInChildren<Collider>();
		
		foreach ( Collider c in colliders)
		{
			if(c.enabled == false)
				c.enabled = true;
		}
		
	}
	void enableRender(GameObject obj)
	{
		
		
		Renderer renderer = obj.GetComponent<Renderer> ();
		
		if (renderer.enabled == false)
			renderer.enabled = true;
		
		
		Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
		
		foreach ( Renderer r in renderers)
		{
			if(r.enabled == false)
				r.enabled = true;
		}
	}
	public bool alreadyHave(Transform Item){


		System.Collections.Generic.List<Transform> newContents = new System.Collections.Generic.List<Transform>(Contents);
		


		
		foreach(Transform i in newContents) //Loop through the Items in the Inventory:
		{
			if(i == Item) //When a match is found, remove the Item.
			{
				return true;
				//No need to continue running through the loop since we found our item.
			}
		}


		return false;
	}
	
	//This will tell you everything that is in the inventory.
//	void  DebugInfo (){
//		Debug.Log("Inventory Debug - Contents");
//		items=0;
//		foreach(Transform i in Contents){
//			items++;
//			Debug.Log(i.name);
//		}
//		Debug.Log("Inventory contains "+items+" Item(s)");
//	}
	
	//Drawing an 'S' in the scene view on top of the object the Inventory is attached to stay organized.
//	void  OnDrawGizmos (){
//		Gizmos.DrawIcon (Vector3(transform.position.x, transform.position.y + 2.3f, transform.position.z), "InventoryGizmo.png", true);
//	}
}