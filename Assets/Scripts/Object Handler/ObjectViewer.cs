using UnityEngine;
using System.Collections;

public class ObjectViewer : MonoBehaviour {

	bool GUIisOn;
	bool Enlarge;
	Shader originalShader;
	// Use this for initialization
	void Start () {
		originalShader = this.renderer.material.shader;
		GUIisOn = false;
		Enlarge = false;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseOver(){
		GUIisOn = true;
		if(this.renderer.material.shader)
			this.renderer.material.shader = Shader.Find("Toon/Basic");

	}
	void OnMouseExit(){
		GUIisOn = false;
		if(this.renderer.material.shader)
			this.renderer.material.shader = originalShader;

	}
	void OnGUI(){
		if(GUIisOn && !Enlarge){
			GUI.contentColor = Color.black;
			GUI.Label(new Rect(Screen.width/2,Screen.height/2,50,50),"Click to Enlarge");
		}
	}

	void OnMouseDown(){
		if(!Enlarge)
		{
			LeanTween.move (this.gameObject, new Vector3(this.transform.position.x,this.transform.position.y + .1f,this.transform.position.z),0.5f).setEase(LeanTweenType.easeOutQuint);
			Enlarge = true;
		}
	}

	
	                   
}
