using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem.UnityGUI;
using PixelCrushers.DialogueSystem;


public class phoneButton : MonoBehaviour {
	
	public float x_offset;
	public float y_offset;
	public bool enabled;
	public bool OnCalling;
	
	private GUITexture myGUITexture;

	public Texture normal;
	public Texture hover;
	public Texture down;
	public Texture isOnCall;


	string conversationName;
	string playerName;


	void Awake()
	{

		enabled = true;

		this.gameObject.GetComponent<GUITexture>().texture = normal;




		myGUITexture = this.gameObject.GetComponent("GUITexture") as GUITexture;
	
		
	}
	
	// Use this for initialization
	void Start()
	{

		OnCalling = false;
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

	void Update(){



	}
	
	void OnMouseEnter(){
		
		
		GameObject.Find(PhotonNetwork.playerName).GetComponent<DetectObjects>().enabled = false;
		GameObject.Find(PhotonNetwork.playerName).GetComponent<ClickMove>().enabled = false;
		GameObject.Find(PhotonNetwork.playerName).GetComponent<CharacterMotor>().inputMoveDirection = Vector3.zero;
		myGUITexture.texture = hover;
		
	}
	
	void OnMouseDown(){
		myGUITexture.texture = down;

		if(OnCalling)
		{
			if(PhotonNetwork.playerName == playerName)
				GameObject.Find("Dialogue Manager").GetComponent<DialogueSystemController>().StartConversation(conversationName,GameObject.Find (PhotonNetwork.playerName).transform);

			OnCalling = false;
		}
		else
		{
		GameObject.Find ("phoneSmallButton1").GetComponent<GUITexture>().enabled = true;
		GameObject.Find ("phoneSmallButton2").GetComponent<GUITexture>().enabled = true;
		GameObject.Find ("phoneSmallButton3").GetComponent<GUITexture>().enabled = true;
		}
		
	}
	
	void OnMouseUpAsButton (){

		
		myGUITexture.texture = hover;
	}
	
	void OnMouseExit(){
		GameObject.Find(PhotonNetwork.playerName).GetComponent<DetectObjects>().enabled = true;
		GameObject.Find(PhotonNetwork.playerName).GetComponent<ClickMove>().enabled = true;	


		myGUITexture.texture = normal;
	}


	public void OnCall(string player, string conversation){
		OnCalling = true;
		playerName = player;
		conversationName = conversation;

		myGUITexture.texture = isOnCall;

	}



	
	
	
}
