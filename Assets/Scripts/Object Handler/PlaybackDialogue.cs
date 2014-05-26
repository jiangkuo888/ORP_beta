using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

public class PlaybackDialogue : MonoBehaviour {

	public Dictionary<string, List<DialogueEntry>> convoList = new Dictionary<string, List<DialogueEntry>>();
	public string convoTitle;
	public int dialogueNum;
	public string dialogueType;
	public string currTitle = "";
	public int currNum = -1;
	public GUIStyle custom;
	public GUISkin customSkin;


	// Use this for initialization
	/*void Start () {
	
		DialogueSystemController gameDSC = GameObject.Find("Dialogue Manager").GetComponent<DialogueSystemController>();

		DialogueDatabase gamedd = gameDSC.initialDatabase;

		foreach (Conversation convoyo in gamedd.conversations)
		{
			convoList.Add (convoyo.Title, convoyo.dialogueEntries);
			//Debug.Log(convoyo.Title);
		}

		convoTitle = "";
		dialogueNum = -1;
	}

	void OnGUI()
	{
		GameManagerVik vikky = GameObject.Find ("GameManager").GetComponent<GameManagerVik>();

		if (vikky.isPlayBack && this.convoTitle != "" && this.dialogueNum != -1)
		{
			//find the particular line
			if (currNum != dialogueNum || convoTitle != currTitle)
			{
				foreach (var pair in convoList)
				{
					if (pair.Key == this.convoTitle)
					{
						foreach (DialogueEntry diaggy in pair.Value)
						{
							if (diaggy.id == this.dialogueNum)
							{
								currNum = this.dialogueNum;
								currTitle = this.currTitle;

								//check if actor is player
								//if (diaggy.DialogueText != "")

									//GUI.skin = customSkin;
								//{
									GUILayout.BeginArea (new Rect (Screen.width/3*1, Screen.height/3*2, Screen.width/2, Screen.height*.5f));
									GUILayout.BeginHorizontal();

									if (dialogueType == "player")
									{
										GUILayout.Label ("The player chose: \"" + diaggy.MenuText + "\"", custom);
									}
									else if (dialogueType == "npc")
									{
										GUILayout.Label ("The npc said: \"" + diaggy.DialogueText + "\"", custom);
									}
									else if (dialogueType == "object")
									{
										GUILayout.Label ("The player noticed: \"" + diaggy.DialogueText + "\"", custom);
									}

									GUILayout.EndHorizontal(); 
									GUILayout.EndArea();
								//}

								break;
							}

						}
						break;
					}
				}
			}

		}
	}*/

}
