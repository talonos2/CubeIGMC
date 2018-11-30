using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreePlayMission : Mission
{
    // Use this for initialization
    void Start () {
        pointers = MissionManager.instance.pointers;
        pointers.restartButton1.gameObject.SetActive(true);
        pointers.restartButton2.gameObject.SetActive(true);
    }

    private float timeSinceStepStarted;

    private bool[] ticks = new bool[4];

    // Update is called once per frame
    void Update()
    {
        if (timeSinceStepStarted == 0)
        {
            MissionManager.freezePlayerBoard = true;
        }

        timeSinceStepStarted += Time.deltaTime;

        //Fade in from black.
        float brightness = Mathf.Clamp01(timeSinceStepStarted / 2);
        pointers.daaaaaknesssss.color = new Color(0, 0, 0, 1 - brightness);

        //Tick before starting.
        for (int x = 0; x < 4; x++)
        {
            if (timeSinceStepStarted > .5 + x && !ticks[x])
            {
                pointers.gameStartTickSound.Play();
                ticks[x] = true;
            }
        }

        //Unfreeze after four ticks.
        if (timeSinceStepStarted > 3.5)
        {
            MissionManager.freezePlayerBoard = false;
        }
    }

    internal override void Unblock() {/*NOOP*/}
    internal override AIParams GetAIParams(){return null;}
    internal override EngineRoomGameType GameType(){return EngineRoomGameType.LOCAL_PVP;}
}
