



using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.SequencerCommands;
using HutongGames.PlayMaker;

public class SequencerCommandFraud : SequencerCommand {


	public void Start() {

		//change to another state
		PlayMakerFSM EventFSM = GameObject.Find ("TrainerManager").GetComponent<PlayMakerFSM> ();

		if(EventFSM.enabled)
		{
			if(EventFSM.ActiveStateName == "receive_call")
			{
				EventFSM.FsmVariables.GetFsmBool("gotFraudWarning").Value = true;
			}
			else if(EventFSM.ActiveStateName == "make_decision")
			{
				EventFSM.FsmVariables.GetFsmBool("makeDecision").Value = true;
			}
		}

	}
	
	public void Update() {

	}

	public void OnDestroy() {

		//change to another state
		PlayMakerFSM EventFSM = GameObject.Find ("TrainerManager").GetComponent<PlayMakerFSM> ();

		if (EventFSM.enabled && EventFSM.ActiveStateName == "make_decision")
		{
			// start conversation
			GameObject.Find("Dialogue Manager").GetComponent<DialogueSystemController>().StartConversation("Exposure decision",GameObject.Find ("Credit Risk").transform);
		}
	}






	
}
 
 

/**/