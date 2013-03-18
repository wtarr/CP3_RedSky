using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour
{
    public GameObject playerPrefab, playerActual, spawnPoint;
    float btnX, btnY, btnW, btnH;
    string gameName = "MultiplayerTestforRedSky", password = "Openup";
    bool wait;
    int playerCounter = -1; // -1 will go to server
    List<HostData> hostdata;


    // Use this for initialization
    void Start()
    {

        btnX = Screen.width * 0.05f;
        btnY = Screen.width * 0.05f;
        btnW = Screen.width * 0.1f;
        btnH = Screen.width * 0.1f;



    }

    void OnGUI()
    {
        if (!Network.isClient && !Network.isServer)
        {

            if (GUI.Button(new Rect(btnX, btnY, btnW, btnH), "Start Server"))
            {
                Debug.Log("Server started");
                StartServer();
            }

            if (GUI.Button(new Rect(btnX, btnY * 1.2f + btnH, btnW, btnH), "Refresh Hosts"))
            {
                Debug.Log("Refreshing");
                RefreshHostList();
            }

            if (hostdata != null)
            {
                int i = 0;
                foreach (var item in hostdata)
                {

                    if (GUI.Button(new Rect((btnX * 1.5f + btnW), (btnY * 1.2f + (btnH * i)), btnW * 3f, btnH), item.gameName.ToString()))
                        Network.Connect(item, password);

                    i++;
                }
            }

        }
    }

    private void RefreshHostList()
    {
        MasterServer.RequestHostList(gameName);
        wait = true;

    }


    private void StartServer()
    {
        bool shouldUseNAT = !Network.HavePublicAddress();
        Network.InitializeServer(4, 25001, shouldUseNAT);
        Network.incomingPassword = password;
        MasterServer.RegisterHost(gameName, "RedSky Multiplayer Game", "This is a tutorial game");
    }

    private void OnServerInitialized()
    {
        Debug.Log("Server initialized!");
        if (Network.isServer)
            SpawnPlayer();
                
    }

    // Checks if the game has been registered with the master server
    private void OnMasterServerEvent(MasterServerEvent e)
    {
        if (e == MasterServerEvent.RegistrationSucceeded)
            Debug.Log("Server registered");
    }

    // Update is called once per frame
    void Update()
    {
        if (wait)
        {
            if (MasterServer.PollHostList().Length > 0)
            {
                wait = false;
                Debug.Log(MasterServer.PollHostList().Length);
                hostdata = new List<HostData>(MasterServer.PollHostList());
            }
        }

    }

    void OnConnectedToServer()
    {           
        SpawnPlayer();
    }

    
    private void SpawnPlayer()
    {        
        GameObject player = (GameObject)Network.Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation, 0);            
    }

    
   
}
