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

        playerCraft.ThrustValue = 5000f;

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
            // Continue to spin the radar sweeper
            sweeper.transform.RotateAround(this.transform.position, this.transform.up, sweepAngleRate * Time.deltaTime);
            
            playerCraft.Acceleration = Vector3.zero;

            CheckForUserInput();

            PlayerMovement();

            PrimeMissile();

            //Clean the target list to ensure that the list stays fresh
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
        if (Input.GetKey(KeyCode.W)) // forward
        {
            playerCraft.Accelerate();

        }

        if (Input.GetKey(KeyCode.Q)) // pitch up
        {
            playerCraft.PitchUp();

        }

        if (Input.GetKey(KeyCode.E)) // pitch down
        {
            playerCraft.PitchDown();

        }

        if (Input.GetKey(KeyCode.S)) // break/reverse
        {
            playerCraft.Decelerate();

        }

        if (Input.GetKey(KeyCode.A)) // yaw left
        {
            playerCraft.YawLeft();

        }

        if (Input.GetKey(KeyCode.D)) // yaw right
        {
            playerCraft.YawRight();

        }

        if (Input.GetKey(KeyCode.Z)) // Roll left
        {
            playerCraft.RollLeft();

        }

        if (Input.GetKey(KeyCode.X)) // Roll right
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
        {
            //If a target is selected set/update the selected missile with info such as target position so that
            //target velocity can continuosly be maintained and missile is ready to launch.

            playerCraft.MissileStock[playerCraft.MissileSelection].PrimaryTarget.TargetPosition = playerCraft.PrimaryTarget.TargetPosition;

            playerCraft.MissileStock[playerCraft.MissileSelection].TargetVelocityVector = 
                playerCraft.MissileStock[playerCraft.MissileSelection].CalculateVelocityVector(
                playerCraft.MissileStock[playerCraft.MissileSelection].oldTargetPosition,
                playerCraft.MissileStock[playerCraft.MissileSelection].PrimaryTarget.TargetPosition,
                Time.deltaTime); // who should do this the players craft or the missile???

            if (Input.GetKey(KeyCode.F) && networkView.isMine && playerCraft.PrimaryTarget != null)
            {
                // If the user has a target selected and hits the F key instantiate the missile and launch
                // The server is responsible for all the logic, the client mearly sees the result of the logic
                if (coolDown <= 0)
                {                    
                    
                    //RPC calls only handle certain arguments - strings, vectors, Net veiw ids ...
                    //if (Network.isClient)
                    //networkView.RPC("PreLaunchInitialize", RPCMode.AllBuffered, playerCraft.PrimaryTarget.TargetID, playerCraft.PrimaryTarget.TargetPosition);
                    //else
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
    private void PreLaunchInitialize(NetworkViewID viewID, Vector3 position)
    {
        // Go for launch!
        //Debug.Log("Primary " + primaryTargetInfo.TargetID + " " + primaryTargetInfo.TargetPosition);
        // Check that it can hit the target first by checking for a zero vector!!!!!
        //Debug.Log("who is seeing this?");
        //Missile preflight initialisation

        playerCraft.MissileStock[playerCraft.MissileSelection].EntityObj = (GameObject)Network.Instantiate(missilePrefab, playerCraft.Position, playerCraft.Rotation, 0);
        Debug.Log(playerCraft.MissileStock[playerCraft.MissileSelection].EntityObj.networkView.viewID);
        playerCraft.MissileStock[playerCraft.MissileSelection].EntityObj.GetComponent<MissileLauncher>().ThisMissile = playerCraft.MissileStock[playerCraft.MissileSelection];
        playerCraft.MissileStock[playerCraft.MissileSelection].EntityObj.GetComponent<MissileLauncher>().ThisMissile.PrimaryTarget = new TargetInfo(viewID, position);
        playerCraft.MissileStock[playerCraft.MissileSelection].EntityObj.GetComponent<MissileLauncher>().Owner = gameObject;
        //NetworkManagerSplashScreen.hotMissileList.Add(new MissileInTheAir(playerCraft.MissileStock[playerCraft.MissileSelection].EntityObj.networkView.viewID, viewID, networkView.viewID));
        
        //networkView.RPC("AddToHotMissileList", RPCMode.AllBuffered, playerCraft.MissileStock[playerCraft.MissileSelection].EntityObj.networkView.viewID, viewID, networkView.viewID);
        AddToHotMissileList(playerCraft.MissileStock[playerCraft.MissileSelection].EntityObj.networkView.viewID, viewID, networkView.viewID);
        
        

    }
        
    private void AddToHotMissileList(NetworkViewID missile, NetworkViewID target, NetworkViewID launcher)
    {
        NetworkManagerSplashScreen.hotMissileList.Add(new MissileInTheAir(missile, target, launcher));
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

        // Read the reply/pong from a radar ping
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

        // If I am hit with a radar sweep, generate a pong/reply
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

        if (networkView.isMine && other.gameObject.name.Contains("MissileRadar(Clone)") && !other.gameObject.name.Contains("reply"))
        {
            if (ThisPlayersNumber > 0)
            {
                pingReplyPrefab.GetComponent<Reply>().message = string.Format("{0}_player_replying_to_{1}", ThisPlayersNumber, other.gameObject.name);
                GameObject temp = (GameObject)Instantiate(pingReplyPrefab, transform.position, playerCraft.Rotation);
                temp.transform.parent = playerCraft.EntityObj.transform;
            }
        }

        // Have I been hit by a missile?
        if (other.gameObject.name.Contains("Aim9") &&
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
        
    void DestroyTarget(Collider other)
    {        
        Network.Instantiate(explosionPrefab, playerCraft.Position, playerCraft.Rotation, 0);
        networkView.RPC("ShouldRespawn", RPCMode.All);

        Debug.Log("Destroy " + other.gameObject.name);

        List<NetworkViewID> destroyList = new List<NetworkViewID>();
        
        Debug.Log("number in list " + NetworkManagerSplashScreen.hotMissileList.Count.ToString());

        Debug.Log("The colliding missiles pTarget " + other.gameObject.GetComponent<MissileLauncher>().ThisMissile.PrimaryTarget.TargetID.ToString() + " the Owner of the missile " + other.gameObject.GetComponent<MissileLauncher>().Owner.networkView.viewID);

        foreach (var item in NetworkManagerSplashScreen.hotMissileList)
        {
             Debug.Log(" mID" + item.TheMissileId.ToString() + " tID " + item.TheTargetId.ToString() +
                " lID " + item.TheLaunchersId.ToString());
        }

        for (int i = 0; i < NetworkManagerSplashScreen.hotMissileList.Count; i++)
        {
           

            if (NetworkManagerSplashScreen.hotMissileList[i].TheTargetId == other.gameObject.GetComponent<MissileLauncher>().ThisMissile.PrimaryTarget.TargetID && NetworkManagerSplashScreen.hotMissileList[i].TheLaunchersId == other.gameObject.GetComponent<MissileLauncher>().Owner.networkView.viewID)
            {
                destroyList.Add(NetworkManagerSplashScreen.hotMissileList[i].TheMissileId);                
            }
        }

        Debug.Log("Destroy list " + destroyList.Count.ToString());

        foreach (var item in destroyList)
        {
            //networkView.RPC
            NetworkManagerSplashScreen.hotMissileList.RemoveAll(o => o.TheMissileId == item);

            Network.Destroy(item);
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
