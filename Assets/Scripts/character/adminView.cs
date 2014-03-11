using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem.Examples;

public class adminView: Photon.MonoBehaviour
{
	MouseCamera cameraScript;
	MouseCamera playerRotationScript;
	
	public Vector3 cameraRelativePosition = new Vector3(0,1.257728f, 0);
	
	void Awake()
	{
		
		
		// controllerScript = GetComponent<FPSInputController>();
		
	}
	void Start()
	{
		
		//TODO: Bugfix to allow .isMine and .owner from AWAKE!
		Debug.Log (photonView.isMine);
		if (photonView.isMine)
		{
			
			Camera.main.transform.parent = transform;
			Camera.main.transform.localPosition = cameraRelativePosition;
			Camera.main.transform.localEulerAngles = new Vector3(0.6651921f, 90, 0);
			
			if(cameraScript == null)
				cameraScript = GameObject.Find ("Main Camera").GetComponent<MouseCamera>();
			if(playerRotationScript == null)
				playerRotationScript = transform.GetComponent<MouseCamera>();
			
			
			playerRotationScript.enabled = true;
			cameraScript.enabled = true;
			
			gameObject.GetComponent<ClickMove>().enabled = true;
		//	gameObject.GetComponent<CharacterMotor>().enabled = true;
			//			gameObject.GetComponent<DetectObjects>().enabled = true;
			
		}
		/*else
		{           
			
			
			
			if(playerRotationScript == null)
				playerRotationScript = transform.GetComponent<MouseCamera>();
			
			Renderer[] rs =  this.transform.GetComponentsInChildren<Renderer>();
			foreach (Renderer r in rs)
				r.enabled = true;
			playerRotationScript.enabled = false;
			gameObject.GetComponent<ClickMove>().enabled = false;
			gameObject.GetComponent<CharacterMotor>().enabled = false;
			//	gameObject.GetComponent<DetectObjects>().enabled = false;
			gameObject.GetComponent<Selector>().enabled = false;
			//  controllerScript.enabled = true;
			
		}*/
		
		//gameObject.GetComponent<ClickMove>().SendMessage("SetIsRemotePlayer", !photonView.isMine);
		
		
		
		
		
		
	}
	
	/*void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			//We own this player: send the others our data
			// stream.SendNext((int)controllerScript._characterState);
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			
			
			switch(this.GetComponent<AnimationController>().state)
			{
			case AnimationController.CharacterState.idle:
				stream.SendNext("idle");
				stream.SendNext (PhotonNetwork.playerName);
				break;
			case AnimationController.CharacterState.run:
				stream.SendNext("run");
				stream.SendNext (PhotonNetwork.playerName);
				break;
			case AnimationController.CharacterState.computer:
				stream.SendNext("computer");
				stream.SendNext (PhotonNetwork.playerName);
				break;
			case AnimationController.CharacterState.walk:
				stream.SendNext("walk");
				stream.SendNext (PhotonNetwork.playerName);
				break;
			default:
				break;
				
			}
			
			
		}
		else
		{
			//Network player, receive data
			//controllerScript._characterState = (CharacterState)(int)stream.ReceiveNext();
			correctPlayerPos = (Vector3)stream.ReceiveNext();
			correctPlayerRot = (Quaternion)stream.ReceiveNext();
			//        rigidbody.velocity = (Vector3)stream.ReceiveNext();
			correctState = (string)stream.ReceiveNext();
			correctRole = (string)stream.ReceiveNext();
			
		}
	}*/
	
	private Vector3 correctPlayerPos = Vector3.zero; //We lerp towards this
	private Quaternion correctPlayerRot = Quaternion.identity; //We lerp towards this
	private string correctState = "idle";
	private string correctRole = "";
	void Update()
	{

		
		if (!photonView.isMine)
		{
			//Update remote player (smooth this, this looks good, at the cost of some accuracy)
			transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * 5);
			transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * 5);
			
			
			//transform.GetComponent<AnimationController>().updateState(correctState,correctRole);
			//.SendMessage("updateState",correctState);
			
			//			print ("correctPlayerPos : "+correctPlayerPos);
			//	print ("transform.position: "+transform.position);
		}
		
	}


}
//