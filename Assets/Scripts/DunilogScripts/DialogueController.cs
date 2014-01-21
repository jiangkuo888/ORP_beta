//this script is attached to the DUGManager prefeb
//this script control which event dialogue will be triggered

using UnityEngine;
using System.Collections;

public class DialogueController : MonoBehaviour {


	public string activeDialogue;
	//PhotonView photonView;


	//Collider enteredObj;

	// Use this for initialization
	void Start () {
		activeDialogue = "";
		this.GetComponent<DUGView>().visible = false;

	}

	public void setActiveDialogue(string dialogName){
		activeDialogue = dialogName;
	}

	void Update () {
		activeDialogueEvent();
	}

	void activeDialogueEvent(){
		switch(activeDialogue)
		{
		case "":
			return;
		case "npcTrigger":
			this.GetComponent<DUGModel>().GetVariableByName("EventName").SetValue("NPCEvent");
			//print(DUGController.Run(this.GetComponent<DUGModel>().GetNodeByName("NPCEvent1").id).id);
			break;
		case "npc_1":
			this.GetComponent<DUGModel>().GetVariableByName("EventName").SetValue("NPC_NotWorkingEvent");
			//print(DUGController.Run(this.GetComponent<DUGModel>().GetNodeByName("NPCEvent1").id).id);
			break;
		case "FET":
			this.GetComponent<DUGModel>().GetVariableByName("EventName").SetValue("FireExtEvent");
			//DUGController.Run(this.GetComponent<DUGModel>().GetNodeByName("FireExtEvent1").id);
			break;
		case "Telephone":
			this.GetComponent<DUGModel>().GetVariableByName("EventName").SetValue("PhoneEvent");
			break;

		case "ObstacleBox":
			this.GetComponent<DUGModel>().GetVariableByName("EventName").SetValue("Obstruction_BoxesEvent");
			break;


		case "SignStand":
			this.GetComponent<DUGModel>().GetVariableByName("EventName").SetValue("Life_SignageEvent");
			break;
		
		case "CustomerNPC":
			this.GetComponent<DUGModel>().GetVariableByName("EventName").SetValue("CustomerEvent");
			break;
		default:
			break;
		}

//		disableCameraAndMotor();
//		moveCameraToObject();

	}

//	void disableCameraAndMotor(){
//		if(enteredObj != null){
//			enteredObj.GetComponent<MouseCamera>().enabled =false;
//			enteredObj.GetComponent<ClickMove>().enabled = false;
//		}
//	}
//	
//	public void moveCameraToObject(){
//		
//		Camera.main.transform.parent = this.transform;
//		Camera.main.transform.localPosition = camera_view_position;
//		Camera.main.transform.localEulerAngles = camera_view_rotation;
//		
//	}
//	
//	[RPC]
//	public void moveCameraToPlayer(){
//		//	print (enteredObj);
//		if(enteredObj != null){
//			Camera.main.transform.parent = enteredObj.transform;
//			Camera.main.transform.localPosition = enteredObj.GetComponent<ThirdPersonNetworkVik>().cameraRelativePosition;
//			Camera.main.transform.localEulerAngles = new Vector3(0.6651921f, 0, 0);
//		}
//	}
//	
//	[RPC]
//	public void enableCameraAndMotor(){
//		if(enteredObj != null)
//		{
//			enteredObj.GetComponent<MouseCamera>().enabled = true;
//			enteredObj.GetComponent<ClickMove>().enabled = true;
//		}
//	}
//	
//	
//	[RPC]
//	void destroyNPC(){
//		GameObject go = GameObject.Find("npc").gameObject;
//		if(go != null){
//			Destroy(go);
//		}
//	}
}
