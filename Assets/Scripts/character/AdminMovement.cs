using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem.Examples;

public class AdminMovement : Photon.MonoBehaviour
{
	MouseCamera cameraScript;
	MouseCamera playerRotationScript;
	
	public Vector3 cameraRelativePosition = new Vector3(0,1.257728f, 0);
	float rotationY = 0F;
	float originalY = 0F;
	public GUIStyle custom;
	
	public bool isEventPopOut;
	
	void Awake()
	{
		isEventPopOut = false;
	}
	
	void Start()
	{
		
		//TODO: Bugfix to allow .isMine and .owner from AWAKE!
		if (photonView.isMine)
		{
			
			Camera.main.transform.parent = transform;
			Camera.main.transform.localPosition = cameraRelativePosition;
			Camera.main.transform.localEulerAngles = transform.localEulerAngles;//new Vector3(0, 90, 0);
			Camera.main.farClipPlane = 100.0f;
			
			if(cameraScript == null)
				cameraScript = GameObject.Find ("Main Camera").GetComponent<MouseCamera>();
			if(playerRotationScript == null)
				playerRotationScript = transform.GetComponent<MouseCamera>();
			
			
			//playerRotationScript.enabled = false;
			cameraScript.enabled = false;
			
			//if admin switch on AdminCamera script
			//AdminCamera admincamera = GameObject.Find ("Main Camera").GetComponent<AdminCamera>();
			//admincamera.enabled = true;
			
		}
		else
		{           
			
			//if(playerRotationScript == null)
			//	playerRotationScript = transform.GetComponent<MouseCamera>();
			
			//playerRotationScript.enabled = false;
			
		}
		
	}
	
	void Update()
	{
		AdminCamera addy = this.gameObject.GetComponent<AdminCamera> ();
		int currFollow = addy.currPlayerFollow;
		
		if (Input.GetKey("w") && currFollow == -1)
		{
			//move forward
			Vector3 move = addy.transform.forward;
			move.x = 0.0f;
			//Debug.Log ("move");
			//Debug.Log (move);
			
			//special case x
			if (transform.right.x < 0)
			{
				move.z = -move.z;
			}
			//	Debug.Log ("move");
			//	Debug.Log (move);
			/*if (transform.right.x < 0.1f && transform.right.x > 0.0f)
			{
				move.x = 1.0f - move.x;
			}
			else if (transform.right.x > -0.1f && transform.right.x < 0.0f)
			{
				move.x = 1.0f + move.x;
			}
			if (transform.right.y < 0.1f && transform.right.y > 0.0f)
			{
				move.x = 1.0f - move.y;
			}
			else if (transform.right.y > -0.1f && transform.right.y < 0.0f)
			{
				move.x = 1.0f + move.y;
			}*/
			
			if (move.y < 0.1 || move.z < 0.1)
			{
				move.z = 1;
			}
			else
			{
				move.Normalize();
			}
			
			//Debug.Log ("forward");
			//Debug.Log (addy.transform.forward);
			//Debug.Log ("move");
			//Debug.Log (move);
			transform.Translate(move * 0.2f);
			
		}
		
		if (Input.GetKeyUp ("left ctrl"))
		{
			isEventPopOut = !isEventPopOut;
		}
	}
	
	void OnGUI()
	{
		//get admin follow
		AdminCamera addy = this.gameObject.GetComponent<AdminCamera>();
		
		GUILayout.BeginArea (new Rect (0, Screen.height*.256f, Screen.width/2, Screen.height*.5f));
		
		if (addy.currPlayerFollow == -1)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label ("Hello! Welcome to trainer mode.", custom);
			GUILayout.EndHorizontal();
		}
		else
		{
			string name =  addy.playerNameList[addy.currPlayerFollow];
			GUILayout.BeginHorizontal();
			GUILayout.Label ("You are currently following: " + name, custom);
			GUILayout.EndHorizontal(); 
			
		}
		
		GUILayout.	EndArea();
		
		
		if (isEventPopOut)
		{
			GUILayout.BeginArea (new Rect (0, Screen.height*.75f, Screen.width, Screen.height*.25f));
			if (GUILayout.Button ("EventOne", GUILayout.Width (100))) {
			}
			if (GUILayout.Button ("EventTwo", GUILayout.Width (100))) {
			}
			GUILayout.EndArea();
		}
		
	}
	
	
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
	}
}

