using UnityEngine;
using System.Collections;

public class pcMode : MonoBehaviour {


	float w,h;
	public float x_offset;
	public float y_offset;
	public Texture2D customerList;


	public Vector2 scrollPosition = Vector2.zero;














	void Start(){

		w = Screen.width;
		h = Screen.height;
	}





	void OnGUI() {
		scrollPosition = GUI.BeginScrollView(new Rect(.4f * w, .2f * h, .5f * w, .6f * h), scrollPosition, new Rect(0, 0, .45f*w, 2000));

		GUI.DrawTexture (new Rect(0, 0, .5f*w, 2000), customerList);
		for (int i = 0; i < 30; i++) {
						GUI.Button (new Rect (.1f*w, 40*i, 300, 30), "Customer "+i+" details");
				}


		GUI.EndScrollView();














	}


}