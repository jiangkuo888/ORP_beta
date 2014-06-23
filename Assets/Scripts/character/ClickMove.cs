using UnityEngine;
using System.Collections;

public class ClickMove : MonoBehaviour
{
	
	public bool gameOn;
	//private CharacterMotor motor;
	private NavMeshAgent navAgent;
	public float smooth; // Determines how quickly object moves towards position
	private Camera myCamera;
	public GameObject arrowPrefab;
	private GameObject arrow;
	
	public Vector3 targetPosition;
	private Vector3 groundPosition;
	public float speed = 0.01f;
	public float heightOffset = 1.0f;
	
	public float moveCursorOffset = .05f;
	
	public bool OnGUI = false;
	
	
	//Shader originalShader = Shader.Find ("Diffuse");
	//Shader highlightShader = Shader.Find ("FX/Flare");
	//GameObject currentHitObj = null;
	
	
	// Use this for initialization
	void Start ()
	{
		OnGUI = false;
		gameOn = true;
		
		groundPosition = transform.position - new Vector3 (0, heightOffset, 0);
		
		arrow = Instantiate (arrowPrefab, transform.position, Quaternion.Euler(90,0,0)) as GameObject;
		
		
		
		myCamera = GameObject.FindGameObjectWithTag ("MainCamera").transform.GetComponent<Camera> ();
		
		//motor = GetComponent<CharacterMotor> ();
	
		navAgent = this.GetComponent<NavMeshAgent>();
		transform.GetComponent<AnimationController> ().state = AnimationController.CharacterState.idle;
		targetPosition = transform.position;
		
	}
	
	void arrowAnimation ()
	{
		
		
		
	}
	
	
	
	//		void  OnControllerColliderHit ( ControllerColliderHit hit  ){
	//			if (hit.normal.y > 0 && hit.normal.y > groundNormal.y && hit.moveDirection.y < 0) {
	//				if ((hit.point - movement.lastHitPoint).sqrMagnitude > 0.001f || lastGroundNormal == Vector3.zero)
	//					groundNormal = hit.normal;
	//				else
	//					groundNormal = lastGroundNormal;
	//				
	//				movingPlatform.hitPlatform = hit.collider.transform;
	//				movement.hitPoint = hit.point;
	//				movement.frameVelocity = Vector3.zero;
	//			}
	//		}
	// Update is called once per frame
	
	void FixedUpdate(){
		if(navAgent.enabled)
		{
			navAgent.destination = targetPosition;
			
			float dist = (targetPosition - transform.position).magnitude;
			
			
			
			if (dist > 1) {
				
				arrow.transform.Rotate(0,0,10);
				arrow.renderer.enabled = true;
				//motor.inputMoveDirection = dir.normalized * move;
				transform.GetComponent<AnimationController> ().state = AnimationController.CharacterState.run;
				
				
				
			} else {
				
				//transform.position = targetPosition;
				
				arrow.renderer.enabled = false;
				//motor.inputMoveDirection = Vector3.zero;
				transform.GetComponent<AnimationController> ().state = AnimationController.CharacterState.idle;
				
				GameObject.Find("SFX Player Footstep").GetComponent<AudioManager>().Stop(GameObject.Find("SFX Player Footstep").GetComponent<AudioManager>().Audioclips[0]);
				
			}
		}
		
	}
	
	[RPC]

	void setSMPosition(Vector3 newPosition){

		this.transform.position = newPosition;


	}



	
	public void SetPosition(Vector3 newPosition){

		navAgent.enabled = false;


		PhotonView SMView = this.gameObject.GetPhotonView();
		SMView.RPC ("setSMPosition",PhotonTargets.AllBuffered,newPosition);



		targetPosition = newPosition;
		
	}

	public void ResetNavAgent(){

		navAgent.enabled = true;
	}
	
	
	void Update ()
	{
		if(gameOn)
		{
			//if (!motor.grounded)
			//	targetPosition = transform.position;
			//	else {
			
			RaycastHit hit;
			
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			
			if (Physics.Raycast (ray, out hit, 1000)) {
				
				// change shader 
				//				if(hit.collider.gameObject != currentHitObj)
				//				{
				//					if(currentHitObj !=null) currentHitObj.transform.renderer.material.shader = originalShader;
				//					currentHitObj = hit.collider.gameObject;
				//					if(currentHitObj.tag == "interactive")
				//					{
				//						currentHitObj.transform.renderer.material.shader = highlightShader;
				//						
				//					}
				//				}
				
				//print (hit.collider.tag);
				
				if (Input.GetKeyUp (KeyCode.Mouse0)) {
					if(OnGUI)
					{
//						print ("nothing");
					}
					else{
						
						GameObject.Find("SFX Player Footstep").GetComponent<AudioManager>().Play(GameObject.Find("SFX Player Footstep").GetComponent<AudioManager>().Audioclips[0],gameObject.transform.position,1f,1f,false);
						
						
						if(hit.collider.gameObject.tag == "ground")
						{
							
							arrowAnimation ();
							
							
							smooth = 1;
							
							
							
							
							Vector3 targetPoint = hit.point;
							
							
							
							// move the arrow to the click point and spin it, disable it after 2s
							arrow.transform.position = new Vector3(targetPoint.x,targetPoint.y + moveCursorOffset,targetPoint.z);
							
							
							
							targetPosition = targetPoint;
							
							
							
							//print (hit.collider.gameObject.name);
							
						}
						
						
						else
						{
							smooth = 1;
							
							
							//print (hit.collider.gameObject.name);
							
							Vector3 targetPoint = new Vector3(hit.point.x,transform.position.y-heightOffset,hit.point.z);
							
							
							
							// move the arrow to the click point and spin it, disable it after 2s
							arrow.transform.position = new Vector3(targetPoint.x,targetPoint.y + moveCursorOffset,targetPoint.z);
							
							arrowAnimation ();
							
							targetPosition = targetPoint;
							
							//						if (hit.collider.gameObject.tag == "npcTrigger") {
							//							
							//							
							//							//hit.transform.renderer.material.color = Color.green;
							//							
							//							if (Input.GetKeyUp (KeyCode.Mouse0)) {
							//								if (hit.collider.transform.parent.gameObject.GetComponent<TriggerHandler> ().enteredObj == null) {
							//									targetPosition = hit.collider.transform.parent.gameObject.transform.position;
							//									arrow.transform.position = targetPosition;
							//								}
							//								
							//							}
							//						}
							
							//						print (hit.collider.name);
							
							if(hit.collider.gameObject.tag =="desk")
							{
								
								targetPoint = new Vector3(hit.collider.transform.position.x+0.8f,hit.collider.transform.position.y,hit.collider.transform.position.z);
								
								arrow.transform.position = new Vector3(targetPoint.x,targetPoint.y + moveCursorOffset,targetPoint.z);
								
								arrowAnimation ();
								
								targetPosition = arrow.transform.position;
							}
						}
					}
				}
				
				//}
				
			}
		}
		
	}
}



