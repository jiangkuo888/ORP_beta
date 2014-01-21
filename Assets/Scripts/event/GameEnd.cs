using UnityEngine;
using System.Collections;

public class GameEnd : MonoBehaviour {
	int playerCount;
	GameObject enteredObj;

	public GameObject GameEndScreen;
	public GameObject MainCamera;

	// Use this for initialization
	void Start () {
		playerCount = 0;
		enteredObj = null;
	}
	
	// Update is called once per frame
	void Update () {
		CheckGameEnd();
	}

	void OnTriggerEnter (Collider Co){

			playerCount++;
			print(playerCount);


	}

	void OnTriggerExit(Collider Co){

			playerCount--;
			print(playerCount);
	


	}


	void CheckGameEnd(){

		if(playerCount >=2)
		{
			if(MainCamera.gameObject != null)
			{
			if(MainCamera.transform.parent != null)
			{

			MainCamera.transform.parent.GetComponent<ClickMove>().gameOn = false;
			MainCamera.transform.parent.GetComponent<DetectObjects>().gameOn = false;
			}

			MainCamera.SetActive(false);
			GameEndScreen.SetActive(true);
			}
		}
	}
}
