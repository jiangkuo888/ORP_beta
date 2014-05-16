public var target : Transform;	//The player
public var smoothTime = 0.3f;	//Smooth Time
private var velocity : Vector2;	//Velocity

function  Update ()
{
	//Set the position
	transform.position = Vector3(Mathf.SmoothDamp(transform.position.x, target.position.x, velocity.x, smoothTime),Mathf.SmoothDamp( transform.position.y, target.position.y, velocity.y, smoothTime),transform.position.z);
}