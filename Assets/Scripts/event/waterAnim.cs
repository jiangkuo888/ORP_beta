using UnityEngine;
using System.Collections;

public class waterAnim : MonoBehaviour {
	public float speed;
	public float waterLevel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void waterGoUp(){
		print ("111");
		LeanTween.move(this.gameObject, new Vector3(this.gameObject.transform.position.x,waterLevel,this.gameObject.transform.position.z),10f/speed).setEase(LeanTweenType.easeOutQuad);

	}




}
