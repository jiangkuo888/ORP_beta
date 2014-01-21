using UnityEngine;
using System.Collections;

public class PopupHandler : MonoBehaviour {

	float w,h;

	bool OneMinutePassed;
	bool TwoMinutePassed;
	public Rect windowRect;
	public GUIStyle MyPopupStyle;
	// Use this for initialization
	void Start () {
		w = Screen.width;
		h = Screen.height;
		windowRect = new Rect(w-.4f*w, 0, .4f*w, .4f*h);
		OneMinutePassed = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		if(OneMinutePassed)
		{
			// tell LPU Officer to place signage in front of lift
			if(PhotonNetwork.playerName == "LPU Officer")
				windowRect = GUI.Window(0, windowRect, DoMyWindow, "Your mission: Place the signage\n in front of lift.");
			else if (PhotonNetwork.playerName == "Sales Manager")
				windowRect = GUI.Window(0, windowRect, DoMyWindow, "Your mission: Clear the obstacles\n in front of the EXIT.");
		}
		if(TwoMinutePassed)
		{

			windowRect = GUI.Window(0, windowRect, DoMyWindow, "The fire drill has started, please \n proceed to the exit and leave the builing.");

		}
	}



	void DoMyWindow(int windowID) {
		if (GUI.Button(new Rect(.3f*w,.3f*h, 100, 20), "OK"))
		{
			OneMinutePassed = false;
			TwoMinutePassed = false;
		}
		
	}

	public void OneMinuteHasPassed(){
		OneMinutePassed = true;
	}

	public void TwoMinuteHasPassed(){
		TwoMinutePassed = true;
	}

	public void ReachedExit(){
		// call NGUI to display ending screen.
	}

}
