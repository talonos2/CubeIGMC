using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomArenaMission : Mission
{
    public TextAsset[] AITexts;
    public int level = 0;
    public AudioSource[] music;

    internal override void Unblock()
    {

    }

    // Use this for initialization
    void Start()
    {
        PointerHolder p = MissionManager.instance.pointers;
        darkness = p.daaaaaknesssss;
        randomGrid = p.player2Grid;
        randomCombatant = p.combatant2;
        p.restartButton1.gameObject.SetActive(true);
        p.restartButton2.gameObject.SetActive(true);
        tick = p.player1Grid.ominousTick;
    }

    private GameGrid randomGrid;
    private Combatant randomCombatant;

    private float timeSinceStepStarted;
    private Image darkness;
    private AudioSource tick;

    private bool[] ticks = new bool[4];

    // Update is called once per frame
    void Update()
    {
        if (timeSinceStepStarted == 0)
        {
            randomCombatant.SetRandomNPC(level);
            music[level].Play();
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
        if (timeSinceStepStarted > 4.5)
        {
            MissionManager.isInCutscene = false;
        }
    }

    internal override AIParams GetAIParams()
    {
        return new AIParams(AITexts[UnityEngine.Random.Range(0,AITexts.Length)].text, false, 0, false);
    }

    internal override EngineRoomGameType GameType()
    {
        return EngineRoomGameType.SINGLE_PLAYER;
    }
}
