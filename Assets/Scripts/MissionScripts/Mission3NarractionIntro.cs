using Narrate;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission3NarractionIntro : Mission {

    public TimerNarrationTrigger[] narrations;
    public GameObject[] gridAttachedPieces;

    private int stepNum;
    private float timeSinceStepStarted;

    public Image darkness;

    public GameGrid gridToSetup;
    public GameGrid gridToTurnIntoAI;

    public List<GameObject> pawnToHide;

    public AudioSource preLockonMusic;
    public AudioSource combatMusicThatsNotAsIntrusive;

    public DamageManager damageManager;
    public DestroySpaceshipOnDeath moreStuffToExplodeOnDeath;

    public GameObject cameraToMove;
    public TextAsset[] droneAIs;

    private bool shipIsMovingIn;
    private float shipMovingInTime;

    public AudioSource loseMusic;

    public GameObject shipWrapper;

    //Ship is at x=32, goes to 19.
    //Length goes from 25 to 12
    internal override void Unblock()
    {
        stepNum++;
        Debug.Log(stepNum);
        switch (stepNum)
        {
            case 1:  // Finish First Narration, Tractor beam turns on, begin second narration;
                darkness.color = new Color(0, 0, 0, 0);
                shipIsMovingIn = true;

                narrations[1].gameObject.SetActive(true);
                timeSinceStepStarted = 0;
                shipWrapper.transform.parent = gridToTurnIntoAI.player.pawn.transform;
                gridToTurnIntoAI.player.pawn.gameObject.SetActive(true);
                break;
            case 2:
                timeSinceStepStarted = 0;
                MissionManager.freezeAI = false;
                MissionManager.freezePlayerBoard = false;
                MissionManager.triggerCallbacksOnAttackHit = true;
                preLockonMusic.Stop();
                combatMusicThatsNotAsIntrusive.Play();
                foreach (GameObject go in gridAttachedPieces)
                {
                    go.SetActive(true);
                }
                break;
            case 3:
                MissionManager.freezeAI = true;
                MissionManager.freezePlayerBoard = true;
                narrations[2].gameObject.SetActive(true);
                MissionManager.triggerCallbacksOnAttackHit = false;
                break;
            case 4:
                MissionManager.freezeAI = false;
                MissionManager.freezePlayerBoard = false;
                MissionManager.triggerCallbackOnShipDestroyed = true;
                break;
            case 5:
                if (gridToSetup.player.IsAlive())
                {
                    MissionManager.freezeAI = true;
                    MissionManager.freezePlayerBoard = true;
                    narrations[3].gameObject.SetActive(true);
                }
                else
                {
                    combatMusicThatsNotAsIntrusive.Stop();
                    loseMusic.Play();
                    Lose();
                }
                break;
            case 6:
                Win(false);
                break;
        }
    }

    // Use this for initialization
    void OnEnable()
    {

        gridAttachedPieces = new GameObject[4];
        CommonMissionScriptingTargets p = MissionManager.instance.pointers;
        gridAttachedPieces[0] = p.combatant1.healthBar.gameObject;
        gridAttachedPieces[1] = p.combatant2.healthBar.gameObject;
        gridAttachedPieces[2] = p.combatant2.multiplierText.gameObject;
        gridAttachedPieces[3] = p.player2Grid.transform.Find("NextPieceText").gameObject;
        darkness = p.daaaaaknesssss;
        gridToSetup = p.player1Grid;
        gridToTurnIntoAI = p.player2Grid;
        pawnToHide = p.ship2.stuffToHideIfThisPawnIsDisabledByTheMission;
        cameraToMove = p.cameraWrapper2.gameObject;
        shipWrapper.GetComponent<DestroySpaceshipOnDeath>().stuffToHide.Add(p.combatant2.multiplierText.GetComponent<SpriteRenderer>());
        shipWrapper.GetComponent<DestroySpaceshipOnDeath>().stuffToHide.Add(gridAttachedPieces[3].GetComponent<SpriteRenderer>());
        damageManager = p.combatant2.damageManager;
        p.restartButton1.gameObject.SetActive(true);
        p.restartButton2.gameObject.SetActive(true);

        narrations[0].gameObject.SetActive(true);
        MissionManager.freezePlayerBoard = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (stepNum == 0)
        {
            if (timeSinceStepStarted == 0)
            {
                MissionManager.freezeAI = true;
                cameraToMove.transform.localPosition = new Vector3(-25, 32, -18.7f);
                //gridToSetup.player.
                foreach (GameObject part in pawnToHide)
                {
                    part.SetActive(false);
                }
                foreach (GameObject go in gridAttachedPieces)
                {
                    go.SetActive(false);
                }
                preLockonMusic.Play();
                damageManager.stuffThatHappensInTheFinalExplosion.Add(moreStuffToExplodeOnDeath);
            }
            timeSinceStepStarted += Time.deltaTime;
            float brightness = Mathf.Clamp01(timeSinceStepStarted / 2);
            darkness.color = new Color(0, 0, 0, 1 - brightness);
        }
        if (shipIsMovingIn)
        {
            shipMovingInTime += Time.deltaTime;
            shipWrapper.transform.localPosition = new Vector3(0, 0, Mathf.Lerp(-10, .23f, shipMovingInTime / 6));
        }
        if (stepNum >= 2)
        {
            timeSinceStepStarted += Time.deltaTime;
            float cameraPosit = Mathf.Lerp(-33f, 0, timeSinceStepStarted / 3);
            cameraToMove.transform.localPosition = new Vector3(cameraPosit, 32, -18.7f);
        }
    }

    internal override AIParams GetAIParams()
    {
        return new AIParams(droneAIs[UnityEngine.Random.Range(0, droneAIs.Length)].text, true, .2f, false);
    }

    internal override EngineRoomGameType GameType()
    {
        return EngineRoomGameType.SINGLE_PLAYER;
    }
}
