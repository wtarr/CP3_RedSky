using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UnityEngine;


[TestFixture]
public class NetworkTesting : MonoBehaviour
{
    private List<HostData> hostData;
    private int maxNumOfPlayers;
    private int port;
    private bool shouldUseNAT;
    private string gameName, password;

    [TestFixtureSetUp]
    public void Init()
    {
        maxNumOfPlayers = 1;
        port = 2225;
        gameName = "CPprojectTesting2013";
        password = "NobodyWillGuessThisPassword";

        
        hostData = new List<HostData>();


    }

    [Test]
    public void TestThatGameIsRegisteringWithMasterServer()
    {
        shouldUseNAT = !Network.HavePublicAddress();
        Network.InitializeServer(maxNumOfPlayers, port, shouldUseNAT);
        Network.incomingPassword = password;
        MasterServer.RegisterHost(gameName, "CPProjectTesting", "This is the result of the unit test");

        MasterServer.RequestHostList(gameName);

        while (hostData.Count == 0)
        {

            hostData = new List<HostData>(MasterServer.PollHostList());
        }


        Assert.AreEqual(hostData[0].gameName, gameName);
        //Assert.That(hostData[0].gameName, Is.EqualTo(gameName).After(300000), "hostdata " + hostData.Count.ToString());
        
    }

}

