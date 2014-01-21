using UnityEngine;
using System.Collections;

public class DeskObjectHandler : MonoBehaviour {
	
	private Vector3 screenPoint;
	private Vector3 offset;

	public string tableName;
	Shader originalShader;
	
	
	
	// Use this for initialization
	void Start () {
		if(this.renderer)
		if(this.renderer.material.shader)
			originalShader = this.renderer.material.shader;
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}
	
	
	
	void OnMouseOver(){
		if(this.renderer.material.shader)
			this.renderer.material.shader = Shader.Find("Toon/Basic");
		
	}
	
	void OnMouseExit(){
		if(this.renderer.material.shader)
			this.renderer.material.shader = originalShader;
	}
	
	
	void OnMouseDown()
	{
		switch(this.name)
		{
			
		case "File":
		{
			GameObject target = GameObject.Find ("File1");
			if(target.GetComponent<ObjectViewer>() == null)
			target.AddComponent<ObjectViewer>();
			float objXmid = (target.collider.bounds.max.x + target.collider.bounds.min.x)/2;
			float objYmid = (target.collider.bounds.max.y + target.collider.bounds.min.y)/2;
			float objZmid = (target.collider.bounds.max.z + target.collider.bounds.min.z)/2;
			
			
			
			Camera.main.transform.position = new Vector3(objXmid,objYmid+ 0.47f,objZmid);
			Camera.main.transform.localEulerAngles = new Vector3(90,0,0);
			
			GameObject.Find (tableName).GetComponent<DeskMode>().mode = DeskMode.DeskModeSubMode.FileMode;
			GameObject.Find (tableName).GetComponent<DeskMode>().FileModeFileIndex = 1;
			break;
		}
		case "Monitor":
		{
			GameObject target = GameObject.Find ("PCMode");
			float objXmid = (target.collider.bounds.max.x + target.collider.bounds.min.x)/2;
			float objYmid = (target.collider.bounds.max.y + target.collider.bounds.min.y)/2;
			float objZmid = (target.collider.bounds.max.z + target.collider.bounds.min.z)/2;
			
			
			
			Camera.main.transform.position = new Vector3(objXmid,objYmid+0.57f,objZmid);
			Camera.main.transform.localEulerAngles = new Vector3(90,0,0);

			GameObject.Find (tableName).GetComponent<DeskMode>().mode = DeskMode.DeskModeSubMode.PCMode;
			GameObject.Find (tableName).GetComponent<DeskMode>().sending = false;
			GameObject.Find (tableName).GetComponent<DeskMode>().checking = false;

			
			break;
		}
		case "Telephone":
		{
			break;
		}
		default:
			break;
			//LeanTween.rotate(this.gameObject,new Vector3(Camera.main.transform.localEulerAngles.x+270f,Camera.main.transform.localEulerAngles.y,Camera.main.transform.localEulerAngles.z),1f).setEase(LeanTweenType.easeOutQuint);
			//LeanTween.move(this.gameObject,new Vector3(56.05037f,3.527377f,54.04376f),1.0f).setEase(LeanTweenType.easeOutQuint);
			
			//screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
			
			//offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		}
		
	}
	
	void OnMouseDrag()
	{
		//Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		
		//Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
		//transform.position = curPosition;
		
	}
	
	
}
