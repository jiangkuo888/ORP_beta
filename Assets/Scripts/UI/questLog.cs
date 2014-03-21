using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem.UnityGUI;


public class questLog : MonoBehaviour {

	public float x_offset;
	public float y_offset;
	public GameObject QuestLogWindow;

	private GUITexture myGUITexture;
	
	void Awake()
	{
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
			scaledHeight = screenHeight/18;
			scaledWidth = (scaledHeight * textureAspectRatio);
		}
		else
		{

			// The scaled size is based on the width
			scaledWidth = screenWidth/18;
			scaledHeight = (scaledWidth / textureAspectRatio);
		}
		float xPosition = screenWidth / 2 * x_offset - scaledWidth;
		float yPosition = screenHeight / 2 * y_offset - scaledHeight;

		myGUITexture.pixelInset = 
			new Rect(xPosition, yPosition, 
			         scaledWidth, scaledHeight);
	}

	void OnMouseOver(){


		GameObject.Find(PhotonNetwork.playerName).GetComponent<DetectObjects>().enabled = false;
		GameObject.Find(PhotonNetwork.playerName).GetComponent<ClickMove>().enabled = false;	


	}

	void OnMouseDown(){


	}

	void OnMouseUpAsButton (){
		// call the questlog
		if(QuestLogWindow)
		QuestLogWindow.GetComponent<UnityQuestLogWindow>().Open();
		else
		{
			QuestLogWindow = GameObject.Find ("Sci-fi Unity Quest Log Window");
		    QuestLogWindow.GetComponent<UnityQuestLogWindow>().Open();
		}


	}

	void OnMouseExit(){
		GameObject.Find(PhotonNetwork.playerName).GetComponent<DetectObjects>().enabled = true;
		GameObject.Find(PhotonNetwork.playerName).GetComponent<ClickMove>().enabled = true;	
	}




}
