using UnityEngine;	
using System.Collections;

public class magnify : MonoBehaviour 
{
	
	public bool enabled = false;

	public GUISkin customSkin;

	Texture2D targetTexture;

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
			targetTexture = targetPage.renderer.material.mainTexture as Texture2D;
			targetTexture.filterMode = FilterMode.Trilinear;
		}


		if(targetTexture)
		enabled = true;




	//	deltaX = deltaZ = 0;
	}
	
	public void disableZoom(){


		enabled = false;
		if(targetTexture != null)
			targetTexture = null;
	}



	
	
}