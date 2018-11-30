using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreePlayMission : Mission {

    internal override void Unblock()
    {
        
    }

    // Use this for initialization
    void Start () {
        PointerHolder p = MissionManager.instance.pointers;
        darkness = p.daaaaaknesssss;
        p.restartButton1.gameObject.SetActive(true);
        p.restartButton2.gameObject.SetActive(true);
        tick = p.player1Grid.ominousTick;
    }

    public GameGrid gridToCheatWith;
    private float timeSinceStepStarted;
    public Image darkness;

    public bool cheaty;

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

        for (int x = 0; x < 4; x++)
            {
                if (timeSinceStepStarted > .5 + x && !ticks[x])
                {
                    tick.Play();
                    ticks[x] = true;
                }
            }
            if (timeSinceStepStarted > 3.5)
            {
                MissionManager.isInCutscene = false;
            }

    }

    internal override AIParams GetAIParams()
    {
        return null;
    }

    internal override EngineRoomGameType GameType()
    {
        return EngineRoomGameType.LOCAL_PVP;
    }
}
