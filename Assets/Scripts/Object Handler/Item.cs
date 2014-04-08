// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
	public Texture2D itemIcon; //The Icon.
	bool canGet= true; //If we can pick up the Item.
	//public string itemType; //This will let us equip the item to specific slots. Ex: Head, Shoulder, or whatever we set up. If the item is equipment (or weapon) this needs to match a slot to work properly.
	//FIXME_VAR_TYPE stackable= false; //Is it stackable? If yes then items with the same itemType will be stacked.
	//FIXME_VAR_TYPE maxStack= 20; //How many Items each stack can have before creating a new one. Remember that the Items that should be stacked should have the same itemType.
	//FIXME_VAR_TYPE stack= 1; //This is how many stack counts this Item will take up.
	//FIXME_VAR_TYPE isEquipment= true; //Can the Item be equipped? This includes weapons.
	//FIXME_VAR_TYPE isAlsoWeapon= false; //Is the Item also a Weapon? This only works with isEquipment set to true.
	
	//This is the object we will instantiate in the Players hand.
	//We use this so we can have two versions of the weapon. One for picking up and one for using.
	//Transform equippedWeaponVersion;
	
	//These will store information about usefull components.
	//static Inventory playersinv;
	
	//private FIXME_VAR_TYPE FPPickUpFound= false;
	
	//@script AddComponentMenu ("Inventory/Items/Item")
		
		//Here we find the components we need.
	void  Awake (){
		//playersinv = FindObjectOfType(typeof(Inventory)); //finding the players inv.
//		if (playersinv == null)
//		{
//			canGet = false;
//			Debug.LogWarning("No 'Inventory' found in game. The Item " + transform.name + " has been disabled for pickup (canGet = false).");
//		}
//		else
//		{
//			gameObject.SendMessage("RetrievePlayer", playersinv, SendMessageOptions.DontRequireReceiver);
//		}
//		
//		if (isEquipment == false && GetComponent<ItemEffect>() == null)
//		{
//			Debug.LogError(gameObject.name + " is not equipment so please assign an ItemEffect script to it");
//		}
//		
//		if (GetComponent<FirstPersonPickUp>() != null)
//		{
//			FPPickUpFound = true;
//		}
//		else if (transform.GetComponentInChildren<FirstPersonPickUp>() != null)
//		{
//			FPPickUpFound = true;
//		}
	}
	
	//When you click an item
	//void  OnMouseDown (){
		//If the 'FirstPersonPickUp' script is not attached we want to pick up the item.
//		if (FPPickUpFound == false)
//		{
//			PickUpItem();
//		}
	//}
	
	//Picking up the Item.
	void  PickUpItem (){
		bool getit=true;
		if(canGet){//if its getable or hasnt been gotten.
			
			//playersinv.gameObject.SendMessage ("PlayPickUpSound", SendMessageOptions.DontRequireReceiver); //Play sound
			
//			if(stackable){
//				Item locatedit;
//				foreach(Transform t in playersinv.Contents){
//					if(t.name==this.transform.name){//if the item we wanna stack this on has the same name
//						Item i=t.GetComponent<Item>();
//						if(i.stack<i.maxStack){
//							locatedit=i;
//						}
//					}
//				}
//				if(locatedit!=null){//if we have a stack to stack it to!
//					getit=false;
//					locatedit.stack+=1;
//					Destroy(this.gameObject);
//				}
//				else{
//					getit=true;
//				}
//			}
			//If we can get it and the inventory isn't full.
			GameObject playersinv = GameObject.Find ("Inventory").gameObject;


			if (getit && playersinv.GetComponent<InventoryNew>().Contents.Length < playersinv.GetComponent<InventoryNew>().MaxContent)
			{
				playersinv.GetComponent<InventoryNew>().AddItem(this.transform);
				//MoveMeToThePlayer(playersinv.GetComponent<InventoryNew>().itemHolderObject);//moves the object, to the player
			}
			else if (playersinv.GetComponent<InventoryNew>().Contents.Length >= playersinv.GetComponent<InventoryNew>().MaxContent)
			{
				Debug.Log("Inventory is full");
			}
		}
	}
	
	//Moves the item to the Players 'itemHolderObject' and disables it. In most cases this will just be the Inventory object.
	void  MoveMeToThePlayer ( Transform itemHolderObject  ){
//		canGet = false;
//		
//		//gameObject.SetActive(false);	It's normally best to disable the individual components so we can keep item effects and update functions alive.
//		
//		if (GetComponent<MeshRenderer>() != null)
//		{
//			GetComponent<MeshRenderer>().enabled = false;
//		}
//		
//		if (GetComponent<Collider>() != null)
//		{
//			GetComponent<Collider>().enabled = false;
//		}
//		
//		GetComponent<"Item">().enabled = false;
//		
//		transform.parent = itemHolderObject;
//		transform.localPosition = Vector3.zero;
	}
	
	//Drops the Item from the Inventory.
//	void  DropMeFromThePlayer ( bool makeDuplicate  ){
//		if (makeDuplicate == false) //We use this if the object is not stacked and so we can just drop it.
//		{
//			canGet = true;
//			gameObject.SetActive(true);
//			
//			if (GetComponent<MeshRenderer>() != null)
//			{
//				GetComponent<MeshRenderer>().enabled = true;
//			}
//			
//			if (GetComponent<Collider>() != null)
//			{
//				GetComponent<Collider>().enabled = true;
//			}
//			
//			GetComponent<"Item">().enabled = true;
//			
//			transform.parent = null;
//			DelayPhysics();
//		}
//		else //If the object is stacked we need to make a clone of it and drop the clone instead.
//		{
//			canGet = true;
//			clone = Instantiate(gameObject, transform.position, transform.rotation);
//			canGet = false;
//			clone.SetActive(true);
//			
//			if (clone.GetComponent<MeshRenderer>() != null)
//			{
//				clone.GetComponent<MeshRenderer>().enabled = true;
//			}
//			
//			if (clone.GetComponent<Collider>() != null)
//			{
//				clone.GetComponent<Collider>().enabled = true;
//			}
//			
//			clone.GetComponent<"Item">().enabled = true;
//			
//			clone.transform.parent = null;
//			clone.name = gameObject.name;
//		}
//	}
//	
//	void  DelayPhysics (){
//		if (playersinv.transform.parent.collider != null && collider != null)
//		{
//			Physics.IgnoreCollision(playersinv.transform.parent.collider, collider, true);
//			yield WaitForSeconds (1);
//			Physics.IgnoreCollision(playersinv.transform.parent.collider, collider, false);
//		}
//	}
//	
//	//Drawing an 'I' icon on top of the Item in the scene to keep organised.
//	void  OnDrawGizmos (){
//		Gizmos.DrawIcon (Vector3(transform.position.x, transform.position.y + 1, transform.position.z), "ItemGizmo.png", true);
//	}
}