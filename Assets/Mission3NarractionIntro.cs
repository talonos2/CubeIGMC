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

    public GameObject[] pawnToHide;

    public AudioSource preLockonMusic;
    public AudioSource combatMusicThatsNotAsIntrusive;

    public DamageManager damageManager;
    public DestroySpaceshipOnDeath moreStuffToExplodeOnDeath;

    public GameObject cameraToMove;
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
                darkness.color = new Color(0, 0, 0, 0);
                shipIsMovingIn = true;
                narrations[1].gameObject.SetActive(true);
                timeSinceStepStarted = 0;
                shipWrapper.transform.parent = gridToTurnIntoAI.player.pawn.transform;
                gridToTurnIntoAI.LoadAI(true, .2f, true);
                break;
            case 2:
                timeSinceStepStarted = 0;
                MissionManager.freezeAI = false;
                MissionManager.isInCutscene = false;
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
                MissionManager.isInCutscene = true;
                narrations[2].gameObject.SetActive(true);
                MissionManager.triggerCallbacksOnAttackHit = false;
                break;
            case 4:
                MissionManager.freezeAI = false;
                MissionManager.isInCutscene = false;
                MissionManager.TriggerCallbackOnShipDestroyed = true;
                break;
            case 5:
                MissionManager.freezeAI = true;
                MissionManager.isInCutscene = true;
                narrations[3].gameObject.SetActive(true);
                break;
        }
    }

    // Use this for initialization
    void Start()
    {
        narrations[0].gameObject.SetActive(true);

        //gridToSetup.player.enemy.damageManager = damageManagerForDoor;

        MissionManager.isInCutscene = true;
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
                gridToTurnIntoAI.isPlayedByAI = true;
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
}
