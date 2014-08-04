using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem.UnityGUI;


public class questLog : MonoBehaviour {

	public float x_offset;
	public float y_offset;
	public GameObject Inventory;

	private GUITexture myGUITexture;
	
	void Awake()
	{
		myGUITexture = this.gameObject.GetComponent("GUITexture") as GUITexture;
//		QuestLogWindow.GetComponent<UnityQuestLogWindow>().Open();

	}
	
	// Use this for initialization
	void Start()
	{
		// Position the billboard in the center, 
		// but respect the picture aspect ratio
		float textureHeight = myGUITexture.texture.height;
		float textureWidth = myGUITexture.texture.width;
		float screenHeight = Screen.height;
		float screenWidth = Screen.width;
		
		float screenAspectRatio = (screenWidth / screenHeight);
		float textureAspectRatio = (textureWidth / textureHeight) ;
		
		float scaledHeight;
		float scaledWidth;


//		print(textureAspectRatio);
		if (textureAspectRatio <= screenAspectRatio)
		{
			// The scaled size is based on the height
			scaledHeight = screenHeight/10;
			scaledWidth = (scaledHeight * textureAspectRatio);
		}
		else
		{

			// The scaled size is based on the width
			scaledWidth = screenWidth/10;
			scaledHeight = (scaledWidth / textureAspectRatio);
		}
		float xPosition = screenWidth / 2 * x_offset - scaledWidth;
		float yPosition = screenHeight / 2 * y_offset - scaledHeight;

		myGUITexture.pixelInset = 
			new Rect(xPosition, yPosition, 
			         scaledWidth, scaledHeight);
	}

	void OnMouseOver(){

		if(GameObject.Find(PhotonNetwork.playerName))
		{
		GameObject.Find(PhotonNetwork.playerName).GetComponent<DetectObjects>().enabled = false;
		GameObject.Find(PhotonNetwork.playerName).GetComponent<ClickMove>().OnGUI = true;	
		}
		else
			if(GameObject.Find(GameObject.Find ("GameManager").GetComponent<GameManagerVik>().characterName))
		{
			GameObject.Find(GameObject.Find ("GameManager").GetComponent<GameManagerVik>().characterName).GetComponent<DetectObjects>().enabled = false;
			GameObject.Find(GameObject.Find ("GameManager").GetComponent<GameManagerVik>().characterName).GetComponent<ClickMove>().OnGUI = true;	
		}
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);


	}
	void OnMouseExit(){
		if(GameObject.Find(PhotonNetwork.playerName))
		{
		GameObject.Find(PhotonNetwork.playerName).GetComponent<DetectObjects>().enabled = true;
		GameObject.Find(PhotonNetwork.playerName).GetComponent<ClickMove>().OnGUI = false;	
		}

		else
			if(GameObject.Find(GameObject.Find ("GameManager").GetComponent<GameManagerVik>().characterName))
		{
			GameObject.Find(GameObject.Find ("GameManager").GetComponent<GameManagerVik>().characterName).GetComponent<DetectObjects>().enabled = true;
			GameObject.Find(GameObject.Find ("GameManager").GetComponent<GameManagerVik>().characterName).GetComponent<ClickMove>().OnGUI = false;	
		}

	}
	void OnMouseDown(){

		Inventory.GetComponent<InventoryDisplayCSharp>().toggle();


	}





}
