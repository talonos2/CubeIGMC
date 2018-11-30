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
    }

    private GameGrid randomGrid;
    private Combatant randomCombatant;

    private float timeSinceStepStarted;
    private Image darkness;

    // Update is called once per frame
    void Update()
    {
        if (timeSinceStepStarted == 0)
        {
            randomCombatant.SetRandomNPC(level);
            music[level].Play();
        }

        timeSinceStepStarted += Time.deltaTime;
        float brightness = Mathf.Clamp01(timeSinceStepStarted / 2);
        darkness.color = new Color(0, 0, 0, 1 - brightness);
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
