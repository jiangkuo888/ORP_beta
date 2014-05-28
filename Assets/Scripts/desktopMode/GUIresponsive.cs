using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem.UnityGUI;
using PixelCrushers.DialogueSystem;


public class GUIresponsive : MonoBehaviour {
	
	public float x_offset;
	public float y_offset;
	public float width;
	public float height;
	
	private GUITexture myGUITexture;
	
	public Texture normal;
	public Texture hover;
	public Texture down;
	



	public int unreadNumber;
	
	bool ListOn;
	float w,h;
	float textureHeight ;
	float textureWidth ;
	float screenHeight ;
	float screenWidth ;
	
	float screenAspectRatio ;
	float textureAspectRatio ;
	
	float scaledHeight;
	float scaledWidth;


	void Awake()
	{
		 textureHeight = height;
		 textureWidth = width;
		 screenHeight = Screen.height;
		 screenWidth = Screen.width;
		
		 screenAspectRatio = (screenWidth / screenHeight);
		 textureAspectRatio = (textureWidth / textureHeight) ;
		w = Screen.width;
		h = Screen.height;
		ListOn = false;
		this.gameObject.GetComponent<GUITexture>().texture = normal;
		
		
		myGUITexture = this.gameObject.GetComponent("GUITexture") as GUITexture;
		unreadNumber = 0;
		
	}
	
	public void updateRedDot(){

		if(GameObject.Find ("phoneButton").GetComponent<phoneButton>().smallButtonOn == true)
			redDotMoveUp();
		else
			redDotMoveDown();

	}
	
	// Use this for initialization
	void Start()
	{
		// Position the billboard in the center, 
		// but respect the picture aspect ratio

		unreadNumber = 0;
		
		//		print(textureAspectRatio);
		if (textureAspectRatio <= screenAspectRatio)
		{
			// The scaled size is based on the height
			scaledHeight = height;
			scaledWidth = width;
		}
		else
		{
			
			// The scaled size is based on the width
			scaledWidth = width;
			scaledHeight = height;
		}
		float xPosition = screenWidth / 2 * x_offset - scaledWidth;
		float yPosition = screenHeight / 2 * y_offset - scaledHeight;


		if(GameObject.Find ("phoneButton").GetComponent<phoneButton>().smallButtonOn == true)
		{

		myGUITexture.pixelInset = new Rect(xPosition, yPosition, scaledWidth, scaledHeight);

		}
		else
		{
			GUITexture phoneTexture = GameObject.Find ("phoneButton").GetComponent<GUITexture>().guiTexture;
			myGUITexture.pixelInset = new Rect(phoneTexture.pixelInset.x+phoneTexture.pixelInset.width,phoneTexture.pixelInset.y+phoneTexture.pixelInset.height,phoneTexture.pixelInset.width/5,phoneTexture.pixelInset.height/5);

		}

		this.transform.Find ("unreadNumber").GetComponent<GUIText>().pixelOffset = new Vector2(myGUITexture.pixelInset.x+height/2-.5f,myGUITexture.pixelInset.y+width/2-.5f);
		this.transform.Find ("EmailShortMessage").GetComponent<GUIText>().pixelOffset = new Vector2(myGUITexture.pixelInset.x+.08f*w,-0.85f*h/2);
	}
	

	
	

	
	public void addRedDot(){

		// updateRedDot position

		print ("Received 1 message, red dot +1");

		updateRedDot();
		unreadNumber += 1;
		this.transform.Find ("unreadNumber").GetComponent<GUIText>().text = unreadNumber.ToString();
		//this.transform.Find ("EmailShortMessage").GetComponent<GUIText>().text = "You have new message.";

		if(this.GetComponent<GUITexture>().enabled == false)
		{
			this.GetComponent<GUITexture>().enabled = true;
			this.transform.GetComponentInChildren<GUIText>().enabled = true;
		}

	}



	public void removeRedDot(){
		updateRedDot();

		unreadNumber -=1;
		this.transform.Find ("unreadNumber").GetComponent<GUIText>().text = unreadNumber.ToString();
		//this.transform.Find ("EmailShortMessage").GetComponent<GUIText>().text = "";
		if(unreadNumber <= 0)
		{
			this.GetComponent<GUITexture>().enabled = false;
			this.transform.GetComponentInChildren<GUIText>().enabled = false;
		}

	}
	public void redDotMoveUp(){



		//		print(textureAspectRatio);
		if (textureAspectRatio <= screenAspectRatio)
		{
			// The scaled size is based on the height
			scaledHeight = height;
			scaledWidth = width;
		}
		else
		{
			
			// The scaled size is based on the width
			scaledWidth = width;
			scaledHeight = height;
		}
		float xPosition = screenWidth / 2 * x_offset - scaledWidth;
		float yPosition = screenHeight / 2 * y_offset - scaledHeight;


		myGUITexture.pixelInset = new Rect(xPosition, yPosition, scaledWidth, scaledHeight);

	
		this.transform.Find ("unreadNumber").GetComponent<GUIText>().pixelOffset = new Vector2(myGUITexture.pixelInset.x+height/2-.5f,myGUITexture.pixelInset.y+width/2-.5f);
	}

	public void redDotMoveDown(){


		GUITexture phoneTexture = GameObject.Find ("phoneButton").GetComponent<GUITexture>().guiTexture;
		myGUITexture.pixelInset = new Rect(phoneTexture.pixelInset.x+phoneTexture.pixelInset.width*.6f,phoneTexture.pixelInset.y+phoneTexture.pixelInset.height*.6f,phoneTexture.pixelInset.width/4,phoneTexture.pixelInset.height/4);


		this.transform.Find ("unreadNumber").GetComponent<GUIText>().pixelOffset = new Vector2(myGUITexture.pixelInset.x+height/2-.5f,myGUITexture.pixelInset.y+width/2-.5f);
	}



	         
	
	
	
}
