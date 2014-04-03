using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

public class pcMode : MonoBehaviour {
	
	public GUISkin customSkin;
	float w,h;
	public float x_offset;
	public float y_offset;
	public string[] customerNames;
	public Texture2D[] customerImages;
	
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
	
	
	
	void OnGUI() {

		if(!InfoModeIsOn)
		{
		scrollPosition = GUI.BeginScrollView(new Rect(.46f * w, .2f * h, .5f * w, .6f * h), scrollPosition, new Rect(0, 0, .45f*w, 1000));
		
			GUI.DrawTexture (new Rect(0, 0, .5f*w, 1000), null);


		for (int i = 0; i < customerNames.Length; i++) {
			if(GUI.Button (new Rect (.06f*w, 50*i, 300, 30), customerNames[i]))
			{

					if(EventFSM.enabled)
						if(EventFSM.ActiveStateName == "Click on Entry")
						EventFSM.FsmVariables.GetFsmBool("EntryClicked").Value = true;

				if(deskTop != null)
				{
					
					currentImage = customerImages[i];

					
					
					
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
		
			scrollPosition = GUI.BeginScrollView(new Rect(.46f * w, .2f * h, .5f * w, .6f * h), scrollPosition, new Rect(0, 0, .45f*w, 1000));
			
			GUI.DrawTexture (new Rect(0, 0, .5f*w, 1000), currentImage, ScaleMode.ScaleToFit);	




			GUI.EndScrollView();


			if(GUI.Button( new LTRect(w - 200f, .9f*h - 50f, 100f, 50f ).rect, "Back",customSkin.button))
			{
				InfoModeIsOn = false;
				scrollPosition = Vector2.zero;
			}

		}
		
		
		
		
		
		
		
		
		
		
		
		
	}
	
	
}