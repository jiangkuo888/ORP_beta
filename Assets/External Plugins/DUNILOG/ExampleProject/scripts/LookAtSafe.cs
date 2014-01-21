using UnityEngine;
using System.Collections;

public class LookAtSafe : MonoBehaviour {

	void Start () {
		gameObject.GetComponent<DUGView>().visible = false;
	}
	
	// Update is called once per frame
	void Update () {
		if( Mathf.Abs( Camera.main.transform.rotation.eulerAngles.y - 270 ) > 5  ) {
			Camera.main.transform.rotation = Quaternion.Lerp( Camera.main.transform.rotation, Quaternion.Euler(0,270,0), 0.99f * Time.deltaTime );
		} else if( Vector3.Distance(Camera.main.transform.position,new Vector3(-2.5f,1.6f,1.2f)) > 0.25f ) {
			Camera.main.transform.position = Vector3.Lerp( Camera.main.transform.position, new Vector3(-2.5f,1.6f,1.2f), 0.99f * Time.deltaTime );
		} else {
			gameObject.GetComponent<DUGView>().visible = true;
			Destroy( gameObject.GetComponent<LookAtSafe>() );
		}
	}
}
