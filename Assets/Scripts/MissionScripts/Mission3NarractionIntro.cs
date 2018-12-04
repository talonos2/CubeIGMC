using Narrate;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission3NarractionIntro : Mission {

    public TimerNarrationTrigger[] narrations;
    private GameObject[] gridAttachedPieces;

    private int stepNum;
    private float timeSinceStepStarted;

    private List<GameObject> pawnToHide;

    private DamageManager damageManager;
    public DestroySpaceshipOnDeath moreStuffToExplodeOnDeath;

    public TextAsset[] droneAIs;

    private bool shipIsMovingIn;
    private float shipMovingInTime;

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
                pointers.daaaaaknesssss.color = new Color(0, 0, 0, 0);
                shipIsMovingIn = true;

                narrations[1].gameObject.SetActive(true);
                timeSinceStepStarted = 0;
                shipWrapper.transform.parent = pointers.ship2.transform;
                pointers.ship2.gameObject.SetActive(true);
                break;
            case 2:
                timeSinceStepStarted = 0;
                MissionManager.freezeAI = false;
                MissionManager.freezePlayerBoard = false;
                MissionManager.triggerCallbacksOnAttackHit = true;
                MusicManager.instance.StopAllMusic();
                MusicManager.instance.music[MusicManager.BLASTING_THROUGH_WALLS].Play();
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
                if (pointers.combatant1.IsAlive())
                {
                    MissionManager.freezeAI = true;
                    MissionManager.freezePlayerBoard = true;
                    narrations[3].gameObject.SetActive(true);
                }
                else
                {
                    MusicManager.instance.StopAllMusic();
                    MusicManager.instance.music[MusicManager.TITLE_SCREEN].Play();
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
        pointers = MissionManager.instance.pointers;
        gridAttachedPieces[0] = pointers.combatant1.healthBar.gameObject;
        gridAttachedPieces[1] = pointers.combatant2.healthBar.gameObject;
        gridAttachedPieces[2] = pointers.combatant2.multiplierText.gameObject;
        gridAttachedPieces[3] = pointers.player2Grid.transform.Find("NextPieceText").gameObject;
        pawnToHide = pointers.ship2.stuffToHideIfThisPawnIsDisabledByTheMission;
        shipWrapper.GetComponent<DestroySpaceshipOnDeath>().stuffToHide.Add(pointers.combatant2.multiplierText.GetComponent<SpriteRenderer>());
        shipWrapper.GetComponent<DestroySpaceshipOnDeath>().stuffToHide.Add(gridAttachedPieces[3].GetComponent<SpriteRenderer>());
        damageManager = pointers.combatant2.damageManager;
        pointers.restartButton1.gameObject.SetActive(true);
        pointers.restartButton2.gameObject.SetActive(true);

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
                pointers.cameraWrapper2.transform.localPosition = new Vector3(-25, 32, -18.7f);
                //gridToSetup.player.
                foreach (GameObject part in pawnToHide)
                {
                    part.SetActive(false);
                }
                foreach (GameObject go in gridAttachedPieces)
                {
                    go.SetActive(false);
                }
                MusicManager.instance.StopAllMusic();
                MusicManager.instance.music[MusicManager.CUTSCENE_1].Play();
                damageManager.stuffThatHappensInTheFinalExplosion.Add(moreStuffToExplodeOnDeath);
            }
            timeSinceStepStarted += Time.deltaTime;
            float brightness = Mathf.Clamp01(timeSinceStepStarted / 2);
            pointers.daaaaaknesssss.color = new Color(0, 0, 0, 1 - brightness);
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
            pointers.cameraWrapper2.transform.localPosition = new Vector3(cameraPosit, 32, -18.7f);
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
