using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem.UnityGUI;
using PixelCrushers.DialogueSystem;


public class phoneShowPlayerButton : MonoBehaviour {
	
	public float x_offset;
	public float y_offset;
	
	
	private GUITexture myGUITexture;
	
	public Texture normal;
	public Texture hover;
	public Texture down;
	
	public Texture SM;
	public Texture SM_hover;
	public Texture SM_down;

	public Texture LO;
	public Texture LO_hover;
	public Texture LO_down;

	public Texture LM;
	public Texture LM_hover;
	public Texture LM_down;

	public Texture CR;
	public Texture CR_hover;
	public Texture CR_down;
	
	public string targetPlayer;
	public string conversation;

	bool ListOn;
	float w,h;
	
	void Awake()
	{
		w = Screen.width;
		h = Screen.height;
		ListOn = false;



	



		
		
		myGUITexture = this.gameObject.GetComponent("GUITexture") as GUITexture;

		
	}
	

	
	// Use this for initialization
	void Start()
	{



		myGUITexture.texture = normal;
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
	
	void OnMouseEnter(){
		
		if(GameObject.Find(PhotonNetwork.playerName))
		{
		GameObject.Find(PhotonNetwork.playerName).GetComponent<DetectObjects>().enabled = false;
		GameObject.Find(PhotonNetwork.playerName).GetComponent<ClickMove>().OnGUI = true;
		}

		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);


		myGUITexture.texture = hover;
		
	}
	
	void OnMouseDown(){
		
		
		myGUITexture.texture = down;
		GameObject.Find ("AudioManager").GetComponent<AudioManager>().Play(GameObject.Find ("AudioManager").GetComponent<AudioManager>().Audioclips[10]);

		startConversation();
		
		
	}
	
	void OnMouseUpAsButton (){
		
		
		myGUITexture.texture = hover;
	}
	
	void OnMouseExit(){
		if(GameObject.Find(PhotonNetwork.playerName))
		{
		GameObject.Find(PhotonNetwork.playerName).GetComponent<DetectObjects>().enabled = true;
		GameObject.Find(PhotonNetwork.playerName).GetComponent<ClickMove>().OnGUI = false;
		}
		
		myGUITexture.texture = normal;
	}
	

	void startConversation(){
		if(conversation !=null)
		{
//			print (conversation);
		GameObject.Find("Dialogue Manager").GetComponent<DialogueSystemController>().StartConversation(conversation,GameObject.Find (PhotonNetwork.playerName).transform);
		}
	}


	void Update(){


	}

	public void updateNormal(){

		switch(targetPlayer)
		{
		case "Sales Manager":
			normal = SM;
			hover = SM_hover;
			down = SM_down;
			myGUITexture.texture = normal;
			break;
		case "LPU Officer":
			normal = LO;
			hover = LO_hover;
			down = LO_down;
			myGUITexture.texture = normal;
			break;
		case "LPU Manager":
			normal = LM;
			hover = LM_hover;
			down = LM_down;
			myGUITexture.texture = normal;
			break;
		case "Credit Risk":
			normal = CR;
			hover = CR_hover;
			down = CR_down;
			myGUITexture.texture = normal;
			break;
		default:
			break;
			
		}
	}
	
	
	
}
