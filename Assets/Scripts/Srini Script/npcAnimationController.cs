using UnityEngine;
using System.Collections;

public class npcAnimationController :  MonoBehaviour {
	public enum CharacterState
	{
		idle,
		run,
		tap,
		computer
		
	}
	
	
	public Animation target;
	// The animation component being controlled
	
	public CharacterState state = CharacterState.idle;
	private bool canLand = true;
	
	
	void Reset ()
		// run setup on component attach, so it is visually more clear which references are used
	{
		Setup ();
	}
	
	
	void Setup ()
		// If target or rigidbody are not set, try using fallbacks
	{
		if (target == null)
		{
			target = GetComponent<Animation> ();
		}
		
	}
	
	
	void Start ()
		// Verify setup, configure
	{
		Setup ();
		// Retry setup if references were cleared post-add
	}
	
	
	
	//	void Onjump ()
	//	// Start a jump
	//	{
	//		canLand = false;
	//		state = CharacterState.jump;
	//		
	//		Invoke ("idle", target["idle"].length);
	//	}
	
	
	void OnLand ()
		// Start a landing
	{
		canLand = false;
		state = CharacterState.idle;
		
		
	}
	
	
	void Land ()
		// End a landing and transition to normal animation state (ignore if not currently landing)
	{
		
		state = CharacterState.idle;
	}
	
	
	public void updateState(string newState){
		switch(newState){
		case "idle":
			state = CharacterState.idle;
			break;
		case "run":
			state = CharacterState.run;
			break;
		case "tap":
			state = CharacterState.tap;
			break;
		case "computer":
			state = CharacterState.computer;
			break;
		default:
			break;
		}
	}
	
	
	void Update ()
		// Animation control
	{
		switch (state)
		{
		case CharacterState.idle:
			target.CrossFade ("Male_idle1_anim");
			break;
		case CharacterState.run:
			target.CrossFade ("Male_run_anim");
			break;
		case CharacterState.tap:
			target.CrossFade ("Male_tap_anim");
			break;
		case CharacterState.computer:
			target.CrossFade ("Male_computer_anim");
			break;
			//			case CharacterState.jump:
			//				target.CrossFade ("jump");
			//			break;
			//			case CharacterState.Falling:
			//				target.CrossFade ("Fall");
			//			break;
			//			case CharacterState.Landing:
			//				target.CrossFade ("Land");
			//			break;
		}
	}
	
	
	
}