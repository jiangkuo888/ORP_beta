using UnityEngine;
using System.Collections;

public class telephonegui : MonoBehaviour {
	
	public bool GUIisOn;
	Shader originalShader;
	// Use this for initialization
	void Start () {
		originalShader = this.renderer.material.shader;
		GUIisOn = false;

	}
	
	// Update is called once per frame

	
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
	void Update(){

			}
		

	void OnGUI(){
		if(GUIisOn==true){
			//Destroy(gameObject);
			GUI.Button(new Rect(0, 0, Screen.width, Screen.height), "This is a title");
		}
	}
}