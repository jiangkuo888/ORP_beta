using UnityEngine;
using System.Collections;

public class pageHandler : MonoBehaviour {

	public bool isLastPage;
	public GameObject [] circles;



	public Texture2D[] circlesTextures;
	public Rect[] positons;
	public bool[] buttonClicked;

	// Use this for initialization
	void Start () {
		collectChildren();
		isLastPage = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void collectChildren(){

		foreach( Transform child in transform)
		{
			if(!child.name.Contains("signArea"))
			addToList(child);

		}
	}

	void addToList(Transform tr){
		
		System.Collections.Generic.List<GameObject> circleList = new System.Collections.Generic.List<GameObject>(circles);
		System.Collections.Generic.List<Texture2D> circleTextureList = new System.Collections.Generic.List<Texture2D>(circlesTextures);
		System.Collections.Generic.List<bool> boolList = new System.Collections.Generic.List<bool>(buttonClicked);


		if(!circleList.Contains(tr.gameObject))
		{
			circleList.Add(tr.gameObject);
			circleTextureList.Add(tr.renderer.material.mainTexture as Texture2D);
			boolList.Add(false);
		}
		
		circles = circleList.ToArray();
		circlesTextures = circleTextureList.ToArray();
		buttonClicked = boolList.ToArray();

	}



}
