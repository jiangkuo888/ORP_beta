using UnityEngine;
using System.Collections;

public class OpenRightDoor : MonoBehaviour {
	
	Transform door;
	
	// Use this for initialization
	void Start () {
		gameObject.GetComponent<DUGView>().visible = false;
		door = GameObject.Find("rightDoor").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if( Mathf.Abs( door.rotation.eulerAngles.y - 240 ) > 10  ) {
			door.rotation = Quaternion.Lerp( door.rotation, Quaternion.Euler(270,240,0), 0.99f * Time.deltaTime );
		} else {
			gameObject.GetComponent<DUGView>().visible = true;
			Destroy( gameObject.GetComponent<OpenRightDoor>() );
		}
	}
}
