using UnityEngine;using System.Collections;
using HutongGames.PlayMaker;

public class SeekSteer : MonoBehaviour
{
	public Transform[] waypoints;
	public float waypointRadius = 1.5f;
	float damping = 0.1f;
	public bool loop = false;
	public float speed = 4.0f;
	bool faceHeading = true;
	
	private Vector3 currentHeading,targetHeading;
	private int targetwaypoint;
	private Transform xform;
	private bool useRigidbody;
	private Rigidbody rigidmember;
	public PlayMakerFSM NPCstate;
	
	
	// Use this for initialization
	protected void Start ()
	{
		//transform.GetComponent<npcAnimationController> = npcAnimationController.CharacterState.run;
		
		
		
		
		
		
		
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
	
	
	
	
	// moves us along current heading
	protected void Update()
	{
		
	if (NPCstate.FsmVariables.GetFsmBool("stateFinished").Value != true) {
			if (targetwaypoint == 0)
				return;
			else {
				
				if (useRigidbody)
					rigidmember.velocity = currentHeading * speed;
				else
					xform.position += targetHeading.normalized * Time.deltaTime * speed;
				if (faceHeading)
					xform.LookAt (xform.position + currentHeading);
				
				if (Vector3.Distance (xform.position, waypoints [targetwaypoint].position) <= 0.2) {
					
					print (Vector3.Distance (xform.position, waypoints [targetwaypoint].position));
					
					NPCstate.Fsm.Event("EndEvent");
					NPCstate.ChangeState("EndEvent");

					NPCstate.FsmStates.SetValue("animation_2",0);

					//NPCstate.Fsm.Event("EndEvent_2");
					//NPCstate.ChangeState("EndEvent_2");

					return;
					
					
					//targetwaypoint = 0;
					//if(!loop)	
					//enabled = false;
					//				aianim=GetComponentsInChildren<Animation>();
					
					
					
					
					//this.transform.GetComponentInChildren<Animation>().animation.CrossFade("Male_idle1_anim");
					
					
					
					
				}
				else
				{
					targetHeading = waypoints[targetwaypoint].position - xform.position;
					
					//currentHeading = Vector3.Lerp(currentHeading,targetHeading,damping*Time.deltaTime);
				}
			}
		}
	}
	public void moveToWaypoint(int wayPointNum){
		
		
		targetwaypoint = wayPointNum;
		
		
		
	}
	
	// draws red line from waypoint to waypoint
	public void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		if(waypoints==null)
			return;
		for(int i=0;i< waypoints.Length;i++)
		{
			Vector3 pos = waypoints[i].position;
			if(i>0)
			{
				Vector3 prev = waypoints[i-1].position;
				Gizmos.DrawLine(prev,pos);
			}
		}
	}
	
}