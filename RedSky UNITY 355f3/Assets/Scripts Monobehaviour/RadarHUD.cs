using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RadarHUD : MonoBehaviour {


    public PlayerCraft pc;
	public Texture radarScreenImage, friendlyImage, targetImage, rotateBeamSpriteSheet, targetHighlight;
	public GameObject me;    
    Vector3 targetRelToScreen;
	float offset, clock;
	
	int textureHeightWidth, padding, radarLeft, radarTop, radarCenterX, radarCenterY, delay, cycle;
	
	
	int scale = 5;

	// Use this for initialization
	void Start () {
		
		me = GameObject.Find("Player");

        pc = new PlayerCraft();
        pc.FindAllTargetsByTag("enemy");


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
		GUI.DrawTexture(new Rect(radarLeft, radarTop, textureHeightWidth, textureHeightWidth), radarScreenImage); 
				
		GUI.DrawTextureWithTexCoords(new Rect(radarLeft, radarTop, textureHeightWidth, textureHeightWidth), rotateBeamSpriteSheet, new Rect(offset * cycle, 0, offset, 1));

        foreach (GameObject tar in pc.Targets)
        {
            if (tar != null)
            {
                Vector3 local = me.transform.InverseTransformDirection(tar.transform.position - me.transform.position);
                targetRelToScreen = camera.WorldToScreenPoint(tar.transform.position);

                // 
                GUI.DrawTexture(new Rect(radarCenterX + (local.x / scale) - (textureHeightWidth / 2), radarCenterY - (local.z / scale) - (textureHeightWidth / 2), textureHeightWidth, textureHeightWidth), targetImage);
                GUI.DrawTexture(new Rect(targetRelToScreen.x - 50, Screen.height - targetRelToScreen.y - 50, 100, 100), targetHighlight);
            }
        }
			
	}
	
	// Update is called once per frame
	void Update () {
               
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
