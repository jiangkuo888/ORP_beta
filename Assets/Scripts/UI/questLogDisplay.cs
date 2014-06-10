using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem.UnityGUI;

public class questLogDisplay : MonoBehaviour {


	public GameObject questLogBig;
	public bool isOpen;
	private int count=0;
	// Use this for initialization
	void Start () {
		isOpen = true;
		gameObject.GetComponent<UnityGUIQuestLogWindow>().Open();
	}
	
	// Update is called once per frame
	void Update () {
		if(isOpen && GameObject.Find ("GameManager").GetComponent<GameManagerVik>().isTutorial == false)
			gameObject.GetComponent<UnityGUIQuestLogWindow>().Open();

		if(Input.GetKey(KeyCode.A))
		{

			open();
		}

	}

	public void close(){

		gameObject.GetComponent<UnityGUIQuestLogWindow>().Close();
		isOpen = false;
	}

	public void open(){

		gameObject.GetComponent<UnityGUIQuestLogWindow>().Open();
		isOpen = true;
	}
}
