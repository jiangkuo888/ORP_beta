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
	private float min = 0.3505293f;
	private float max = 0.4711867f;
	private Vector3 newCamPos;
	private bool dragging =false;
	private Vector2 mouseDownPos;
	private Vector2 mouseDragPos;
	private Vector2 lastPos;
	private Vector2 MouseMovement;
	private float deltaX,deltaZ;
	private Vector3 originalPos;
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
			distance -= Input.GetAxis("Mouse ScrollWheel") * sensitivityDistance;
			distance = Mathf.Clamp(distance, min, max);
			newCamPos.y = Mathf.Lerp(transform.localPosition.y, distance, Time.deltaTime * damping);
			transform.localPosition = newCamPos;
			
			if(Input.GetButtonDown ("Fire2"))
			{
				dragging = true;
				mouseDownPos = Input.mousePosition;
				lastPos = mouseDownPos;
			}
			
			if(dragging)
			{
				
				mouseDragPos = Input.mousePosition;
				MouseMovement = mouseDragPos - lastPos;
				lastPos = mouseDragPos;
			}
			
			if(Input.GetButtonUp ("Fire2"))
			{
				
				dragging = false;
				// update 
				MouseMovement = Vector2.zero;
				//mouseDownPos = transform.localPosition;
			}
			
			

			
			
			 deltaX = deltaX + (MouseMovement.x)/(Screen.width/horizontalSpeed);
			 deltaZ = deltaZ + (MouseMovement.y)/(Screen.height/verticalSpeed);
			
			transform.localPosition = new Vector3(transform.localPosition.x*(1+deltaX),transform.localPosition.y,transform.localPosition.z*(1+deltaZ));
			
			
		}
	}
	public void enableZoom(){
		if(originalPos == Vector3.zero)
			originalPos= transform.localPosition;
		else
		transform.localPosition = originalPos;


		enabled = true;
		newCamPos = transform.localPosition;
		distance = max;
		deltaX = deltaZ = 0;
	}
	
	public void disableZoom(){
		enabled = false;

	}
	
	
	
}