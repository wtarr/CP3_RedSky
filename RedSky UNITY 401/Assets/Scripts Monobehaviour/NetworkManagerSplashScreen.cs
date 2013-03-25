using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManagerSplashScreen : MonoBehaviour
{

    public GameObject playerPrefab, spawnPoint;
    float btnX, btnY, btnW, btnH;
    string gameName = "RedSky", password = "Openup";
    bool waitForServerResponse;
    List<HostData> hostdata;

    // Use this for initialization
    void Start()
    {



        DontDestroyOnLoad(this);

        btnX = Screen.width * 0.05f;
        btnY = Screen.width * 0.05f;
        btnW = Screen.width * 0.1f;
        btnH = Screen.width * 0.1f;

    }

    void OnGUI()
    {
        if (!Network.isClient && !Network.isServer) // NObody is connected
        {

            if (GUI.Button(new Rect(btnX, btnY, btnW, btnH), "Start Server"))
            {
                Debug.Log("Server started");
                StartServer();
            }

            if (GUI.Button(new Rect(btnX, btnY * 1.2f + btnH, btnW, btnH), "Refresh Hosts"))
            {
                Debug.Log("Refreshing");
                if (!Network.isServer)
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
        waitForServerResponse = true;

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
        {
            LoadScene();
            SpawnPlayer();
        }

    }

    
    private void LoadScene()
    {        
        Application.LoadLevel(1);        
    }

    private IEnumerator WaitForHostList(float seconds)
    {
        yield return new WaitForSeconds(seconds);

    }

    // Checks if the game has been registered with the master server
    private void OnMasterServerEvent(MasterServerEvent e)
    {
        if (e == MasterServerEvent.RegistrationSucceeded)
        {
            Debug.Log("Server registered");
            Debug.Log("Master Server Info:" + MasterServer.ipAddress + ":" + MasterServer.port);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!Network.isServer && waitForServerResponse)
        {
            if (MasterServer.PollHostList().Length > 0)
            {
                waitForServerResponse = false;
                Debug.Log(MasterServer.PollHostList().Length);
                hostdata = new List<HostData>(MasterServer.PollHostList());
            }
        }

    }

    void OnConnectedToServer()
    {

        LoadScene();        
        SpawnPlayer();

    }

    
    void SpawnPlayer()
    {
        Network.Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation, 0);
    }
}
