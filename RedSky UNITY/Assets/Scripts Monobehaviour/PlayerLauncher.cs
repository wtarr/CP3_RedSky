using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class PlayerLauncher : MonoBehaviour
{
    #region Class State
    public GameObject explosionPrefab, missilePrefab, radarHUDPrefab, goRadar, sweeper, pingReplyPrefab; // prefabs
    private PlayerCraft playerCraft;
    private Vector3 interceptforward;
    private float sweepAngleRate = 1500;
    private int 
        thisPlayersNumber = -1, 
        targetIndex = 0,          
        coolDown = 0, 
        listCleanTimer, 
        respawnTimer;
    private bool respawn = false;
    #endregion

    #region Class properties
    public int ThisPlayersNumber
    {
        get { return thisPlayersNumber; }
        set { thisPlayersNumber = value; }
    }

    public PlayerCraft PlayerCraft
    {
        get { return playerCraft; }
    }

    #endregion

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);

        ThisPlayersNumber = int.Parse(networkView.viewID.ToString().Split(' ').Last());

        if (!networkView.isMine)
        {
            GetComponentInChildren<Camera>().enabled = false;

        }

        if (networkView.isMine)
        {

            GetComponentInChildren<AudioListener>().enabled = true;
            Debug.Log("ThisPlayerNumber" + ThisPlayersNumber);
        }

        playerCraft = new PlayerCraft();

        playerCraft.EntityObj = this.gameObject;

        playerCraft.Targets = new List<TargetInfo>();

        if (networkView.isMine)
        {
            goRadar = (GameObject)Instantiate(radarHUDPrefab, playerCraft.Position, playerCraft.Rotation);
            goRadar.transform.parent = playerCraft.EntityObj.transform;
            goRadar.GetComponent<RadarHUD>().PlayerCraft = playerCraft;

            sweeper = (GameObject)Instantiate(sweeper, playerCraft.Position, playerCraft.Rotation);
            sweeper.transform.parent = playerCraft.EntityObj.transform;

        }




        playerCraft.Velocity = Vector3.zero;

        playerCraft.ThrustValue = 600f;

        playerCraft.DecelerationValue = 300f;

        playerCraft.PitchAngle = 0.01f;

        playerCraft.YawAngle = 0.01f;

        playerCraft.RollAngle = 0.01f;

        playerCraft.AtmosphericDrag = -0.03f;

        playerCraft.Targets = new List<TargetInfo>();



    }


    // Update is called once per frame
    void Update()
    {
        if (networkView.isMine)
        {
            sweeper.transform.RotateAround(this.transform.position, this.transform.up, sweepAngleRate * Time.deltaTime);

            //playerCraft.FindAllTargetsByTag("enemy");

            playerCraft.Acceleration = Vector3.zero;

            CheckForUserInput();

            PlayerMovement();

            PrimeMissile();

            //Clean the target list
            listCleanTimer++;

            if (listCleanTimer > 200)
            {
                listCleanTimer = 0;
                CleanTargetList(); // keep the list fresh
            }

            if (respawn)
                SetToRespawn();

            if (respawnTimer > 0)
                respawnTimer--;
        }

    } // update

    private void PlayerMovement()
    {
        playerCraft.Velocity += playerCraft.Acceleration * Time.deltaTime;

        Vector3 resistance = playerCraft.AtmosphericDrag * playerCraft.Velocity * Vector3.Magnitude(playerCraft.Velocity);

        playerCraft.Velocity += resistance * Time.deltaTime;

        playerCraft.EntityObj.transform.position += playerCraft.Velocity * Time.deltaTime;
    }

    private void CheckForUserInput()
    {
        if (Input.GetKey(KeyCode.Keypad7)) // forward
        {
            playerCraft.Accelerate();

        }

        if (Input.GetKey(KeyCode.Keypad8)) // pitch up
        {
            playerCraft.PitchUp();

        }

        if (Input.GetKey(KeyCode.Keypad2)) // pitch down
        {
            playerCraft.PitchDown();

        }

        if (Input.GetKey(KeyCode.Keypad9)) // break/reverse
        {
            playerCraft.Decelerate();

        }

        if (Input.GetKey(KeyCode.Keypad6)) // yaw left
        {
            playerCraft.YawLeft();

        }

        if (Input.GetKey(KeyCode.Keypad4)) // yaw right
        {
            playerCraft.YawRight();

        }

        if (Input.GetKey(KeyCode.Keypad1)) // Roll left
        {
            playerCraft.RollLeft();

        }

        if (Input.GetKey(KeyCode.Keypad3)) // Roll right
        {
            playerCraft.RollRight();

        }

        if (Input.GetKey(KeyCode.Tab))
        {
            ToggleTarget();
        }
    }

    private void PrimeMissile()
    {
        if (playerCraft.PrimaryTarget != null && playerCraft.PrimaryTarget.IsPrimary && playerCraft.MissileSelection < playerCraft.MissileStock.Length)
        { //check that target still exists


            playerCraft.MissileStock[playerCraft.MissileSelection].PrimaryTarget.TargetPosition = playerCraft.PrimaryTarget.TargetPosition;

            playerCraft.MissileStock[playerCraft.MissileSelection].TargetVelocityVector = 
                playerCraft.MissileStock[playerCraft.MissileSelection].CalculateVelocityVector(
                playerCraft.MissileStock[playerCraft.MissileSelection].oldTargetPosition,
                playerCraft.MissileStock[playerCraft.MissileSelection].PrimaryTarget.TargetPosition,
                Time.deltaTime); // who should do this the players craft or the missile???

            if (Input.GetKey(KeyCode.F) && networkView.isMine)
            {

                if (coolDown <= 0)
                {
                    if (networkView.isMine && Network.isClient)
                        networkView.RPC("PreLaunchInitialize", RPCMode.Server, playerCraft.PrimaryTarget.TargetID, playerCraft.PrimaryTarget.TargetPosition);
                    if (networkView.isMine && Network.isServer)
                        PreLaunchInitialize(playerCraft.PrimaryTarget.TargetID, playerCraft.PrimaryTarget.TargetPosition);

                    playerCraft.MissileSelection++; // next missile on the rack

                    coolDown = 30;
                }

            }

            if (coolDown > 0)
                coolDown--;

            if (playerCraft.MissileSelection < playerCraft.MissileStock.Length)
                playerCraft.MissileStock[playerCraft.MissileSelection].oldTargetPosition = playerCraft.MissileStock[playerCraft.MissileSelection].PrimaryTarget.TargetPosition;
        }
    }

    [RPC]
    private void PreLaunchInitialize(NetworkViewID primTarget, Vector3 primPosition)
    {
        // Go for launch!
        Debug.Log(primTarget + " " + primPosition);
        // Check that it can hit the target first by checking for a zero vector!!!!!
        Debug.Log("who is seeing this?");
        //Missile preflight initialisation

        playerCraft.MissileStock[playerCraft.MissileSelection].EntityObj = (GameObject)Network.Instantiate(missilePrefab, playerCraft.Position, playerCraft.Rotation, 0);
        Debug.Log(playerCraft.MissileStock[playerCraft.MissileSelection].EntityObj.networkView.viewID);
        playerCraft.MissileStock[playerCraft.MissileSelection].EntityObj.GetComponent<MissileLauncher>().ThisMissile = playerCraft.MissileStock[playerCraft.MissileSelection];
        playerCraft.MissileStock[playerCraft.MissileSelection].EntityObj.GetComponent<MissileLauncher>().ThisMissile.PrimaryTarget = new TargetInfo(primTarget, primPosition);
        playerCraft.MissileStock[playerCraft.MissileSelection].EntityObj.GetComponent<MissileLauncher>().Owner = gameObject;

    }



    private void ToggleTarget()
    {

        foreach (TargetInfo t in playerCraft.Targets)
        {
            t.IsPrimary = false;
        }

        if (playerCraft.Targets.Count > 0 && targetIndex < playerCraft.Targets.Count)
        {
            playerCraft.Targets[targetIndex].IsPrimary = true;
            playerCraft.PrimaryTarget = playerCraft.Targets[targetIndex];

            targetIndex++;
        }
        else
        {
            targetIndex = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.transform.name);

        if (other.gameObject.name.Contains("reply") && other.gameObject.name.Contains("player"))
        {

            int id = int.Parse(other.gameObject.name.Split('_').First());

            if (id != ThisPlayersNumber) // it is not me
            {

                TargetInfo t = new TargetInfo(other.gameObject.transform.parent.networkView.viewID, other.gameObject.transform.position); ///////

                int indexOfitem = playerCraft.Targets.FindIndex(tar => tar.TargetID == t.TargetID); // -1 means its new

                if (indexOfitem < 0)
                {
                    playerCraft.Targets.Add(t);
                }

                if (indexOfitem >= 0)
                {
                    if (playerCraft.PrimaryTarget != null)
                    {
                        if (playerCraft.PrimaryTarget.TargetID.Equals(t.TargetID))
                        {
                            playerCraft.Targets[indexOfitem].IsPrimary = true;
                        }
                    }

                    playerCraft.Targets[indexOfitem].TargetPosition = t.TargetPosition;

                }
            }
        }


        if (other.gameObject.name.Contains("RadarSweep(Clone)") && !other.gameObject.name.Contains("reply"))
        {
            //Debug.Log("Reply to sweep " + other.gameObject.name);
            if (ThisPlayersNumber > 0)
            {
                pingReplyPrefab.GetComponent<Reply>().message = string.Format("{0}_player_replying_to_{1}", ThisPlayersNumber, other.gameObject.name);
                GameObject temp = (GameObject)Instantiate(pingReplyPrefab, transform.position, playerCraft.Rotation);
                temp.transform.parent = playerCraft.EntityObj.transform;
            }
        }

        if (Network.isServer && other.gameObject.name.Contains("MissileRadar(Clone)") && !other.gameObject.name.Contains("reply"))
        {
            if (ThisPlayersNumber > 0)
            {
                pingReplyPrefab.GetComponent<Reply>().message = string.Format("{0}_player_replying_to_{1}", ThisPlayersNumber, other.gameObject.name);
                GameObject temp = (GameObject)Instantiate(pingReplyPrefab, transform.position, playerCraft.Rotation);
                temp.transform.parent = playerCraft.EntityObj.transform;
            }
        }

        if (Network.isServer &&
            other.gameObject.name.Contains("Aim9") &&
            other.gameObject.transform.parent == null
            && other.gameObject.GetComponent<MissileLauncher>().Owner.transform.networkView.viewID != networkView.viewID) // dont blow myself up :-o
        {
            //Debug.Log("Missile hit me");
            try
            {              
                DestroyTarget(other);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        if (other.gameObject.name.Contains("MainLevelTerrain") ||
            other.gameObject.name.Contains("Ocean"))
        {

            TerrainCollision();
        }

    }

    void TerrainCollision()
    {
        Debug.Log("TerrainCollision");
        Network.Instantiate(explosionPrefab, playerCraft.Position, playerCraft.Rotation, 0);
        ShouldRespawn();
    }

    void SetToRespawn()
    {        
        if (respawnTimer <= 0)
        {
            respawn = false;
            networkView.RPC("RespawnTarget", RPCMode.AllBuffered);
        }
    
    }

    [RPC]
    void DestroyTarget(Collider other)
    {        
        Network.Instantiate(explosionPrefab, playerCraft.Position, playerCraft.Rotation, 0);
        networkView.RPC("ShouldRespawn", RPCMode.All);

        if (Network.isServer)
        {
            Debug.Log("Destroy " + other.gameObject.name);
            Network.Destroy(other.networkView.viewID);
        }
    }

    [RPC]
    void ShouldRespawn()
    {
        respawn = true;
        respawnTimer = 30;
    }

    [RPC]
    void RespawnTarget()
    {
        playerCraft.Velocity = Vector3.zero;
        
        System.Random r = new System.Random();
        int ranNum = r.Next(0, NetworkManagerSplashScreen.spawnPoints.Count);

        playerCraft.EntityObj.transform.position = NetworkManagerSplashScreen.spawnPoints[ranNum].transform.position;
        playerCraft.Velocity = Vector3.zero;


    }

    void CleanTargetList()
    {
        playerCraft.Targets.Clear();
    }


}
