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
	
	void Awake()
	{
		w = Screen.width;
		h = Screen.height;
		ListOn = false;
		this.gameObject.GetComponent<GUITexture>().texture = normal;
		
		
		myGUITexture = this.gameObject.GetComponent("GUITexture") as GUITexture;
		unreadNumber = 0;
		
	}
	
	
	
	// Use this for initialization
	void Start()
	{
		// Position the billboard in the center, 
		// but respect the picture aspect ratio
		float textureHeight = height;
		float textureWidth = width;
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
		
		myGUITexture.pixelInset = 
			new Rect(xPosition, yPosition, 
			         scaledWidth, scaledHeight);


		this.transform.GetComponentInChildren<GUIText>().pixelOffset = new Vector2(myGUITexture.pixelInset.x+height/2-.5f,myGUITexture.pixelInset.y+width/2-.5f);
	}
	

	
	

	
	public void addRedDot(){


		unreadNumber += 1;
		this.transform.GetComponentInChildren<GUIText>().text = unreadNumber.ToString();

		if(this.GetComponent<GUITexture>().enabled == false)
		{
			this.GetComponent<GUITexture>().enabled = true;
			this.transform.GetComponentInChildren<GUIText>().enabled = true;
		}

	}

	public void removeRedDot(){
		unreadNumber -=1;
		this.transform.GetComponentInChildren<GUIText>().text = unreadNumber.ToString();

		if(unreadNumber <= 0)
		{
			this.GetComponent<GUITexture>().enabled = false;
			this.transform.GetComponentInChildren<GUIText>().enabled = false;
		}

	}
	
	
	
	
}
