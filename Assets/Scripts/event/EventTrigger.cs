using UnityEngine;
using System.Collections;

public class EventTrigger : MonoBehaviour {

	float starting_time;
	float duration;
	GameObject water;
	Vector3 initialPosition;
	Vector3 finalPosition;
	float animTime = 500f;
	int n = 0;
	bool floodFlag = true;
	float timer;
	bool start = true;
	public bool floodMissionFlag = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(GameObject.FindWithTag("manager") != null && start){
			start = !start;
			starting_time = Time.time;
		//	water = GameObject.Find("Daylight Water");
		//	initialPosition = water.transform.position;
		//	finalPosition = water.transform.position + new Vector3(0,1,0);
		}
		//Debug.Log(duration);
		duration = Time.time - starting_time;
		if(duration > 5 && floodFlag){
			//water.GetComponent<MeshRenderer>().renderer.enabled = true;
			//Debug.Log("flood is coming!");
			floodFlag = !floodFlag;
		}else if(duration > 5){
			//floodMissionStart();
		}
		floodMissionComplete();

	}

	void floodMissionStart(){
		if(timer < animTime){
			water.transform.position = Vector3.Slerp(water.transform.position, finalPosition, timer/animTime);
		}
		timer += Time.deltaTime;
		//Debug.Log(water.transform.position);
	}

	void floodMissionComplete(){
		if(floodMissionFlag == false){
			water.GetComponent<MeshRenderer>().renderer.enabled = false;
			Debug.Log("You are survived!");
		}
	}

}
