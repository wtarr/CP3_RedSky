using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

/*************************************************
 * Result of two tutorials
 *http://cgcookie.com/unity/2011/12/20/introduction-to-networking-in-unity/ 
 *http://www.jarloo.com/c-udp-multicasting-tutorial/
 *************************************************/

public class NetworkManagerSplashScreen : MonoBehaviour
{

    public GameObject playerPrefab, spawnPointsPrefab;
    public static List<PlayerInfo> playerInfoList;
    public static List<GameObject> spawnPoints;
    private float btnX, btnY, btnW, btnH, textfieldH, textfieldW, textfieldX, textfieldY, playerNameLabelX, playerNameLabelY, playerNameLabelH, playerNameLabelW;
    private string gameName = "RedSky", password = "Openup", playerName = string.Empty;
    private bool waitForServerResponse;
    private List<HostData> hostdata;
    private bool startServerCalled = false;
    private volatile bool listen = false, iamserver = false, iamclient = false;
    private volatile List<string> lanHosts;
    private int maxNumOfPlayers = 4;
    private int port = 25001;
    private Thread thread;
    private UdpClient udpClient_broadcast, udpClient_listen;
    private IPEndPoint local_ipEP, remote_ipEP;
    private IPAddress multiCastAddress;
    private int multiCastPort = 2222;
    private string multicastAddressAsString = "239.0.0.222";
    private string myIPPrivateAddress;



    // Use this for initialization
    void Start()
    {
        myIPPrivateAddress = Network.player.ipAddress;
        lanHosts = new List<string>();

        playerInfoList = new List<PlayerInfo>();
        spawnPoints = new List<GameObject>();

        foreach (Transform child in spawnPointsPrefab.transform)
        {
            spawnPoints.Add(child.gameObject);
        }

        DontDestroyOnLoad(this); // This game object needs to stay alive to maintain the network view records

        playerNameLabelX = Screen.width * 0.05f;
        playerNameLabelY = Screen.width * 0.05f;
        playerNameLabelH = Screen.width * 0.02f;
        playerNameLabelW = Screen.width * 0.1f;

        textfieldX = Screen.width * 0.05f;
        textfieldY = Screen.width * 0.07f;
        textfieldH = Screen.width * 0.03f;
        textfieldW = Screen.width * 0.2f;

        btnX = Screen.width * 0.05f;
        btnY = Screen.width * 0.12f;
        btnW = Screen.width * 0.2f;
        btnH = Screen.width * 0.05f;

        
        udpClient_listen = new UdpClient();

        multiCastAddress = IPAddress.Parse(multicastAddressAsString);

        local_ipEP = new IPEndPoint(IPAddress.Any, multiCastPort);
        remote_ipEP = new IPEndPoint(multiCastAddress, multiCastPort);

        udpClient_listen.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

        udpClient_listen.ExclusiveAddressUse = false;

        udpClient_listen.Client.Bind(local_ipEP);

        udpClient_listen.JoinMulticastGroup(multiCastAddress);

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

            if (GUI.Button(new Rect(btnX, btnY * 1.8f + btnH, btnW, btnH), "Start LAN") && playerName != string.Empty)
            {
                StartLANServer();
                iamserver = true;
            }

            if (GUI.Button(new Rect(btnX, btnY * 2.4f + btnH, btnW, btnH), "Search For LAN game"))
            {
                SearchForLANServers();
                iamclient = true;
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

            if (lanHosts.Count > 0)
            {
                int x = 0;
                foreach (var item in lanHosts)
                {
                    if (GUI.Button(new Rect((btnX * 1.5f + btnW), (btnY * 4f + (btnH * x)), btnW * 3f, btnH), item.ToString()) && playerName != string.Empty)
                    {
                        string ipaddress = item.Split('_').GetValue(6).ToString();
                        Network.Connect(ipaddress, port, password);
                    }
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
        if (!startServerCalled)
        {
            startServerCalled = true;
            bool shouldUseNAT = !Network.HavePublicAddress();
            Network.InitializeServer(maxNumOfPlayers, port, shouldUseNAT);
            Network.incomingPassword = password;
            MasterServer.RegisterHost(gameName, "RedSky Multiplayer Game", "This is a tutorial game");
        }
    }

    private void StartLANServer()
    {
        Network.InitializeServer(4, 25001, false);
        Network.incomingPassword = password;

        listen = true;

        thread = new Thread(new ThreadStart(UDPListen));
        thread.Start();
    }

    void UDPListen()
    {
        Debug.Log("Listener Started");
        while (listen)
        {
            byte[] data = udpClient_listen.Receive(ref local_ipEP);
            string strData = Encoding.Unicode.GetString(data);
            Debug.Log(strData);
            if (strData.Equals("game_request") && iamserver)
            {
                BroadcastMessage(string.Format("RedSky_ServerIP_hosted_by_{0}_at_{1}", playerName, myIPPrivateAddress), 1);               
            }

            if (strData.Contains("RedSky_ServerIP"))
            {
                if (!lanHosts.Contains(strData))
                {
                    lanHosts.Add(strData);
                }
            }

            Thread.Sleep(100);
        }              

    }

    void OnApplicationQuit()
    {
        try
        {
            if (thread != null)
            {
                if (thread.IsAlive)
                {
                    listen = false;
                    if (udpClient_listen != null)
                        udpClient_listen.Close();
                    if (udpClient_broadcast != null)
                        udpClient_broadcast.Close();
                    thread.Abort();
                }
            }
        }
        catch (Exception ex)
        { 
        
        }
    }


    private void SearchForLANServers()
    {
        //Network.Connect("192.168.1.2", port, password);
        //throw new System.NotImplementedException();

        listen = true;

        if (thread == null)
        {
            thread = new Thread(UDPListen);
            thread.Start();
        }

        if (iamclient)
            BroadcastMessage("game_request", 10);

    }

    private void BroadcastMessage(string msg, int timestosend)
    {
        udpClient_broadcast = new UdpClient();

        udpClient_broadcast.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

        udpClient_broadcast.ExclusiveAddressUse = false;

        udpClient_broadcast.Client.Bind(local_ipEP);

        udpClient_broadcast.JoinMulticastGroup(multiCastAddress);

        byte[] buffer = null;

        for (int i = 0; i < timestosend; i++)
        {
            buffer = Encoding.Unicode.GetBytes(msg);
            udpClient_broadcast.Send(buffer, buffer.Length, remote_ipEP);
        }

        udpClient_broadcast.Close();
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

    void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        if (Network.isServer)
            Debug.Log("Local server connection disconnected");
        else
            if (info == NetworkDisconnection.LostConnection)
                Debug.Log("Lost connection to the server");
            else
                Debug.Log("Successfully diconnected from the server");
    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {
        Debug.Log("Clean up after player " + player);
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }

    void SpawnPlayer()
    {
        // Pick a random spawn point
        System.Random r = new System.Random();
        int ranNum = r.Next(0, spawnPoints.Count);

        GameObject go = (GameObject)Network.Instantiate(playerPrefab, spawnPoints[ranNum].transform.position, spawnPoints[ranNum].transform.rotation, 0);

        networkView.RPC("AddToPlayerList", RPCMode.AllBuffered, playerName, go.networkView.viewID);

    }

    [RPC]
    private void AddToPlayerList(string playerName, NetworkViewID viewID)
    {
        //GameObject go = NetworkView.g
        NetworkManagerSplashScreen.playerInfoList.Add(new PlayerInfo(playerName, viewID));
    }


}
