using UnityEngine;
using System.Collections;

public class Game3_Player : MonoBehaviour
{
	public GUISkin skin;					//GUI skin
	public GameObject mesh;					//Mesh
	public Texture2D[] texMove;				//Move texture
	public Texture2D texJump;				//Jump texture
	public AudioClip audioJump;				//Jump sound
	public AudioClip audioDead;				//Dead sound
	private int selectedTex;				//Selected texture
	public float texUpdateTime;				//Texture update time
	private float tmpTexUpdateTime;			//Tmp texture update time
	public float moveSpeed;					//Move speed
	public float jumpSpeed;					//Jump speed
	public float gravity;					//Gravity
	private Vector3 dir;					//The direction the player is movin
	private GameObject rightTouchPad;		//Right touchpad
	private GameObject leftTouchPad;		//Left touchpad
	private bool dead;						//Are we dead
	private CharacterController controller;	//The character controller
	
	void Start ()
	{
		//Find the character controller
		controller = GetComponent<CharacterController>();
		//Screen orientation to landscape left
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		//Find left touchpad
		leftTouchPad = GameObject.Find("LeftTouchPad");
		//Find right touchpad
		rightTouchPad = GameObject.Find("RightTouchPad");
		//Start SetupJoysticks
		StartCoroutine("SetupJoysticks");
		//Set sleep time to never
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}
	
	void Update ()
	{
		//If we are not dead
		if (!dead)
		{
			//Update
			MoveUpdate();
			TexUpdate();
		}
	}
	
	void MoveUpdate()
	{
		//If we hit a object
		RaycastHit hit;
		if (Physics.Raycast(transform.position, Vector3.up, out hit, 0.5f))
		{
			//If it is not the player
			if (hit.transform.gameObject.tag != "Player")
			{
				//Set dir y to -1
				dir.y = -1;
			}
		}
		
		//If we are grounded
		if (controller.isGrounded)
		{
			//If the game is not running on a android device
			if (Application.platform != RuntimePlatform.Android)
			{
				//Set dir x to Horizontal
				dir.x = Input.GetAxis("Horizontal") * moveSpeed;
				//If we get Space key down
				if (Input.GetKeyDown(KeyCode.Space))
				{
					//Set dir y to jumpSpeed
					dir.y = jumpSpeed;
					//Play jump sound
					audio.clip = audioJump;
					audio.Play();
				}
			}
			//If the game is running on a android device
			else
			{
				//Get left touchpad position x
				float pX = leftTouchPad.GetComponent<Joystick>().position.x;
				//Get left touchpad tap count
				float tC = rightTouchPad.GetComponent<Joystick>().tapCount;
				
				//Set dir x to touchpad x position
				dir.x = pX * moveSpeed;
				//If touchpad tap count are not 0
				if (tC != 0)
				{
					//Set dir y to jumpSpeed
					dir.y = jumpSpeed;
					//Play jump sound
					audio.clip = audioJump;
					audio.Play();
				}
			}
		}
		//If we are not grounded
		else
		{
			//Set dir y to gravity
			dir.y -= gravity * Time.deltaTime;
		}
		
		//Move the player
		controller.Move(dir * Time.smoothDeltaTime);
	}
	
	void TexUpdate()
	{
		//If we are not grounded
		if (!controller.isGrounded)
		{
			//Set main texture to jump texture
			mesh.renderer.material.mainTexture = texJump;
			return;
		}
		//If the game is not running on a android device
		if (Application.platform != RuntimePlatform.Android)
		{
			//Get Horizontal
			float h = Input.GetAxis("Horizontal");
			//If Horizontal is not 0
			if (h != 0)
			{
				//If Horizontal is bigger than 0
				if (h > 0)
				{
					//Set scale to 1,1,1
					mesh.transform.localScale = new Vector3(1,1,1);
				}
				//If Horizontal is less than 0
				else
				{
					//Set scale to -1,1,1
					mesh.transform.localScale = new Vector3(-1,1,1);
				}
			}
			//If Horizontal is 0
			else
			{
				//Set main texture to move texture
				mesh.renderer.material.mainTexture = texMove[0];
				return;	
			}
		}
		//If the game is running on a android device
		else
		{
			//Get left touchpad x position
			float pX = leftTouchPad.GetComponent<Joystick>().position.x;
			//If touchpad x position is not 0
			if (pX != 0)
			{
				//If touchpad x position is bigger than 0
				if (pX > 0)
				{
					//Set scale to 1,1,1
					mesh.transform.localScale = new Vector3(1,1,1);
				}
				//If touchpad x position is less than 0
				else
				{
					//Set scale to -1,1,1
					mesh.transform.localScale = new Vector3(-1,1,1);
				}
			}
			else
			{
				//Set main texture to move texture
				mesh.renderer.material.mainTexture = texMove[0];
				return;	
			}
		}
		
		//If tmpTexUpdateTime is bigger than texUpdateTime
		if (tmpTexUpdateTime > texUpdateTime)
		{
			//Set tmpTexUpdateTime to 0
			tmpTexUpdateTime = 0;
			
			//Add one to selectedTex
			selectedTex++;
			//If selectedTex si bigger than texMove.Length - 1
			if (selectedTex > texMove.Length - 1)
			{
				//Set selectedTex to 0
				selectedTex = 0;
			}
			//Set main texture to move texture
			mesh.renderer.material.mainTexture = texMove[selectedTex];
		}
		else
		{
			//Add 1 to tmpTexUpdateTime
			tmpTexUpdateTime += 1 * Time.deltaTime;
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		//If we are in a enemy trigger
		if (other.tag == "Enemy")
		{
			//Play dead sound
			audio.clip = audioDead;
			audio.Play();
			//Dont show renderer
			mesh.renderer.enabled = false;
			//Kill
			dead = true;
		}
	}
	
	void OnGUI()
	{
		GUI.skin = skin;
		
		//Menu Button
		if(GUI.Button(new Rect(Screen.width - 120,0,120,40),"Menu"))
		{
			Application.LoadLevel("Menu");
		}
		//If we are dead
		if (dead)
		{
			//Play Again Button
			if(GUI.Button(new Rect(Screen.width / 2 - 90,Screen.height / 2 - 60,180,50),"Play Again"))
			{
				Application.LoadLevel("Game 3");
			}
			//Menu Button
			if(GUI.Button(new Rect(Screen.width / 2 - 90,Screen.height / 2,180,50),"Menu"))
			{
				Application.LoadLevel("Menu");
			}
		}	
	}
	
	IEnumerator SetupJoysticks()
	{
		//Set touchpad position
		leftTouchPad.transform.position = new Vector3(0,0,0);
		rightTouchPad.transform.position = new Vector3(1,0,0);
		
		//Wait 1 second
		yield return new WaitForSeconds(1);
		
		//Start the touchpads
		leftTouchPad.GetComponent<Joystick>().StartGame();
		rightTouchPad.GetComponent<Joystick>().StartGame();
	}
}
