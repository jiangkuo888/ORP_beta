using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

public class pcMode : MonoBehaviour {
	
	public GUISkin customSkin;
	float w,h;
	public float x_offset;
	public float y_offset;
	public string[] customerNames;
	public Texture[] customerImages;
	
	public GameObject deskTop;

	public Vector2 scrollPosition = Vector2.zero;
	
	public bool InfoModeIsOn;
	
	Texture2D currentImage;
	
	public Texture2D pcWallPaper;
	
	
	
	PlayMakerFSM EventFSM;
	
	
	
	
	void Start(){



		InfoModeIsOn = false;
		w = Screen.width;
		h = Screen.height;

		EventFSM = GameObject.Find ("EventManager-Tutorial").GetComponent<PlayMakerFSM>();
	}
	
	void InfoModeOn(){

		InfoModeIsOn = true;
	}
	
	public void addCustomerInfo(string newName, Texture newTexture){
		//if(PhotonNetwork.playerName == targetPlayer)
	//	{
			// add the conversationName to the list
			System.Collections.Generic.List<string> nameList = new System.Collections.Generic.List<string>(customerNames);
			
			if(!nameList.Contains(newName))
				nameList.Add(newName);
			
			customerNames = nameList.ToArray();
			
			
			// add the display text to the textlist
			System.Collections.Generic.List<Texture> textureList = new System.Collections.Generic.List<Texture>(customerImages);
			
			if(!textureList.Contains(newTexture))
				textureList.Add(newTexture);
			
			customerImages = textureList.ToArray();
	//	}



	}
	
	void OnGUI() {

		if(!InfoModeIsOn)
		{
		scrollPosition = GUI.BeginScrollView(new Rect(.46f * w, .2f * h, .52f * w, .6f * h), scrollPosition, new Rect(0, 0, .48f*w, 1000));
		
			GUI.DrawTexture (new Rect(0, 0, .5f*w, 1000), null);


		for (int i = 0; i < customerNames.Length; i++) {
			if(GUI.Button (new Rect (.14f*w, 50*i, 300, 30), customerNames[i],customSkin.button))
			{

					if(EventFSM.enabled)
						if(EventFSM.ActiveStateName == "Click on Entry")
						EventFSM.FsmVariables.GetFsmBool("EntryClicked").Value = true;

				if(deskTop != null)
				{
					
					currentImage = customerImages[i] as Texture2D;

					
					
					
						InfoModeIsOn = true;

					//deskTop.GetComponent<DeskMode>().mode = DeskMode.DeskModeSubMode.InfoMode;
					
				}
				
			}
		}
		
		
		GUI.EndScrollView();
		
		}
		else{

			//scrollPosition = GUI.BeginScrollView(new Rect(.46f * w, .2f * h, .5f * w, .6f * h), scrollPosition, new Rect(0, 0, .45f*w, 2000));
			
			//GUI.DrawTexture (new Rect(0, 0, .5f*w, 2000), currentImage, ScaleMode.ScaleToFit);	
		
			scrollPosition = GUI.BeginScrollView(new Rect(.46f * w, .2f * h, .52f * w, .6f * h), scrollPosition, new Rect(0, 0, .48f*w, 1000));
			
			GUI.DrawTexture (new Rect(.06f*w, 0, .5f*w, 1000), currentImage, ScaleMode.ScaleToFit);	




			GUI.EndScrollView();


			if(GUI.Button( new LTRect(w - 120f, .9f*h - 50f, 100f, 50f ).rect, "Back",customSkin.button))
			{
				InfoModeIsOn = false;
				scrollPosition = Vector2.zero;
			}

		}
		
		
		
		
		
		
		
		
		
		
		
		
	}
	
	
}