using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem.UnityGUI;
using PixelCrushers.DialogueSystem;
using HutongGames.PlayMaker;



public class TutorialTriggers : MonoBehaviour {

	public bool trigger1;
	public bool trigger2;
	public bool trigger3;

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
		if(trigger1)
		{
		oldRotation = co.transform.rotation;
		EventFSM.FsmVariables.GetFsmBool("Left_clicked").Value = true;
		}
		else if(trigger2)
		{
		GameObject.Find ("Dialogue Manager").GetComponent<DialogueSystemController>().ShowAlert("Left click on the door to open it.",5.0f);
		}
		else if(trigger3)
		{
		EventFSM.FsmVariables.GetFsmBool("InRoom2").Value = true;

			GameObject.Find ("Common Table").transform.Find("DocumentHolder").GetComponent<documentData>().enabled = true;

		}
	}
	void OnTriggerStay(Collider co){
		if(co.transform.rotation != oldRotation)
		{
			EventFSM.FsmVariables.GetFsmBool("Right_holded").Value = true;

		}

	}
}
