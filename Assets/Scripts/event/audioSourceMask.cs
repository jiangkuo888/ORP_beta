using UnityEngine;
using System.Collections;

public class audioSourceMask : MonoBehaviour
{
	
	
	private float defaultVolume;
	
	public float level2MinHeight = 6.0f;
	
	void Start()
	{
		
		defaultVolume = audio.volume;
		
	}
	void Update()
	{
		//switch target player 
		
		
		
		
		switch(PhotonNetwork.playerName)
		{
		case "Sales Manager":
			if(GameObject.Find("Sales Manager"))
			{
				if(GameObject.Find("Sales Manager").transform.position.y <= level2MinHeight)
				{
					if(this.tag == "lvl1")
						audio.volume = defaultVolume;
					else if(this.tag == "lvl2")
						audio.volume = 0;
				}
				else
				{
					if(this.tag == "lvl1")
						audio.volume = 0;
					else if(this.tag == "lvl2")
						audio.volume = defaultVolume;

				}
			}
			break;
		case "LPU Officer":
			if(this.tag == "lvl1")
				audio.volume = 0;
			else if(this.tag == "lvl2")
				audio.volume = defaultVolume;
			break;
		case "LPU Manager":
			if(this.tag == "lvl1")
				audio.volume = 0;
			else if(this.tag == "lvl2")
				audio.volume = defaultVolume;
			break;
		case "Credit Risk":
			if(this.tag == "lvl1")
				audio.volume = 0;
			else if(this.tag == "lvl2")
				audio.volume = defaultVolume;
			break;
		default:
			break;
			
			
			
			
			
			
			
			
		}
		
		
		
		
		
		
		
		
	}
}