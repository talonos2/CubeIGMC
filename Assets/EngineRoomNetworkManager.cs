using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;

public class EngineRoomNetworkManager : MonoBehaviour
{
    public bool isAtStartup = true;
    NetworkClient myClient;

    public short sendMoveMessageID = 1003;
    public short sendSeedID = 1004;
    public short sendCharSheet = 1005;

    private RemoteNetworkedPVPMover moverListener;
    private LocalNetworkedPVPMover moverSender;
    private GameGrid serverGrid;
    private GameGrid clientGrid;
    private bool isServer;

    private string ip;
    public NetworkedMission omm;


    internal void AttachToSenderMover(LocalNetworkedPVPMover localNetworkedPVPMover, bool isServer)
    {
        this.moverSender = localNetworkedPVPMover;
        this.isServer = isServer;
    }

    internal void Send(int toSend)
    {
        if (isServer)
        {
            Debug.Log("Server sending: " + toSend);
            NetworkServer.SendToAll(sendMoveMessageID, new IntegerMessage(toSend));
        }
        else
        {
            Debug.Log("Client sending: " + toSend);
            myClient.Send(sendMoveMessageID, new IntegerMessage(toSend));
        }
    }

    // Create a server and listen on a port
    public void SetupServer()
    {
        omm.isHost = true;
        NetworkServer.Listen(4444);
        NetworkServer.RegisterHandler(MsgType.Connect, ServerHandlesConnection);
        NetworkServer.RegisterHandler(sendMoveMessageID, ServerHandlesMove);
        NetworkServer.RegisterHandler(sendSeedID, ServerAcceptsSeed);
        NetworkServer.RegisterHandler(sendCharSheet, ServerAcceptsCharSheet);
        isAtStartup = false;
    }

    // Create a client and connect to the server port
    public void SetupClient()
    {
        Debug.Log(ip);

        myClient = new NetworkClient();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        myClient.RegisterHandler(sendMoveMessageID, RecieveMove);
        myClient.RegisterHandler(sendSeedID, ClientAcceptsSeed);
        myClient.RegisterHandler(sendCharSheet, ClientAcceptsCharSheet);
        myClient.Connect(ip, 4444);
        isAtStartup = false;
    }

    internal void loadIPSlug(string text)
    {
        this.ip = text;
    }

    // Create a local client and connect to the local server
    public void SetupLocalClient()
    {
        myClient = ClientScene.ConnectLocalServer();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        myClient.RegisterHandler(sendMoveMessageID, RecieveMove);
        myClient.RegisterHandler(sendSeedID, ClientAcceptsSeed);
        myClient.RegisterHandler(sendCharSheet, ServerAcceptsCharSheet);
        isAtStartup = false;
    }

    // client function
    public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Connected to server");
        //myClient.Send(sendMoveMessageID, new IntegerMessage(254));
    }

    // client function
    public void ServerHandlesConnection(NetworkMessage netMsg)
    {
        Debug.Log("Connected to client");
        NetworkServer.SendToAll(sendSeedID, new IntegerMessage(moverSender.GetParentSeed()));
    }

    // client function
    public void RecieveMove(NetworkMessage netMsg)
    {
        IntegerMessage beginMessage = netMsg.ReadMessage<IntegerMessage>();
        if (!isServer)
        {
            moverListener.HandleMove(beginMessage.value);
        }
    }

    public void ServerHandlesMove(NetworkMessage netMsg)
    {
        IntegerMessage beginMessage = netMsg.ReadMessage<IntegerMessage>();
        if (isServer)
        {
            moverListener.HandleMove(beginMessage.value);
        }
    }

    internal void AttachToListenerMover(RemoteNetworkedPVPMover mover, bool isServer)
    {
        this.moverListener = mover;
        this.isServer = isServer;
    }

    public void ClientAcceptsCharSheet(NetworkMessage charSheetMessage)
    {
        String toReturn = charSheetMessage.ReadMessage<StringMessage>().value;

        if (!isServer)
        {
//            charSheetListener.HandleMove(beginMessage.value);
        }

    }

    public void ServerAcceptsCharSheet(NetworkMessage charSheetMessage)
    {
        String toReturn = charSheetMessage.ReadMessage<StringMessage>().value;
        //        string loadedString = streamReader.ReadToEnd();
        //        return JsonUtility.FromJson<PlayerCharacterSheet>(loadedString);
        if (isServer)
        {
//            moverListener.HandleMove(beginMessage.value);
        }
    }

    public void ClientAcceptsSeed(NetworkMessage seedMessage)
    {
        int toReturn = seedMessage.ReadMessage<IntegerMessage>().value;
        Debug.Log(toReturn);
        if(!isServer)
            this.moverSender.AcceptSeed(toReturn);
    }

    public void ServerAcceptsSeed(NetworkMessage seedMessage)
    {
        Debug.LogError("I am the server. Why are you telling me what do do?");
    }

}

