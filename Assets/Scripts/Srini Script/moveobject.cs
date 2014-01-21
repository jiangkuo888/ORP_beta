using UnityEngine;
using System.Collections;

public class moveobject : MonoBehaviour {
	public GameObject other;
	public ClickMove clickmove;
	
	
	
	
	void  OnGUI (){
		
		Ray ray= Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		//Vector3 mouseWorldPosition;
		
		if (Physics.Raycast (ray, out hit, 3)) {
			if(hit.collider.gameObject.name=="Cabinet_L"){Files();//Debug.DrawLine(transform.position, mouseWorldPosition, Color.red);
			}
			if(hit.collider.gameObject.name=="Bin"){Bin(); }
			if(hit.collider.gameObject.name=="Kettle"){Kettle(); }
			if(hit.collider.gameObject.name=="FireAlarm"){Alarm();}
			if(hit.collider.gameObject.name=="Cube"){Cube();}
//			if(hit.collider.gameObject.name=="AItrigger"){ai_1();}
			
		}  
	}

//	void OnTriggerEnter(Collider co)
//	{
//		if (co.name=="AItrigger"){ai_1();}
//	}


	void  Files (){GUI.Label( new Rect(Screen.width / 2,Screen.height/2,300,300),"Files are Stored Safely");}
	void  Bin (){GUI.Label( new Rect(Screen.width / 2,Screen.height/2,300,300),"Use to throw waste");}
	void  Kettle (){GUI.Label( new Rect(Screen.width / 2,Screen.height/2,300,300),"Kettle: Use to heat your coffee");}
	void  Alarm (){GUI.Label( new Rect(Screen.width/2,Screen.height/2,300,300),"This will buzz in case of emergency");}
	
	
	void  Cube(){	
		other = GameObject.Find ("Cube");
		clickmove=GetComponent<ClickMove> ();
		if (GUI.Button (new Rect (Screen.width / 2, Screen.height / 2, 300, 300), "Change Color")) {	
			other.renderer.material.color = Color.blue;
			clickmove.enabled = false;
		} else
			clickmove.enabled = true;


	}	
//	void  ai_1(){	
//		//other = GameObject.Find ("npc_1");
//		clickmove=GetComponent<ClickMove> ();
//		//if (GUI.Button (new Rect (Screen.width / 2, Screen.height / 2, 300, 300), "Move left")) {	
//			//transform.position += Vector3.left * 10.0f;
//			//other.renderer.material.color = Color.blue;
//			clickmove.enabled = false;
//		// else
//			//clickmove.enabled = true;
//		
//	}	
	void dontmove()
	{
		clickmove = GetComponent<ClickMove> ();
		clickmove.enabled = false;
		}
//	void Update(){
//
//		clickmove.enabled = false;
//	}
	
}