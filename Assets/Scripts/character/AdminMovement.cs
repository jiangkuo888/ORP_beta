﻿using UnityEngine;
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

	void Awake()
	{
	
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
			//Debug.Log (transform.forward);
			Vector3 move = addy.transform.forward;
			move.x = 0.0f;

			//special case x
			if (transform.right.x < 0)
			{
				move.z = -move.z;
			}
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
			move.Normalize();
			transform.Translate(move * 0.2f);

		}
	}

	void OnGUI()
	{
		//get admin follow
		AdminCamera addy = this.gameObject.GetComponent<AdminCamera>();

		GUILayout.BeginArea (new Rect (0, Screen.height*.275f, Screen.width/2, Screen.height*.5f));

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

		GUILayout.EndArea();
	}

	
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
	}
}

