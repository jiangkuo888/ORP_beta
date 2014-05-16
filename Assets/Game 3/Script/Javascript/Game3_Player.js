	public var skin : GUISkin;						//GUI skin
	public var mesh : GameObject;					//Mesh
	public var texMove : Texture2D[];				//Move texture
	public var texJump : Texture2D;					//Jump texture
	public var audioJump : AudioClip;				//Jump sound
	public var audioDead : AudioClip;				//Dead sound
	private var selectedTex : int;					//Selected texture
	public var texUpdateTime : float;				//Texture update time
	private var tmpTexUpdateTime : float;			//Tmp texture update time
	public var moveSpeed : float;					//Move speed
	public var jumpSpeed : float;					//Jump speed
	public var gravity : float;						//Gravity
	private var dir : Vector3;						//The direction the player is movin
	private var rightTouchPad : GameObject;			//Right touchpad
	private var leftTouchPad : GameObject;			//Left touchpad
	private var dead = false;						//Are we dead
	private var controller : CharacterController;	//The character controller
	
	function Start ()
	{
		//Find the character controller
		controller = GetComponent(CharacterController);
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
	
	function Update ()
	{
		//If we are not dead
		if (!dead)
		{
			//Update
			MoveUpdate();
			TexUpdate();
		}
	}
	
	function MoveUpdate()
	{
		//If we hit a object
		var hit : RaycastHit;
		if (Physics.Raycast(transform.position, Vector3.up, hit, 0.5f))
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
				var pX = leftTouchPad.GetComponent(Joystick).position.x;
				//Get left touchpad tap count
				var tC = rightTouchPad.GetComponent(Joystick).tapCount;
				
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
	
	function TexUpdate()
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
			var h = Input.GetAxis("Horizontal");
			//If Horizontal is not 0
			if (h != 0)
			{
				//If Horizontal is bigger than 0
				if (h > 0)
				{
					//Set scale to 1,1,1
					mesh.transform.localScale = Vector3(1,1,1);
				}
				//If Horizontal is less than 0
				else
				{
					//Set scale to -1,1,1
					mesh.transform.localScale = Vector3(-1,1,1);
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
			var pX = leftTouchPad.GetComponent(Joystick).position.x;
			//If touchpad x position is not 0
			if (pX != 0)
			{
				//If touchpad x position is bigger than 0
				if (pX > 0)
				{
					//Set scale to 1,1,1
					mesh.transform.localScale = Vector3(1,1,1);
				}
				//If touchpad x position is less than 0
				else
				{
					//Set scale to -1,1,1
					mesh.transform.localScale = Vector3(-1,1,1);
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
	
	function OnTriggerEnter(other : Collider)
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
	
	function OnGUI()
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
	
	function SetupJoysticks()
	{
		//Set touchpad position
		leftTouchPad.transform.position = Vector3(0,0,0);
		rightTouchPad.transform.position = Vector3(1,0,0);
		
		//Wait 1 second
		yield WaitForSeconds(1);
		
		//Start the touchpads
		leftTouchPad.GetComponent(Joystick).StartGame();
		rightTouchPad.GetComponent(Joystick).StartGame();
	}