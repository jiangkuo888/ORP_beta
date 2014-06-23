// /////////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audio Manager.
//
// This code is release under the MIT licence. It is provided as-is and without any warranty.
//
// Developed by Daniel Rodríguez (Seth Illgard) in April 2010
// http://www.silentkraken.com
//
// /////////////////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
	public AudioClip[] Audioclips;


	public AudioSource Play(AudioClip clip){

		if (PhotonNetwork.playerName == "admin")
		{
			return Play(clip, GameObject.Find ("Main Camera").transform, .7f, 1f);
		}
		else
		{
			return Play(clip, GameObject.Find (PhotonNetwork.playerName).transform, .7f, 1f);
		}

	}

	public AudioSource Play(AudioClip clip, Transform emitter)
	{
		return Play(clip, emitter, 1f, 1f);
	}
	
	public AudioSource Play(AudioClip clip, Transform emitter, float volume)
	{
		return Play(clip, emitter, volume, 1f);
	}
	
	/// <summary>
	/// Plays a sound by creating an empty game object with an AudioSource
	/// and attaching it to the given transform (so it moves with the transform). Destroys it after it finished playing.
	/// </summary>
	/// <param name="clip"></param>
	/// <param name="emitter"></param>
	/// <param name="volume"></param>
	/// <param name="pitch"></param>
	/// <returns></returns>
	public AudioSource Play(AudioClip clip, Transform emitter, float volume, float pitch)
	{
		//Create an empty game object
		GameObject go = new GameObject ("Audio: " +  clip.name);
		go.transform.position = emitter.position;
		go.transform.parent = emitter;
		
		//Create the source
		AudioSource source = go.AddComponent<AudioSource>();
		if(source.isPlaying == false){
		source.clip = clip;
		source.volume = volume;
		source.pitch = pitch;

		source.Play ();
		Destroy (go, clip.length);
		return source;
		}
		else
			return null;
	}
	
	public AudioSource Play(AudioClip clip, Vector3 point)
	{
		return Play(clip, point, 1f, 1f,false);
	}
	
	public AudioSource Play(AudioClip clip, Vector3 point, float volume)
	{
		return Play(clip, point, volume, 1f,false);
	}
	
	/// <summary>
	/// Plays a sound at the given point in space by creating an empty game object with an AudioSource
	/// in that place and destroys it after it finished playing.
	/// </summary>
	/// <param name="clip"></param>
	/// <param name="point"></param>
	/// <param name="volume"></param>
	/// <param name="pitch"></param>
	/// <returns></returns>
	public AudioSource Play(AudioClip clip, Vector3 point, float volume, float pitch,bool oneshot)
	{
		GameObject go;
		AudioSource source;
		//Create an empty game object
		if(GameObject.Find ("Audio: "+clip.name)==null)
		{

			go = new GameObject("Audio: " + clip.name);
			source = go.AddComponent<AudioSource>();
		}
		else{
			go = GameObject.Find ("Audio: "+clip.name);
			source = go.GetComponent<AudioSource>();
		}



		go.transform.position = point;
		
		//Create the source
		if(source.isPlaying == false){
		source.clip = clip;
		source.volume = volume;
		source.pitch = pitch;
		source.Play();

		if(oneshot)
		{
			source.loop = false;
			Destroy(go, clip.length);
			return source;
		}
		else
		{
			source.loop = true;
			return source;


		}

		}
		else
			return null;

	}

	public void Stop(AudioClip clip){

		if(GameObject.Find ("Audio: " +clip.name))
		{
			GameObject.Find ("Audio: "+clip.name).GetComponent<AudioSource>().Stop();
			Destroy (GameObject.Find ("Audio: "+ clip.name).gameObject);

		}

	}

	public void PlayRandom(){





	}
}