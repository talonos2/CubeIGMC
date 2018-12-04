using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class NetworkedMission : Mission
{
    internal override void Unblock()
    {
        waiting.gameObject.SetActive(false);
        starting = true;
        timeSinceStepStarted = 0;
    }

    // Use this for initialization
    void Start()
    {
        pointers = MissionManager.instance.pointers;
        tick = pointers.player1Grid.ominousTick;
    }

    private float timeSinceStepStarted;

    public SpriteRenderer waiting;
    public Image[] countdown;

    private bool starting = false;
    private AudioSource tick;
    private bool[] ticks = new bool[4];

    // Update is called once per frame
    void Update()
    {
        if (timeSinceStepStarted == 0)
        {
            MissionManager.freezeAI = true;
            MissionManager.freezePlayerBoard = true;
            MissionManager.triggerCallbackOnRemotePlayerConnected = true;
            MusicManager.instance.StopAllMusic();
            MusicManager.instance.music[MusicManager.CUTSCENE_1].Play();
        }

        timeSinceStepStarted += Time.deltaTime;
        float brightness = Mathf.Clamp01(timeSinceStepStarted / 2);
        pointers.daaaaaknesssss.color = new Color(0, 0, 0, 1 - brightness);

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
            if (timeSinceStepStarted > 3.5)
            {
                MissionManager.freezeAI = false;
                MissionManager.freezePlayerBoard = false;
            }
        }

    }

    internal override AIParams GetAIParams()
    {
        return null;
    }
}
