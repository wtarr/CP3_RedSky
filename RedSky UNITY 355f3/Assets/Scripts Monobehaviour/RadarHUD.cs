using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RadarHUD : MonoBehaviour {


    public PlayerCraft pc;
    Camera camera;
	public Texture radarScreenImage, friendlyImage, targetImage, rotateBeamSpriteSheet, targetHighlight;	    
    Vector3 targetRelToScreen;
	float offset, clock;
	
	int textureHeightWidth, padding, radarLeft, radarTop, radarCenterX, radarCenterY, delay, cycle;
	
	
	int scale = 5;

	// Use this for initialization
	void Start () {

        camera = GameObject.Find("Camera").camera;

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

        if (pc.Targets.Count > 0)
        {
            foreach (TargetInfo tar in pc.Targets)
            {
                if (tar.targetName != string.Empty)
                {
                    Vector3 local = pc.EntityObj.transform.InverseTransformDirection(tar.targetPosition - pc.EntityObj.transform.position);
                    targetRelToScreen = camera.WorldToScreenPoint(tar.targetPosition);

                    // 
                    GUI.DrawTexture(new Rect(radarCenterX + (local.x / scale) - (textureHeightWidth / 2), radarCenterY - (local.z / scale) - (textureHeightWidth / 2), textureHeightWidth, textureHeightWidth), targetImage);
                    GUI.DrawTexture(new Rect(targetRelToScreen.x - 50, Screen.height - targetRelToScreen.y - 50, 100, 100), targetHighlight);
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
