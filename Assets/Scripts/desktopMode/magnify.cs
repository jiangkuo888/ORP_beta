using UnityEngine;	
using System.Collections;

public class magnify : MonoBehaviour 
{
	
	public bool enabled = false;
	
	public GUISkin customSkin;
	
	GameObject currentPage;
	
	
	Texture2D targetTexture;
	
	Texture2D highlightArea;
	Texture2D[] circleTextures;
	Rect[] positons;
	bool[] buttonClicked;
	
	int w,h;
	
	Vector2 scrollPosition = Vector2.zero;
	
	void  Start ()
	{
		//		originalPos = Vector3.zero;
		//		distance = -10f;
		//		distance = transform.localPosition.y;
		//		deltaX =0;
		//		deltaZ=0;
		
		w = Screen.width;
		h = Screen.height;
		
	}
	void  Update ()
	{
		
	}
	
	
	
	
	
	void OnGUI(){
		
		if(enabled)
		{
			
			scrollPosition = GUI.BeginScrollView(new Rect(0, 0, .5f*w, h), scrollPosition, new Rect(0, 0, .49f*w, 1300));
			
			GUI.DrawTexture (new Rect(0, 0, .49f*w, 1300), targetTexture);
			
			
			for(int i =0; i< buttonClicked.Length;i++)
			{
				if(buttonClicked[i] == false)
				{
					if(GUI.Button(new Rect(positons[i].x,positons[i].y, positons[i].width, positons[i].height),"",customSkin.customStyles[4])){
						buttonClicked[i] = true;
					}
				}
				else{
					GUI.DrawTexture(new Rect(positons[i].x,positons[i].y, positons[i].width, positons[i].height),circleTextures[i]);
				}
			}
			
			GUI.EndScrollView();
			
			
			
			if(GUI.Button ( new LTRect(.5f*w +20f, .9f*h - 50f, 100f,50f).rect, "Zoom Out", customSkin.button))
			{
				disableZoom();
			}
		}
		
		
		
		
	}
	
	
	public void enableZoom(GameObject targetPage){
		
		if(targetPage !=null && targetPage.renderer.material.mainTexture)
		{

			currentPage = targetPage;

			targetTexture = targetPage.renderer.material.mainTexture as Texture2D;
			targetTexture.filterMode = FilterMode.Trilinear;
			
			
			circleTextures = targetPage.GetComponent<pageHandler>().circlesTextures;
			buttonClicked = targetPage.GetComponent<pageHandler>().buttonClicked;
			positons = targetPage.GetComponent<pageHandler>().positons;
			
			
		}
		
		
		if(targetTexture)
			enabled = true;
		
		
		
		
		//	deltaX = deltaZ = 0;
	}
	
	public void disableZoom(){
		
		
		enabled = false;
		if(targetTexture != null)
		{
			targetTexture = null;
			
			
			
			currentPage.GetComponent<pageHandler>().buttonClicked = buttonClicked;

			currentPage = null;
			
		}
	}
	
	
	
	
	
}