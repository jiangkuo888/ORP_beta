using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using dbConnect;

/// <summary>
/// This simple chat example showcases the use of RPC targets and targetting certain players via RPCs.
/// </summary>
public class ChatVik : Photon.MonoBehaviour
{
	public GUISkin customSkin;
	public Texture2D playerTextureSM;
	public Texture2D playerTextureLO;
	public Texture2D playerTextureLM;
	public Texture2D playerTextureCR;

    public static ChatVik SP;
    public List<string> messages = new List<string>();
	public List<Color> messageColor = new List<Color>();

	public float ChatArea_x;
	public float ChatArea_y;
	public float chatBoxHideTime = 20f;

	Color textColor;
	Texture2D playerTexture;
	bool textOn = false;

	private int chatHeight = (int)200;
	private Vector2 scrollPos = Vector2.zero;
	private string chatInput = "";
	private float lastUnfocusTime = 0;


    void Awake()
    {
		playerTexture = null;
        SP = this;
    }

    void OnGUI()
    {        
        GUI.SetNextControlName("");

		GUILayout.BeginArea(new Rect(Screen.width - Screen.width/2+ChatArea_x+100, ChatArea_y, Screen.width/2, Screen.height/3));
		GUILayout.BeginHorizontal(); 
		
		switch(PhotonNetwork.playerName)
		{
		case "Sales Manager":
			playerTexture = playerTextureSM;

			break;
		case "LPU Manager":
			playerTexture = playerTextureLM;

			break;
		case "LPU Officer":
			playerTexture = playerTextureLO;

			break;
		case "Credit Risk":
			playerTexture = playerTextureCR;
		
			break;
		default:
			break;
			
		}
		
		if(playerTexture != null)
			GUILayout.Box(playerTexture,customSkin.box);
		
		
		GUIStyle myStyle = new GUIStyle (GUI.skin.textField); 
		// do whatever you want with this style, e.g.:
		myStyle.margin=new RectOffset(0,0,15,0);
		
		GUI.SetNextControlName("ChatField");
		chatInput = GUILayout.TextField(chatInput,myStyle,GUILayout.Width(300));

		// if the user has no action for a period of time, disable the chat box
		if(lastUnfocusTime <Time.time - chatBoxHideTime)
			textOn = false;


		// if the user click the chat input bar, show the chat box.
		if (GUI.GetNameOfFocusedControl() == "ChatField")
		{   
			lastUnfocusTime = Time.time;
			
			enableTextBox();
		}

		// if the user hit ENTER, show the chat box.
		if (Event.current.type == EventType.Layout && Event.current.character == '\n'){



			enableTextBox();

			// if already focus of chat input, send the message
			if (GUI.GetNameOfFocusedControl() == "ChatField")
			{                
				SendChat(PhotonTargets.All);
				lastUnfocusTime = Time.time;
				GUI.FocusControl("");
				GUI.UnfocusWindow();
			}
			else  // else focus on the chat input
			{
				if (lastUnfocusTime < Time.time - 0.1f)
				{
					GUI.FocusControl("ChatField");
				}
			}
		}
		
		//if (GUILayout.Button("SEND", GUILayout.Height(17)))
		//   SendChat(PhotonTargets.All);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();


		if(textOn)
		{
	        //Show scroll list of chat messages
	        scrollPos = GUILayout.BeginScrollView(scrollPos,customSkin.customStyles[3]);

	        for (int i = messages.Count - 1; i >= 0; i--)
	        {
				GUI.contentColor = messageColor[i]; 
	            GUILayout.Label(messages[i]);
	        }
	        GUILayout.EndScrollView();


		}
        GUI.color = Color.white;

        //Chat input
        

    

        GUILayout.EndArea();
    }

    public static void AddMessage(string text,Color color)
    {
		SP.messageColor.Add(color);
        SP.messages.Add(text);
        if (SP.messages.Count > 15)
		{
            SP.messages.RemoveAt(0);
			SP.messageColor.RemoveAt(0);
		}
    }


    [RPC]
    void SendChatMessage(string text, PhotonMessageInfo info)
    {

		switch(info.sender.ToString())
		{
		case "Sales Manager":

			textColor = Color.yellow;
			break;
		case "LPU Manager":

			textColor = Color.blue;
			break;
		case "LPU Officer":

			textColor = Color.cyan;
			break;
		case "Credit Risk":

			textColor = Color.red;
			break;
		default:
			break;
			
		}


		AddMessage("[" + info.sender + "] " + text,textColor);

		//get roomID & playerName
		GameObject gameManager = GameObject.Find("GameManager");  
		GameManagerVik vikky = gameManager.GetComponent<GameManagerVik>();
		string sessionID = vikky.sessionID.ToString ();
		string playerName = vikky.loginName;
		string playerRole = PlayerPrefs.GetString("playerName");
		
		//add to db
		dbClass db = new dbClass();
		db.addFunction("addChatLog");
		db.addValues("playerName", playerName);
		db.addValues("playerRole", playerRole);
		db.addValues("sessionID", sessionID);
		db.addValues("chatString", text);
		string dbReturn = db.connectToDb();
		
		if (dbReturn != "SUCCESS NO RETURN") {
//			print (dbReturn);
		}

    }

    void SendChat(PhotonTargets target)
    {
        if (chatInput != "")
        {
            photonView.RPC("SendChatMessage", target, chatInput);
            chatInput = "";
        }
    }

    void SendChat(PhotonPlayer target)
    {
        if (chatInput != "")
        {
            chatInput = "[PM] " + chatInput;
            photonView.RPC("SendChatMessage", target, chatInput);
            chatInput = "";
        }
    }

    void OnLeftRoom()
    {
        this.enabled = false;
    }

    void OnJoinedRoom()
    {
        this.enabled = true;
    }
    void OnCreatedRoom()
    {
        this.enabled = true;
    }




	void enableTextBox(){

		textOn = true;

	}

}
