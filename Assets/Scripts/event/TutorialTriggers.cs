using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem.UnityGUI;
using PixelCrushers.DialogueSystem;
using HutongGames.PlayMaker;



public class TutorialTriggers : MonoBehaviour {


	Quaternion oldRotation;

	PlayMakerFSM EventFSM;

	// Use this for initialization
	void Start () {
	

		EventFSM = GameObject.Find ("EventManager-Tutorial").GetComponent<PlayMakerFSM>();



	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider co){

		oldRotation = co.transform.rotation;
		EventFSM.FsmVariables.GetFsmBool("Left_clicked").Value = true;
		//GameObject.Find ("Dialogue Manager").GetComponent<DialogueSystemController>().ShowAlert("Left click to move forward.");
	}
	void OnTriggerStay(Collider co){
		if(co.transform.rotation != oldRotation)
		{
			EventFSM.FsmVariables.GetFsmBool("Right_holded").Value = true;

		}

	}
}
