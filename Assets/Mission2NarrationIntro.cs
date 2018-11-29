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
    private Vector3 storedHomePosition;
    public Image darkness;
    
    public HackyCallback hackyCallback;
    public ParticleSystem tractorParticles;

    public GameGrid gridToSetup;
    public GameGrid gridToTurnIntoAI;
    public List<GameObject> pawnToHide;

    public AudioSource tractorBeamLockonSound;
    public AudioSource spaceDoorOpenSound;

    public AudioSource preLockonMusic;
    public AudioSource postLockonMusic;
    public AudioSource combatMusicThatsNotAsIntrusive;

    public AnnoyingTutorialPopup shieldInfo;

    public GameObject mothershipToMoveIn;

    public DamageManager damageManagerForTractorBeam;
    public DestroySpaceshipOnDeath moreStuffToExplodeOnDeath;
    private float TractorBeamHPNum = 25;

    public GameObject cameraToMove;
    public TextAsset tractorBeamAI;

    public SpaceshipPawn shipToMakeFlyAway;
    private bool sceneSwitched;

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
                tractorParticles.gameObject.SetActive(true);
                narrations[1].gameObject.SetActive(true);
                timeSinceStepStarted = 0;

                preLockonMusic.Stop();
                postLockonMusic.Play();
                tractorBeamLockonSound.Play();

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
                rebootTutorialObject.SetActive(false);
                MissionManager.isInCutscene = true;
                narrations[2].gameObject.SetActive(true);
                MissionManager.triggerCallbacksOnShipReboot = false;
                break;
            case 4:  //Narration complete, tutorial thing pops up
                gridToSetup.player.GetCharacterSheet().ShieldEquippedID = 1;
                gridToSetup.SetGridCellTypeStateAndAttendentVFX();
                foreach (GameObject go in gridAttachedPieces)
                {
                    go.SetActive(true);
                }
                shieldInfo.gameObject.SetActive(true);

                break;
            case 5:  //TutorialGone
                postLockonMusic.Stop();
                combatMusicThatsNotAsIntrusive.Play();
                MissionManager.isInCutscene = false;
                MissionManager.freezeAI = false;
                MissionManager.TriggerCallbackOnShipDestroyed = true;
                break;
            case 6: // Tutorial complete, You talk
                MissionManager.isInCutscene = true;
                MissionManager.freezeAI = true;
                narrations[3].gameObject.SetActive(true);
                tractorParticles.gameObject.SetActive(false);
                break;
            case 7:
                timeSinceStepStarted = 0f;
                this.storedHomePosition = shipToMakeFlyAway.rootPosition;
                shipToMakeFlyAway.SetHomePosition(new Vector3 (-5, 12.5f, 25), shipToMakeFlyAway.transform.rotation);
                shipToMakeFlyAway.takeoff(2, shipToMakeFlyAway.transform.position, shipToMakeFlyAway.transform.rotation);
                narrations[4].gameObject.SetActive(true);
                break;
            case 8: //Talk is over, level is over.
                narrations[5].gameObject.SetActive(true);
                spaceDoorOpenSound.Play();
                break;
        }
    }

    // Use this for initialization
    void OnEnable()
    {
        gridAttachedPieces = new GameObject[4];
        PointerHolder p = MissionManager.instance.pointers;
        gridAttachedPieces[0] = p.combatant1.healthBar.gameObject;
        gridAttachedPieces[1] = p.combatant2.healthBar.gameObject;
        gridAttachedPieces[2] = p.combatant2.multiplierText.gameObject;
        gridAttachedPieces[3] = p.player2Grid.transform.Find("NextPieceText").gameObject;
        darkness = p.daaaaaknesssss;
        gridToSetup = p.player1Grid;
        gridToTurnIntoAI = p.player2Grid;
        pawnToHide = p.ship2.stuffToHideIfThisPawnIsDisabledByTheMission;
        pawnToHide.Add(p.ship2.transform.Find("Shield").gameObject);
        tractorBeamLockonSound = p.ship1.getHitHeavySound;
        damageManagerForTractorBeam = p.combatant2.damageManager;
        cameraToMove = p.cameraWrapper2.gameObject;
        shipToMakeFlyAway = p.ship1;
        mothershipToMoveIn.GetComponent<DestroySpaceshipOnDeath>().stuffToHide.Add(p.combatant2.multiplierText.GetComponent<SpriteRenderer>());
        mothershipToMoveIn.GetComponent<DestroySpaceshipOnDeath>().stuffToHide.Add(gridAttachedPieces[3].GetComponent<SpriteRenderer>());

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
                gridToSetup.SetGridCellTypeStateAndAttendentVFX();
                gridToSetup.player.energy = 160;
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
            }
            timeSinceStepStarted += Time.deltaTime;
            float brightness = Mathf.Clamp01(timeSinceStepStarted / 2);
            darkness.color = new Color(0, 0, 0, 1 - brightness);
            gridToTurnIntoAI.player.health = TractorBeamHPNum;
            gridToTurnIntoAI.player.SetCharacterSheet(2);
        }
        else if (stepNum < 3)
        {
            if (timeSinceStepStarted == 0)
            {
                //Enemy AI cheats a lot.
                List<int[,]> piecesToForce = getForcedPieces();
                for (int x = 0; x < 15; x++)
                {
                    piecesToForce.AddRange(getForcedPieces());
                }

                gridToTurnIntoAI.ForcePieces(piecesToForce);
                damageManagerForTractorBeam.stuffThatHappensInTheFinalExplosion.Add(moreStuffToExplodeOnDeath);
            }
            timeSinceStepStarted += Time.deltaTime;
            float positionOfMotherShip = Mathf.Lerp(32, 19, timeSinceStepStarted / 10);
            mothershipToMoveIn.gameObject.transform.localPosition = new Vector3(positionOfMotherShip, 25.33f, -15.78f);
        }
        else if (stepNum <=6)
        {
            timeSinceStepStarted += Time.deltaTime;
            float cameraPosit = Mathf.Lerp(-33f, 0, timeSinceStepStarted/10);
            cameraToMove.transform.localPosition = new Vector3(cameraPosit, 32, -18.7f);
        }
        else if (stepNum < 1000)
        {
            timeSinceStepStarted += Time.deltaTime;
            if (timeSinceStepStarted < 3)
            {
                Color colorToTurnDarkness = new Color(0, 0, 0, Mathf.Lerp(0, 1, timeSinceStepStarted / 2));
                darkness.color = colorToTurnDarkness;
                combatMusicThatsNotAsIntrusive.volume = Mathf.Lerp(1, 0, timeSinceStepStarted / 3);
            }
            else
            {
                if (!sceneSwitched)
                {
                    sceneSwitched = true;
                    moreStuffToExplodeOnDeath.gameObject.SetActive(false);
                    combatMusicThatsNotAsIntrusive.Stop();
                    preLockonMusic.Play();
                    shipToMakeFlyAway.SetHomePosition(storedHomePosition, shipToMakeFlyAway.transform.rotation);
                    shipToMakeFlyAway.takeoff(.1f, storedHomePosition, shipToMakeFlyAway.transform.rotation);
                }
                preLockonMusic.volume = Mathf.Lerp(0, 1, (timeSinceStepStarted - 3));
                darkness.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, (timeSinceStepStarted - 4) / 2));
            }
        }
    }

    private static List<int[,]> getForcedPieces()
    {
        List<int[,]> piecesToForce = new List<int[,]>
                {
                    new int[3, 3] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 0, 0 } }, //1-1
                    new int[3, 3] { { 0, 1, 0 }, { 1, 1, 1 }, { 1, 0, 1 } }, //1-2
                    new int[3, 3] { { 0, 0, 0 }, { 1, 1, 1 }, { 1, 0, 1 } }, //1-3
                    new int[3, 3] { { 0, 0, 0 }, { 1, 1, 1 }, { 0, 0, 0 } }, //1-4

                    new int[3, 3] { { 0, 1, 1 }, { 1, 1, 1 }, { 0, 0, 0 } }, //5-1

                    new int[3, 3] { { 1, 0, 0 }, { 1, 1, 1 }, { 0, 0, 0 } }, //2-1
                    new int[3, 3] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 1, 0 } }, //2-2
                    new int[3, 3] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 0, 0 } }, //2-3

                    new int[3, 3] { { 1, 0, 0 }, { 1, 1, 0 }, { 0, 1, 0 } }, //5-2
                    new int[3, 3] { { 1, 1, 1 }, { 0, 1, 1 }, { 0, 0, 1 } }, //5-3

                    new int[3, 3] { { 0, 1, 0 }, { 1, 1, 1 }, { 1, 0, 1 } }, //2-4
                    new int[3, 3] { { 0, 0, 0 }, { 1, 1, 1 }, { 1, 0, 1 } }, //2-5

                    new int[3, 3] { { 1, 0, 0 }, { 1, 1, 1 }, { 0, 1, 0 } }, //4-1
                    new int[3, 3] { { 1, 0, 1 }, { 1, 1, 1 }, { 0, 0, 0 } }, //4-2

                    new int[3, 3] { { 1, 0, 0 }, { 1, 1, 1 }, { 0, 0, 0 } }, //3-1
                    new int[3, 3] { { 0, 0, 1 }, { 1, 1, 1 }, { 1, 1, 0 } }, //3-2
                    new int[3, 3] { { 0, 0, 0 }, { 1, 1, 1 }, { 1, 1, 0 } }, //3-3

                    new int[3, 3] { { 0, 1, 0 }, { 1, 1, 0 }, { 1, 1, 0 } }, //5-4
                    new int[3, 3] { { 0, 1, 0 }, { 0, 1, 0 }, { 0, 0, 0 } }, //5-5

                    new int[3, 3] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 0, 0 } }, //3-4
                    new int[3, 3] { { 0, 1, 1 }, { 1, 1, 1 }, { 0, 0, 0 } }, //3-5

                    new int[3, 3] { { 0, 0, 1 }, { 0, 1, 1 }, { 1, 1, 1 } }, //4-3
                    new int[3, 3] { { 0, 0, 0 }, { 0, 1, 0 }, { 0, 0, 0 } }, //4-4
                    new int[3, 3] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 0, 0 } }, //4-5

                    new int[3, 3] { { 0, 1, 0 }, { 0, 1, 1 }, { 0, 1, 1 } }, //5-6
                    new int[3, 3] { { 1, 0, 1 }, { 1, 1, 1 }, { 1, 1, 1 } }, //5-7
        };
        return piecesToForce;
    }

    internal override AIParams GetAIParams()
    {
        return new AIParams(tractorBeamAI.text, true, .4f, true);
    }

    internal override EngineRoomGameType GameType()
    {
        return EngineRoomGameType.SINGLE_PLAYER;
    }
}
