using UnityEngine;
using System.Collections;

public class pageData : MonoBehaviour {
	public string documentName;
	public GameObject pagePrototype;
	public Texture2D[] pageTextures;
	
	
	GameObject newPage;
	int currentPage;
	
	
	
	
	// Use this for initialization
	void Start () {
		
		currentPage = 0;
		
		if(pagePrototype == null)
			Debug.LogError("please attach a page model for " + this.name);
		
		if(documentName == null)
			Debug.LogError("please assign a document name for " + this.name);
		
		
		
		
		
		
		
		newPage = Instantiate(pagePrototype.gameObject,pagePrototype.transform.position,Quaternion.identity) as GameObject;
		newPage.renderer.material.mainTexture = pageTextures[currentPage];
		newPage.transform.parent = this.transform;
		newPage.name = "page_content";
		newPage.transform.localPosition = new Vector3(-0.0002115475f,0.001669302f ,0.001225402f);
		newPage.transform.localEulerAngles = new Vector3(90,0,0);
	}
	
	
	
	// Update is called once per frame
	void Update () {
		
	}
	
	
	public void showNextPage(){


		if(currentPage< pageTextures.Length-1)
		{
			currentPage++;
			newPage.renderer.material.mainTexture = pageTextures[currentPage];
		}
		
		
		
	}
	
	public void showPreviousPage(){
		if(currentPage> 0)
		{
			currentPage--;
			newPage.renderer.material.mainTexture = pageTextures[currentPage];
		}
	}
}



