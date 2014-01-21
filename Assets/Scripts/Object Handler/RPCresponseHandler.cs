using UnityEngine;
using System.Collections;

public class RPCresponseHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[RPC]

	void disableRenderer(){

		Renderer[] renderers = this.GetComponentsInChildren<Renderer>();
		
		foreach ( Renderer r in renderers)
		{
			if(r.enabled == true)
				r.enabled = false;
		}
	}

	[RPC]
	void disableCollider(){

		Collider[] colliders = this.GetComponentsInChildren<Collider>();
		
		foreach ( Collider c in colliders)
		{
			if(c.enabled == true)
				c.enabled = false;
		}
	}

	[RPC]
	void disableRigidbody(){

		if (this.GetComponent<Rigidbody>()!=null)
			Destroy(this.GetComponent<Rigidbody>());

	}

	[RPC]

	void updateAllInfo(Vector3 newPosition){


		this.transform.position = newPosition;
	}



	[RPC]
	
	void enableRenderer(){
		
		Renderer[] renderers = this.GetComponentsInChildren<Renderer>();
		
		foreach ( Renderer r in renderers)
		{
			if(r.enabled == false)
				r.enabled = true;
		}
	}
	
	[RPC]
	void enableCollider(){
		
		Collider[] colliders = this.GetComponentsInChildren<Collider>();
		
		foreach ( Collider c in colliders)
		{
			if(c.enabled == false)
				c.enabled = true;
		}
	}
	
	[RPC]
	void enableRigidbody(){
		
		if (this.GetComponent<Rigidbody>()==null)
			this.gameObject.AddComponent<Rigidbody>();
		
	}
}
