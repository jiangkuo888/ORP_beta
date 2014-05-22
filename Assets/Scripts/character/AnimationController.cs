using UnityEngine;
using System.Collections;


public class AnimationController: MonoBehaviour
{
	public string playerName;
	public enum CharacterState
	{
		idle,
		run,
		walk,
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
		playerName = PhotonNetwork.playerName;
		// Retry setup if references were cleared post-add
	}
	
	
	
	
	
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
	
	
	public void updateState(string newState,string pName){
		
		switch(newState){
		case "idle":
			state = CharacterState.idle;
			break;
		case "run":
			state = CharacterState.run;
			break;
		case "walk":
			state = CharacterState.walk;
			break;
		case "computer":
			state = CharacterState.computer;
			break;
			
		default:
			break;
		}

		playerName = pName;
	}
	
	
	void Update ()
		// Animation control
	{
		switch (state)
		{
		case CharacterState.idle:
			switch(playerName)
			{
			case "Sales Manager":
				target.CrossFade("MaleC_Idle");
				break;
			case "LPU Officer":
				target.CrossFade("MaleB_Idle");
				break;
			case "LPU Manager":
				target.CrossFade("MaleA_Idle");
				break;
			case "Credit Risk":
				target.CrossFade("MaleD_Idle");
				break;
			default:
				break;
				
			}
			//			if(playerName=="LPU Manager"){target.CrossFade("MaleA_Idle");}
			//			else if(playerName =="LPU Officer")target.CrossFade ("Male_idle1_anim");
			break;
		case CharacterState.run:
			switch(playerName)
			{
			case "Sales Manager":
				target.CrossFade("MaleC_Run");
				break;
			case "LPU Officer":
				target.CrossFade("MaleB_Run");
				break;
			case "LPU Manager":
				target.CrossFade("MaleA_Run");
				break;
			case "Credit Risk":
				target.CrossFade("MaleD_Run");
				break;
			default:
				break;
				
			}
			break;
			
		case CharacterState.computer:
			switch(playerName)
			{
			case "Sales Manager":
				target.CrossFade("MaleC_Computer");
				break;
			case "LPU Officer":
				target.CrossFade("MaleB_Computer");
				break;
			case "LPU Manager":
				target.CrossFade("MaleA_Computer");
				break;
			case "Credit Risk":
				target.CrossFade("MaleD_Computer");
				break;
			default:
				break;
				
			}
			break;
			
			
		case CharacterState.walk:
			switch(playerName)
			{
			case "Sales Manager":
				target.CrossFade("MaleC_Walk");
				break;
			case "LPU Officer":
				target.CrossFade("MaleB_Walk");

				break;
			case "LPU Manager":
				target.CrossFade("MaleA_Walk");
				break;
			case "Credit Risk":
				target.CrossFade("MaleD_Walk");
				break;
			default:
				break;
				
			}
			break;
			
		}
	}
	
	
	
}
