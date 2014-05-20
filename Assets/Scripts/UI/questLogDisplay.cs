using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem.UnityGUI;

public class questLogDisplay : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<UnityQuestLogWindow>().Open();
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.GetComponent<UnityQuestLogWindow>().Open();
	}


	public void updateQuestLog(){

		gameObject.GetComponent<UnityQuestLogWindow>().Open();
	}
}
