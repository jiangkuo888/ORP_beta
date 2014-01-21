using UnityEngine;
using System.Collections;

public class OpenLeftDoor : MonoBehaviour
{
	Transform door;

	// Use this for initialization
	void Start ()
	{
		gameObject.GetComponent<DUGView>().visible = false;
		door = GameObject.Find("leftDoor").transform;
		Destroy(GameObject.Find("key"));
	}
	
	// Update is called once per frame
	void Update ()
	{
		if( Vector3.Distance(Camera.main.transform.position,new Vector3(-1.65f,1.6f,-1.6f)) > 0.25f ) {
			Camera.main.transform.position = Vector3.Lerp( Camera.main.transform.position, new Vector3(-1.65f,1.6f,-1.6f), 0.99f * Time.deltaTime ); 
		} else if( Mathf.Abs( door.rotation.eulerAngles.y - 120 ) > 10  ) {
			door.rotation = Quaternion.Lerp( door.rotation, Quaternion.Euler(270,120,0), 0.99f * Time.deltaTime );
		} else {
			gameObject.GetComponent<DUGView>().visible = true;
			Destroy( gameObject.GetComponent<OpenLeftDoor>() );
		}
	}
}

