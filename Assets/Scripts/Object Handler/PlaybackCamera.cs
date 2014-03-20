using UnityEngine;
using HutongGames.PlayMaker;

public class PlaybackCamera : Photon.MonoBehaviour {

	public bool isMainCameraChild = true;

	void Update()
	{
		GameObject gameManager = GameObject.Find("GameManager");  
		MainMenuVik vikky = gameManager.GetComponent<MainMenuVik>();
		bool isPlayback = vikky.isPlayback;

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
				this.gameObject.transform.parent = null;
			}

		}
		//main camera that is not child
		else if (this.gameObject.transform.parent != null && this.gameObject.transform.parent.name == "EZReplayM_sParent" && isPlayback  && this.gameObject.name != "Main Camera")
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

		}


	}
	
	
	
	
}