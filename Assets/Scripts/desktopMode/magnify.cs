using UnityEngine;	
using System.Collections;

public class magnify : MonoBehaviour 
{
	
	public bool enabled = false;
	public float distance;
	public float horizontalSpeed;
	public float verticalSpeed;
	private float sensitivityDistance = -7.5f;
	private float damping = 2.5f;
	private float ZoomedX = -0.0736046f;
	private float ZoomedY = -0.01220705f;
	private float ZoomedZ = 0.1197472f;
	private Vector3 newPos;
	private bool dragging =false;
	private Vector2 mouseDownPos;
	private Vector2 mouseDragPos;

	private Vector2 MouseMovement;
	private float deltaX,deltaZ;
	private Vector3 originalPos;

	GameObject target;

	void  Start ()
	{
		originalPos = Vector3.zero;
		distance = -10f;
		distance = transform.localPosition.y;
		deltaX =0;
		deltaZ=0;

	}
	void  Update ()
	{
		if(enabled)
		{


			target.transform.localPosition = new Vector3(ZoomedX,ZoomedY,ZoomedZ);
			
//			if(Input.GetButtonDown ("Fire2"))
//			{
//				dragging = true;
//				mouseDownPos = Input.mousePosition;
//		
//			}
//			
//			if(dragging)
//			{
//				
//				mouseDragPos = Input.mousePosition;
//				MouseMovement = mouseDragPos - mouseDownPos;
//				mouseDownPos = mouseDragPos;
//			}
//			
//			if(Input.GetButtonUp ("Fire2"))
//			{
//				
//				dragging = false;
//				// update 
//				MouseMovement = Vector2.zero;
//				//mouseDownPos = transform.localPosition;
//			}
//			
//			
//
//			
//			
//			 deltaX = deltaX + (MouseMovement.x)/(Screen.width/horizontalSpeed);
//			 deltaZ = deltaZ + (MouseMovement.y)/(Screen.height/verticalSpeed);
//
//
//			transform.localPosition = transform.localPosition - new Vector3(deltaX,0,deltaZ);

		}
	}
	public void enableZoom(GameObject targetDocument){

		target = targetDocument.transform.Find ("page_content").gameObject;


		originalPos = target.transform.localPosition;


		enabled = true;




	//	deltaX = deltaZ = 0;
	}
	
	public void disableZoom(){


		enabled = false;
		target.transform.localPosition = originalPos;
	}



	
	
}