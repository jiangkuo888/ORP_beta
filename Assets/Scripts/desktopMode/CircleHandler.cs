using UnityEngine;
using System.Collections;

public class CircleHandler : MonoBehaviour {

	public bool active = false;
	// Use this for initialization
	void Start () {
		renderer.enabled = false;
		active = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseUpAsButton(){

		print ("I'm clicked!");


		if(renderer.enabled == false)
		{
			renderer.enabled = true;
			active = true;
		}
	

	}

	public void disableRenderer(){

		renderer.enabled = false;
		active = false;
	}
}
