using UnityEngine;
using System.Collections;

public class Email : MonoBehaviour {
	public GUIText emailShort;
	public Texture2D emailIcon;
	public Texture2D documentIcon;

	public string[] EmailsToBeSend;
	public string[] EmailsReceived;


	public string[] EmailSenders;
	public string[] EmailReceivers;

	public bool[] EmailHasBeenReceived;
	// Use this for initialization
	void Start () {
		this.GetComponent<GUITexture>().texture = null;
		emailShort.text = null;
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void hasNewEmail(string content,string sender){
		this.GetComponent<GUITexture>().texture = emailIcon;
		this.GetComponent<GUITexture>().enabled = true;
		emailShort.text = "You have unread email.";


		for (int i = 0 ; i < EmailsToBeSend.Length; i++)
		{
			if (content==EmailsToBeSend[i])
				EmailHasBeenReceived[i] = true;
			else 
				EmailHasBeenReceived[i] = false;
		}

	}

	public void hasNewDocument(string sender){
		this.GetComponent<GUITexture>().texture = documentIcon;
		this.GetComponent<GUITexture>().enabled = true;
		emailShort.text = "You have new document from "+ sender+" on your desk.";
		

		
	}



	public void clearNewEmail(){
		this.GetComponent<GUITexture>().texture = null;
		this.GetComponent<GUITexture>().enabled = false;
		emailShort.text = null;
	}


}
