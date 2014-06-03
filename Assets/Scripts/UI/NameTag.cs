using UnityEngine;
using System.Collections;

[RequireComponent (typeof (GUIText))]
public class NameTag : MonoBehaviour {
	
	public bool useMainCamera = true;   // Use the camera tagged MainCamera
	public Camera cameraToUse ;   // Only use this if useMainCamera is false
	Camera cam ;
	Transform thisTransform;
	Transform parentTransform;
	public string TargetTag;

	
	void Start () 
	{



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
		
		
		
		GameObject Target = GameObject.FindGameObjectWithTag(TargetTag);

		if(Target != null)
		{

			transform.position = Target.transform.position + new Vector3(0,1.2f,0);

			transform.LookAt(Camera.main.transform.position);

		}
	
		
		
		
		
	}

}
