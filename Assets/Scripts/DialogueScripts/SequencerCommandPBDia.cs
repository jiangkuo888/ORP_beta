using UnityEngine;
using System.Collections;
using System;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.SequencerCommands;

public class SequencerCommandPBDia : SequencerCommand {

	private string convoTitle;
	private int dialogueNum;
	private string dialogueType;

	// Use this for initialization
	void Start () {

		this.convoTitle = GetParameter(0);
		this.dialogueNum = Convert.ToInt32(GetParameter(1));
		this.dialogueType = GetParameter(2);
	
		//update other component
		PlaybackDialogue diaggy = GameObject.Find ("Main Camera").GetComponent<PlaybackDialogue> ();
		diaggy.convoTitle = convoTitle;
		diaggy.dialogueNum = dialogueNum;
		diaggy.dialogueType = dialogueType;
		
		Stop ();
	
	}

}
