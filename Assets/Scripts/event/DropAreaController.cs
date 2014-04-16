using UnityEngine;
using System.Collections;

public class DropAreaController : MonoBehaviour {
	
	
	public GameObject DropObj;
	public float rotateSpeed = 2f;
	public string dropSuccessID;
	bool DropTaskFinished;

	public bool AreaActivated;
	// Use this for initialization
	void Start () {
		AreaActivated = false;
		DropTaskFinished = false;
		//if(DropObj == null)
//			Debug.Log(this.name + " has no drop object, please attach one.");
		dropSuccessID = "3A";
	}
	
	// Update is called once per frame
	void Update () {
		if(AreaActivated)
		{
			if(this.transform.Find ("GreenArrow").renderer.enabled == false)
			this.transform.Find ("GreenArrow").renderer.enabled = true;
		}
		else 
		{
			if(this.transform.Find ("GreenArrow").renderer.enabled == true)
			this.transform.Find ("GreenArrow").renderer.enabled = false;
		}


	}
	void FixedUpdate(){
		if(AreaActivated)
		{
			this.transform.Find("GreenArrow").transform.localEulerAngles = new Vector3(this.transform.Find("GreenArrow").transform.localEulerAngles.x,this.transform.Find("GreenArrow").transform.localEulerAngles.y+rotateSpeed,this.transform.Find("GreenArrow").transform.localEulerAngles.z);
		}
	}
	
	void OnTriggerEnter(Collider co)
	{
		if(co.gameObject == DropObj)
		{
			DropTaskFinished = true;

			switch(DropObj.name)
			{
			case "ObstacleBox":

				// log the drop success action
				GameObject.Find ("Dialogue Manager").GetComponent<PlayerActionLog>().addToPlayerActionLog(dropSuccessID, DropObj.name + " has been dropped to the correct place by " + PhotonNetwork.playerName);


				print(" you have finished task : Clear the obstacle box and move it to target area.");
				break;
			case "SignStand":
				
				print(" you have finished task : Take the sign stand and place it in front of the lift.");
				break;
			}

			
			
		}
		
		
	}
	
	void OnTriggerExit(Collider co)
	{
		if(co.gameObject == DropObj)
		{
			DropTaskFinished = false;
			print(" you have failed task 1: Clear the obstacle box.");
			
		}
	}
}
