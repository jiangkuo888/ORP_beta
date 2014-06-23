using UnityEngine;
using System.Collections;

public class waterDetect : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerEnter(Collider co)
	{

		if(co.name == "Sales Manager")
		{
			co.GetComponent<ClickMove>().inWater = true;


		}

	}

	void OnTriggerExit(Collider co)
	{
		if(co.name == "Sales Manager")
		{
			co.GetComponent<ClickMove>().inWater = false;
			
			
		}

	}
}
