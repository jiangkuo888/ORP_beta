using UnityEngine;
using System.Collections;

public class ChairHandler : Photon.MonoBehaviour {
	
	PhotonView photonView;
	Transform grabber;
	bool isGrabbed;
	// Use this for initialization
	void Start () {
		photonView = PhotonView.Get(this);
		if(photonView.isMine){
			isGrabbed = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(isGrabbed && photonView.isMine){
			photonView.RPC("positionChanged", PhotonTargets.All, grabber.position, grabber.rotation);
		}
	}
	[RPC]
	void positionChanged(Vector3 position, Quaternion rotation){
			transform.position = position;
			transform.rotation = rotation;
			Destroy(transform.GetComponent<MeshCollider>());
	}

	public void grab(Transform tr){
		Destroy(transform.GetComponent<MeshCollider>());
		grabber = tr;
		isGrabbed = true;
	}

	void OnTriggerEnter(Collider co){
		if(co.CompareTag("manager") || co.CompareTag("officer")){
			co.GetComponent<PlayerActionHandler>().focusItem = this.transform;
			co.GetComponent<PlayerActionHandler>().ableToGrab = true;
			Debug.Log ("Press 'f' to grab this chair");
		}
	}

	void OnTriggerExit(Collider co){
		if(co.CompareTag("manager") || co.CompareTag("officer")){
			co.GetComponent<PlayerActionHandler>().focusItem = null;
			co.GetComponent<PlayerActionHandler>().ableToGrab = false;
		}
	}
}
