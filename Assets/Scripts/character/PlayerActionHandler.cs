using UnityEngine;
using System.Collections;

public class PlayerActionHandler : Photon.MonoBehaviour {

	public bool ableToGrab;
	public Transform focusItem;
	bool fireExtTrigger;
	bool phoneEventTrigger;
	bool phoneEventStart;



	// Use this for initialization
	void Start () {
		ableToGrab = false;
	}
	
	// Update is called once per frame
	void Update () {
		Grab ();
	}

	//for grab action: if this object is able to be grab.
	void Grab(){
		if(ableToGrab && Input.GetKeyDown("f")){
			PhotonView photonView = PhotonView.Get(this);
			photonView.RPC("GrabRPC", PhotonTargets.All);
		}
	}
	[RPC]
	void GrabRPC(){
		if(focusItem != null){
			//Debug.Log("test 4: " + focusItem.name);
			if(focusItem.GetComponent<ChairHandler>() != null){
				//Debug.Log("test 5: "+transform.name);
				focusItem.GetComponent<ChairHandler>().grab(transform);
			}
		}
	}

//	void GUIphoneCall(){
//		PhotonView photonView = PhotonView.Get(this);
//		if(phoneEventTrigger && photonView.isMine){
//			photonView.RPC("GUIphoneCallRPC", PhotonTargets.All);
//		}
//	}
//
//	[RPC]
//	void GUIphoneCallRPC(){
//		//phoneEventTrigger = !phoneEventTrigger;
//		Debug.Log("bring the fire-ext");
//		GameObject go = GameObject.Find("Fire_Extinguisher");
//		Debug.Log(go.name);
//		if(go != null){
//			//for(go.GetComponentInChildren
//			//go.SetActive(true);
//			for(int i = 0; i < go.transform.childCount; i++){
//				go.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
//			}
//			//go.GetComponentInChildren<MeshRenderer>().enabled = true;
//			//go.GetComponent<MeshRenderer>().enabled = true;
//		}
//
//	}

//	void GUIcallSecurity(){
//		if(fireExtTrigger){
//			//GUI BOX;
//			fireExtTrigger = !fireExtTrigger;
//			phoneEventStart = true;
//
//			Debug.Log("should call security");
//		}
//	}
//	
//
//	void OnTriggerEnter(Collider co){
//		//Debug.Log(co.name);
//		if(co.name == "FireExintuguihserBox"){
//				fireExtTrigger = true;
//		}
//		if(phoneEventStart && co.name == "Telephone Cube"){
//			phoneEventTrigger = true;
//		}
//	}
}
