using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;


public class DocumentHandler : MonoBehaviour {
	public GUISkin customSkin;
	public string documentName;
	//public GameObject pagePrototype;
	public GameObject[] pages;
	
	public GameObject LO_signature;
	public GameObject LM_signature;
	public GameObject CR_signature;
	PlayMakerFSM EventFSM;
	
	//Kris test access of enum from DeskMode.cs
	
	
	//GameObject newPage;
	public int currentPageIndex;
	
	//public bool lastPage;
	float w,h;
	
	public bool LO_signed,LM_signed,CR_signed;
	
	
	public bool correct_document;
	
	public string mode = "FileMode";

	public string pick_up_doc_id;
	public string deposit_doc_id;

	// wrong document IDs
	public string reject_wrong_doc;
	public string sign_wrong_doc;
	public string send_unsign_wrong_doc;
	public string send_sign_wrong_doc;
	
	// correct document IDs
	public string reject_correct_doc;
	public string sign_correct_doc;
	public string send_unsign_correct_doc;
	public string send_sign_correct_doc;
	
	public Rect signArea = new Rect(50f, 50f, 100f, 50f );
	
	// Use this for initialization
	void Start () {
		EventFSM = GameObject.Find ("EventManager-Tutorial").GetComponent<PlayMakerFSM>();
		LO_signed = false;
		LM_signed = false;
		CR_signed = false;
		
		w = Screen.width;
		h = Screen.height;
		

		currentPageIndex = 0;
		
		//if(pagePrototype == null)
			//Debug.LogError("please attach a page model for " + this.name);
		
		if(documentName == null)
			Debug.LogError("please assign a document name for " + this.name);
		
		
		collectPageToList();
		
		
		
//		newPage = Instantiate(pagePrototype.gameObject,pagePrototype.transform.position,Quaternion.identity) as GameObject;
//		newPage.renderer.material.mainTexture = pageTextures[currentPage];
//		newPage.transform.parent = this.transform;
//		newPage.name = "page_content";
//		newPage.transform.localPosition = new Vector3(-0.0002115475f,0.001669302f ,0.001225402f);
//		newPage.transform.localEulerAngles = new Vector3(90,0,0);
		


		// define pickup ID
		pick_up_doc_id = "20H";

		// define deposit safe ID
		deposit_doc_id = "21B";
		
		// define actionID
		reject_wrong_doc = "11A";
		sign_wrong_doc = "11B";
		send_unsign_wrong_doc = "11C";
		send_sign_wrong_doc = "11D";
		
		// correct document IDs
		reject_correct_doc = "12A";
		sign_correct_doc = "12B";
		send_unsign_correct_doc = "12C";
		send_sign_correct_doc = "12D";
	}
	void Update(){

		
	}

	
	void collectPageToList(){
		
		foreach( Transform child in transform)
		{
			if(child.name.Contains("page"))
			{
				addToList(child);
				child.gameObject.SetActive(false);
			}
			
		}

		pages[0].SetActive(true);


	}
	void addToList(Transform tr){
		
		System.Collections.Generic.List<GameObject> list = new System.Collections.Generic.List<GameObject>(pages);
		
		if(!list.Contains(tr.gameObject))
			list.Add(tr.gameObject);
		
		pages = list.ToArray();
		
		
	}
	
	void OnGUI(){
		if (mode == "PCMode") {
			//print ("pc");
						signArea = new Rect ( w/4 - 50f, .9f * h - 50f, 100f, 50f);
				}
		if (mode == "FileMode") {
			//print ("file");
						signArea = new Rect (.5f * w - 50f, .9f * h - 105f, 100f, 50f);
				}


		if(pages[currentPageIndex].GetComponent<pageHandler>().isLastPage)
		{
			if(LO_signed)
				LO_signature.SetActive(true); 
			if(LM_signed)
				LM_signature.SetActive(true);
			if(CR_signed)
				CR_signature.SetActive(true);
			
			switch(PhotonNetwork.playerName)
			{
			case "LPU Officer":
				
				if(!LO_signed)
					
				{
					if(GUI.Button( signArea, "Sign", customSkin.button))
					{
						PhotonView photonView = this.gameObject.GetPhotonView();
						photonView.RPC ("signDoc",PhotonTargets.AllBuffered,"LO");
						
						if(EventFSM.enabled)
							if(EventFSM.ActiveStateName == "Sign")
								EventFSM.FsmVariables.GetFsmBool("signed").Value = true;
						// log the user action in database
						if(correct_document)
						{
							GameObject.Find ("Dialogue Manager").GetComponent<PlayerActionLog>().addToPlayerActionLog(sign_correct_doc,"LPU Officer signed correct document "+this.name);
						}
						else
						{
							GameObject.Find ("Dialogue Manager").GetComponent<PlayerActionLog>().addToPlayerActionLog(sign_wrong_doc,"LPU Officer signed wrong document "+this.name);
							
						}
					}
				}
				
				else
					
				{
					//photonView.RPC ("sendDocument",PhotonTargets.AllBuffered,"Sales Manager","LPU Officer",this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].name);
					
				}           
				
				// accept the document

				break;
				
				
				
				
				
			case "LPU Manager":
				
				if(!LM_signed)
					
				{
					if(GUI.Button( signArea, "Sign", customSkin.button))
					{
						PhotonView photonView = this.gameObject.GetPhotonView();
						photonView.RPC ("signDoc",PhotonTargets.AllBuffered,"LM");
						
						if(EventFSM.enabled)
							if(EventFSM.ActiveStateName == "Sign")
								EventFSM.FsmVariables.GetFsmBool("signed").Value = true;
						// log the user action in database
						if(correct_document)
						{
							GameObject.Find ("Dialogue Manager").GetComponent<PlayerActionLog>().addToPlayerActionLog(sign_correct_doc,"LPU Manager signed correct document "+this.name);
						}
						else
						{
							GameObject.Find ("Dialogue Manager").GetComponent<PlayerActionLog>().addToPlayerActionLog(sign_wrong_doc,"LPU Manager signed wrong document "+this.name);
							
						}
					}
				}
				
				else
					
				{
					//photonView.RPC ("sendDocument",PhotonTargets.AllBuffered,"Sales Manager","LPU Officer",this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].name);
					
				}           
				
				// accept the document
				
				break;
				
			case "Credit Risk":
				
				if(!CR_signed)
					
				{
					if(GUI.Button( new LTRect(.5f*w - 50f, .9f*h - 105f, 100f, 50f ).rect, "Sign", customSkin.button))
					{
						
						
						
						PhotonView photonView = this.gameObject.GetPhotonView();
						photonView.RPC ("signDoc",PhotonTargets.AllBuffered,"CR");
						
						
						if(EventFSM.enabled)
							if(EventFSM.ActiveStateName == "Sign")
								EventFSM.FsmVariables.GetFsmBool("signed").Value = true;
						// log the user action in database
						if(correct_document)
						{
							GameObject.Find ("Dialogue Manager").GetComponent<PlayerActionLog>().addToPlayerActionLog(sign_correct_doc,"Credit Risk signed correct document "+this.name);
						}
						else
						{
							GameObject.Find ("Dialogue Manager").GetComponent<PlayerActionLog>().addToPlayerActionLog(sign_wrong_doc,"Credit Risk signed wrong document "+this.name);
							
						}
					}
				}
				
				else
					
				{
					//photonView.RPC ("sendDocument",PhotonTargets.AllBuffered,"Sales Manager","LPU Officer",this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].name);
					
				}           
				
				// accept the document
				
				break;
				
				
			default:
				break;
				
				
			}
			
		}
		
		else // not the last page
		{

				LO_signature.SetActive(false);
			

				LM_signature.SetActive(false);
			

				CR_signature.SetActive(false);
			
		}
		
		
	}
	
	[RPC]
	
	public void signDoc(string signerName){
		
		switch(signerName)
		{
		case "LO":
			LO_signed = true;
			
			break;
			
		case "LM":
			LM_signed = true;
			
			break;
			
		case "CR":
			CR_signed = true;
			
			break;
		default:
			break;
			
			
			
			
		}
	}
	
	
	
	public void showNextPage(){
		
		
		if(currentPageIndex < pages.Length-1)
		{
			// set current page inactive
			pages[currentPageIndex].SetActive(false);

			// increment the page index
			currentPageIndex++;

			// set next page active
			pages[currentPageIndex].SetActive(true);
			
			if(currentPageIndex == pages.Length - 1)
			{
				pages[currentPageIndex].GetComponent<pageHandler>().isLastPage = true;
			}
			else{
				
				pages[currentPageIndex].GetComponent<pageHandler>().isLastPage = false;
			}
		}
		
		
		
		
	}
	
	public void showPreviousPage(){
		if(currentPageIndex> 0)
		{
			// set current page inactive
			pages[currentPageIndex].SetActive(false);
			
			// increment the page index
			currentPageIndex--;
			
			// set next page active
			pages[currentPageIndex].SetActive(true);

		
			
			if(currentPageIndex == pages.Length - 1)
			{
				pages[currentPageIndex].GetComponent<pageHandler>().isLastPage = true;
			}
			else{
				
				pages[currentPageIndex].GetComponent<pageHandler>().isLastPage = false;
			}
		}
	}



}



