using UnityEngine;
using System.Collections;

namespace PixelCrushers.DialogueSystem.Examples {
	
	/// <summary>
	/// This component indicates that the game object is usable. This component works in
	/// conjunction with the Selector component.
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
		
		public void OnUse(Transform actorTransform){

			PlayerPrefs.SetString("LocalActor",actorTransform.name);
			PlayerPrefs.SetString("OnUsedObj",this.name);

		}
		
		
			
		public void OnConversationStart(Transform actor) {
			DialogueLua.SetVariable("Actor", actor.name);				
			print(DialogueLua.GetVariable("Actor").AsString);
		}
	
	}
	
	
	
}
