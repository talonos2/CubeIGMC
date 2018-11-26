using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Narrate;
using UnityEngine.UI;

public class Mission2NarrationIntro : Mission
{
    public TimerNarrationTrigger[] narrations;
    public GameObject[] gridAttachedPieces;
    public GameObject rebootTutorialObject;
    public GameObject HPTutorialObject;
    private int stepNum;
    private float timeSinceStepStarted;

    public Image darkness;
    
    public HackyCallback hackyCallback;
    public ParticleSystem tractorParticles;

    public GameGrid gridToSetup;

    public AudioSource InTheAirMusic;
    public AnnoyingTutorialPopup shieldInfo;

    public GameObject mothershipToMoveIn;

    //Ship is at x=32, goes to 19.
    //Length goes from 25 to 12
    internal override void Unblock()
    {
        Debug.Log(stepNum++);
        switch (stepNum)
        {
            case 1:  // Finish First Narration, Tractor beam turns on, begin second narration;
                darkness.color = new Color(0, 0, 0, 0);
                tractorParticles.gameObject.SetActive(true);
                narrations[1].gameObject.SetActive(true);
                timeSinceStepStarted = 0;

                List<int[,]> piecesToForce = new List<int[,]>
                {
                    new int[3, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 0, 1 } }
                };
                gridToSetup.DropNewCubeAt(1, 3);
                gridToSetup.DropNewCubeAt(3, 4);
                gridToSetup.DropNewCubeAt(6, 3);
                gridToSetup.DropNewCubeAt(7, 5);
                gridToSetup.DropNewCubeAt(10, 5);
                gridToSetup.DropNewCubeAt(11, 3);
                gridToSetup.DropNewCubeAt(13, 4);
                gridToSetup.DropNewCubeAt(14, 5);
                
                gridToSetup.DropNewCubeAt(8, 6);
                gridToSetup.DropNewCubeAt(13, 6);
                gridToSetup.DropNewCubeAt(4, 7);
                gridToSetup.DropNewCubeAt(9, 7);
                gridToSetup.DropNewCubeAt(6, 8);
                gridToSetup.DropNewCubeAt(13, 8);
                gridToSetup.DropNewCubeAt(10, 9);
                gridToSetup.DropNewCubeAt(7, 9);
                gridToSetup.DropNewCubeAt(2, 10);
                gridToSetup.DropNewCubeAt(5, 10);
                gridToSetup.DropNewCubeAt(0, 11);
                gridToSetup.DropNewCubeAt(13, 11);
                gridToSetup.DropNewCubeAt(9, 12);
                gridToSetup.DropNewCubeAt(8, 12);
                gridToSetup.DropNewCubeAt(14, 13);
                gridToSetup.DropNewCubeAt(3, 13);
                gridToSetup.DropNewCubeAt(1, 14);
                gridToSetup.DropNewCubeAt(8, 14);
                gridToSetup.DropNewCubeAt(5, 15);
                gridToSetup.DropNewCubeAt(2, 15);
                gridToSetup.DropNewCubeAt(12, 16);
                gridToSetup.DropNewCubeAt(7, 16);
                gridToSetup.DropNewCubeAt(14, 17);
                gridToSetup.DropNewCubeAt(6, 17);

                gridToSetup.player.cameraToShake.ShakeCamera(1.5f, .7f);
                gridToSetup.ForcePieces(piecesToForce);
                break;
            case 2:  // Narration done, Game starts
                MissionManager.isInCutscene = false;
                rebootTutorialObject.SetActive(true);
                MissionManager.triggerCallbacksOnShipReboot = true;
                break;
            case 3:  // Ship reboot complete, more narration
                mothershipToMoveIn.gameObject.transform.localPosition = new Vector3(19, 25.33f, -15.78f);
                timeSinceStepStarted = 0;
                MissionManager.isInCutscene = true;
                narrations[2].gameObject.SetActive(true);
                MissionManager.triggerCallbacksOnShipReboot = false;
                break;
            case 4:  //Narration complete, tutorial thing pops up
                MissionManager.isInCutscene = false;
                gridToSetup.player.howManyItemsIHave = 2;
                gridToSetup.SetGridCellTypeStateAndAttendentVFX();
                foreach (GameObject go in gridAttachedPieces)
                {
                    go.SetActive(true);
                }
                //shieldInfo.gameObject.SetActive(true);
                break;
            case 5: // Tutorial complete, You talk
                MissionManager.isInCutscene = true;
                timeSinceStepStarted = 0f;
                narrations[4].gameObject.SetActive(true);
                break;
            case 6: //Talk is done, you fly away, then black out, then black in, then talk.
                break;
            case 7: //Talk is over, level is over.
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

    private bool playedRunningSound;
    public DamageManager damageManagerForDoor;
    private float doorHPNum = 50;
    public GameObject structure;
    private float shipAccelleration = .5f;
    public GameObject escapeParticles;
    public GameObject cameraToMove;

    // Update is called once per frame
    void Update()
    {
        if (stepNum == 0)
        {
            if (timeSinceStepStarted == 0)
            {
                gridToSetup.player.howManyItemsIHave = 1;
                gridToSetup.SetGridCellTypeStateAndAttendentVFX();
                gridToSetup.player.energy = 160;
                cameraToMove.transform.localPosition = new Vector3(-25, 32, -18.7f);
                foreach (GameObject go in gridAttachedPieces)
                {
                    go.SetActive(false);
                }
            }
            timeSinceStepStarted += Time.deltaTime;
            float brightness = Mathf.Clamp01(timeSinceStepStarted / 2);
            darkness.color = new Color(0, 0, 0, 1 - brightness);
        }
        else if (stepNum < 3)
        {
            timeSinceStepStarted += Time.deltaTime;
            float positionOfMotherShip = Mathf.Lerp(32, 19, timeSinceStepStarted / 10);
            mothershipToMoveIn.gameObject.transform.localPosition = new Vector3(positionOfMotherShip, 25.33f, -15.78f);
        }
        else if (stepNum < 1000)
        {
            timeSinceStepStarted += Time.deltaTime;
            float cameraPosit = Mathf.Lerp(-33f, 0, timeSinceStepStarted/10);
            cameraToMove.transform.localPosition = new Vector3(cameraPosit, 32, -18.7f);
        }
    }
}
