using UnityEngine;
using System.Collections;

public class Game3_Camera : MonoBehaviour
{
	public Transform target;		//The player
	public float smoothTime= 0.3f;	//Smooth Time
	private Vector2 velocity;		//Velocity
	void  Update ()
	{
		//Set the position
		transform.position = new Vector3(Mathf.SmoothDamp(transform.position.x, target.position.x, ref velocity.x, smoothTime),Mathf.SmoothDamp( transform.position.y, target.position.y, ref velocity.y, smoothTime),transform.position.z);
	}
}
