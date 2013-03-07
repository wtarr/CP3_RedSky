using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RadarHUD : MonoBehaviour {


    public PlayerCraft pc;    
    Camera camera;
	public Texture radarScreenImage, friendlyImage, targetImage, rotateBeamSpriteSheet, targetHighlight;	    
    Vector3 targetRelToScreen;
	float offset, clock;
	
	int radarScreenTextureHeightWidth, // radar screen/ radar sweep 
        targetHUDHighlight,
        targetRadarBlip,
        padding,
        radarLeft,
        radarTop,
        radarCenterX,
        radarCenterY,
        delay,
        cycle;
	
	
	int scale = 5;

	// Use this for initialization
	void Start () {

        camera = GameObject.Find("Camera").camera;

		radarScreenTextureHeightWidth = 200;
        targetHUDHighlight = 100;
        targetRadarBlip = 100;
		padding = 10;
		radarLeft = 10;
		radarTop = Screen.height - (radarScreenTextureHeightWidth + padding);
		
		radarCenterX = radarLeft + (radarScreenTextureHeightWidth / 2);
		radarCenterY = radarTop + (radarScreenTextureHeightWidth / 2);
		
		offset = 0.0833f;
		
		delay = 3;
		
		cycle = 1;
			
	}

    	
	void OnGUI()
	{
		GUI.DrawTexture(new Rect(radarLeft, radarTop, radarScreenTextureHeightWidth, radarScreenTextureHeightWidth), radarScreenImage); 
				
		GUI.DrawTextureWithTexCoords(new Rect(radarLeft, radarTop, radarScreenTextureHeightWidth, radarScreenTextureHeightWidth), rotateBeamSpriteSheet, new Rect(offset * cycle, 0, offset, 1));

        if (pc.Targets.Count > 0)
        {
            foreach (TargetInfo tar in pc.Targets)
            {
                if (tar.TargetName != string.Empty)
                {
                    Vector3 local = pc.EntityObj.transform.InverseTransformDirection(tar.TargetPosition - pc.EntityObj.transform.position);
                    targetRelToScreen = camera.WorldToScreenPoint(tar.TargetPosition);

                    // 
                    GUI.DrawTexture(new Rect(radarCenterX + (local.x / scale) - (targetRadarBlip / 2), radarCenterY - (local.z / scale) - (targetRadarBlip / 2), targetRadarBlip, targetRadarBlip), targetImage);
                    GUI.DrawTexture(new Rect(targetRelToScreen.x - targetHUDHighlight/2, Screen.height - targetRelToScreen.y - targetHUDHighlight/2, targetHUDHighlight, targetHUDHighlight), targetHighlight);
                }
            }
        }	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
               
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
