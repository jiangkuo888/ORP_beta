using UnityEngine;
using System.Collections;

public class pageHandler : MonoBehaviour {

	public bool isLastPage;
	public GameObject [] Children;

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
		
		System.Collections.Generic.List<GameObject> list = new System.Collections.Generic.List<GameObject>(Children);
		
		if(!list.Contains(tr.gameObject))
			list.Add(tr.gameObject);
		
		Children = list.ToArray();


	}



}
