using UnityEngine;
using System.Collections;

public class ClickMove : MonoBehaviour
{
	
	public bool gameOn;
	private CharacterMotor motor;
	public float smooth; // Determines how quickly object moves towards position
	private Camera myCamera;
	public GameObject arrowPrefab;
	private GameObject arrow;
	
	public Vector3 targetPosition;
	private Vector3 groundPosition;
	public float speed = 0.01f;
	public float heightOffset = 1.0f;


	
	
	//Shader originalShader = Shader.Find ("Diffuse");
	//Shader highlightShader = Shader.Find ("FX/Flare");
	//GameObject currentHitObj = null;
	
	
	// Use this for initialization
	void Start ()
	{

		gameOn = true;

		groundPosition = transform.position - new Vector3 (0, heightOffset, 0);
		
		arrow = Instantiate (arrowPrefab, transform.position, Quaternion.identity) as GameObject;
		
		myCamera = GameObject.FindGameObjectWithTag ("MainCamera").transform.GetComponent<Camera> ();
		
		motor = GetComponent<CharacterMotor> ();
		
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
		Vector3 dir = targetPosition - transform.position;
		
		float dist = dir.magnitude - 1;
		
		float move = speed * Time.deltaTime;
		
		if (dist > move) {
			
			
			arrow.renderer.enabled = true;
			motor.inputMoveDirection = dir.normalized * move;
			transform.GetComponent<AnimationController> ().state = AnimationController.CharacterState.run;
			
			
		} else {
			
			//transform.position = targetPosition;
			
			arrow.renderer.enabled = false;
			motor.inputMoveDirection = Vector3.zero;
			transform.GetComponent<AnimationController> ().state = AnimationController.CharacterState.idle;
			
			
		}
		
		
	}
	
	
	
	
	
	
	void Update ()
	{
		if(gameOn)
		{
		if (!motor.grounded)
			targetPosition = transform.position;
		else if(!GameObject.Find ("Inventory").GetComponent<inventory>().mouseOnGUIButton){
			
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
					if(hit.collider.gameObject.tag == "ground")
					{
						
						smooth = 1;
						
						
						
						
						Vector3 targetPoint = hit.point;
						
						
						
						// move the arrow to the click point and spin it, disable it after 2s
						arrow.transform.position = targetPoint;
						
						arrowAnimation ();
						
						targetPosition = targetPoint;
						
						//print (hit.collider.gameObject.name);
						
					}


					else
					{
						smooth = 1;
						
					
						//print (hit.collider.gameObject.name);
						
						Vector3 targetPoint = new Vector3(hit.point.x,transform.position.y-heightOffset,hit.point.z);
						
						
						
						// move the arrow to the click point and spin it, disable it after 2s
						arrow.transform.position = targetPoint;
						
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
							arrow.transform.position = targetPoint;
							arrowAnimation ();
							
							targetPosition = arrow.transform.position;
						}
					}
				}
				
			}
			
		}
	}

	}
}



