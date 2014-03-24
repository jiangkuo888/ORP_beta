using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem.UnityGUI;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.ChatMapper;


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


	bool smallButtonOn;
	string conversationName;
	string playerName;
	int count = 0;

	void Awake()
	{
		OnCalling = false;
		smallButtonOn = false;
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
		if(OnCalling)
		{
			blinking ();

		}
		else{


		}


	}
	
	void OnMouseEnter(){
		
		
		GameObject.Find(PhotonNetwork.playerName).GetComponent<DetectObjects>().enabled = false;
		GameObject.Find(PhotonNetwork.playerName).GetComponent<ClickMove>().OnGUI = true;

		myGUITexture.texture = hover;
		
	}
	void blinking(){



		if(count%50<25)
		{
		myGUITexture.texture = isOnCall;

		}
		else{

			myGUITexture.texture = normal;
		}

		count++;

	}
	void OnMouseDown(){


		if(OnCalling)
		{
			// start conversation
			GameObject.Find("Dialogue Manager").GetComponent<DialogueSystemController>().StartConversation(conversationName,GameObject.Find (PhotonNetwork.playerName).transform);
			
			OnCalling = false;


			GameObject.Find ("AudioManager").GetComponent<AudioManager>().Stop(GameObject.Find ("AudioManager").GetComponent<AudioManager>().Audioclips[0]);


			myGUITexture.texture = normal;
		}
		else
		{

		







		myGUITexture.texture = down;






		if(smallButtonOn)
		{

				GameObject.Find ("phoneSmallButton1").GetComponent<GUITexture>().enabled = false;
				GameObject.Find ("phoneSmallButton2").GetComponent<GUITexture>().enabled = false;
				GameObject.Find ("phoneSmallButton3").GetComponent<GUITexture>().enabled = false;
				GameObject.Find ("phoneSmallButton4").GetComponent<GUITexture>().enabled = false;
				GameObject.Find ("EmailIcon").GetComponent<GUITexture>().enabled = false;
				smallButtonOn = false;
		}
			else
			{
				GameObject.Find ("phoneSmallButton1").GetComponent<GUITexture>().enabled = true;
				GameObject.Find ("phoneSmallButton2").GetComponent<GUITexture>().enabled = true;
				GameObject.Find ("phoneSmallButton3").GetComponent<GUITexture>().enabled = true;
				GameObject.Find ("phoneSmallButton4").GetComponent<GUITexture>().enabled = true;
				GameObject.Find ("EmailIcon").GetComponent<GUITexture>().enabled = true;
				smallButtonOn = true;

			}
		

		}
		
		
	}
	
	void OnMouseUpAsButton (){


		myGUITexture.texture = hover;
	}
	
	void OnMouseExit(){
		GameObject.Find(PhotonNetwork.playerName).GetComponent<DetectObjects>().enabled = true;
	
		GameObject.Find(PhotonNetwork.playerName).GetComponent<ClickMove>().OnGUI = false;


		myGUITexture.texture = normal;
	}


	public void OnCall(string player, string conversation){


		if(PhotonNetwork.playerName == player){

		OnCalling = true;
		GameObject.Find ("AudioManager").GetComponent<AudioManager>().Play(GameObject.Find ("AudioManager").GetComponent<AudioManager>().Audioclips[0],Camera.main.transform.position,1f,1f,false);

		playerName = player;



		conversationName = conversation;



		
		}

	}

    

	public void loadSmallButtonCharacter(){

		switch(PhotonNetwork.playerName)
		{
		case "Sales Manager":
			GameObject.Find("phoneSmallButton1").GetComponent<phoneShowPlayerButton>().targetPlayer = "LPU Officer";
			GameObject.Find("phoneSmallButton2").GetComponent<phoneShowPlayerButton>().targetPlayer = "LPU Manager";
			GameObject.Find("phoneSmallButton3").GetComponent<phoneShowPlayerButton>().targetPlayer = "Credit Risk";

			GameObject.Find("phoneSmallButton1").GetComponent<phoneShowPlayerButton>().conversation = "SM to LO";
			GameObject.Find("phoneSmallButton2").GetComponent<phoneShowPlayerButton>().conversation = "SM to LM";
			GameObject.Find("phoneSmallButton3").GetComponent<phoneShowPlayerButton>().conversation = "SM to CR";
			GameObject.Find("EmailIcon").GetComponent<phoneShowInbox>().conversation = "SM Inbox";

			break;
		case "LPU Manager":
			GameObject.Find("phoneSmallButton1").GetComponent<phoneShowPlayerButton>().targetPlayer = "LPU Officer";
			GameObject.Find("phoneSmallButton2").GetComponent<phoneShowPlayerButton>().targetPlayer = "Sales Manager";
			GameObject.Find("phoneSmallButton3").GetComponent<phoneShowPlayerButton>().targetPlayer = "Credit Risk";

			GameObject.Find("phoneSmallButton1").GetComponent<phoneShowPlayerButton>().conversation = "LM to LO";
			GameObject.Find("phoneSmallButton2").GetComponent<phoneShowPlayerButton>().conversation = "LM to SM";
			GameObject.Find("phoneSmallButton3").GetComponent<phoneShowPlayerButton>().conversation = "LM to CR";
			GameObject.Find ("EmailIcon").GetComponent<phoneShowInbox>().conversation = "LM Inbox";
			break;
		case "LPU Officer":
			GameObject.Find("phoneSmallButton1").GetComponent<phoneShowPlayerButton>().targetPlayer = "Sales Manager";
			GameObject.Find("phoneSmallButton2").GetComponent<phoneShowPlayerButton>().targetPlayer = "LPU Manager";
			GameObject.Find("phoneSmallButton3").GetComponent<phoneShowPlayerButton>().targetPlayer = "Credit Risk";

			GameObject.Find("phoneSmallButton1").GetComponent<phoneShowPlayerButton>().conversation = "LO to SM";
			GameObject.Find("phoneSmallButton2").GetComponent<phoneShowPlayerButton>().conversation = "LO to LM";
			GameObject.Find("phoneSmallButton3").GetComponent<phoneShowPlayerButton>().conversation = "LO to CR";
			GameObject.Find ("EmailIcon").GetComponent<phoneShowInbox>().conversation = "LO Inbox";
			break;
		case "Credit Risk":
			GameObject.Find("phoneSmallButton1").GetComponent<phoneShowPlayerButton>().targetPlayer = "LPU Officer";
			GameObject.Find("phoneSmallButton2").GetComponent<phoneShowPlayerButton>().targetPlayer = "LPU Manager";
			GameObject.Find("phoneSmallButton3").GetComponent<phoneShowPlayerButton>().targetPlayer = "Sales Manager";

			GameObject.Find("phoneSmallButton1").GetComponent<phoneShowPlayerButton>().conversation = "CR to LO";
			GameObject.Find("phoneSmallButton2").GetComponent<phoneShowPlayerButton>().conversation = "CR to LM";
			GameObject.Find("phoneSmallButton3").GetComponent<phoneShowPlayerButton>().conversation = "CR to SM";
			GameObject.Find ("EmailIcon").GetComponent<phoneShowInbox>().conversation = "CR Inbox";
			break;
		default:
			break;

		}



	}



	public void enableSenderOutboxMessage(string senderName, string targetName,string messageName){

		if(PhotonNetwork.playerName == senderName)
		{
			//string senderTag = GameObject.Find (senderName).tag;
			//string receiverTag = GameObject.Find (targetName).tag;

			//string conversationTitle = senderTag + " to " +receiverTag;


			DialogueLua.SetItemField(messageName,"State","success");

		}


	}
	
	
}
