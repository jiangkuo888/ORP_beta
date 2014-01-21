using UnityEngine;
using System.Collections;

public class liftTriggerHandler : MonoBehaviour {
	
	
	
	public bool enteredLift;
	Transform dugManager;
	bool GUIisOn;


	GameObject enteredObj;
	
	// Use this for initialization
	void Start () {
		GUIisOn = false;
		enteredLift = false;
		enteredObj = null;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnGUI(){
		
		if(GUIisOn)
			if (GUI.Button(new Rect(100, 400,300, 30), "Please restrain from using the lift!!"))

		{
			if(enteredObj != null)
			{
				//transform.Translate(-Vector3.forward);
				enteredObj.GetComponent<DetectObjects>().moveCameraToPlayer();
				enteredObj.GetComponent<DetectObjects>().enableCameraAndMotor();
				enteredObj.GetComponent<ClickMove>().enabled=false;
				StartCoroutine(wait());				



			}

			GUIisOn = false;
			
		}
		
		
		

	}

	IEnumerator wait() {
		yield return new WaitForSeconds(1);
		enteredObj.GetComponent<ClickMove>().enabled=true;
	}
	
	
	void OnTriggerEnter(Collider obj){
		if(obj.tag == "manager")
		{
			enteredObj = obj.gameObject;
			//dugManager = enteredObj.transform.Find("DUGManager");
			
			//hitObjPhotonView = PhotonView.Get(hit.collider.gameObject);
			
			//hit.transform.renderer.material.color = Color.green;
			
			
			enteredLift = true;
			
			if(enteredLift)
			{
				//currentHitObj.renderer.material.shader = originalShader;
				
				
				//				DUGController.Init( dugManager.GetComponent<DUGModel>() );
				//				DUGController.Run("notWorkingNode1");
				
				GUIisOn = true;
				
				//print(enteredObj.collider.name);
				//dugManager.GetComponent<DialogueController>().setActiveDialogue("npcTrigger");
				enteredObj.transform.Translate(-enteredObj.transform.forward*1.5f);


				enteredObj.GetComponent<DetectObjects>().disableCameraAndMotor();
				enteredObj.GetComponent<DetectObjects>().moveCameraToObject(GameObject.Find("liftTrigger").collider.gameObject);
				
			}
			
		}
	}
	
}
