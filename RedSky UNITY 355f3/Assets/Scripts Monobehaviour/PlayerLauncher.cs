using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class PlayerLauncher : MonoBehaviour
{
	public PlayerCraft playerCraft;    
	public GameObject explosionPrefab, missilePrefab, radarHUDPrefab, goRadar, sweeper; // prefabs
    
	Vector3 interceptforward;

    float sweepAngleRate = 1000;

    int targetIndex = 0, missileSelection = 0, coolDown = 0, listCleanTimer;
	
	
	// Use this for initialization
	void Start ()
	{
		
		playerCraft = new PlayerCraft();
                		
		playerCraft.EntityObj = this.gameObject;
		
		playerCraft.Targets = new List<TargetInfo>();

        goRadar = (GameObject)Instantiate(radarHUDPrefab, playerCraft.Position, playerCraft.Rotation);
        goRadar.transform.parent = playerCraft.EntityObj.transform;
        goRadar.GetComponent<RadarHUD>().pc = playerCraft;

        sweeper = (GameObject)Instantiate(sweeper);
        sweeper.transform.parent = playerCraft.EntityObj.transform;
		
		//Physics.IgnoreCollision(sweeper.GetComponent<Collider>(), 
				
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
	void Update ()
	{
        
        sweeper.transform.RotateAround(this.transform.position, this.transform.up, sweepAngleRate * Time.deltaTime);


        //playerCraft.FindAllTargetsByTag("enemy");

		playerCraft.Acceleration = Vector3.zero;
        
        #region MovementControls
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
        #endregion

        #region Physics
        playerCraft.Velocity += playerCraft.Acceleration * Time.deltaTime;

        Vector3 resistance = playerCraft.AtmosphericDrag * playerCraft.Velocity * Vector3.Magnitude(playerCraft.Velocity);

        playerCraft.Velocity += resistance * Time.deltaTime;

        playerCraft.EntityObj.transform.position += playerCraft.Velocity * Time.deltaTime; 
        #endregion
				
        #region MissilePrimer
        if (playerCraft.PrimaryTarget != null && missileSelection < playerCraft.missileStock.Length)
        { //check that target still exists


            playerCraft.missileStock[missileSelection].TargetPosition = playerCraft.PrimaryTarget.transform.position;

            playerCraft.missileStock[missileSelection].TargetVelocityVector = playerCraft.missileStock[missileSelection].CalculateVelocityVector(playerCraft.missileStock[missileSelection].oldTargetPosition, playerCraft.missileStock[missileSelection].TargetPosition, Time.deltaTime); // who should do this the players craft or the missile???

            if (Input.GetKey(KeyCode.F))
            {

                if (coolDown <= 0)
                {
                    PreLaunchInitialize();

                    missileSelection++; // next missile on the rack

                    coolDown = 30;
                }

            }

            if (coolDown > 0)
                coolDown--;
			
			if (missileSelection < playerCraft.missileStock.Length)
            	playerCraft.missileStock[missileSelection].oldTargetPosition = playerCraft.missileStock[missileSelection].TargetPosition;
        } 
        #endregion
        
        //Clean the target list
        listCleanTimer++;

        if (listCleanTimer > 200)
        {
            listCleanTimer = 0;
            CleanTargetList(); // keep the list fresh
        }

	} // update

    private void PreLaunchInitialize()
    {
        // Go for launch!

        // Check that it can hit the target first by checking for a zero vector!!!!!

        //Missile preflight initialisation
        playerCraft.missileStock[missileSelection].EntityObj = (GameObject)Instantiate(missilePrefab, playerCraft.Position, playerCraft.Rotation);
        playerCraft.missileStock[missileSelection].EntityObj.GetComponent<MissileLauncher>().me = playerCraft.missileStock[missileSelection]; // whoa!! ,,, crazy stuff here man 
        playerCraft.missileStock[missileSelection].EntityObj.GetComponent<MissileLauncher>().me.PrimaryTarget = playerCraft.PrimaryTarget;

        //If You Love Someone, Set Them Free. If They Come Back, RUN!!!
    }

    private void ToggleTarget()
    {
        

        if (playerCraft.Targets.Count > 0 && targetIndex < playerCraft.Targets.Count)
        {

            string[] splitname = playerCraft.Targets[targetIndex].TargetName.Split('_');
            
            playerCraft.PrimaryTarget = GameObject.Find(splitname[0]);
                        
            targetIndex++;
        }
        else
        {
            targetIndex = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        

        if (other.gameObject.name.Contains("reply"))
        {          

            TargetInfo t = new TargetInfo(other.gameObject.name, other.gameObject.transform.position); 
			
			int indexOfitem = playerCraft.Targets.FindIndex(tar => tar.TargetName == t.TargetName); // -1 means its new
			
            if (indexOfitem < 0)
			{
                playerCraft.Targets.Add(t);
			} 
			
			if (indexOfitem >= 0)
			{
				playerCraft.Targets[indexOfitem].TargetPosition = t.TargetPosition;               
			}

            
        }
               
    }

    void CleanTargetList()
    {
        playerCraft.Targets.Clear();
    }
       	
}
