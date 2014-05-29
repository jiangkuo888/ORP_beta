using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem.Examples;
using PixelCrushers.DialogueSystem;

public class AdminMovement : Photon.MonoBehaviour
{
	MouseCamera cameraScript;
	MouseCamera playerRotationScript;
	
	public Vector3 cameraRelativePosition = new Vector3(0,1.257728f, 0);
	float rotationY = 0F;
	float originalY = 0F;
	public GUIStyle custom;
	
	public bool isEventPopOut;
	public bool isEventDesc;
	public int eventNum;
	
	void Awake()
	{
		isEventPopOut = false;
		isEventDesc = false;
		eventNum = 1;
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
		if (photonView.isMine)
		{
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
				if (GUILayout.Button ("Fraud", GUILayout.Width (100))) {
					isEventPopOut = false;
					isEventDesc = true;
					eventNum = 1;

				}
				if (GUILayout.Button ("Leave", GUILayout.Width (100))) {
					isEventPopOut = false;
					isEventDesc = true;
					eventNum = 2;
				}
				GUILayout.EndArea();
			}


			//event description
			string eventDesc = "";
			if (isEventDesc)
			{
				switch (eventNum) 
				{
					case 1:
						eventDesc = "Credit risk gets a call from his peer is ABS Bank regarding\n a fraudulent loan sydicate that they have identified.";
						break;

					case 2:
					eventDesc = "Credit Risk finds one of his staff has not taken her\n block leave and there are two months left in the year.";
						break;
				}

				GUILayout.BeginArea (new Rect (Screen.width*0.25f, Screen.height*.5f, Screen.width/2, Screen.height/2));
				
				GUILayout.Box (eventDesc);
				
				GUILayout.BeginHorizontal();
				
				if (GUILayout.Button ("Activate", GUILayout.Width (80))) {

					if (eventNum == 1)
					{
						phoneButton buttony = GameObject.Find("phoneButton").GetComponent<phoneButton>();
						buttony.OnCallRPC("Credit Risk", "Peer phone call");
						isEventDesc = false;
					}
					else if (eventNum == 2)
					{
						PhotonView photonViewTwo = this.gameObject.GetPhotonView();
						photonViewTwo.RPC ("sendAlert", PhotonTargets.OthersBuffered);
					}
				}
				
				
				if (GUILayout.Button ("Cancel", GUILayout.Width (80))) {
					
					isEventPopOut = true;
					isEventDesc = false;
					
				}
				GUILayout.EndHorizontal();
				
				GUILayout.EndArea();

			}
		}
	
	}

	[RPC]
	public void sendAlert()
	{

		if (PhotonNetwork.playerName == "Credit Risk")
		{
			GameObject.Find("Dialogue Manager").GetComponent<DialogueSystemController>().ShowAlert("You have a new file on your table");
		}

	}
	
	
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
	}
}

