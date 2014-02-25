using UnityEngine;
using System.Collections;

public class pageData : MonoBehaviour {
	public string documentName;
	public GameObject pagePrototype;
	public Texture2D[] pageTextures;





	// Use this for initialization
	void Start () {

		if(pagePrototype == null)
			Debug.LogError("please attach a page model for " + this.name);

		if(documentName == null)
			Debug.LogError("please assign a document name for " + this.name);






		for ( int i = 0; i< pageTextures.Length; i++)
		{
			GameObject newPage = Instantiate(pagePrototype.gameObject,pagePrototype.transform.position,Quaternion.identity) as GameObject;
			newPage.renderer.material.mainTexture = pageTextures[i];
			newPage.transform.parent = this.transform;
			newPage.name = "page_" + (i+1);
			newPage.transform.localPosition = new Vector3(-0.0002115475f,0.001669302f,0.001225402f);
			newPage.transform.localEulerAngles = new Vector3(90,90,0);
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
