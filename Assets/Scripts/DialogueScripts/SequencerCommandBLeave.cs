using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.SequencerCommands;
using HutongGames.PlayMaker;

public class SequencerCommandBLeave : SequencerCommand {

	public void Start() {

	}
	
	public void Update() {
		
	}
	
	public void OnDestroy() {

		// start conversation
		GameObject.Find("Dialogue Manager").GetComponent<DialogueSystemController>().StartConversation("Block leave talk",GameObject.Find ("Credit Risk").transform);

	}
}
