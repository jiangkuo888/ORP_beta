using UnityEngine;
using System.Collections;

public class suitcaseObjList : MonoBehaviour {
	public string owner;
	public GameObject[] suitcaseList;	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void addObj(GameObject obj){

		System.Collections.Generic.List<GameObject> list = new System.Collections.Generic.List<GameObject>(suitcaseList);
		list.Add(obj);
		suitcaseList = list.ToArray();
	}
}
