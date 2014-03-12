using UnityEngine;	
using System.Collections;

public class magnify : MonoBehaviour 
{
	
	public bool enabled = false;
	public float distance;
	private float sensitivityDistance = -7.5f;
	private float damping = 2.5f;
	private float min = 0.3505293f;
	private float max = 0.4711867f;
	private Vector3 newCamPos;
	private bool dragging =false;
	private Vector2 mouseDownPos;
	private Vector2 mouseDragPos;
	void  Start ()
	{
		distance = -10f;
		distance = transform.localPosition.y;
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
			}
			
			if(dragging)
			{
				
				mouseDragPos = Input.mousePosition;
			}
			
			if(Input.GetButtonUp ("Fire2"))
			{
				
				dragging = false;
				
			}
			
			
			
			
			
			float deltaX = (mouseDragPos.x - mouseDownPos.x)/(Screen.width/3);
			float deltaZ = (mouseDragPos.y - mouseDownPos.y)/(Screen.height/2);
			
			transform.localPosition = new Vector3(transform.localPosition.x*(1+deltaX),transform.localPosition.y,transform.localPosition.z*(1+deltaZ));
			
			
		}
	}
	public void enableZoom(){
		enabled = true;
		newCamPos = transform.localPosition;
	}
	
	public void disableZoom(){
		enabled = false;
	}
	
	
	
}