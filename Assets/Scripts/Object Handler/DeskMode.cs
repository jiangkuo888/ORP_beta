using UnityEngine;
using System.Collections;

public class DeskMode : MonoBehaviour {
	public string deskOwner;
	float w,h;
	public enum DeskModeSubMode: int{FileMode,PCMode,PhoneMode,None};
	
	public DeskModeSubMode mode;
	
	public GameObject FileMode;
	public GameObject PCMode;
	public GameObject TelephoneMode;
	
	
	
	Vector3 FileModeOriginalPosition;
	Vector3 PCModeOriginalPosition;
	Vector3 TelephoneModeOriginalPosition;
	
	public int FileModeFileIndex;
	public int FileModeMaxIndex;
	public Light highlight;
	
	float lightOffset;
	float cameraOffset;
	
	public bool sending;
	public bool checking;
	// email content flags
	bool computerIsOn;
	// Use this for initialization
	void Start () {
		w = Screen.width;
		h = Screen.height;
		mode = DeskModeSubMode.None;
		
		sending = false;
		checking = false;
		computerIsOn = false;
		
		FileModeOriginalPosition = FileMode.transform.position;
		//PCModeOriginalPosition = PCMode.transform.position;
		//TelephoneModeOriginalPosition = TelephoneMode.transform.position;
		
		FileModeFileIndex=1;
		lightOffset = 0.32f;
		cameraOffset = 0.47f;
		
		enableChildren();
	}
	
	void OnGUI(){
		switch (mode)
		{
		case DeskModeSubMode.FileMode:
		{
			
			
			
			if(GUI.Button( new LTRect(w - 200f, .9f*h - 50f, 100f, 50f ).rect, "Next"))
			{
				if(FileModeFileIndex<FileModeMaxIndex)
				{
					// remove viewer for current obj
					if(GameObject.Find("File"+FileModeFileIndex).GetComponent<ObjectViewer>())
						Destroy(GameObject.Find("File"+FileModeFileIndex).GetComponent<ObjectViewer>());
					// add viewer for next obj
					FileModeFileIndex++;
					Transform nextTr = GameObject.Find ("File"+FileModeFileIndex).transform;
					nextTr.gameObject.AddComponent<ObjectViewer>();
					
					// calculate the next obj mid point
					float midX = (nextTr.renderer.bounds.max.x + nextTr.renderer.bounds.min.x)/2;
					float midY = (nextTr.renderer.bounds.max.y + nextTr.renderer.bounds.min.y)/2;
					float midZ = (nextTr.renderer.bounds.max.z + nextTr.renderer.bounds.min.z)/2;
					
					LeanTween.move(Camera.main.gameObject,new Vector3(midX,midY+cameraOffset,midZ),.6f).setEase(LeanTweenType.easeOutQuint);
					LeanTween.move(highlight.gameObject,new Vector3(midX,midY+lightOffset,midZ),.6f).setEase(LeanTweenType.easeOutQuint);
					
				}
				else{
					
				}
				
			}
			if(GUI.Button( new LTRect(100f, .9f*h - 50f, 100f, 50f ).rect, "Back"))
			{
				if(FileModeFileIndex>1)
				{
					// remove viewer for current obj
					if(GameObject.Find("File"+FileModeFileIndex).GetComponent<ObjectViewer>())
						Destroy(GameObject.Find("File"+FileModeFileIndex).GetComponent<ObjectViewer>());
					// add viewer for next obj
					FileModeFileIndex--;
					Transform nextTr = GameObject.Find ("File"+FileModeFileIndex).transform;
					nextTr.gameObject.AddComponent<ObjectViewer>();
					
					
					
					float midX = (nextTr.renderer.bounds.max.x + nextTr.renderer.bounds.min.x)/2;
					float midY = (nextTr.renderer.bounds.max.y + nextTr.renderer.bounds.min.y)/2;
					float midZ = (nextTr.renderer.bounds.max.z + nextTr.renderer.bounds.min.z)/2;
					
					LeanTween.move(Camera.main.gameObject,new Vector3(midX,midY+cameraOffset,midZ),.6f).setEase(LeanTweenType.easeOutQuint);
					LeanTween.move(highlight.gameObject,new Vector3(midX,midY+lightOffset,midZ),.6f).setEase(LeanTweenType.easeOutQuint);
					
				}
				else{
					
				}
				
			}
			if(GUI.Button( new LTRect(w - 200f, .1f*h - 50f, 100f, 50f ).rect, "Back to DeskMode"))
			{
				mode = DeskModeSubMode.None;
				moveCameraToDesk();
				
			}
			
			break;
			
		}
		case DeskModeSubMode.PhoneMode:
		{
			break;
		}
		case DeskModeSubMode.PCMode:
		{
			
			if(!sending && !checking)
			{
				if(GUI.Button( new LTRect(0.6f*w, .5f*h - 50f, 100f, 50f ).rect, "Send Email"))
				{
					sending = true;
				}
				
				if(GUI.Button( new LTRect(0.6f*w, .7f*h - 50f, 100f, 50f ).rect, "Check Email"))
				{
					checking = true;
				}
			}
			else if (sending)
			{

				string[] EmailsToSend = GameObject.Find("EmailIcon").GetComponent<Email>().EmailsToBeSend;
				string[] Receivers = GameObject.Find ("EmailIcon").GetComponent<Email>().EmailReceivers;
				string[] Sender = GameObject.Find ("EmailIcon").GetComponent<Email>().EmailSenders;

				for ( int i =0;i<EmailsToSend.Length; i++)
				{

					if(Sender[i] == PhotonNetwork.playerName)
					{

						if(GUI.Button( new LTRect(0.3f*w, (0.2f+i*.1f)*h, 300f,30f).rect, EmailsToSend[i]))
						{
						
						// RPC call to display email
						PhotonView photonView = this.gameObject.GetPhotonView();
						
						
						photonView.RPC ("receiveEmail",PhotonTargets.OthersBuffered, EmailsToSend[i] , Receivers[i] ,PhotonNetwork.playerName);
						
							print(PhotonNetwork.playerName + " send: " + EmailsToSend[i] + " to " + Receivers[i]);
						
						
						
						}
					}

				}

			
			
			}
			else if (checking)
			{

				string[] receivedEmails = GameObject.Find("EmailIcon").GetComponent<Email>().EmailsReceived;
				string[] Senders = GameObject.Find ("EmailIcon").GetComponent<Email>().EmailSenders;
				// to be decided later
				for ( int i =0;i<receivedEmails.Length; i++)
				{
					
					if(GameObject.Find ("EmailIcon").GetComponent<Email>().EmailHasBeenReceived[i] == true)
					{
						
						if(GUI.Button( new LTRect(0.3f*w, (0.2f+i*.1f)*h, 300f,30f).rect, Senders[i] +" : " +receivedEmails[i]))
						{
							
							// call to clear email
							
							
						 GameObject.Find ("EmailIcon").GetComponent<Email>().clearNewEmail();
							
						}
					}

					
				}
				
			}
			
			
			if(GUI.Button( new LTRect(1.0f*w - 100f, 1.0f*h - 50f, 100f, 50f ).rect, "Back to DeskMode"))
			{
				mode = DeskModeSubMode.None;
				moveCameraToDesk();
				
			}
			
			break;
		}
		case DeskModeSubMode.None:
		{
			if(GUI.Button( new LTRect(1.0f*w - 100f, 1.0f*h - 50f, 100f, 50f ).rect, "Quit DeskMode"))
			{


				StartCoroutine(WaitAndQuit(0.3f));
				
			}
			break;
		}
			
		}
	}

	[RPC]
	void receiveEmail(string content, string receiverName, string senderName){
		if(PhotonNetwork.playerName == receiverName)
		{
			
			// enable email icon
			GameObject.Find ("EmailIcon").GetComponent<Email>().hasNewEmail(content,senderName);
			
			
			
		}
		
		
	}



	void enableChildren(){
		foreach(Transform child in transform)
		{
			child.gameObject.AddComponent<DeskObjectHandler>();
			child.gameObject.GetComponent<DeskObjectHandler>().tableName = this.name;
		}
	}
	
	void disableChildren(){
		foreach(Transform child in transform)
			if(child.gameObject.GetComponent<DeskObjectHandler>() !=null)
				Destroy(child.gameObject.GetComponent<DeskObjectHandler>());
	}
	void moveCameraToDesk(){
		
		Vector3 newPosition = this.transform.position - this.transform.forward*1.3f + new Vector3 (0,1.5f,0);
		
		
		// set camera position and rotation
		Camera.main.transform.parent = null;
		//Camera.main.transform.localPosition = new Vector3(cameraX,cameraY,cameraZ);
		//Camera.main.transform.localEulerAngles = new Vector3(90f,-90f,0);

		Camera.main.transform.localPosition = newPosition;
		Camera.main.transform.LookAt(this.transform);
		Camera.main.transform.localEulerAngles = new Vector3(Camera.main.transform.localEulerAngles.x-37f,Camera.main.transform.localEulerAngles.y,Camera.main.transform.localEulerAngles.z);

	}
	// Update is called once per frame
	void Update () {
		if(!transform.GetComponentInChildren<DeskObjectHandler>())
			enableChildren();
	}




	IEnumerator WaitAndQuit(float sec){


		GameObject.Find ("Inventory").GetComponent<inventory>().mouseOnGUIButton = true;
//		print ("111");
		disableChildren();
		GameObject.Find(deskOwner).GetComponent<DetectObjects>().moveCameraToPlayer();
		GameObject.Find(deskOwner).GetComponent<DetectObjects>().enableCameraAndMotor();
		GameObject.Find(deskOwner).GetComponent<DetectObjects>().enteredDialog = false;
		
		GetComponent<DeskMode>().enabled = false;

		yield return new WaitForSeconds (sec);

		GameObject.Find ("Inventory").GetComponent<inventory>().mouseOnGUIButton = false;

	}
	// -----------------------------------------------------------------------------------------------
//	public void OnDrawGizmos()
//	{
//		Gizmos.color = Color.red;
//
//		Gizmos.DrawLine(this.transform.position,Vector3.forward);
//			
//
//	}
}
