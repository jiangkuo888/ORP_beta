using UnityEngine;
using System.Collections;

public class OpenSafe : MonoBehaviour {
	
	Transform door;
	Vector3 doorPos;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<DUGView>().visible = false;
		door = GameObject.Find("safe").transform;
		doorPos = door.position;
		doorPos.z += 1.25f;
	}
	
	// Update is called once per frame
	void Update () {
		if( Vector3.Distance(door.position,doorPos) > 0.25f ) {
			door.position = Vector3.Lerp( door.position, doorPos, 0.99f * Time.deltaTime );
		} else {
			gameObject.GetComponent<DUGView>().visible = true;
			Destroy( gameObject.GetComponent<OpenSafe>() );
		}
	}
}
