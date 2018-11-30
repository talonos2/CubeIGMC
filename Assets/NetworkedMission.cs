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
        tick = p.player1Grid.ominousTick;
    }

    public GameGrid gridToCheatWith;
    private float timeSinceStepStarted;
    public Image darkness;
    public bool isHost;

    public Image waiting;
    public Image[] countdown;

    public bool cheaty;
    private bool starting = false;
    private AudioSource tick;

    private bool[] ticks = new bool[4];

    // Update is called once per frame
    void Update()
    {
        if (timeSinceStepStarted == 0)
        {
            MissionManager.isInCutscene = true;
        }
        timeSinceStepStarted += Time.deltaTime;
        float brightness = Mathf.Clamp01(timeSinceStepStarted / 2);
        darkness.color = new Color(0, 0, 0, 1 - brightness);

        if (MissionManager.instance.weAreAllHere == true)
        {
            waiting.gameObject.SetActive(false);
            starting = true;
            MissionManager.instance.weAreAllHere = false;
            timeSinceStepStarted = 0;
        }
        if (starting)
        {
            for (int x = 0; x < 4; x++)
            {
                if (timeSinceStepStarted > .5+x && !ticks[x])
                {
                    tick.Play();
                    ticks[x] = true;
                }
            }
            if (timeSinceStepStarted > 4.5)
            {
                MissionManager.isInCutscene = false;
            }
        }

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
