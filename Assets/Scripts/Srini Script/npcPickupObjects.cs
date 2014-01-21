using UnityEngine;
using System.Collections;

public class npcPickupObjects : MonoBehaviour {

	private bool reachedDestinationTarget;

	public string destinationTarget = "";
	public string mover="";
	public Vector3 positioninDestination ;
	public Vector3 positioninmover ;
	
	void Start(){
		reachedDestinationTarget = false;
	}

	void  OnTriggerEnter ( Collider other  ){
		print(other.collider.name);
		print (reachedDestinationTarget);

			if (other.transform.name==destinationTarget|| other.transform.tag==destinationTarget)
			
		{

			transform.parent=other.transform;
			transform.localPosition = new Vector3(positioninDestination.x,positioninDestination.y,positioninDestination.z);





			reachedDestinationTarget = true;

				
			
		}
		else if((other.transform.name == mover || other.transform.tag== mover) && reachedDestinationTarget == false)
		{
			//print ("111");
			transform.parent = other.transform;
			transform.localPosition = new Vector3(positioninmover.x,positioninmover.y,positioninmover.z);

			//this.transform.parent.GetComponent<npcAnimationController> ().animation.CrossFade ("Male_run_anim");

	
		}
		
	
	}
}

