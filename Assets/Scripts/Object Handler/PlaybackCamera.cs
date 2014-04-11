using UnityEngine;
using HutongGames.PlayMaker;

public class PlaybackCamera : Photon.MonoBehaviour {

	public bool isMainCameraChild = false;
	public Transform parent;

	void start()
	{
	}

	void Update()
	{
		GameObject gameManager = GameObject.Find("GameManager"); 
		GameManagerVik vikky = gameManager.GetComponent<GameManagerVik>();
		bool isPlayback = vikky.isPlayBack;

		if (isPlayback && this.gameObject.name == "Main Camera") 
		{
			this.gameObject.SetActive (false);
		}
		else if (isPlayback && this.gameObject.name != "Main Camera") 
		{
			var nameArray = this.gameObject.name.Split('_');
			bool isMainCam = false;
			foreach (string namePart in nameArray)
			{
				if (namePart == "go")
				{
					isMainCam = true;
				}
			}
			if (!isMainCam)
			{
				this.gameObject.SetActive (false);
			}
		}

		if (isPlayback && Camera.main.farClipPlane < 1)
		{
			Camera.main.farClipPlane = 1000;
		}


	}

	public void test(string tag)
	{
		GameObject gameManager = GameObject.Find("GameManager"); 
		GameManagerVik vikky = gameManager.GetComponent<GameManagerVik>();
		bool isPlayback = vikky.isPlayBack;

		//main camera that is child of main guy
		if (this.gameObject.transform.parent != null && this.gameObject.transform.parent.name != "EZReplayM_sParent" && isPlayback)
		{
			//if isMainCameraChild is true, means the main camera in the real game is child of main guy
			/*if (isMainCameraChild && !this.gameObject.activeInHierarchy)
			{
				this.gameObject.SetActive(true);
			}
			//if isMainCameraChild is false, means the main camera in the real game is currently not a child
			else if (!isMainCameraChild && this.gameObject.activeInHierarchy)
			{
				this.gameObject.SetActive(false);
			}*/

			if (!isMainCameraChild)
			{
				this.parent = this.gameObject.transform.parent;
				this.gameObject.transform.parent = GameObject.Find("EZReplayM_sParent").transform;
			}

		}
		//main camera that is not child
		else if (this.gameObject.transform.parent != null && this.gameObject.transform.parent.name == "EZReplayM_sParent" && isPlayback)
		{
			//if isMainCameraChild is true, means the main camera in the real game is child of main guy
			/*if (isMainCameraChild && this.gameObject.activeInHierarchy)
			{
				this.gameObject.SetActive(false);
			}
			//if isMainCameraChild is false, means the main camera in the real game is currently not a child
			else if (!isMainCameraChild && !this.gameObject.activeInHierarchy)
			{
				this.gameObject.SetActive(true);
			}*/
			if (isMainCameraChild)
			{
				if (parent == null)
				{
					this.parent = GameObject.FindWithTag(tag).transform;
				}
				this.gameObject.transform.parent = this.parent;
				//this.gameObject.transform.localPosition = this.GetComponent<ThirdPersonNetworkVik>().cameraRelativePosition;
				//this.gameObject.transform.localEulerAngles = new Vector3(0.6651921f, 90, 0);
			}

		}

	}

	public bool isSubCam()
	{
		GameObject gameManager = GameObject.Find("GameManager"); 
		GameManagerVik vikky = gameManager.GetComponent<GameManagerVik>();
		bool isPlayback = vikky.isPlayBack;
		
		if (isPlayback) 
		{
			//check if name has go in it; if yes, means it is the real main cam; if not, disable it
			var nameArray = this.gameObject.name.Split('_');
			foreach (string namePart in nameArray)
			{
				if (namePart == "go")
				{
					return false;
				}
			}
			return true;

		}
		return true;
	}

}