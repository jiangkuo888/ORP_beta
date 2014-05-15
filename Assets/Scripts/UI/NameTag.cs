using UnityEngine;
using System.Collections;

[RequireComponent (typeof (GUIText))]
public class NameTag : MonoBehaviour {
	
	public string targetTag;  // Object that this label should follow
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
		if(GameObject.FindGameObjectWithTag(targetTag) !=null)
		{
			// find the nametag target in the game

			//print (targetTag);

			GameObject target = GameObject.FindGameObjectWithTag(targetTag);
			
		//	print (target.name);
			if(target.GetPhotonView().isMine == true)
			{
				// do nothing if it is mine.
				GetComponent<TextMesh>().text = "";
			}
			else 
			{
//				print (targetTag + target.name);

				// if not mine, show the name tag
				switch(targetTag)
					
				{
				case "SM":
					GetComponent<TextMesh>().text = "Sales Manager";
					break;
				case "LM":
					GetComponent<TextMesh>().text = "LPU Manager";
					break;
				case "LO":
					GetComponent<TextMesh>().text = "LPU Officer";
					break;
				case "CR":
					GetComponent<TextMesh>().text = "Credit Risk";
					break;
				default:
					break;
					

				}


				thisTransform.position = target.transform.position + offset;
				
				
				
				thisTransform.LookAt(Camera.main.transform);
			}
		}
	}
}