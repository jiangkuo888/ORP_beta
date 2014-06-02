using UnityEngine;
using System.Collections;

[RequireComponent (typeof (GUIText))]
public class NameTag : MonoBehaviour {

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


		thisTransform.localPosition = new Vector3(0,2.1f,0);
		thisTransform.localEulerAngles= new Vector3(0,90f,0);
		thisTransform.localScale = new Vector3(0.03330903f,0.0342589f,0.03330902f);
	}

//	public void resetPRS(){
//		
//		thisTransform.localPosition = new Vector3(0,2.1f,0);
//		thisTransform.localEulerAngles= new Vector3(0,90f,0);
//		thisTransform.localScale = new Vector3(0.03330903f,0.0342589f,0.03330902f);
//
//	}
	
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
				thisTransform.localPosition = new Vector3(0,2.1f,0);
				
				thisTransform.LookAt(Camera.main.transform);


			}
		}
	}
