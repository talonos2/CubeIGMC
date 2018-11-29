using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class TestNetworkMessage : MonoBehaviour
{

    const short MyBeginMsg = 1002;

    NetworkClient m_client;

    public void SendReadyToBeginMessage(int myId)
    {
        var msg = new IntegerMessage(myId);
        m_client.Send(MyBeginMsg, msg);
    }

    public void Init(NetworkClient client)
    {
        m_client = client;
        NetworkServer.RegisterHandler(MyBeginMsg, OnServerReadyToBeginMessage);
    }

    void OnServerReadyToBeginMessage(NetworkMessage netMsg)
    {
        var beginMessage = netMsg.ReadMessage<IntegerMessage>();
        Debug.Log("received OnServerReadyToBeginMessage " + beginMessage.value);
    }
    private void Update()
    {
        SendReadyToBeginMessage(23232);
    }
}
