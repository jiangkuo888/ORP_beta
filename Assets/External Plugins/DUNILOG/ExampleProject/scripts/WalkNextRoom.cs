using UnityEngine;
using System.Collections;

public class WalkNextRoom : MonoBehaviour
{

	Vector3 pos;
	
	// Use this for initialization
	void Start ()
	{
		gameObject.GetComponent<DUGView>().visible = false;
		pos = Camera.main.transform.position;
		pos.x -= 3.5f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if( Vector3.Distance(Camera.main.transform.position,pos) > 0.25f ) {
			Camera.main.transform.position = Vector3.Lerp( Camera.main.transform.position, pos, 0.99f * Time.deltaTime ); 
		} else {
			gameObject.GetComponent<DUGView>().visible = true;
			Destroy( gameObject.GetComponent<WalkNextRoom>() );
		}
	}
}

