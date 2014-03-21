using UnityEngine;
using System.Collections;

public class PhoneHandler : Photon.MonoBehaviour {



	public enum phoneState {
		idle, beforeCall, afterCall
	};

	public phoneState state;
	public bool phoneEventStart;
	bool phoneTrigger;
	bool phoneEventEnd;
	Collider collider;
	// Use this for initialization
	void Start () {
		state = phoneState.idle;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		if(phoneTrigger){
			if( (state == phoneState.beforeCall) && collider.GetComponent<PhotonView>().isMine){
				GUI.Box (new Rect(10,10,500,50),"Calling Security Manager: fire extinguisher is not available");
			}

		}
	}

	void OnTriggerEnter(Collider co) {
		collider = co;
		if(co.CompareTag("manager")){
			phoneTrigger = true;
		}
	}
	
	void OnTriggerExit(Collider co){
		if(co.CompareTag("manager")){
			phoneTrigger = false;
			state = phoneState.afterCall;
			//PhotonMessageInfo info;
			//Debug.Log(info.sender+" "+ info.photonView+" "+ info.timestamp);
		
		}
	}

}
