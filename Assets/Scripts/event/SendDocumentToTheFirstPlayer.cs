using UnityEngine;
using System.Collections;

public class SendDocumentToTheFirstPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SendDocument(string targetPlayer,string DocName){
		if(PhotonNetwork.playerName == targetPlayer)
		{
			GameObject.Find(PhotonNetwork.playerName+" Table").gameObject.transform.Find ("DocumentHolder").GetComponent<documentData>().addDocument(GameObject.Find (DocName));

			print ("sent");
		}
	}

}
