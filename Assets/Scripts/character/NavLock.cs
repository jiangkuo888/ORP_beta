using UnityEngine;
using System.Collections;

public class NavLock : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.GetComponent<NavMeshAgent>().destination = this.transform.position;
	}
}
