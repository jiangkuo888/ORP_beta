using UnityEngine;
using System.Collections;

namespace PixelCrushers.DialogueSystem.Examples {

	public class LeaveSheet : MonoBehaviour {

		public Vector3 sheetsOriginalPosition;
		public Quaternion sheetsOriginalRotation;
		public float x = -0.05f;
		public float y = 0.07f;
		public float z = 0.08f;

		void Start()
		{
			sheetsOriginalPosition = this.gameObject.transform.position;
		}

		void Update()
		{
			//Vector3 newPosition = Camera.main.transform.position +  Camera.main.transform.forward*0.2f /*+ new Vector3 (0,1.5f,0)*/;;
			//newPosition.x += x;
			//newPosition.y += y;
			//newPosition.z += z;
			//this.gameObject.transform.position = newPosition;
		}


		public void OnConversationStart(){

			Debug.Log (PhotonNetwork.playerName);



			//disable click of desktop
			if (GameObject.Find (PhotonNetwork.playerName))
			{
				GameObject.Find (PhotonNetwork.playerName).GetComponent<ClickMove>().enabled = false;
				GameObject.Find (PhotonNetwork.playerName).GetComponent<MouseCamera>().enabled = false;
				GameObject.Find (PhotonNetwork.playerName).GetComponent<DetectObjects>().enabled = false;
				GameObject.Find (PhotonNetwork.playerName).GetComponent<Selector>().enabled = false;

				//move camera to desk and sheets in front of him
				GameObject.Find("Credit Risk Table").GetComponent<DeskMode>().moveCameraToDesk();
				sheetsOriginalPosition = this.gameObject.transform.position;
				sheetsOriginalRotation = this.gameObject.transform.rotation;

				Vector3 newPosition = Camera.main.transform.position +  Camera.main.transform.forward*0.2f /*+ new Vector3 (0,1.5f,0)*/;
				newPosition.x += x;
				newPosition.y += y;
				newPosition.z += z;
				this.gameObject.transform.position = newPosition;
				this.gameObject.transform.localEulerAngles = new Vector3(90.0f,180.0f,0);

			}

		}
		
		public void OnConversationEnd(){

			//disable click of desktop
			if (GameObject.Find (PhotonNetwork.playerName))
			{
				GameObject.Find (PhotonNetwork.playerName).GetComponent<ClickMove>().enabled = true;
				GameObject.Find (PhotonNetwork.playerName).GetComponent<MouseCamera>().enabled = true;
				GameObject.Find (PhotonNetwork.playerName).GetComponent<DetectObjects>().enabled = true;
				GameObject.Find (PhotonNetwork.playerName).GetComponent<Selector>().enabled = true;

				//move back to original places
				GameObject.Find("Credit Risk").GetComponent<DetectObjects>().moveCameraToPlayer();

				this.gameObject.transform.position = sheetsOriginalPosition;
				this.gameObject.transform.rotation = sheetsOriginalRotation;
			}
		}
	}

}
