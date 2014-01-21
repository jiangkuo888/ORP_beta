﻿using UnityEngine;
using System.Collections;

public class NPCEventRunToFireEscape : Photon.MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.GetComponent<DUGView>().visible = false;
		
		string hitColliderName = transform.GetComponent<DialogueController>().activeDialogue;
		
		
		
		
		
		transform.parent.GetComponent<DetectObjects>().moveCameraToPlayer();
		transform.parent.GetComponent<DetectObjects>().enableCameraAndMotor();
		transform.parent.GetComponent<DetectObjects>().enteredDialog = false;
		//set lock to false so others can interact with this object
		//GameObject.Find(hitColliderName).GetComponent<TriggerHandler>().photonView.RPC("setInteractLock",PhotonTargets.AllBuffered, false);
		
		//delete the useless script generated by dunilog
		GameObject.Destroy(this.gameObject.transform.GetComponent<NPCEventRunToFireEscape>());




		if(GameObject.Find (hitColliderName).GetComponent<Seekpath>())
		GameObject.Find (hitColliderName).GetComponent<Seekpath>().enabled = true;



	}



}
