using UnityEngine;
using System.Collections;

public class ObjectViewer : MonoBehaviour {

	bool GUIisOn;
	bool Enlarge;
	Shader originalShader;

	Vector3 originalPosition;

	// Use this for initialization
	void Start () {
		originalShader = this.renderer.material.shader;
		GUIisOn = false;
		Enlarge = false;

		originalPosition = this.transform.position;

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
			GUI.Label(new Rect(Screen.width/2,Screen.height/2,50,50),"Click to read");
		}
	}

	public void readDocument(){
		if(!Enlarge)
		{

			LeanTween.move (this.gameObject, new Vector3(this.transform.position.x+.1f,this.transform.position.y + .1f,this.transform.position.z),0.5f).setEase(LeanTweenType.easeOutQuint);
			Enlarge = true;

			showPage();
		}

	}

    void showPage(){

		playOpenFileAnim();


		

		}
	

	public void playOpenFileAnim(){


	}

	public void playCloseFileAnim(){

	}

	public void resetDocumentPosition(){

		LeanTween.move (this.gameObject, originalPosition,0.5f).setEase(LeanTweenType.easeOutQuint);

	}
	                   
}
