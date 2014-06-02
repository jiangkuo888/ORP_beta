using UnityEngine;
using System.Collections;

[RequireComponent (typeof (GUIText))]
public class NameTag : MonoBehaviour {

	public Vector3 offset = Vector3.up;    // Units in world space to offset; 1 unit above object by default
	public bool clampToScreen = false;  // If true, label will be visible even if object is off screen
	public float clampBorderSize = 0.05f;  // How much viewport space to leave at the borders when a label is being clamped
	public bool useMainCamera = true;   // Use the camera tagged MainCamera
	public Camera cameraToUse ;   // Only use this if useMainCamera is false
	Camera cam ;
	Transform thisTransform;
	Transform camTransform;
	
	void Start () 
	{
		thisTransform = transform;
		if (useMainCamera)
			cam = Camera.main;
		else
			cam = cameraToUse;
		camTransform = cam.transform;
	}
	
	
	void Update()
	{
	

			GameObject target = this.transform.parent.gameObject;
			
		//	print (target.name);
			if(target.GetPhotonView().isMine == true)
			{
				// do nothing if it is mine.
				GetComponent<TextMesh>().text = "";
			}
			else 
			{
				thisTransform.position = target.transform.position + offset;

				
				
				
				
				thisTransform.LookAt(Camera.main.transform);
			}
		}
	}
