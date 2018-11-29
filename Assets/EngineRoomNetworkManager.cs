using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class EngineRoomNetworkManager : MonoBehaviour
{
    public bool isAtStartup = true;
    NetworkClient myClient;

    public short sendMoveMessageID = 1003;
    public short sendSeedID = 1004;

    private RemoteNetworkedPVPMover moverListener;
    private LocalNetworkedPVPMover moverSender;
    private bool isServer;

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

    /*void Update()
    {
        //Debug.Log("Update:" + NetworkServer.handlers);
        if (isAtStartup)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                SetupServer();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                SetupClient();
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                SetupServer();
                SetupLocalClient();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                //Debug.Log("Here" + NetworkServer.handlers + ", " + NetworkServer.handlers[1003]);
                myClient.Send(sendMoveMessageID, new IntegerMessage(254));
            }
        }
    }
    void OnGUI()
    {
        if (isAtStartup)
        {
            GUI.Label(new Rect(2, 10, 150, 100), "Press S for server");
            GUI.Label(new Rect(2, 30, 150, 100), "Press B for both");
            GUI.Label(new Rect(2, 50, 150, 100), "Press C for client");
        }
        else
        {
            GUI.Label(new Rect(2, 10, 150, 100), "Press I for moveMessage");
        }
    }*/

    // Create a server and listen on a port
    public void SetupServer()
    {
        NetworkServer.Listen(4444);
        NetworkServer.RegisterHandler(MsgType.Connect, ServerHandlesConnection);
        NetworkServer.RegisterHandler(sendMoveMessageID, ServerHandlesMove);
        myClient.RegisterHandler(sendSeedID, ServerAcceptsSeed);
        isAtStartup = false;
    }

    // Create a client and connect to the server port
    public void SetupClient()
    {
        myClient = new NetworkClient();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        myClient.RegisterHandler(sendMoveMessageID, RecieveMove);
        myClient.RegisterHandler(sendSeedID, ClientAcceptsSeed);
        myClient.Connect("127.0.0.1", 4444);
        isAtStartup = false;
    }

    // Create a local client and connect to the local server
    public void SetupLocalClient()
    {
        myClient = ClientScene.ConnectLocalServer();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        myClient.RegisterHandler(sendMoveMessageID, RecieveMove);
        myClient.RegisterHandler(sendSeedID, ClientAcceptsSeed);
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
        Debug.Log("Client Recieved Move:" +", "+beginMessage.value);
        if (!isServer)
        {
            Debug.Log("Client getting: " + beginMessage.value);
            moverListener.HandleMove(beginMessage.value);
        }
    }

    public void ServerHandlesMove(NetworkMessage netMsg)
    {
        IntegerMessage beginMessage = netMsg.ReadMessage<IntegerMessage>();
        Debug.LogWarning("Server Recieved Move:" + ", " + beginMessage.value);
        if (isServer)
        {
            Debug.Log("Server getting: " + beginMessage.value);
            moverListener.HandleMove(beginMessage.value);
        }
    }

    internal void AttachToListenerMover(RemoteNetworkedPVPMover mover, bool isServer)
    {
        this.moverListener = mover;
        this.isServer = isServer;
    }

    public void ClientAcceptsSeed(NetworkMessage seedMessage)
    {
        int toReturn = seedMessage.ReadMessage<IntegerMessage>().value;
        Debug.Log(toReturn);
        this.moverSender.AcceptSeed(toReturn);
    }

    public void ServerAcceptsSeed(NetworkMessage seedMessage)
    {
        Debug.LogError("I am the server. Why are you telling me what do do?");
    }

}

