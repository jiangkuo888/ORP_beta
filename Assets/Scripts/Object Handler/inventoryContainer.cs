using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem.UnityGUI;
using PixelCrushers.DialogueSystem;


public class inventoryContainer : MonoBehaviour {
	
	public float x_offset;
	public float y_offset;
	public float scaleX;
	public float scaleY;

	public bool enabled;

	
	private GUITexture myGUITexture;

	public Texture normal;
	public Texture hover;






	void Awake()
	{

		enabled = true;

		this.gameObject.GetComponent<GUITexture>().texture = normal;




		myGUITexture = this.gameObject.GetComponent("GUITexture") as GUITexture;
	
		
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
			scaledHeight = screenHeight*scaleY;
			scaledWidth = screenWidth*scaleX;
		}
		else
		{
			
			// The scaled size is based on the width
			scaledWidth = screenWidth*scaleX;
			scaledHeight = screenHeight*scaleY;
		}
		float xPosition = screenWidth / 2 * x_offset - scaledWidth;
		float yPosition = screenHeight / 2 * y_offset - scaledHeight;
		
		myGUITexture.pixelInset = 
			new Rect(xPosition, yPosition, 
			         scaledWidth, scaledHeight);
	}

	void Update(){



	}
	
	void OnMouseEnter(){
		
		GameObject.Find(PhotonNetwork.playerName).GetComponent<DetectObjects>().enabled = false;
		GameObject.Find(PhotonNetwork.playerName).GetComponent<ClickMove>().enabled = false;
		GameObject.Find(PhotonNetwork.playerName).GetComponent<CharacterMotor>().inputMoveDirection = Vector3.zero;

		myGUITexture.texture = normal;
		
	}

	void OnMouseExit(){
		myGUITexture.texture = normal;
		GameObject.Find(PhotonNetwork.playerName).GetComponent<DetectObjects>().enabled = true;
		GameObject.Find(PhotonNetwork.playerName).GetComponent<ClickMove>().enabled = true;
		GameObject.Find(PhotonNetwork.playerName).GetComponent<CharacterMotor>().inputMoveDirection = Vector3.zero;
	}
	
	void OnMouseDown(){

		myGUITexture.texture = hover;


		GameObject.Find ("InventoryObj").GetComponent<inventory>().Drop();

		
	}
	
	void OnMouseUpAsButton (){

		
		myGUITexture.texture = normal;
	}








	
	
	
}
