using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RadarHUD : MonoBehaviour
{    
    public Texture radarScreenImage, friendlyImage, targetImage, rotateBeamSpriteSheet, targetHighlight, primaryTargetHighlighter;
    private PlayerCraft playerCraft; // pointer to the owner
    private Camera cam;

    private Vector3 targetRelToScreen;
    private float offset, clock;

    private int
        radarScreenTextureHeightWidth, // radar screen/ radar sweep 
        targetHUDHighlight,
        targetRadarBlip,
        padding,
        radarLeft,
        radarTop,
        radarCenterX,
        radarCenterY,
        delay,
        cycle;

    private int scale = 5;

    #region Properties
    public PlayerCraft PlayerCraft
    {
        set { playerCraft = value; }
    }
    #endregion

    // Use this for initialization
    void Start()
    {

        cam = GameObject.FindGameObjectWithTag("MainCamera").camera;

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
                
        if (playerCraft.Targets.Count > 0)
        {
            foreach (TargetInfo tar in playerCraft.Targets)
            {
                if (tar.TargetID.ToString() != string.Empty)
                {
                    // Convert global position to a local positon for displaying on radar screen
                    Vector3 local = playerCraft.EntityObj.transform.InverseTransformDirection(tar.TargetPosition - playerCraft.EntityObj.transform.position);
                    // Convert the targets positon to a screen position for positioning the highlighter 
                    targetRelToScreen = cam.WorldToScreenPoint(tar.TargetPosition);

                    // Always draw
                    GUI.DrawTexture(new Rect(radarCenterX + (local.x / scale) - (targetRadarBlip / 2), radarCenterY - (local.z / scale) - (targetRadarBlip / 2), targetRadarBlip, targetRadarBlip), targetImage);

                    //check that we are facing the target
                    Vector3 meToTarget = tar.TargetPosition - playerCraft.Position;

                    if (Vector3.Dot(meToTarget, playerCraft.EntityObj.transform.forward) > 0)
                    {

                        PlayerInfo pi = NetworkManagerSplashScreen.playerInfoList.Find(p => p.ViewID == tar.TargetID);

                        if (pi != null)
                        {
                            GUI.contentColor = Color.green;
                            GUI.Label(new Rect(targetRelToScreen.x - (targetHUDHighlight / 2), (Screen.height - targetRelToScreen.y - (targetHUDHighlight / 2)) - 10, targetHUDHighlight, targetHUDHighlight), pi.PlayerName);
                            GUI.contentColor = Color.black;
                        }
                        GUI.DrawTexture(new Rect(targetRelToScreen.x - targetHUDHighlight / 2, Screen.height - targetRelToScreen.y - targetHUDHighlight / 2, targetHUDHighlight, targetHUDHighlight), targetHighlight);

                        if (tar.IsPrimary)
                            GUI.DrawTexture(new Rect(targetRelToScreen.x - targetHUDHighlight / 2, Screen.height - targetRelToScreen.y - targetHUDHighlight / 2, targetHUDHighlight, targetHUDHighlight), primaryTargetHighlighter);
                    }
                }
            }
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {

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
