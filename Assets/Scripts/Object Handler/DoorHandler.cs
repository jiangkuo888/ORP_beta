using UnityEngine;
using System.Collections;

public class DoorHandler : Photon.MonoBehaviour {
	
	float smooth = 2.0f;
	float DoorOpenAngle = 90.0f;
	float DoorCloseAngle = 180.0f;
	bool isOpen ;
	bool enter ;
	bool doorState;
	public bool clicked;
	
	// Use this for initialization
	void Start () {
		isOpen = false;
		enter = false;
		clicked = false;
	}
	
	// Update is called once per frame
	void Update() {
		//if this photonview player enter, and press f
		//change state and send, open door.
		if(enter && clicked){
//			print("111");
			PhotonView photonView = PhotonView.Get (this);
			photonView.RPC("Open",PhotonTargets.AllBuffered);
			clicked = ! clicked;
		}


		

	}
	

	
	[RPC]
	void Open(){
		Transform child = transform.Find("doortestrun");
		isOpen = !isOpen;
		if(isOpen){
			if(child != null)
				child.animation.Play("Anim_Wooden_Open");
			else
				Debug.Log("did not get child");
		}//Debug.Log("Door is open");
		else{
			if(child != null)
				child.animation.Play("Anim_wooden_close");
			else
				Debug.Log("did not get child");
		}
	}
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
//		if (stream.isWriting)
//		{
//			//We own this player: send the others our data
//			// stream.SendNext((int)controllerScript._characterState);
//			stream.SendNext(isOpen);
//
//		}
//		else
//		{
//			isOpen = (bool)stream.ReceiveNext();
//			Open ();
//		}
	}
	
	
	
	
	//Activate the Main function when player is near the door
	void OnTriggerEnter (Collider Co){
		//Debug.Log("name: " + other.gameObject.transform.name);
		if(Co.gameObject.tag == "manager" && Co.GetComponent<PhotonView>().isMine)
			enter = true;
	}
	
	//Deactivate the Main function when player is go away from door
	void OnTriggerExit (Collider Co){
		enter = false;
	}
}