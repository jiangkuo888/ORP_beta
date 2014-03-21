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
	
	public string[] dialogueDisplayText;
	public string targetPlayer;
	
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
			scrollPosition = GUI.BeginScrollView(new Rect(.1f * w, .5f * h, 120, .3f * h), scrollPosition, new Rect(0, 0, 100, dialogueDisplayText.Length*80f));
			
			GUI.DrawTexture (new Rect(0, 0, 110, 2000), null);
			
			
			for (int i = 0; i < dialogueDisplayText.Length; i++) {
				if(GUI.Button (new Rect (0, 80*i, 100, 60), dialogueDisplayText[i]))
				{
					
					if(dialogueNames[i] !=null)
					{
						PhotonView phoneView = this.gameObject.GetPhotonView();
						
						phoneView.RPC ("talkTo",PhotonTargets.OthersBuffered,dialogueNames[i],targetPlayer);
					}
					
					
					
					
					
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
	
	
	
	
	[RPC]
	void talkTo(string dialogueName,string targetPlayerName)
	{
		if(PhotonNetwork.playerName == targetPlayerName)
		{
			GameObject.Find ("Dialogue Manager").GetComponent<DialogueSystemController>().StartConversation(dialogueName,GameObject.Find (PhotonNetwork.playerName).transform,GameObject.Find (PhotonNetwork.playerName).transform);
			
			//print ("111");
			
			
			GameObject.Find ("phoneSmallButton1").GetComponent<GUITexture>().enabled = false;
			GameObject.Find ("phoneSmallButton2").GetComponent<GUITexture>().enabled = false;
			GameObject.Find ("phoneSmallButton3").GetComponent<GUITexture>().enabled = false;
			GameObject.Find ("phoneSmallButton4").GetComponent<GUITexture>().enabled = false;
			ListOn = false;
		}
		// disable list
	}


//	public void addDialogue(string dialogueName, string dialogueButtonText){
//
//		System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>(dialogueNames);
//		list.Add(dialogueName);
//		dialogueNames = list.ToArray();
//
//
//
//		System.Collections.Generic.List<string> list2 = new System.Collections.Generic.List<string>(dialogueDisplayText);
//		list2.Add(dialogueButtonText);
//		dialogueDisplayText = list2.ToArray();
//
//
//
//	}
	
	
	
}
