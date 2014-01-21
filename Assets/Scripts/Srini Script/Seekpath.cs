using UnityEngine;
using System.Collections;

public class Seekpath : MonoBehaviour
{
	
	public Transform[] waypoints;
	public string[] animations;

	public float waypointRadius = 1.5f;
	public float damping = 0.1f;
	public bool loop = false;
	public float speed = 2.0f;
	public bool faceHeading = true;
	
	private Vector3 currentHeading,targetHeading;
	private int targetwaypoint;
	private Transform xform;
	private bool useRigidbody;
	private Rigidbody rigidmember;
	private bool start;
	
	bool reachedTarget;
	// Use this for initialization
	protected void Start ()
	{
		reachedTarget = false;
		
		//		start = false;
		//		 aianim= GameProperties.Find("npc_1")
		//		aianim = GetComponentsInChildren<Animation> ();
		xform = transform;
		currentHeading = xform.forward;
		if(waypoints.Length<=0)
		{
			Debug.Log("No waypoints on "+name);
			enabled = false;
		}
		targetwaypoint = 0;
		if(rigidbody!=null)
		{
			useRigidbody = true;
			rigidmember = rigidbody;
		}
		else
		{
			useRigidbody = false;
		}
	}
	
	// calculates a new heading
	protected void FixedUpdate ()
	{
		if (!reachedTarget) {
			
			targetHeading = waypoints [targetwaypoint].position - xform.position;
			
			currentHeading = Vector3.Lerp (currentHeading, targetHeading, damping * Time.deltaTime);
			
			this.GetComponent<Animation>().animation.CrossFade (animations[0]);
		}
		
		
	}
	
	// moves us along current heading
	protected void Update()
	{
		
		if (!reachedTarget) {
			
			
			if (useRigidbody)
				rigidmember.velocity = currentHeading * speed;
			else
				xform.position += currentHeading * Time.deltaTime * speed;
			if (faceHeading)
				xform.LookAt (xform.position + currentHeading);
			
			if (Vector3.Distance (xform.position, waypoints [targetwaypoint].position) <= 0.2) {
				
				transform.position = waypoints[targetwaypoint].position;
				transform.LookAt(transform.position + currentHeading);
				reachedTarget = true;
				targetHeading = new Vector3 (0, 0, 0);
				currentHeading = new Vector3(0,0,0);
				this.GetComponent<Animation> ().animation.CrossFade (animations[1]);
				
				
				
			}
			
			
		}
		
	}
	
	
	// draws red line from waypoint to waypoint
	public void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		if(waypoints==null)
			return;
		
		
		
		for(int i=0;i< waypoints.Length;i++)
		{
			Vector3 pos = waypoints[i].position;
			Vector3 prev = transform.position;
			if(i>0)
				prev = waypoints[i-1].position;	

			Gizmos.DrawLine(prev,pos);
		}
	}
	
}