using UnityEngine;
using System.Collections;

public class DeskMode : MonoBehaviour {
	public string deskOwner;
	
	public enum DeskModeSubMode: int{FileMode,PCMode,pageMode,InfoMode,None};
	
	public DeskModeSubMode mode;
	
	public GameObject FileMode;
	public GameObject PCMode;
	public GameObject TelephoneMode;
	
	
	public int currentDocumentIndex;
	public int currentPageIndex;
	
	
	
	
	public Light highlight;
	
	
	
	public bool sending;
	public bool checking;
	public bool callingfacility;
	public bool callingsecurity;


	float w,h;
	float lightOffset;
	float cameraOffset;
	
	Vector3 FileModeOriginalPosition;
	Vector3 PCModeOriginalPosition;
	Vector3 TelephoneModeOriginalPosition;
	public Vector3 CameraOriginalPosition;


	// Use this for initialization
	void Start () {
		
		w = Screen.width;
		h = Screen.height;
		mode = DeskModeSubMode.None;
		
		callingfacility = false;	
		callingsecurity = false;	
		sending = false;
		checking = false;
	
		
		PCMode = GameObject.Find ("PCMode").gameObject;
		
		FileModeOriginalPosition = FileMode.transform.position;
		PCModeOriginalPosition = PCMode.transform.position;
		//TelephoneModeOriginalPosition = TelephoneMode.transform.position;
		
		currentDocumentIndex=1;
		currentPageIndex=1;
		
		
		
		lightOffset = 0.32f;
		cameraOffset = 0.47f;
		
		enableChildren();
		
		
	}
	
	void OnGUI(){
		
		
		switch (mode) // check which sub mode
		{
			
		case DeskModeSubMode.FileMode:
		{
			
			GameObject.Find ("EmailIcon").GetComponent<Email>().clearNewEmail();
			//nofunction added
			if(GUI.Button( new LTRect(w/2 - 50f, .9f*h - 100f, 100f, 30f ).rect, "Send"))
			{
				if(this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents.Length>0)
				{
					// RPC call to display email
					PhotonView photonView = this.gameObject.GetPhotonView();
					
					
					
					if(PhotonNetwork.playerName == "LPU Officer")
						photonView.RPC ("sendDocument",PhotonTargets.AllBuffered,"LPU Officer","LPU Manager",this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].name);
					if(PhotonNetwork.playerName == "LPU Manager")
						photonView.RPC ("sendDocument",PhotonTargets.AllBuffered,"LPU Manager","Credit Risk",this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].name);
					if(PhotonNetwork.playerName == "Credit Risk")
						photonView.RPC ("sendDocument",PhotonTargets.AllBuffered,"Credit Risk","LPU Manager",this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].name);
				}            
				
				// accept the document
			}
			if(GUI.Button( new LTRect(w/2 - 50f, .9f*h - 50f, 100f, 30f ).rect, "Reject"))
			{
				if(this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents.Length>0)
				{
					
					
				}
				// reject the document
			}
			
			if(GUI.Button(new LTRect(w/2 - 50f, .9f*h - 150f, 100f, 30f ).rect, "Read"))
			{
				// read the document
				if(this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents.Length>0)
				{

					CameraOriginalPosition = Camera.main.gameObject.transform.position;

					this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].GetComponent<ObjectViewer>().readDocument();
					mode = DeskModeSubMode.pageMode;
					
					
					if(this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].GetComponent<pageData>().currentPage == 
					   this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].GetComponent<pageData>().pageTextures.Length - 1)
						this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].GetComponent<pageData>().lastPage = true;
				}
				
			}
			
			
			
			
			//functions in use
			if(GUI.Button( new LTRect(w - 200f, .9f*h - 50f, 100f, 50f ).rect, "Next"))
			{
				
				
				
				
				int documentIndex = this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents.Length;
				
				
				
				
				if(currentDocumentIndex<documentIndex)
				{
					// remove viewer for current obj
					if(this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].GetComponent<ObjectViewer>())
					{
						this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].GetComponent<ObjectViewer>().resetDocumentPosition();
						Destroy(this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].GetComponent<ObjectViewer>());
					}
					// add viewer for next obj
					currentDocumentIndex++;
					Transform nextTr = this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].transform;
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
				
				
				
				
				
				
				
				if(currentDocumentIndex>1)
				{
					// remove viewer for current obj
					if(this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].GetComponent<ObjectViewer>())
					{
						this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].GetComponent<ObjectViewer>().resetDocumentPosition();
						Destroy(this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].GetComponent<ObjectViewer>());
					}
					// add viewer for next obj
					currentDocumentIndex--;
					Transform nextTr = this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].transform;
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
			if(GUI.Button( new LTRect(1.0f*w - 100f, 1.0f*h - 50f, 100f, 50f ).rect, "Back to DeskMode"))
			{
				mode = DeskModeSubMode.None;
				moveCameraToDesk();
				
			}
			
			break;
			
		}
		case DeskModeSubMode.pageMode:
		{
			
			if(GUI.Button( new LTRect(w - 200f, .9f*h - 50f, 100f, 50f ).rect, "Next Page"))
			{
				this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].GetComponent<pageData>().showNextPage();
			}
			
			
			if(GUI.Button( new LTRect(100f, .9f*h - 50f, 100f, 50f ).rect, "Previous Page"))
			{
				this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].GetComponent<pageData>().showPreviousPage();
			}
			

				
			

				if(GUI.Button( new LTRect(1.0f*w - 100f, 1.0f*h - 50f, 100f, 50f ).rect, "Back to Documents"))
				{
					
				mode = DeskModeSubMode.FileMode;
					this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].GetComponent<pageData>().lastPage = false;
					this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].GetComponent<ObjectViewer>().playCloseFileAnim();
					this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].GetComponent<ObjectViewer>().resetDocumentPosition();
					
				}
			
			
			
			
			if(GUI.Button( new LTRect(.5f*w - 50f, .9f*h - 50f, 100f, 50f ).rect, "Verify"))
			{
				
				Transform thisTr = this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].transform;
				GameObject pc = GameObject.Find ("PCMode").gameObject;
				pc.transform.position = new Vector3(thisTr.position.x + .35f, thisTr.position.y-.1f , thisTr.position.z);
				
				
				float midX = (pc.transform.renderer.bounds.max.x + thisTr.renderer.bounds.min.x)/2 - .15f;
				float midY = (pc.transform.renderer.bounds.max.y + pc.transform.renderer.bounds.min.y)/2;
				float midZ = (pc.transform.renderer.bounds.max.z + pc.transform.renderer.bounds.min.z)/2;
				
				
				
				LeanTween.move(Camera.main.gameObject,new Vector3(midX,midY+cameraOffset-.05f,midZ),.6f).setEase(LeanTweenType.easeOutQuint).setOnComplete(enablePCmode);

			}
			
			
			
			break;
		}
		case DeskModeSubMode.PCMode:
		{


			GameObject.Find("PCscreen").GetComponent<pcMode>().deskTop = this.gameObject;



			if(GUI.Button( new LTRect(w - 200f, .9f*h - 50f, 100f, 50f ).rect, "Next Page"))
			{
				this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].GetComponent<pageData>().showNextPage();
			}
			
			
			if(GUI.Button( new LTRect(100f, .9f*h - 50f, 100f, 50f ).rect, "Previous Page"))
			{
				this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].GetComponent<pageData>().showPreviousPage();
			}


			if(GUI.Button( new LTRect(1.0f*w - 100f, 1.0f*h - 50f, 100f, 50f ).rect, "Back to read page"))
			{

				if(GameObject.Find("PCscreen").GetComponent<pcMode>().enabled == true)
					GameObject.Find ("PCscreen").GetComponent<pcMode>().enabled = false;
				
				if(this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents.Length>0)
				{
					if(this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents.Length>0)
					{
						LeanTween.move(Camera.main.gameObject,CameraOriginalPosition,.6f).setEase(LeanTweenType.easeOutQuint);
						mode = DeskModeSubMode.pageMode;

						
						if(this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].GetComponent<pageData>().currentPage == 
						   this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].GetComponent<pageData>().pageTextures.Length - 1)
							this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].GetComponent<pageData>().lastPage = true;

						LeanTween.move(PCMode,PCModeOriginalPosition,.6f).setEase(LeanTweenType.easeOutQuint);
					}
				}
				
			}
			
			break;
		}




		case DeskModeSubMode.InfoMode:
		{
			if(GUI.Button( new LTRect(1.0f*w - 100f, 1.0f*h - 50f, 100f, 50f ).rect, "Back to Compare mode"))
			{

				Transform thisTr = this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].transform;
				GameObject pc = GameObject.Find ("PCMode").gameObject;

				
				
				float midX = (pc.transform.renderer.bounds.max.x + thisTr.renderer.bounds.min.x)/2 - .15f;
				float midY = (pc.transform.renderer.bounds.max.y + pc.transform.renderer.bounds.min.y)/2;
				float midZ = (pc.transform.renderer.bounds.max.z + pc.transform.renderer.bounds.min.z)/2;
				

				if(GameObject.Find("PCscreen").GetComponent<pcMode>().enabled == true)
					GameObject.Find ("PCscreen").GetComponent<pcMode>().enabled = false;


				
				LeanTween.move(Camera.main.gameObject,new Vector3(midX,midY+cameraOffset-.05f,midZ),.6f).setEase(LeanTweenType.easeOutQuint).setOnComplete(enablePCmode);

				




				






				
			}
			break;
		}

		case DeskModeSubMode.None:
		{
			if(GUI.Button( new LTRect(1.0f*w - 100f, 1.0f*h - 50f, 100f, 50f ).rect, "Quit DeskMode"))
			{
				if(GameObject.Find ("InventoryObj").GetComponent<inventory>().inventoryObject !=null)
					GameObject.Find ("InventoryObj").GetComponent<GUITexture>().enabled = true;



				GameObject.Find ("InventoryContainer").GetComponent<GUITexture>().enabled = true;
				GameObject.Find ("InventoryButton1").GetComponent<GUITexture>().enabled = true;
				GameObject.Find ("InventoryButton2").GetComponent<GUITexture>().enabled = true;


				GameObject.Find ("phoneButton").GetComponent<GUITexture>().enabled = true;
				GameObject.Find ("QuestLogButton").GetComponent<GUITexture>().enabled = true;
				
				StartCoroutine(WaitAndQuit(0.3f));
				
			}
			break;
		}
			
			
			
			
		}
		
		
		
		
	}
	
	void enablePCmode()
	{
		if(GameObject.Find("PCscreen").GetComponent<pcMode>().enabled == false)
			GameObject.Find ("PCscreen").GetComponent<pcMode>().enabled = true;
		
		mode = DeskModeSubMode.PCMode;
		GameObject.Find("PCscreen").GetComponent<pcMode>().InfoModeIsOn = false;

	}
	
	
	
	[RPC]
	void receiveEmail(string content, string receiverName, string senderName){
		if(PhotonNetwork.playerName == receiverName)
		{
			
			// enable email icon
			GameObject.Find ("EmailIcon").GetComponent<Email>().hasNewEmail(content,senderName);
			
			
			
		}
		
		
	}
	
	[RPC]
	
	void sendDocument(string sender, string receiver,string documentName){
		
		// receiver action
		if(PhotonNetwork.playerName == receiver)
		{
			
			GameObject document = GameObject.Find (documentName);
			
			//			if(document)
			//				print (document.name);
			//
			//			if(GameObject.Find (receiver+" Table").gameObject.transform.Find ("DocumentHolder").GetComponent<documentData>())
			//				print ("got data");
			
			
			GameObject.Find (receiver+" Table").gameObject.transform.Find ("DocumentHolder").GetComponent<documentData>().addDocument(document);
			
			
			
			// notify the receiver 
			GameObject.Find ("EmailIcon").GetComponent<Email>().hasNewDocument(sender);
		}
		
		// document sender action
		if(PhotonNetwork.playerName == sender)
		{
			
			GameObject document = this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1];
			this.transform.Find ("DocumentHolder").GetComponent<documentData>().removeDocument(document);
			// move the document out of the table
			document.transform.parent = GameObject.Find ("AllDocuments").transform;
			document.transform.localPosition = new Vector3(0,0,0);
			// move the first document position to next document
			
			if(this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents.Length >0)
				GameObject.Find ("documentHidden").transform.position = this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[0].transform.position;
			
			
			// quit the deskMode
			mode = DeskModeSubMode.None;
			moveCameraToDesk();
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
		
		

		//		print ("111");
		disableChildren();
		GameObject.Find(deskOwner).GetComponent<DetectObjects>().moveCameraToPlayer();
		GameObject.Find(deskOwner).GetComponent<DetectObjects>().enableCameraAndMotor();
		GameObject.Find(deskOwner).GetComponent<DetectObjects>().enteredDialog = false;
		
		GetComponent<DeskMode>().enabled = false;
		
		yield return new WaitForSeconds (sec);
		

		
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
