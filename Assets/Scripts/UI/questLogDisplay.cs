using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem.UnityGUI;

public class questLogDisplay : MonoBehaviour {

	public bool isOpen;
	// Use this for initialization
	void Start () {
		isOpen = true;
		gameObject.GetComponent<UnityQuestLogWindow>().Open();
	}
	
	// Update is called once per frame
	void Update () {
		if(isOpen)
		gameObject.GetComponent<UnityQuestLogWindow>().Open();
	}


	public void close(){

		gameObject.GetComponent<UnityQuestLogWindow>().Close();
		isOpen = false;
	}

	public void open(){

		gameObject.GetComponent<UnityQuestLogWindow>().Open();
		isOpen = true;
	}
}
