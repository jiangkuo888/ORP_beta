// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class InventoryDisplayCSharp : MonoBehaviour {
	//Displaying the Inventory.
	
	//Variables for dragging:
	//private Component itemBeingDragged; //This refers to the 'Item' script when dragging.
	//private Vector2 draggedItemPosition; //Where on the screen we are dragging our Item.
	//private Vector2 draggedItemSize;//The size of the item icon we are dragging.

	//Variables for the window:
	public Vector2 windowSize = new Vector2(375, 162.5f); //The size of the Inventory window.
	public bool useCustomPosition= false; //Do we want to use the customPosition variable to define where on the screen the Inventory window will appear?
	public Vector2 customPositionFromRightBottom = new Vector2 (70, 400); // The custom position of the Inventory window.
	public Vector2 itemIconSize = new Vector2(60.0f, 60.0f); //The size of the item icons.
	
	//Variables for updating the inventory
	int updateListDelay= 9999;//This can be used to update the Inventory with a certain delay rather than updating it every time the OnGUI is called.
	//This is only useful if you are expanding on the Inventory System cause by default Inventory has a system for only updating when needed (when an item is added or removed).
	private float lastUpdate= 0.0f; //Last time we updated the display.
	private Transform[] UpdatedList; //The updated inventory array.

	//More variables for the window:
	static bool displayInventory= false; //If inv is opened.
	private Rect windowRect= new Rect(200,200,108,130); //Keeping track of the Inventory window.
	public GUISkin invSkin; //This is where you can add a custom GUI skin or use the one included (InventorySkin) under the Resources folder.
	public Vector2 Offset = new Vector2 (7, 12); //This will leave so many pixels between the edge of the window (x = horizontal and y = vertical).
	public bool canBeDragged= false; //Can the Inventory window be dragged?
	
	public KeyCode onOffButton = KeyCode.I; //The button that turns the Inventory window on and off.
	
	//Keeping track of components.
	private GameObject associatedInventory;
	private bool cSheetFound= false;

	public GameObject questWindow;
	//private Character cSheet;
	
	//@script AddComponentMenu ("Inventory/Inventory Display")
	//	@script RequireComponent(Inventory)
			
			//Store components and adjust the window position.
	void  Awake (){
		if (useCustomPosition == false)
		{
			windowRect= new Rect(Screen.width-windowSize.x-70,Screen.height-windowSize.y-70,windowSize.x,windowSize.y);
		}
		else
		{
			windowRect = new Rect(Screen.width - windowSize.x - customPositionFromRightBottom.x, Screen.height - windowSize.y - customPositionFromRightBottom.y, windowSize.x, windowSize.y);
		}
		associatedInventory = GameObject.Find ("Inventory").gameObject;//keepin track of the inventory script
//		if (GetComponent<Character>() != null)
//		{
//			cSheetFound = true;
//			cSheet = GetComponent<Character>();
//		}
//		else
//		{
//			//Debug.LogError ("No Character script was found on this object. Attaching one allows for functionality such as equipping items.");
//			cSheetFound = false;
//		}
	}
	
	//Update the inv list
	public void  UpdateInventoryList (){

		UpdatedList = associatedInventory.GetComponent<InventoryNew>().Contents;
		//Debug.Log("Inventory Updated");
	}

	public void toggle(){


			if (displayInventory)
			{

				questWindow.GetComponent<questLogDisplay>().open();
				
				displayInventory = false;
				
				gameObject.SendMessage ("ChangedState", false, SendMessageOptions.DontRequireReceiver);
			//	gameObject.SendMessage("PauseGame", false, SendMessageOptions.DontRequireReceiver); //StopPauseGame/EnableMouse/ShowMouse
			}
			else
			{
				questWindow.GetComponent<questLogDisplay>().close();

				displayInventory = true;
				
				gameObject.SendMessage ("ChangedState", true, SendMessageOptions.DontRequireReceiver);
			//	gameObject.SendMessage("PauseGame", true, SendMessageOptions.DontRequireReceiver); //PauseGame/DisableMouse/HideMouse
			}

	}

	public void Open(){
		if (!displayInventory)
		{
			
			questWindow.GetComponent<questLogDisplay>().open();
			
			displayInventory = true;
			
			gameObject.SendMessage ("ChangedState", true, SendMessageOptions.DontRequireReceiver);
			//	gameObject.SendMessage("PauseGame", false, SendMessageOptions.DontRequireReceiver); //StopPauseGame/EnableMouse/ShowMouse
		}

	}

	public void Close(){
		if (displayInventory)
		{
			
			questWindow.GetComponent<questLogDisplay>().close();
			
			displayInventory = false;
			
			gameObject.SendMessage ("ChangedState", false, SendMessageOptions.DontRequireReceiver);
			//	gameObject.SendMessage("PauseGame", false, SendMessageOptions.DontRequireReceiver); //StopPauseGame/EnableMouse/ShowMouse
		}

	}



	void  Update (){
//		if(Input.GetKeyDown(KeyCode.Escape)) //Pressed escape
//		{
//			ClearDraggedItem(); //Get rid of the dragged item.
//		}
//		if(Input.GetMouseButtonDown(1)) //Pressed right mouse
//		{
//			ClearDraggedItem(); //Get rid of the dragged item.
//		}
		
		//Turn the Inventory on and off and handle audio + pausing the game.
		if(Input.GetKeyDown(onOffButton))
		{
			if (displayInventory)
			{
				displayInventory = false;
				
				gameObject.SendMessage ("ChangedState", false, SendMessageOptions.DontRequireReceiver);
			//	gameObject.SendMessage("PauseGame", false, SendMessageOptions.DontRequireReceiver); //StopPauseGame/EnableMouse/ShowMouse
			}
			else
			{
				displayInventory = true;
				
				gameObject.SendMessage ("ChangedState", true, SendMessageOptions.DontRequireReceiver);
			//	gameObject.SendMessage("PauseGame", true, SendMessageOptions.DontRequireReceiver); //PauseGame/DisableMouse/HideMouse
			}
		}
		
		//Making the dragged icon update its position
//		if(itemBeingDragged!=null)
//		{
//			//Give it a 15 pixel space from the mouse pointer to allow the Player to click stuff and not hit the button we are dragging.
//			draggedItemPosition.y=Screen.height-Input.mousePosition.y+15;
//			draggedItemPosition.x=Input.mousePosition.x+15;
//		}
		
		//Updating the list by delay
		if(Time.time>lastUpdate){
			lastUpdate=Time.time+updateListDelay;
			UpdateInventoryList();
		}
	}
	
	//Drawing the Inventory window
	void  OnGUI (){
		GUI.skin = invSkin; //Use the invSkin
//		if(itemBeingDragged != null) //If we are dragging an Item, draw the button on top:
//		{
//			GUI.depth = 3;
//			GUI.Button( new Rect(draggedItemPosition.x,draggedItemPosition.y,draggedItemSize.x,draggedItemSize.y),itemBeingDragged.itemIcon);
//			GUI.depth = 0;
//		}
		
		//If the inventory is opened up we create the Inventory window:
		if(displayInventory)
		{
			windowRect = GUI.Window (0, windowRect, DisplayInventoryWindow, "Inventory");
		}
	}
	
	//Setting up the Inventory window
	public void  DisplayInventoryWindow ( int windowID  ){
		
		if (canBeDragged == true)
		{
			GUI.DragWindow ( new Rect(0,0, 10000, 30));  //the window to be able to be dragged
		}
		
		float currentX= 0 + Offset.x; //Where to put the first items.
		float currentY= 18 + Offset.y; //Im setting the start y position to 18 to give room for the title bar on the window.
		
		foreach(Transform i in UpdatedList) //Start a loop for whats in our list.
		{

			if (cSheetFound) //CSheet was found (recommended)
			{
				if(GUI.Button( new Rect(currentX,currentY,itemIconSize.x,itemIconSize.y),i.GetComponent<Item>().itemIcon))
				{

					if (Event.current.button == 0) //Check to see if it was a left click
					{


//						if(dragitem)
//						{
//							if (item.isEquipment == true) //If it's equipment
//							{
//								itemBeingDragged = item; //Set the item being dragged.
//								draggedItemSize=itemIconSize; //We set the dragged icon size to our item button size.
//								//We set the position:
//								draggedItemPosition.y=Screen.height-Input.mousePosition.y-15;
//								draggedItemPosition.x=Input.mousePosition.x+15;
//							}
//							else
//							{
//								i.GetComponent<ItemEffect>().UseEffect(); //It's not equipment so we just use the effect.
//							}
//						}
					}
					else if (Event.current.button == 1) //If it was a right click.
					{
						print ("right clicked");
						associatedInventory.GetComponent<InventoryNew>().DropItem(i);
					}
				}
			}
			else //No CSheet was found (not recommended)
			{
				if(GUI.Button( new Rect(currentX,currentY,itemIconSize.x,itemIconSize.y),i.GetComponent<Item>().itemIcon))
				{
					if (Event.current.button == 1) //If it was a right click we want to drop the item.
					{
//						print ("right clicked");
						associatedInventory.GetComponent<InventoryNew>().DropItem(i);
						Close ();
					}
				}
			}
			
//			if(item.stackable) //If the item can be stacked:
//			{
//				GUI.Label( new Rect(currentX, currentY, itemIconSize.x, itemIconSize.y), "" + item.stack, "Stacks"); //Showing the number (if stacked).
//			}
			
			currentX += itemIconSize.x;
			if(currentX + itemIconSize.x + Offset.x > windowSize.x) //Make new row
			{
				currentX=Offset.x; //Move it back to its startpoint wich is 0 + offsetX.
				currentY+=itemIconSize.y; //Move it down a row.
				if(currentY + itemIconSize.y + Offset.y > windowSize.y) //If there are no more room for rows we exit the loop.
				{
					return;
				}
			}
		}
	}
	
	//If we are dragging an item, we will clear it.
//	void  ClearDraggedItem (){
//		itemBeingDragged=null;
//	}
}