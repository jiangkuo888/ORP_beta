using UnityEngine;
using System.Collections;

namespace PixelCrushers.DialogueSystem.Examples {

	public class LeaveSheet : MonoBehaviour {

		public void OnConversationStart(){
			
			//disable click of desktop
			if (GameObject.Find (PhotonNetwork.playerName))
			{
				GameObject.Find (PhotonNetwork.playerName).GetComponent<ClickMove>().enabled = false;
				GameObject.Find (PhotonNetwork.playerName).GetComponent<MouseCamera>().enabled = false;
				GameObject.Find (PhotonNetwork.playerName).GetComponent<DetectObjects>().enabled = false;
				GameObject.Find (PhotonNetwork.playerName).GetComponent<Selector>().enabled = false;



			}

		}
		
		public void OnConversationEnd(){
			
			GameObject.Find (PhotonNetwork.playerName).GetComponent<ClickMove>().enabled = true;
			GameObject.Find (PhotonNetwork.playerName).GetComponent<MouseCamera>().enabled = true;
			GameObject.Find (PhotonNetwork.playerName).GetComponent<DetectObjects>().enabled = true;
			GameObject.Find (PhotonNetwork.playerName).GetComponent<Selector>().enabled = true;

			
		}
	}

}
