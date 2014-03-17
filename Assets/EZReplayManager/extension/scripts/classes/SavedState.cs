using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using System;

/*
 * SoftRare - www.softrare.eu
 * This class represents a state of a single object in one single frame. 
 * You may only use and change this code if you purchased it in a legal way.
 * Please read readme-file, included in this package, to see further possibilities on how to use/execute this code. 
 */

[Serializable()]
public class SavedState : ISerializable {
	//so far 3 state attributes are saved: position, rotation, and if the game object was emitting particles when being in this state
	public SerVector3 position;
	public SerVector3 localPosition;
	public SerQuaternion rotation;
	public SerQuaternion localRotation;
	
	public bool emittingParticles = false;
	public bool isMainCameraChild = true;
	public bool isActive = false;
	
	//serialization constructor
	protected SavedState(SerializationInfo info,StreamingContext context) {
		
		//this.position = (SerVector3)info.GetValue("position",typeof(SerVector3));
		this.localPosition = (SerVector3)info.GetValue("localPosition",typeof(SerVector3));
		//this.rotation = (SerQuaternion)info.GetValue("rotation",typeof(SerQuaternion));
		this.localRotation = (SerQuaternion)info.GetValue("localRotation",typeof(SerQuaternion));	
		
		emittingParticles = info.GetBoolean("emittingParticles");
		isActive = info.GetBoolean("isActive");
	}			
	
	//as this is not derived from MonoBehaviour, we have a constructor
	public SavedState(GameObject go) {
		
		if (go != null) {
			if(go.GetComponent<ParticleEmitter>())
				emittingParticles = go.GetComponent<ParticleEmitter>().emit;
			
			//this.position = new SerVector3(go.transform.position);
			//this.rotation = new SerQuaternion(go.transform.rotation);
			this.localPosition = new SerVector3(go.transform.localPosition);
			this.localRotation = new SerQuaternion(go.transform.localRotation);
			this.isActive = go.activeInHierarchy;

			//only for main camera
			if (go.tag == "MainCamera" /*&& go.transform.parent == null*/)
			{

				this.isMainCameraChild = go.GetComponent<PlaybackCamera>().isMainCameraChild;
			}

		} else {
			//this.position = new SerVector3(Vector3.zero);
			//this.rotation = new SerQuaternion(Quaternion.identity);
			this.localPosition = new SerVector3(Vector3.zero);
			this.localRotation = new SerQuaternion(Quaternion.identity);			
			this.isActive = false;	
		}

		if (go.tag == "MainCamera")
		{
			//Debug.Log (this.localPosition.x);
			//Debug.Log (this.localPosition.y);
			//Debug.Log (this.localPosition.z);
			//Debug.Log (this.isMainCameraChild);
		}

	}
	
	public Vector3 serVec3ToVec3(SerVector3 serVec3) {
		return new Vector3(serVec3.x,serVec3.y,serVec3.z);
	}
	
	public Quaternion serQuatToQuat(SerQuaternion serQuat) {
		return new Quaternion(serQuat.x,serQuat.y,serQuat.z,serQuat.w);
	}	
	
	public bool isDifferentTo(SavedState otherState) {
		bool changed = false;
		
		if (!changed && isActive != otherState.isActive)
			changed = true;			
		
		/*if (!changed && position.isDifferentTo(otherState.position) )
			changed = true;
		
		if (!changed && rotation.isDifferentTo(otherState.rotation) )
			changed = true;*/
		
		if (!changed && localPosition.isDifferentTo( otherState.localPosition) )
			changed = true;
		
		if (!changed && localRotation.isDifferentTo( otherState.localRotation) )
			changed = true;
		
		if (!changed && emittingParticles != otherState.emittingParticles)
			changed = true;	
	
		if (!changed && isMainCameraChild != otherState.isMainCameraChild)
		{

			changed = true;	
		}
		if ((!this.isMainCameraChild || !otherState.isMainCameraChild))
		{
			//Debug.Log ("this");
			//Debug.Log (this.isMainCameraChild);
			//Debug.Log ("other");
			//Debug.Log (otherState.isMainCameraChild);
		}
		return changed;
	}		
	
	//called to synchronize gameObjectClone of Object2PropertiesMapping back to this saved state
	public void synchronizeProperties(GameObject go) {
		
		//HINT: lerping is still highly experimental
		//EZReplayManager.singleton.StartCoroutine_Auto(EZReplayManager.singleton.MoveTo (go.transform,serVec3ToVec3(this.localPosition),0.08f));
		
		go.transform.localPosition = serVec3ToVec3(this.localPosition);
		go.transform.localRotation = serQuatToQuat(this.localRotation);		
	
		//go.transform.position = serVec3ToVec3(this.position);
		//go.transform.rotation = serQuatToQuat(this.rotation);
		
		go.SetActive( this.isActive );
		
		if (emittingParticles) 
			go.GetComponent<ParticleEmitter>().emit = true;
		else if ( go.GetComponent<ParticleEmitter>() ) 
			go.GetComponent<ParticleEmitter>().emit = false;

		//only for main camera
		//Debug.Log (this.isMainCameraChild);
		//Debug.Log (go.tag);
		if (go.tag == "MainCamera" && go.transform.parent != null)
		{
			//Debug.Log (go.name);
			//Debug.Log (go.name);
			//Debug.Log (this.localPosition.x);
			//Debug.Log (this.localPosition.y);
			//Debug.Log (this.localPosition.z);
			//Debug.Log (this.isMainCameraChild);
			bool compareOne = go.GetComponent<PlaybackCamera>().isMainCameraChild;
			bool compareTwo = this.isMainCameraChild;
			if (compareOne != compareTwo)
			{
				//Debug.Log(compareTwo);
				//Debug.Log (go.position);
				go.GetComponent<PlaybackCamera>().isMainCameraChild = this.isMainCameraChild;
			}		
		}

	}
	
	/*[SecurityPermissionAttribute(
	            SecurityAction.Demand,
	            SerializationFormatter = true)]		*/
	public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		
		//info.AddValue("position",position);		
		info.AddValue("localPosition",localPosition);
		//info.AddValue("rotation",rotation);		
		info.AddValue("localRotation",localRotation);		
		
		info.AddValue("emittingParticles", this.emittingParticles);
		info.AddValue("isActive", this.isActive);
		//base.GetObjectData(info, context);
	}	
	
}