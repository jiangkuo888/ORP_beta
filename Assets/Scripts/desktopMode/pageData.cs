using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;


public class pageData : MonoBehaviour {
	public GUISkin customSkin;
	public string documentName;
	public GameObject pagePrototype;
	public Texture2D[] pageTextures;
	
	public GameObject LO_signature;
	public GameObject LM_signature;
	public GameObject CR_signature;
	PlayMakerFSM EventFSM;
	
	//Kris test access of enum from DeskMode.cs
	
	
	GameObject newPage;
	public int currentPage;
	
	public bool lastPage;
	float w,h;
	
	public bool LO_signed,LM_signed,CR_signed;
	
	
	public bool correct_document;
	
	public string mode = "FileMode";
	
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
		
		lastPage = false;
		currentPage = 0;
		
		if(pagePrototype == null)
			Debug.LogError("please attach a page model for " + this.name);
		
		if(documentName == null)
			Debug.LogError("please assign a document name for " + this.name);
		
		
		
		
		
		
		newPage = Instantiate(pagePrototype.gameObject,pagePrototype.transform.position,Quaternion.identity) as GameObject;
		newPage.renderer.material.mainTexture = pageTextures[currentPage];
		newPage.transform.parent = this.transform;
		newPage.name = "page_content";
		newPage.transform.localPosition = new Vector3(-0.0002115475f,0.001669302f ,0.001225402f);
		newPage.transform.localEulerAngles = new Vector3(90,0,0);
		
		
		
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
	
	void OnGUI(){
		if (mode == "PCMode") {
			print ("pc");
						signArea = new Rect (.5f * w - 250f, .9f * h - 105f, 100f, 50f);
				}
		if (mode == "FileMode") {
			print ("file");
						signArea = new Rect (.5f * w - 50f, .9f * h - 105f, 100f, 50f);
				}


		if(lastPage)
		{
			if(LO_signature.renderer.enabled == false && LO_signed)
				LO_signature.renderer.enabled = true;  
			if(LM_signature.renderer.enabled == false && LM_signed)
				LM_signature.renderer.enabled = true;
			if(CR_signature.renderer.enabled == false && CR_signed)
				CR_signature.renderer.enabled = true;
			
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
			if(LO_signature.renderer.enabled == true)
				LO_signature.renderer.enabled = false;
			
			if(LM_signature.renderer.enabled == true)
				LM_signature.renderer.enabled = false;
			
			if(CR_signature.renderer.enabled == true)
				CR_signature.renderer.enabled = false;
			
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
		
		
		if(currentPage< pageTextures.Length-1)
		{
			currentPage++;
			newPage.renderer.material.mainTexture = pageTextures[currentPage];
			
			
			if(currentPage == pageTextures.Length -1)
			{
				lastPage = true;
			}
			else{
				
				lastPage = false;
			}
		}
		
		
		
		
	}
	
	public void showPreviousPage(){
		if(currentPage> 0)
		{
			currentPage--;
			newPage.renderer.material.mainTexture = pageTextures[currentPage];
			
			if(currentPage == pageTextures.Length -1)
			{
				lastPage = true;
			}
			else{
				
				lastPage = false;
			}
		}
	}
}



