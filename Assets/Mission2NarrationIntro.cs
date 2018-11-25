using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Narrate;
using UnityEngine.UI;

public class Mission2NarrationIntro : Mission
{
    public TimerNarrationTrigger[] narrations;
    public GameObject shieldTutorialObject;
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
                tractorParticles.gameObject.SetActive(true);
                narrations[1].gameObject.SetActive(true);

                List<int[,]> piecesToForce = new List<int[,]>
                {
                    new int[3, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 0, 1 } }
                };
                gridToSetup.DropNewCubeAt(5, 17);
                gridToSetup.player.cameraToShake.ShakeCamera(3, 1);
                gridToSetup.ForcePieces(piecesToForce);
                break;
            case 2:  // Narration done, Game starts
                MissionManager.isInCutscene = false;
                MissionManager.triggerCallbacksOnShipReboot = false;
                break;
            case 3:  // Ship reboot complete, more narration
                MissionManager.isInCutscene = true;
                narrations[2].gameObject.SetActive(true);
                break;
            case 4:  //Narration complete, tutorial thing pops up
                shieldInfo.gameObject.SetActive(true);
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

    // Update is called once per frame
    void Update()
    {
        if (stepNum == 0)
        {
            if (timeSinceStepStarted == 0)
            {
                gridToSetup.player.howManyItemsIHave = 0;
                gridToSetup.SetGridCellTypeStateAndAttendentVFX();
                gridToSetup.player.energy = 160;
            }
            timeSinceStepStarted += Time.deltaTime;
            float brightness = Mathf.Clamp01(timeSinceStepStarted / 2);
            darkness.color = new Color(0, 0, 0, 1 - brightness);
        }
    }
}
