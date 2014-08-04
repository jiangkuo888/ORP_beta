using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem.UnityGUI;
using PixelCrushers.DialogueSystem;


public class phoneShowNPCButton : MonoBehaviour {
	public GUISkin customSkin;

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
	public bool ListOn;
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
		//Add GUISkin
		GUI.skin = customSkin;
		if(ListOn)
		{
			scrollPosition = GUI.BeginScrollView(new Rect(.1f * w, .5f * h, 220, .3f * h), scrollPosition, new Rect(0, 0, 200, dialogueDisplayText.Length*80f));
			
			GUI.DrawTexture (new Rect(0, 0, 110, 2000), null);
			
			
			for (int i = 0; i < dialogueDisplayText.Length; i++) {
				if(GUI.Button (new Rect (0, 80*i, 200, 60), dialogueDisplayText[i]))
				{
					
					if(dialogueNames[i] !=null)
					{

						if(GameObject.Find (PhotonNetwork.playerName))
						GameObject.Find ("Dialogue Manager").GetComponent<DialogueSystemController>().StartConversation(dialogueNames[i],GameObject.Find (PhotonNetwork.playerName).transform,GameObject.Find (PhotonNetwork.playerName).transform);

						else
							GameObject.Find ("Dialogue Manager").GetComponent<DialogueSystemController>().StartConversation(dialogueNames[i],GameObject.Find (GameObject.Find ("GameManager").GetComponent<GameManagerVik>().characterName).transform);


						//print ("111");
						
						
						GameObject.Find("phoneButton").GetComponent<phoneButton>().hide();

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

		myGUITexture.texture = hover;
		
	}
	
	
	
	void OnMouseDown(){
		myGUITexture.texture = down;
		GameObject.Find ("AudioManager").GetComponent<AudioManager>().Play(GameObject.Find ("AudioManager").GetComponent<AudioManager>().Audioclips[10]);

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
		
		myGUITexture.texture = normal;
	}
	
	
	public void addCallToNPCList(string buttonDisplayText, string conversationName, string playerName){

		if (PhotonNetwork.playerName == playerName) {

						// add the conversationName to the list
						System.Collections.Generic.List<string> conversationList = new System.Collections.Generic.List<string> (dialogueNames);
		
						if (!conversationList.Contains (conversationName))
								conversationList.Add (conversationName);
		
						dialogueNames = conversationList.ToArray ();


						// add the display text to the textlist
						System.Collections.Generic.List<string> textList = new System.Collections.Generic.List<string> (dialogueDisplayText);
		
						if (!textList.Contains (buttonDisplayText))
								textList.Add (buttonDisplayText);
		
						dialogueDisplayText = textList.ToArray ();
				}

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
