using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkedMission : Mission
{
    internal override void Unblock()
    {

    }

    // Use this for initialization
    void Start()
    {
        PointerHolder p = MissionManager.instance.pointers;
        darkness = p.daaaaaknesssss;
    }

    public GameGrid gridToCheatWith;
    private float timeSinceStepStarted;
    public Image darkness;
    public bool isHost;

    public bool cheaty;

    // Update is called once per frame
    void Update()
    {
        timeSinceStepStarted += Time.deltaTime;
        float brightness = Mathf.Clamp01(timeSinceStepStarted / 2);
        darkness.color = new Color(0, 0, 0, 1 - brightness);
    }

    internal override AIParams GetAIParams()
    {
        return null;
    }

    internal override EngineRoomGameType GameType()
    {
        if (isHost)
        {
            return EngineRoomGameType.SERVER_PVP;
        }
        else
        {
            return EngineRoomGameType.CLIENT_PVP;
        }
    }
}
