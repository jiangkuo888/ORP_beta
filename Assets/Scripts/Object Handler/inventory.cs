using UnityEngine;
using System.Collections;

public class inventory : MonoBehaviour {



	public GameObject inventoryObject;
	public Texture inventoryObjectTexture;

	public Vector3 inventoryObjectOriginalScale;

	public Texture2D option1_texture;
	//public Texture2D option2_texture;
	//public Texture2D option3_texture;
	public Texture2D option4_texture;
	
	private float w;
	private float h;
	

	private LTRect option1;
	//private LTRect option2;
	//private LTRect option3;
	private LTRect option4;

	private bool optionIsOn;
	private bool animationIsOn;

	public bool mouseOnGUIButton;
	bool animationFinished;
	// Use this for initialization
	void Start () {

		animationFinished = false;

		mouseOnGUIButton =false;

		optionIsOn = false;
		animationIsOn = false;

		w = Screen.width;
		h = Screen.height;
	
		
		initOptions();
		

		inventoryObject = null;
		inventoryObjectTexture = null;
		this.GetComponent<GUITexture>().texture = null;
		this.GetComponent<GUITexture>().pixelInset= new Rect(.5f*w - 130, -(.5f*h)+20, 100 ,100);
	}
	


	void initOptions(){
		option1 = new LTRect(1.1f*w - option1_texture.width*0.2f, 1.1f*h - option1_texture.height*0.2f, option1_texture.width*.2f, option1_texture.height*.2f );
		//option2 = new LTRect(1.1f*w - option1_texture.width*0.2f, 1.1f*h - option1_texture.height*0.2f, option1_texture.width*.2f, option1_texture.height*.2f );
		//option3 = new LTRect(1.1f*w - option1_texture.width*0.2f, 1.1f*h - option1_texture.height*0.2f, option1_texture.width*.2f, option1_texture.height*.2f );
		option4 = new LTRect(1.1f*w - option1_texture.width*0.2f, 1.1f*h - option1_texture.height*0.2f, option1_texture.width*.2f, option1_texture.height*.2f );
	}
	// Update is called once per frame
	void OnGUI () {

		if(optionIsOn)
		{



	



		if(GUI.Button( option1.rect, option1_texture))
		{
				mouseOnGUIButton = true;

				initOptions();
				optionIsOn = false;
				animationIsOn = false;
				animationFinished = false;



				StartCoroutine(WaitAndDrop(0.5f));




		}
//		if(GUI.Button( option2.rect, option2_texture))
//			{
//				initOptions();
//				optionIsOn = false;
//				animationIsOn = false;
//			}
//		if(GUI.Button( option3.rect, option3_texture))
//			{
//				initOptions();
//				optionIsOn = false;
//				animationIsOn = false;
//			}
		if(GUI.Button( option4.rect, option4_texture))
			{
				initOptions();
				optionIsOn = false;
				animationIsOn = false;
			}

		}
		
		if(animationIsOn){

			optionIsOn = true;
			LeanTween.move(option1,new Vector2(0.92f*w,0.7f*h),1.0f).setEase(LeanTweenType.easeOutElastic);
//			LeanTween.move(option2,new Vector2(0.84f*w,0.73f*h),1.0f).setDelay(.05f).setEase(LeanTweenType.easeOutElastic);
//			LeanTween.move(option3,new Vector2(0.78f*w,0.8f*h),1.0f).setDelay(.1f).setEase(LeanTweenType.easeOutElastic);
			LeanTween.move(option4,new Vector2(0.76f*w,0.90f*h),1.0f).setDelay(.15f).setEase(LeanTweenType.easeOutElastic);

			animationIsOn = false;
			animationFinished = true;

		}
		GUI.matrix = Matrix4x4.identity;
	}


	
	// Update is called once per frame
	void Update () {
		if(inventoryObject != null)
		{
			if(!inventoryObjectTexture){
				Debug.LogError("Assign a Texture in the inspector.");
				return;
			}
			

			this.GetComponent<GUITexture>().texture = inventoryObjectTexture;
		}
	}


	public void updateInventoryObject(GameObject obj){

		GameObject.Find ("DropArea_"+obj.name).GetComponent<DropAreaController>().AreaActivated = true;
		inventoryObject = obj;
		string texture = "Assets/Resources/Textures/"+obj.name+".png";
		inventoryObjectTexture = (Texture)Resources.LoadAssetAtPath(texture, typeof(Texture));
	}

	public void clearInventory(){
		inventoryObject = null;
		inventoryObjectTexture = null;
		this.GetComponent<GUITexture>().texture = null;

	}



	IEnumerator WaitAndDrop(float sec){



		// Distance from your player    
		//float distance   = 3;     
		
		// Transforms a forward position relative to your player into the world space    
		Vector3 throwPos = new Vector3(GameObject.Find ("ClickArrow(Clone)").transform.position.x,GameObject.Find ("ClickArrow(Clone)").transform.position.y+ 1f,GameObject.Find ("ClickArrow(Clone)").transform.position.z);

		GameObject.Find ("DropArea_"+inventoryObject.name).GetComponent<DropAreaController>().AreaActivated = false;

		inventoryObject.transform.parent = null;
		inventoryObject.transform.position = throwPos;
		inventoryObject.transform.localScale = inventoryObjectOriginalScale;

		if(inventoryObject.GetComponent<Rigidbody>() == null)
		inventoryObject.AddComponent<Rigidbody> ();

		enableCollider (inventoryObject);
		enableRender (inventoryObject);

		PhotonView photonView = inventoryObject.GetPhotonView();

		photonView.RPC ("enableRenderer",PhotonTargets.AllBuffered);
		photonView.RPC ("enableCollider",PhotonTargets.AllBuffered);
		photonView.RPC ("enableRigidbody",PhotonTargets.AllBuffered);
		photonView.RPC ("updateAllInfo",PhotonTargets.AllBuffered,throwPos);





		// empty inventory area
		inventoryObject = null;
		inventoryObjectTexture = null;
		this.GetComponent<GUITexture>().texture = null;

		this.GetComponent<GUITexture>().enabled = false;


		yield return new WaitForSeconds (sec);
		mouseOnGUIButton = false;

	}


	void enableCollider(GameObject obj){

		Collider collider = obj.GetComponent<Collider> ();

		if (collider.enabled == false)
						collider.enabled = true;



		Collider[] colliders = obj.GetComponentsInChildren<Collider>();
		
		foreach ( Collider c in colliders)
		{
			if(c.enabled == false)
				c.enabled = true;
		}
		
	}
	void enableRender(GameObject obj)
	{


		Renderer renderer = obj.GetComponent<Renderer> ();
		
		if (renderer.enabled == false)
			renderer.enabled = true;


		Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
		
		foreach ( Renderer r in renderers)
		{
			if(r.enabled == false)
				r.enabled = true;
		}
	}



	void OnMouseOver(){

		optionOn();
	}

	void optionOn(){
	
		animationIsOn = true;

	}
}
