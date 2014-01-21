using UnityEngine;
using System.Collections;

public class aiFollow : MonoBehaviour {


	public Transform target;
	public bool manager;
	public bool IT;
	public bool customer;
	public bool officer;

	// Use this for initialization
	void Start () {
	

	}
	
	// Update is called once per frame
	void Update () {
	
		if (manager==true)
		{
		if (GameObject.FindGameObjectWithTag ("manager")) {
			GameObject goTo = GameObject.FindGameObjectWithTag ("manager");
			target = goTo.transform;

			GetComponent<NavMeshAgent>().destination= target.position;
			}
			}
		if (IT==true)
		{
			if (GameObject.FindGameObjectWithTag ("IT")) {
				GameObject goTo = GameObject.FindGameObjectWithTag ("IT");
				target = goTo.transform;
				
				GetComponent<NavMeshAgent>().destination= target.position;
			}
		}
		if (customer==true)
		{
			if (GameObject.FindGameObjectWithTag ("cutomer")) {
				GameObject goTo = GameObject.FindGameObjectWithTag ("customer");
				target = goTo.transform;
				
				GetComponent<NavMeshAgent>().destination= target.position;
			}
		}
		if (officer==true)
		{
			if (GameObject.FindGameObjectWithTag ("officer")) {
				GameObject goTo = GameObject.FindGameObjectWithTag ("officer");
				target = goTo.transform;
				
				GetComponent<NavMeshAgent>().destination= target.position;
			}
		}

}
}
