using UnityEngine;
using System.Collections;

public class telephonegui : MonoBehaviour {
	
	bool GUIisOn;
	bool Enlarge;
	Shader originalShader;
	// Use this for initialization
	void Start () {
		originalShader = this.renderer.material.shader;
		GUIisOn = false;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnMouseOver(){
		GUIisOn = false;
		if(this.renderer.material.shader)
			this.renderer.material.shader = Shader.Find("Toon/Basic");
		
	}
	void OnMouseExit(){
		GUIisOn = false;
		if(this.renderer.material.shader)
			this.renderer.material.shader = originalShader;
	}
	void OnMouseDown(){
		{
			GUIisOn=true;

		}
	}
	void OnGUI(){
		if(GUIisOn){
			GUI.contentColor = Color.black;
			GUI.Label(new Rect(Screen.width/2,Screen.height/2,50,50),"Click to Enlarge");
		}
	}
}