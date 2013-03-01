using UnityEngine;
using System.Collections;

public class Radar : MonoBehaviour {
	
	public Texture image, friendly, targetimg, rotateBeam;
	public GameObject me, target;
	Vector3 local;
	float offset, clock;
	
	int textureHeightWidth, padding, radarLeft, radarTop, radarCenterX, radarCenterY, delay, cycle;
	
	
	int scale = 5;

	// Use this for initialization
	void Start () {
		me = GameObject.Find("helicopter");
		target = GameObject.Find("target");
		
		textureHeightWidth = 200;
		padding = 10;
		radarLeft = 10;
		radarTop = Screen.height - (textureHeightWidth + padding);
		
		radarCenterX = radarLeft + (textureHeightWidth / 2);
		radarCenterY = radarTop + (textureHeightWidth / 2);
		
		offset = 0.0833f;
		
		delay = 3;
		
		cycle = 1;
			
	}
	
	void OnGUI()
	{
		GUI.DrawTexture(new Rect(radarLeft, radarTop, textureHeightWidth, textureHeightWidth), image); 
		
		

		//GUI.DrawTextureWithTexCoords(new Rect(radarLeft, radarTop, textureHeightWidth, textureHeightWidth), rotateBeam, new Rect(0, 0.5f , 1,  0.3332f));
		//GUI.DrawTextureWithTexCoords(new Rect(radarLeft, radarTop, textureHeightWidth, textureHeightWidth), rotateBeam, new Rect(offset * (cycle - 1),0, offset * cycle, 1));
		GUI.DrawTextureWithTexCoords(new Rect(radarLeft, radarTop, textureHeightWidth, textureHeightWidth), rotateBeam, new Rect(offset * cycle, 0, offset, 1));
		
		if (target != null)
			GUI.DrawTexture(new Rect(radarCenterX + (local.x / scale) - (textureHeightWidth /2), radarCenterY - (local.z / scale) - (textureHeightWidth /2), textureHeightWidth, textureHeightWidth), targetimg);
		
		//GUI.Box(
	}
	
	// Update is called once per frame
	void Update () {
	
		if (target != null)
			local = me.transform.InverseTransformDirection(target.transform.position - me.transform.position);
		
		clock++;
		
		if (clock >= delay)
		{
			clock = 0;
			cycle++;
			
			if (cycle > 12)
				cycle = 1;
		}
		
	}
}
