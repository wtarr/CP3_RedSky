using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInfo
{
    
    public PlayerInfo()
    {

    }

    public PlayerInfo(string playerName, NetworkViewID viewID)
    {
        this.playerName = playerName;
        this.viewID = viewID;
    }

    string playerName;
    NetworkViewID viewID;

    public string PlayerName
    {
        get { return playerName; }
        set { playerName = value; }
    }
    
    public NetworkViewID ViewID
    {
        get { return viewID; }
        set { viewID = value; }
    }

    
}

public class NetworkManagerSplashScreen : MonoBehaviour
{

   
    public GameObject playerPrefab, spawnPoint;
    float btnX, btnY, btnW, btnH, textfieldH, textfieldW, textfieldX, textfieldY, playerNameLabelX, playerNameLabelY, playerNameLabelH, playerNameLabelW;
    string gameName = "RedSky", password = "Openup", playerName = string.Empty;
    bool waitForServerResponse;
    List<HostData> hostdata;

    public static List<PlayerInfo> playerInfoList;


    // Use this for initialization
    void Start()
    {

        playerInfoList = new List<PlayerInfo>();

        DontDestroyOnLoad(this); // This game object needs to stay alive to maintain the network view records

        playerNameLabelX = Screen.width * 0.05f;
        playerNameLabelY = Screen.width * 0.05f;
        playerNameLabelH = Screen.width * 0.02f;
        playerNameLabelW = Screen.width * 0.1f;

        textfieldX = Screen.width * 0.05f;
        textfieldY = Screen.width * 0.07f;
        textfieldH = Screen.width * 0.03f;
        textfieldW = Screen.width * 0.1f;               

        btnX = Screen.width * 0.05f;
        btnY = Screen.width * 0.12f;
        btnW = Screen.width * 0.1f;
        btnH = Screen.width * 0.05f;
                       

    }

    void OnGUI()
    {
        if (!Network.isClient && !Network.isServer) // NObody is connected
        {
            GUI.Label(new Rect(playerNameLabelX, playerNameLabelY, playerNameLabelW, playerNameLabelH), "Username *required");
            playerName = GUI.TextField(new Rect(textfieldX, textfieldY, textfieldW, textfieldH), playerName);

            if (GUI.Button(new Rect(btnX, btnY, btnW, btnH), "Start Server") && playerName != string.Empty)
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

                    if (GUI.Button(new Rect((btnX * 1.5f + btnW), (btnY * 1.2f + (btnH * i)), btnW * 3f, btnH), item.gameName.ToString()) && playerName != string.Empty)
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
        GameObject go = (GameObject)Network.Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation, 0);
        //go.GetComponent<PlayerLauncher>().PlayerCraft.PlayerInfoList = null;
                       
        networkView.RPC("AddToPlayerList", RPCMode.AllBuffered, playerName, go.networkView.viewID);
        

        //go.GetComponent<PlayerLauncher>().playerCraft.Position;
        

    }

    [RPC]
    private void AddToPlayerList(string playerName, NetworkViewID viewID)
    {
        //GameObject go = NetworkView.g
        NetworkManagerSplashScreen.playerInfoList.Add(new PlayerInfo(playerName, viewID));
    }
}
