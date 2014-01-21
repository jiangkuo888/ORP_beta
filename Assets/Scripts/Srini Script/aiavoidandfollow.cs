using UnityEngine;
using System.Collections;

public class aiavoidandfollow : MonoBehaviour
{   
	public Transform target;
	public float moveSpeed;
	public float rotationSpeed;
	public float minDistance = 0.5f;
	public static aiavoidandfollow enemyAIself; 
	RaycastHit hit;
	
	void Awake()
	{
		enemyAIself = this;   
	}
	void Start () 
	{
		
	}
	
	void Update () 
	{
		
		if (GameObject.FindGameObjectWithTag ("manager")) {
			GameObject goTo = GameObject.FindGameObjectWithTag ("manager");
			target = goTo.transform;

			Vector3 dir = (target.position - transform.position).normalized;
			if (Physics.Raycast(transform.position, transform.forward, out hit, 5.0f, (1<<8)))
			{
				Debug.DrawRay(transform.position, hit.point, Color.blue);
				dir += hit.normal  * 50;
			}
			
			Vector3 leftR = transform.position;
			Vector3 rightR = transform.position;
			
			leftR.x -= 2;
			rightR.x += 2;
			
			if (Physics.Raycast(leftR, transform.forward, out hit, 5.0f, (1<<8)))
			{
				Debug.DrawRay(leftR, hit.point, Color.blue);
				dir += hit.normal  * 50;
			}
			
			if (Physics.Raycast(rightR, transform.forward, 5.0f, (1<<8)))
			{
				Debug.DrawRay(rightR, hit.point, Color.blue);
				dir += hit.normal  * 50;
				
			} 
			Quaternion rot = Quaternion.LookRotation(dir);
			transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
			
			if (Vector3.SqrMagnitude(target.position - transform.position)> (minDistance *  minDistance))
			{
				//move towards the target
				transform.position += transform.forward * moveSpeed * Time.deltaTime;
			}

		}
		
		

	}
	
	
}