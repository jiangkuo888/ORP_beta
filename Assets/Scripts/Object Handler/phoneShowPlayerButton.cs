﻿using UnityEngine;
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
	
	
	
	public string targetPlayer;

	bool ListOn;
	float w,h;
	
	void Awake()
	{
		w = Screen.width;
		h = Screen.height;
		ListOn = false;
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
		
		
		GameObject.Find(PhotonNetwork.playerName).GetComponent<DetectObjects>().enabled = false;
		GameObject.Find(PhotonNetwork.playerName).GetComponent<ClickMove>().OnGUI = true;
		myGUITexture.texture = hover;
		
	}
	
	void OnMouseDown(){
		
		
		myGUITexture.texture = down;
		
		VOIP(targetPlayer);
		
		
	}
	
	void OnMouseUpAsButton (){
		
		
		myGUITexture.texture = hover;
	}
	
	void OnMouseExit(){
		GameObject.Find(PhotonNetwork.playerName).GetComponent<DetectObjects>().enabled = true;
		GameObject.Find(PhotonNetwork.playerName).GetComponent<ClickMove>().OnGUI = false;
		
		
		myGUITexture.texture = normal;
	}
	

	void VOIP(string target){

		GameObject.Find("Dialogue Manager").GetComponent<DialogueSystemController>().ShowAlert("You are now chating with "+target+"....");

	}
	

	
	
	
	
}
