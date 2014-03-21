using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem.Examples;

public class ThirdPersonNetworkVik : Photon.MonoBehaviour
{
    MouseCamera cameraScript;
	MouseCamera playerRotationScript;

	public Vector3 cameraRelativePosition = new Vector3(0,1.257728f, 0);
	public Vector3 cloneCameraRotation = new Vector3(0,0, 0);

    void Awake()
    {


       // controllerScript = GetComponent<FPSInputController>();

    }
    void Start()
    {

        //TODO: Bugfix to allow .isMine and .owner from AWAKE!
		if (photonView.isMine)
        {
            //MINE: local player, simply enable the local scripts


				

           // controllerScript.enabled = true;
			Renderer[] rs =  this.transform.GetComponentsInChildren<Renderer>();
			foreach (Renderer r in rs)
				r.enabled = false;


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
			gameObject.GetComponent<CharacterMotor>().enabled = true;
//			gameObject.GetComponent<DetectObjects>().enabled = true;
			
        }
        else
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

        }

		//gameObject.GetComponent<ClickMove>().SendMessage("SetIsRemotePlayer", !photonView.isMine);
	


    

       
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
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

			//get camera rotation and send it
			Transform mainCam = this.gameObject.transform.FindChild("Main Camera");
			if (mainCam != null)
			{
				stream.SendNext(mainCam.eulerAngles);
			}
			else
			{
				stream.SendNext(new Vector3(0,0,0));
			}
			//Debug.Log("send");
			//Debug.Log(mainCam.rotation);

			
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
			correctCameraRotation = (Vector3)stream.ReceiveNext();
			//Debug.Log("get");
			//Debug.Log(correctCameraRotation);

        }
    }

    private Vector3 correctPlayerPos = Vector3.zero; //We lerp towards this
    private Quaternion correctPlayerRot = Quaternion.identity; //We lerp towards this
	private string correctState = "idle";
	private string correctRole = "";
	private Vector3 correctCameraRotation = Vector3.zero; //We lerp towards this
    void Update()
    {




        if (!photonView.isMine)
        {
            //Update remote player (smooth this, this looks good, at the cost of some accuracy)
            transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * 5);


			transform.GetComponent<AnimationController>().updateState(correctState,correctRole);
			this.cloneCameraRotation = this.correctCameraRotation;
			//Debug.Log ("updated camera rotation");
			//Debug.Log (cloneCameraRotation);
			//.SendMessage("updateState",correctState);

//			print ("correctPlayerPos : "+correctPlayerPos);
		//	print ("transform.position: "+transform.position);
        }

    }
}

//    void OnPhotonInstantiate(PhotonMessageInfo info)
//    {
//        //We know there should be instantiation data..get our bools from this PhotonView!
//        object[] objs = photonView.instantiationData; //The instantiate data..
//        bool[] mybools = (bool[])objs[0];   //Our bools!
//
//        //disable the axe and shield meshrenderers based on the instantiate data
//        MeshRenderer[] rens = GetComponentsInChildren<MeshRenderer>();
//        rens[0].enabled = mybools[0];//Axe
//        rens[1].enabled = mybools[1];//Shield
//
//    }
//