using UnityEngine;
using System.Collections;

public class InspectFridge : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<DUGView>().visible = false;
	}
	
	// Update is called once per frame
	void Update () {
		if( Vector3.Distance(Camera.main.transform.position,new Vector3(0.9f,1.6f,2)) > 0.25f  ) {
			Camera.main.transform.position = Vector3.Lerp( Camera.main.transform.position, new Vector3(0.9f,1.6f,2), 0.99f * Time.deltaTime );
		} else {
			gameObject.GetComponent<DUGView>().visible = true;
			Destroy( gameObject.GetComponent<InspectFridge>() );
		}
		
	}
}
