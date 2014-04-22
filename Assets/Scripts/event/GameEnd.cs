using UnityEngine;
using System.Collections;

public class GameEnd : Photon.MonoBehaviour {
	int playerCount;
	GameObject enteredObj;

	public bool enabled;
	public bool blocked;
	public bool canEnd;


	void Start () {
		playerCount = 0;
		enteredObj = null;
		
		enabled = false;
		blocked = true;
		canEnd = false;
	}

	void OnTriggerStay(Collider obj){
		
		if(obj.name == "ObstacleBox")
			blocked = true;

		
	}

	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
		if(blocked == false && enabled)
			canEnd = true;
	}

	void OnTriggerEnter (Collider Co){

		if(Co.tag == "SM" || Co.tag == "LM" ||Co.tag == "LO" ||Co.tag == "CR")
			if(canEnd)
			{
			photonView.RPC("endGameRPC",PhotonTargets.AllBuffered);

			}
		   

	}
	public void enableExitRPC(){
		photonView.RPC("enableExit",PhotonTargets.AllBuffered);

	}


	[RPC]
	public void endGameRPC(){
		if(GameObject.Find ("GameManager"))
		GameObject.Find ("GameManager").GetComponent<GameManagerVik>().EndGame();

	}


	[RPC]
	public void enableExit(){
		enabled = true;

	}

	[RPC]

	public void notBlockingAnymore(){

		blocked = false;
	}


}
