using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Camera-Control/Mouse Look")]
public class AdminCamera : Photon.MonoBehaviour {

	public Vector3 relativePosition = new Vector3(0,0, 0);
	public int currPlayerFollow;
	public string[] playerNameList = new string[] {"Sales Manager", "LPU Officer", "LPU Manager", "Credit Risk"};
	public Vector3 lastPos;
	public Quaternion lastRot;

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2, MouseNone = 3 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 5F;
	public float sensitivityY = 5F;
	
	public float minimumX = -360F;
	public float maximumX = 360F;
	
	public float minimumY = -60F;
	public float maximumY = 60F;
	
	
	float rotationY = 0F;
	float originalY = 0F;

	// Use this for initialization
	void Start () {
		this.currPlayerFollow = -1;
		originalY = transform.localEulerAngles.x;
	}
	
	// Update is called once per frame
	void Update () {

		GameObject gameManager = GameObject.Find ("GameManager");  
		GameManagerVik vikky = gameManager.GetComponent<GameManagerVik> ();
		HashSet<string> selectedPlayerList = vikky.selectedPlayerList;
	
		//if admin watch other players
		if (Input.GetKeyUp ("space") && selectedPlayerList.Count > 0) 
		{
			currPlayerFollow++;
			while (true)
			{
				if (currPlayerFollow >= playerNameList.Length)
				{
					//goes back to admin
					this.gameObject.transform.position = lastPos;
					this.gameObject.transform.rotation = lastRot;
					currPlayerFollow = -1;
					break;

				}
				else
				{

					if (selectedPlayerList.Contains(playerNameList[currPlayerFollow]))
					{
						//save current position and rotation
						this.lastPos = this.gameObject.transform.position;
						this.lastRot = this.gameObject.transform.rotation;

						//toggle to next player
						this.gameObject.transform.position = GameObject.Find(playerNameList[currPlayerFollow]+"(Clone)").transform.position;
						this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
						Vector3 cameraEuler = GameObject.Find(playerNameList[currPlayerFollow]+"(Clone)").GetComponent<ThirdPersonNetworkVik>().cloneCameraRotation;
						this.gameObject.transform.rotation = Quaternion.Euler(cameraEuler.x, cameraEuler.y, cameraEuler.z);

						//mainCam.transform.localPosition = relativePosition;
						//mainCam.transform.localEulerAngles = new Vector3(0, 90, 0);

						break;
					}
					else
					{
						currPlayerFollow++;
					}

				}
			}

		}

		//mouse camera
		if(Input.GetButton ("Fire2") && this.currPlayerFollow == -1)
		{
			if (axes == RotationAxes.MouseXAndY)
			{
				float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
				
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				
				transform.localEulerAngles = new Vector3(-rotationY, rotationX, /*rotationY*/0);
				//Debug.Log(transform.right);
			}
			else if (axes == RotationAxes.MouseX)
			{
				transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
			}
			else if(axes == RotationAxes.MouseY)
			{
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				
				transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
			}
		}

		if (this.currPlayerFollow != -1)
		{
			//Debug.Log(GameObject.Find(playerNameList[currPlayerFollow]+"(Clone)").GetComponent<ThirdPersonNetworkVik>().cloneCameraRotation);
			//movwe to where following player is
			this.gameObject.transform.position = GameObject.Find(playerNameList[currPlayerFollow]+"(Clone)").transform.position;
			this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
			Vector3 cameraEuler = GameObject.Find(playerNameList[currPlayerFollow]+"(Clone)").GetComponent<ThirdPersonNetworkVik>().cloneCameraRotation;
			this.gameObject.transform.rotation = Quaternion.Euler(cameraEuler.x, cameraEuler.y, cameraEuler.z);
		}

	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			
			//do nothing
			
		}
		else
		{
			//do nothing
		}
	}

}
