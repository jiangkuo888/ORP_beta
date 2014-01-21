using UnityEngine;
using System.Collections;

public class FireExtHandler : Photon.MonoBehaviour {

	public enum FireExtState : int{
		notAvailable, Available, beingChecked
	};

	public FireExtState state;
	public bool fireExtVisible;
	bool trigger;
	bool fireExtEventEnd;
	Collider collider;
	// Use this for initialization
	void Start () {

		for(int i=0; i<this.transform.parent.childCount;i++){
			this.transform.parent.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
		}
		state = FireExtState.notAvailable;

	}
	
	// Update is called once per frame
	void Update () {
		if((state == FireExtState.notAvailable) && (GameObject.Find("Telephone").GetComponent<PhoneHandler>().state == PhoneHandler.phoneState.afterCall)){
			PhotonView photonView = PhotonView.Get(this);
			photonView.RPC("SetFireExtVisible",PhotonTargets.AllBuffered);

		}

	}

	[RPC]
	void SetFireExtVisible(PhotonMessageInfo info){
		for(int i=1; i<this.transform.parent.childCount;i++){
			this.transform.parent.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
		}
		//this.transform.parent.parent.gameObject.AddComponent<Rigidbody>();
		GameObject go = GameObject.Find("Telephone");
		state = FireExtState.Available;

	}

	void OnGUI(){
		if(trigger && collider){

			if((state == FireExtState.notAvailable) && collider.GetComponent<PhotonView>().isMine){
//				transform.FindChild("FireExtGUI").visible = true;



				GUI.Box (new Rect(10,10,500,50),"The fire extinguisher is not available, please use the phone on the right to contact the security manager");
			}
			if(state == FireExtState.notAvailable){
				//GameObject go = GameObject.Find("Telephone Cube");
				//go.GetComponent<PhoneHandler>().phoneState
				GameObject.Find("Telephone").GetComponent<PhoneHandler>().state = PhoneHandler.phoneState.beforeCall;
			}
		}
	}

	void OnTriggerEnter(Collider co) {
		collider = co;
		if(co.CompareTag("manager")){

			trigger = true;
		}
	}

	void OnTriggerExit(Collider co){
		if(co.CompareTag("manager") || co.CompareTag("officer")){
			trigger = false;
		}
	}

}
