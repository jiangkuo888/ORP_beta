using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

public class DoorHandler : Photon.MonoBehaviour {
	
	float smooth = 2.0f;
	float DoorOpenAngle = 90.0f;
	float DoorCloseAngle = 180.0f;
	bool isOpen ;
	public bool enter ;
	bool doorState;
	public bool clicked;
	public bool enabled;
	PlayMakerFSM EventFSM;
	public bool isTriggerA;
	public bool isTriggerB;
	
	public string currentOpenedDirection;
	
	// Use this for initialization
	void Start () {
		isOpen = false;
		enter = false;
		clicked = false;
		EventFSM = GameObject.Find ("EventManager-Tutorial").GetComponent<PlayMakerFSM>();
	}
	
	// Update is called once per frame
	void Update() {
		if(enabled){
			//if this photonview player enter, and press f
			//change state and send, open door.
			if(enter && clicked){
				//			print("111");
				if(EventFSM.enabled)
					EventFSM.FsmVariables.GetFsmBool("Opened_door").Value = true;
				
				
				if(GameObject.Find ("GameManager").GetComponent<GameManagerVik>().isTutorial)
					open ();
				else{
					PhotonView photonView = PhotonView.Get (this);
					photonView.RPC("Open",PhotonTargets.AllBuffered);
				}
				
				clicked = ! clicked;
			}
			
		}
		
		
	}
	
	void open()
	{
		if(isTriggerA)
		{
			Transform child = transform.parent.transform.Find("Door");
			isOpen = !isOpen;
			if(isOpen){
				if(child != null)
				{
					child.animation.Play("DoorOpenA");
					
					
					transform.parent.Find("TriggerB").GetComponent<DoorHandler>().currentOpenedDirection = "A";
					currentOpenedDirection = "A";
				}
				else
					Debug.Log("did not get child");
			}//Debug.Log("Door is open");
			else{
				if(child != null)
				{
					if(currentOpenedDirection == "A")
						child.animation.Play("DoorCloseA");
					else
						child.animation.Play("DoorCloseB");
				}
				else
					Debug.Log("did not get child");
			}
			
			
			//this.transform.parent.transform.Find("TriggerB").GetComponent<DoorHandler>().clicked = false;
		}
		else if(isTriggerB){
			Transform child = transform.parent.transform.Find("Door");
			isOpen = !isOpen;
			if(isOpen){
				if(child != null)
				{
					child.animation.Play("DoorOpenB");
					
					transform.parent.Find("TriggerA").GetComponent<DoorHandler>().currentOpenedDirection = "B";
					currentOpenedDirection = "B";
				}
				else
					Debug.Log("did not get child");
			}//Debug.Log("Door is open");
			else{
				if(child != null)
				{
					if(currentOpenedDirection == "B")
						child.animation.Play("DoorCloseB");
					else
						child.animation.Play("DoorCloseA");
				}
				else
					Debug.Log("did not get child");
			}
			
			//this.transform.parent.transform.Find("TriggerA").GetComponent<DoorHandler>().clicked = false;
		}
		
	}
	
	[RPC]
	void Open(){
		if(isTriggerA)
		{
			Transform child = transform.parent.transform.Find("Door");
			isOpen = !isOpen;
			if(isOpen){
				if(child != null)
				{
					child.animation.Play("DoorOpenA");
					
					
					transform.parent.Find("TriggerB").GetComponent<DoorHandler>().currentOpenedDirection = "A";
					currentOpenedDirection = "A";
				}
				else
					Debug.Log("did not get child");
			}//Debug.Log("Door is open");
			else{
				if(child != null)
				{
					if(currentOpenedDirection == "A")
						child.animation.Play("DoorCloseA");
					else
						child.animation.Play("DoorCloseB");
				}
				else
					Debug.Log("did not get child");
			}
			
			
			//this.transform.parent.transform.Find("TriggerB").GetComponent<DoorHandler>().clicked = false;
		}
		else if(isTriggerB){
			Transform child = transform.parent.transform.Find("Door");
			isOpen = !isOpen;
			if(isOpen){
				if(child != null)
				{
					child.animation.Play("DoorOpenB");
					
					transform.parent.Find("TriggerA").GetComponent<DoorHandler>().currentOpenedDirection = "B";
					currentOpenedDirection = "B";
				}
				else
					Debug.Log("did not get child");
			}//Debug.Log("Door is open");
			else{
				if(child != null)
				{
					if(currentOpenedDirection == "B")
						child.animation.Play("DoorCloseB");
					else
						child.animation.Play("DoorCloseA");
				}
				else
					Debug.Log("did not get child");
			}
			
			//this.transform.parent.transform.Find("TriggerA").GetComponent<DoorHandler>().clicked = false;
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
		if(Co.gameObject.tag == "SM" || Co.gameObject.tag == "LM" || Co.gameObject.tag == "LO" || Co.gameObject.tag == "CR" && (Co.GetComponent<PhotonView>().isMine ||GameObject.Find ("GameManager").GetComponent<GameManagerVik>().isTutorial))
			enter = true;
	}
	
	//Deactivate the Main function when player is go away from door
	void OnTriggerExit (Collider Co){
		enter = false;
	}
}