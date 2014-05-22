using UnityEngine;
using System.Collections;

namespace PixelCrushers.DialogueSystem.Examples {
	
	/// <summary>
	/// This component indicates that the game object is usable. This component works in
	/// conjunction with the Selector component. If you leave overrideName blank but there
	/// is an OverrideActorName component on the same object, this component will use
	/// the name specified in OverrideActorName.
	/// </summary>
	public class Usable : MonoBehaviour {
		
		/// <summary>
		/// (Optional) Overrides the name shown by the Selector.
		/// </summary>
		public string overrideName;
		
		/// <summary>
		/// (Optional) Overrides the use message shown by the Selector.
		/// </summary>
		public string overrideUseMessage;
		
		/// <summary>
		/// The max distance at which the object can be used.
		/// </summary>
		public float maxUseDistance = 5f;
		
		public void Start() {
			if (string.IsNullOrEmpty(overrideName)) {
				OverrideActorName overrideActorName = GetComponentInChildren<OverrideActorName>();
				if (overrideActorName != null) overrideName = overrideActorName.overrideName;
			}
		}
		
		
		public void OnUse (Transform actor){
			
			PlayerPrefs.SetString("LocalActor",actor.name);
			PlayerPrefs.SetString("OnUsedObj",this.transform.name);
			
//			if(this.GetComponent<suitcaseObjList>()!= null)
//			{
//				if(this.GetComponent<suitcaseObjList>().owner ==actor.name)
//					DialogueLua.SetVariable("belongToYou",true);
//				else
//					DialogueLua.SetVariable("false",true);
//			}
		}

		public void OnConversationStart(){

			GameObject.Find ("phoneButton").GetComponent<phoneButton>().hide();
			GameObject.Find ("phoneButton").GetComponent<GUITexture>().enabled = false;
	//		GameObject.Find ("QuestLogButton").GetComponent<GUITexture>().enabled = false;
		}

		public void OnConversationEnd(){


			GameObject.Find ("phoneButton").GetComponent<GUITexture>().enabled = true;
//			GameObject.Find ("QuestLogButton").GetComponent<GUITexture>().enabled = true;

			GameObject.Find("Dialogue Manager").GetComponent<NPCsync>().addUsable(this.gameObject);

		}
		
	}
	
}
