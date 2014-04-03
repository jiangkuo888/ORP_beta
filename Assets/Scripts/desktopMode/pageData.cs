using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;


public class pageData : MonoBehaviour {
	public string documentName;
	public GameObject pagePrototype;
	public Texture2D[] pageTextures;

	public GameObject LO_signature;
	public GameObject LM_signature;
	public GameObject CR_signature;
	PlayMakerFSM EventFSM;


	
	GameObject newPage;
	public int currentPage;
	
	public bool lastPage;
	float w,h;

	public bool LO_signed,LM_signed,CR_signed;
	
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
	}
	
	
	void OnGUI(){
		
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


					// RPC call to display email

					if(!LO_signed)

					{
					if(GUI.Button( new LTRect(w/2 - 100f, .2f*h - 100f, 200f, 50f ).rect, "Sign"))
					{

						if(EventFSM.enabled)
							if(EventFSM.ActiveStateName == "Sign")
								EventFSM.FsmVariables.GetFsmBool("signed").Value = true;



						PhotonView photonView = this.gameObject.GetPhotonView();
						photonView.RPC ("signDoc",PhotonTargets.AllBuffered,"LO");

					}
					}
						
					else

					{
					 //GUI.DrawTexture( new LTRect(w/2 - 100f,.2f*h - 100f, 200f,50f).rect, LO_signature);
						//photonView.RPC ("sendDocument",PhotonTargets.AllBuffered,"Sales Manager","LPU Officer",this.transform.Find ("DocumentHolder").GetComponent<documentData>().documents[currentDocumentIndex-1].name);

					       
					}
					// accept the document
				
				break;

			case "LPU Manager":

					if(!LM_signed)
						
					{
					if(GUI.Button( new LTRect(w/2 - 100f, .4f*h - 100f, 200f, 50f ).rect, "Sign"))
					{
						PhotonView photonView = this.gameObject.GetPhotonView();
						photonView.RPC ("signDoc",PhotonTargets.AllBuffered,"LM");


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
					if(GUI.Button( new LTRect(w/2 - 100f, .6f*h - 100f, 200f, 50f ).rect, "Sign"))
					{



						PhotonView photonView = this.gameObject.GetPhotonView();
						photonView.RPC ("signDoc",PhotonTargets.AllBuffered,"CR");
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


	// Update is called once per frame
	void Update () {
		
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



