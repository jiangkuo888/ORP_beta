using UnityEngine;
using System.Collections;

public class documentData : MonoBehaviour {


	public GameObject[] documents;
	public GameObject fileModeObj;



	// Use this for initialization
	void Start () {


		if(documents.Length != 0)
			for(int i =0; i < documents.Length; i ++)
		{

			documents[i].transform.parent = fileModeObj.transform;


			documents[i].transform.localPosition = new Vector3(-0.2275543f +i*0.35f,0,-0.03671265f);
		}

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void addDocument(GameObject toBeAdd){

		System.Collections.Generic.List<GameObject> list = new System.Collections.Generic.List<GameObject>(documents);
		list.Add(toBeAdd);
		documents = list.ToArray();
	}

	public void removeDocument(GameObject toBeDelete){
		System.Collections.Generic.List<GameObject> list = new System.Collections.Generic.List<GameObject>(documents);
		list.Remove(toBeDelete);
		documents = list.ToArray();
	}
}
