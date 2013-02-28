using UnityEngine;
using System.Collections;

public class Radar : MonoBehaviour {
	
	public Texture image, friendly, targetimg;
	public GameObject me, target;
	Vector3 local;
	
	int textureHeightWidth, padding, radarLeft, radarTop, radarCenterX, radarCenterY;
	
	
	int scale = 10;

	// Use this for initialization
	void Start () {
		me = GameObject.Find("helicopter");
		target = GameObject.Find("target");
		
		textureHeightWidth = 200;
		padding = 10;
		radarLeft = 10;
		radarTop = Screen.height - (textureHeightWidth + padding);
		
		
		
		
			
	}
	
	void OnGUI()
	{
		GUI.DrawTexture(new Rect(radarLeft, radarTop, textureHeightWidth, textureHeightWidth), image); 
		
		
		GUI.DrawTexture(new Rect(radarLeft, radarTop, textureHeightWidth, textureHeightWidth), friendly);
		
		
		
		
		
		GUI.DrawTexture(new Rect((local.x / scale), radarTop, textureHeightWidth, textureHeightWidth), targetimg);
		
		//GUI.Box(
	}
	
	// Update is called once per frame
	void Update () {
	
		if (target != null)
			local = me.transform.InverseTransformDirection(target.transform.position - me.transform.position);
		
	}
}
