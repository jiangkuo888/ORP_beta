using UnityEngine;
using System.Collections;

public class NGUIpanelHandler : MonoBehaviour {


	public GameObject GameEnd;
	public GameObject GameEndText;
	public GameObject Pause;



	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void show(string panel){
		// show NGUI panel

		switch(panel)
		{
		case "GameEndScreen":


			// update player score
			PlayerActionLog loggy = GameObject.Find ("Dialogue Manager").GetComponent<PlayerActionLog> ();
			
			int[] scoreArray = loggy.getPlayerScore ();


			print(scoreArray[0]);
			print (scoreArray[1]);
			NGUITools.SetActive(GameEnd, true);
			GameEndText.GetComponent<UILabel>().text= "Game Over\n\n\n\n\n\n\nYour Score :                      "+scoreArray[0].ToString() + "\nTeam Score:                      "+scoreArray[1].ToString();

			break;
		case "PauseScreen":
			if(GameObject.Find ("GameEndScreen") == null)
			NGUITools.SetActive(Pause, true);
			break;
		default:
			break;
		}



		// disable UI elements
		GameObject.Find ("QuestLogButton").GetComponent<GUITexture>().enabled = false;
		GameObject.Find ("phoneButton").GetComponent<GUITexture>().enabled = false;
		GameObject.Find ("GameManager").GetComponent<ChatVik>().enabled = false;

		// disable user control

		//Camera.main.transform.parent.GetComponent<ClickMove>().gameOn = false;
		//Camera.main.transform.parent.GetComponent<DetectObjects>().gameOn = false;



	}

	public void hide(string panel){
		// hide NGUI panel
		switch(panel)
		{
		case "GameEndScreen":
			NGUITools.SetActive(GameEnd, false);
			GameObject.Find ("GameManager").GetComponent<GameManagerVik>().SaveAndQuit();
			break;
		case "PauseScreen":
			NGUITools.SetActive(Pause, false);
			break;
		default:
			break;
		}


		// enable UI elements
		GameObject.Find ("QuestLogButton").GetComponent<GUITexture>().enabled = true;
		GameObject.Find ("phoneButton").GetComponent<GUITexture>().enabled = true;
		GameObject.Find ("GameManager").GetComponent<ChatVik>().enabled = true;
		
		// enable user control


	}


}
