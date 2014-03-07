﻿using UnityEngine;
using System.Collections;


public class pcMode : MonoBehaviour {
	
	
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
	
	
	
	
	
	
	
	
	void Start(){
		InfoModeIsOn = false;
		w = Screen.width;
		h = Screen.height;
	}
	
	void InfoModeOn(){

		InfoModeIsOn = true;
	}
	
	
	
	void OnGUI() {

		if(!InfoModeIsOn)
		{
		scrollPosition = GUI.BeginScrollView(new Rect(.46f * w, .2f * h, .5f * w, .6f * h), scrollPosition, new Rect(0, 0, .45f*w, 2000));
		
			GUI.DrawTexture (new Rect(0, 0, .5f*w, 2000), pcWallPaper);
		for (int i = 0; i < customerNames.Length; i++) {
			if(GUI.Button (new Rect (.1f*w, 40*i, 300, 30), customerNames[i]))
			{
				if(deskTop != null)
				{
					
					currentImage = customerImages[i];

					GameObject pc = GameObject.Find ("PCMode").gameObject;


					
					float midX = (pc.transform.renderer.bounds.max.x + pc.transform.renderer.bounds.min.x)/2;
					float midY = (pc.transform.renderer.bounds.max.y + pc.transform.renderer.bounds.min.y)/2;
					float midZ = (pc.transform.renderer.bounds.max.z + pc.transform.renderer.bounds.min.z)/2;
					
					
					
					LeanTween.move(Camera.main.gameObject,new Vector3(midX,midY+.343f,midZ),.6f).setEase(LeanTweenType.easeOutQuint).setOnComplete(InfoModeOn);

					deskTop.GetComponent<DeskMode>().mode = DeskMode.DeskModeSubMode.InfoMode;
					
				}
				
			}
		}
		
		
		GUI.EndScrollView();
		
		}
		else{

			scrollPosition = GUI.BeginScrollView(new Rect(.11f * w, .1f* h, .8f * w, .8f * h), scrollPosition, new Rect(0, 0, .78f*w, 3000));
			
			GUI.DrawTexture (new Rect(0, 0, .78f*w, 3000), currentImage);
		
			

			GUI.EndScrollView();

		}
		
		
		
		
		
		
		
		
		
		
		
		
	}
	
	
}