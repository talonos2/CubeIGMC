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
    public static EngineRoomNetworkManager instance;

    internal bool isAtStartup = true;
    internal short sendMoveMessageID = 1003;
    internal short sendSeedID = 1004;
    internal short sendCharSheet = 1005;

    private NetworkClient myClient;
    private RemoteNetworkedPVPMover moverListener;
    private LocalNetworkedPVPMover moverSender;
    private bool isServer;                           //THIS IS PROBABLY WHAT"S WRONG WITH SENDING CHAR SHEETS. Where does it come from?

    private string ip;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    internal void AttachToSenderMover(LocalNetworkedPVPMover localNetworkedPVPMover, bool isServer)
    {
        this.moverSender = localNetworkedPVPMover;
        this.isServer = isServer;
    }

    internal void Send(int toSend)
    {
        if (isServer)
        {
            //Debug.Log("Server sending: " + toSend);
            NetworkServer.SendToAll(sendMoveMessageID, new IntegerMessage(toSend));
        }
        else
        {
            //Debug.Log("Client sending: " + toSend);
            myClient.Send(sendMoveMessageID, new IntegerMessage(toSend));
        }
    }

    // Create a server and listen on a port
    public void SetupServer()
    {
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
        MissionManager.instance.DelayedCallback();
        if (!isServer)
        {
            myClient.Send(sendCharSheet, new StringMessage(moverSender.GetParentCharSheetString()));
        }
    }

    // server function
    public void ServerHandlesConnection(NetworkMessage netMsg)
    {
        Debug.Log("Connected to client");
        MissionManager.instance.DelayedCallback();
        if (isServer)
        {
            NetworkServer.SendToAll(sendSeedID, new IntegerMessage(moverSender.GetParentSeed()));
            NetworkServer.SendToAll(sendCharSheet, new StringMessage(moverSender.GetParentCharSheetString()));
        }
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
        Debug.Log("Client gets: " + toReturn);
        if (!isServer)
        {
            moverListener.AcceptRemoteCharacterSheet(JsonUtility.FromJson<PlayerCharacterSheet>(toReturn));
        }

    }

    public void ServerAcceptsCharSheet(NetworkMessage charSheetMessage)
    {
        if (charSheetMessage.conn.connectionId==0)
        {
            Debug.Log("Killed you!");
            return;
        }
        String toReturn = charSheetMessage.ReadMessage<StringMessage>().value;
        Debug.Log("Server gets: " + toReturn);
        if (isServer)
        {
            moverListener.AcceptRemoteCharacterSheet(JsonUtility.FromJson<PlayerCharacterSheet>(toReturn));
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

