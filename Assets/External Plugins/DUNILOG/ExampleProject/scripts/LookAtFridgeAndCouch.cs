using UnityEngine;
using System.Collections;

public class LookAtFridgeAndCouch : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<DUGView>().visible = false;
	}
	
	// Update is called once per frame
	void Update () {
		if( Mathf.Abs( Camera.main.transform.rotation.eulerAngles.y - 70 ) > 5  ) {
			Camera.main.transform.rotation = Quaternion.Lerp( Camera.main.transform.rotation, Quaternion.Euler(0,70,0), 0.99f * Time.deltaTime );
		} else {
			gameObject.GetComponent<DUGView>().visible = true;
			Destroy( gameObject.GetComponent<LookAtFridgeAndCouch>() );
		}
	}
}
