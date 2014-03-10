using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem.UnityGUI;
using PixelCrushers.DialogueSystem;


public class phoneShowNPCButton : MonoBehaviour {
	
	public float x_offset;
	public float y_offset;
	
	
	private GUITexture myGUITexture;
	
	public Texture normal;
	public Texture hover;
	public Texture down;
	
	
	
	public string[] dialogueNames;

	public string[] NPCnames;
	
	
	public Vector2 scrollPosition = Vector2.zero;
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
	
	void OnGUI(){
		if(ListOn)
		{
			scrollPosition = GUI.BeginScrollView(new Rect(.1f * w, .5f * h, 120, .3f * h), scrollPosition, new Rect(0, 0, 100, NPCnames.Length*80f));
			
			GUI.DrawTexture (new Rect(0, 0, 110, 2000), null);
			
			
			for (int i = 0; i < NPCnames.Length; i++) {
				if(GUI.Button (new Rect (0, 80*i, 100, 60), NPCnames[i]))
				{
					if(dialogueNames[i] !=null)
						talkTo(dialogueNames[i]);
					
				}
			}
			
			GUI.EndScrollView();
		}
		
		
		
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
		if(ListOn)
		{
			ListOn = false;
		}
		else
		{
		
			ListOn = true;
		
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
	
	

	
	
	void talkTo(string dialogueName)
	{
		
		GameObject.Find ("Dialogue Manager").GetComponent<DialogueSystemController>().StartConversation(dialogueName,GameObject.Find (PhotonNetwork.playerName).transform,GameObject.Find (PhotonNetwork.playerName).transform);
		
		//print ("111");
		
		
		GameObject.Find ("phoneSmallButton1").GetComponent<GUITexture>().enabled = false;
		GameObject.Find ("phoneSmallButton2").GetComponent<GUITexture>().enabled = false;
		GameObject.Find ("phoneSmallButton3").GetComponent<GUITexture>().enabled = false;
		GameObject.Find ("phoneSmallButton4").GetComponent<GUITexture>().enabled = false;
		ListOn = false;
		// disable list
	}
	
	
	
	
}
