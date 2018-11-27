using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Narrate;
using UnityEngine.UI;

public class Mission1NarrationIntro : Mission
{
    public TimerNarrationTrigger[] narrations;
    public GameObject[] tutorialTexts;
    private int stepNum;
    private float timeSinceStepStarted;
    public Image darkness;
    public GameObject[] thingsToHide;
    public Camera cameraToDisable;
    public Transform cameraToMove;
    public Vector3 toMoveCameraTo;
    public SpaceshipPawn shipToMakeNotWiggle;
    public Light spaceLightToDisable;
    private GameObject ship;
    public GameObject firesToTurnOff;

    public AudioSource turnOnLightsSound;
    public AudioSource runningSound;
    public AudioSource openShipSound;
    public AudioSource startShipSound;
    public AudioSource alarmSound;
    public AudioSource wallImpactExplosionSound;

    public KlaxonSpin klaxonToTurnOn;
    public CameraShake cameraToShake;

    public Vector3 person1Start;
    public Vector3 person1End;
    public Vector3 person2Start;
    public Vector3 person2End;

    public HackyCallback hackyCallback;
    public GameObject person1;
    public GameObject person2;

    public GameObject tutorialPlacement1;
    public GameObject tutorialPlacement2;
    public GameObject tutorialPlacement3;
    public GameObject tutorialPlacement4;
    public GameObject doorHP;

    public GameGrid gridToSetup;

    public AudioSource sneakyShipMusic;
    public AudioSource blastingTHroughWallsMusic;


    internal override void Unblock()
    {
        Debug.Log(stepNum++);
        switch (stepNum)
        {
            case 1:  // Finish first narration, characters walk in.
                timeSinceStepStarted = 0f;
                ship.transform.localPosition = new Vector3(-58, 5.67f, 15.38f); //DON'T LOOK! MAGIC NUMBERS!
                ship.transform.localRotation = Quaternion.Euler(new Vector3(-180, -90, 121.93f));
                turnOnLightsSound.Play();
                //Turn on lights
                //Tiny figures walk up to ship.
                break;
            case 2:  // Characters have finished walking in.
                narrations[1].gameObject.SetActive(true);
                break;
            case 3:  // Narration fnishes, characters open up ship, next narration plays.
                openShipSound.Play();
                narrations[2].gameObject.SetActive(true);
                break;
            case 4:  //Narration finishes, ships takes off, next narration plays.
                startShipSound.Play();
                shipToMakeNotWiggle.takeoff(5, ship.transform.position, ship.transform.rotation);
                shipToMakeNotWiggle.enabled = true;
                shipToMakeNotWiggle.SetHomePosition(new Vector3(-8, 6.25f, 15f), Quaternion.Euler(new Vector3(-180, -90, 120f)));
                person1.gameObject.SetActive(false);
                person2.gameObject.SetActive(false);
                narrations[3].gameObject.SetActive(true);
                break;
            case 5: // Narration finishes, board is setup and moves into view, next narration plays.
                List<int[,]> piecesToForce = new List<int[,]>();
                piecesToForce.Add(new int[3,3] { { 0,1,0},{ 0,1,1},{ 0,0,0} });
                piecesToForce.Add(new int[3, 3] { { 0, 1, 0 }, { 1, 1, 0 }, { 0, 0, 0 } });
                piecesToForce.Add(new int[3, 3] { { 1, 1, 0 }, { 1, 1, 0 }, { 1, 1, 0 } });
                piecesToForce.Add(new int[3, 3] { { 0, 1, 0 }, { 0, 1, 0 }, { 0, 1, 0 } });

                gridToSetup.ForcePieces(piecesToForce);

                gridToSetup.DropNewCubeAt(5, 5);
                gridToSetup.DropNewCubeAt(5, 6);
                gridToSetup.DropNewCubeAt(5, 7);
                gridToSetup.DropNewCubeAt(4, 6);
                gridToSetup.DropNewCubeAt(4, 7);
                gridToSetup.DropNewCubeAt(3, 7);

                gridToSetup.DropNewCubeAt(9, 5);
                gridToSetup.DropNewCubeAt(9, 6);
                gridToSetup.DropNewCubeAt(9, 7);
                gridToSetup.DropNewCubeAt(10, 6);
                gridToSetup.DropNewCubeAt(10, 7);
                gridToSetup.DropNewCubeAt(11, 7);

                gridToSetup.DropNewCubeAt(4, 8);
                gridToSetup.DropNewCubeAt(4, 9);
                gridToSetup.DropNewCubeAt(4, 10);
                gridToSetup.DropNewCubeAt(10, 8);
                gridToSetup.DropNewCubeAt(10, 9);
                gridToSetup.DropNewCubeAt(10, 10);

                gridToSetup.DropNewCubeAt(4, 11);
                gridToSetup.DropNewCubeAt(4, 12);
                gridToSetup.DropNewCubeAt(4, 13);
                gridToSetup.DropNewCubeAt(4, 14);
                gridToSetup.DropNewCubeAt(5, 11);
                gridToSetup.DropNewCubeAt(5, 12);
                gridToSetup.DropNewCubeAt(5, 13);
                gridToSetup.DropNewCubeAt(5, 14);
                gridToSetup.DropNewCubeAt(6, 13);
                gridToSetup.DropNewCubeAt(6, 14);
                gridToSetup.DropNewCubeAt(7, 13);
                gridToSetup.DropNewCubeAt(7, 14);
                gridToSetup.DropNewCubeAt(8, 13);
                gridToSetup.DropNewCubeAt(8, 14);
                gridToSetup.DropNewCubeAt(9, 11);
                gridToSetup.DropNewCubeAt(9, 12);
                gridToSetup.DropNewCubeAt(9, 13);
                gridToSetup.DropNewCubeAt(9, 14);
                gridToSetup.DropNewCubeAt(10, 11);
                gridToSetup.DropNewCubeAt(10, 12);
                gridToSetup.DropNewCubeAt(10, 13);
                gridToSetup.DropNewCubeAt(10, 14);

                gridToSetup.DropNewCubeAt(6, 15);
                gridToSetup.DropNewCubeAt(6, 16);
                gridToSetup.DropNewCubeAt(6, 17);
                gridToSetup.DropNewCubeAt(8, 15);
                gridToSetup.DropNewCubeAt(8, 16);
                gridToSetup.DropNewCubeAt(8, 17);
                gridToSetup.DropNewCubeAt(9, 17);
                gridToSetup.DropNewCubeAt(5, 17);

                gridToSetup.player.energy = 35;

                gridToSetup.SetGridCellTypeStateAndAttendentVFX();

                timeSinceStepStarted = 0f;
                narrations[4].gameObject.SetActive(true);
                break;
            case 6: //narration finishes, game starts, "How to move and drop" tutorial.
                MissionManager.isInCutscene = false;
                sneakyShipMusic.Play();
                MissionManager.triggerCallbacksOnBlockDrop = true;
                tutorialPlacement1.gameObject.SetActive(true);
                List<Vector2> placements6 = new List<Vector2>(new Vector2[1] { new Vector2(11, 5) });
                List<int> rotations6 = new List<int>(new int[1] { 0 });
                gridToSetup.AddForcedPosition(rotations6, placements6);
                gridToSetup.player.bulletFlightTime = 1;
                shipToMakeNotWiggle.distanceBetweenShips = 9;
                tutorialTexts[0].gameObject.SetActive(true);
                break;
            case 7: //first block placed, "How to rotate" tutorial plays.
                tutorialPlacement1.gameObject.SetActive(false);
                tutorialPlacement2.gameObject.SetActive(true);
                List<Vector2> placements7 = new List<Vector2>(new Vector2[1] { new Vector2(3, 5) });
                List<int> rotations7 = new List<int>(new int[1] { 2 });
                gridToSetup.AddForcedPosition(rotations7, placements7);
                tutorialTexts[0].gameObject.SetActive(false);
                tutorialTexts[1].gameObject.SetActive(true);
                break;
            case 8: //Second block dropped, "Make big squares" tutorial plays.
                tutorialPlacement2.gameObject.SetActive(false);
                tutorialPlacement3.gameObject.SetActive(true);
                List<Vector2> placements8 = new List<Vector2>(new Vector2[2] { new Vector2(7, 12), new Vector2(7,11 )});
                List<int> rotations8 = new List<int>(new int[2] { 0,2 });
                gridToSetup.AddForcedPosition(rotations8, placements8);
                tutorialTexts[1].gameObject.SetActive(false);
                tutorialTexts[2].gameObject.SetActive(true);
                break;
            case 9: //Third block dropped, "Combo Multiplier" tutorial plays.
                thingsToHide[2].SetActive(true);
                tutorialPlacement3.gameObject.SetActive(false);
                tutorialPlacement4.gameObject.SetActive(true);
                List<Vector2> placements9 = new List<Vector2>(new Vector2[2] { new Vector2(7, 16), new Vector2(7, 16) });
                List<int> rotations9 = new List<int>(new int[2] { 1, 3 });
                gridToSetup.AddForcedPosition(rotations9, placements9);
                tutorialTexts[2].gameObject.SetActive(false);
                tutorialTexts[3].gameObject.SetActive(true);
                break;
            case 10: // Fourth block dropped, cutscene starts.
                MissionManager.isInCutscene = true;
                tutorialPlacement4.gameObject.SetActive(false);
                narrations[5].gameObject.SetActive(true);
                tutorialTexts[3].gameObject.SetActive(false);
                break;
            case 11: //Dialogue finished, attack spots appear.
                gridToSetup.player.GetCharacterSheet().WeaponEquippedID = 1;
                MissionManager.isInCutscene = false;
                gridToSetup.SetGridCellTypeStateAndAttendentVFX();
                MissionManager.triggerCallbacksOnBlockDrop = false;
                MissionManager.triggerCallbacksOnAttackHit = true;
                tutorialTexts[4].gameObject.SetActive(true);
                gridToSetup.player.enemy.health = doorHPNum;
                break;
            case 12: //Wall impacted. Stuff becomes intense. Cutscene starts.
                MissionManager.isInCutscene = true;
                klaxonToTurnOn.TurnOn();
                wallImpactExplosionSound.Play();
                cameraToShake.ShakeCamera(3, 1);
                sneakyShipMusic.Stop();
                blastingTHroughWallsMusic.Play();
                narrations[6].gameObject.SetActive(true);
                MissionManager.triggerCallbacksOnAttackHit = false;
                tutorialTexts[4].gameObject.SetActive(false);
                MissionManager.TriggerCallbackOnShipDestroyed = true;
                break;
            case 13: //Cutscene ends.
                MissionManager.isInCutscene = false;
                MissionManager.triggerCallbacksOnAttackHit = true;
                doorHP.transform.GetChild(0).gameObject.GetComponent<Text>().text = "DOOR HP: " + (int)(gridToSetup.player.enemy.health / doorHPNum*100)+"%";
                doorHP.SetActive(true);
                break;
            case 14:
                wallImpactExplosionSound.Play();
                cameraToShake.ShakeCamera(3, 1);
                doorHP.transform.GetChild(0).gameObject.GetComponent<Text>().text = "DOOR HP: " + (int)(gridToSetup.player.enemy.health / doorHPNum * 100) + "%";
                if (gridToSetup.player.enemy.IsAlive())
                {
                    stepNum--;
                }
                else
                {
                    doorHP.SetActive(false);
                    MissionManager.isInCutscene = true;
                    narrations[7].gameObject.SetActive(true);
                }
                break;
            case 15: // Cutscene ends, door is gone. Fly fly away!
                timeSinceStepStarted = 0f;
                escapeParticles.SetActive(true);
                break;
        }
}

    // Use this for initialization
    void Start ()
    {
        narrations[0].gameObject.SetActive(true);
        foreach (GameObject go in thingsToHide)
        {
            go.SetActive(false);
        }
        cameraToDisable.enabled = false;
        cameraToMove.localPosition = toMoveCameraTo;
        ship = shipToMakeNotWiggle.gameObject;
        spaceLightToDisable.enabled = false;
        shipToMakeNotWiggle.enabled = false;
        gridToSetup.player.enemy.damageManager = damageManagerForDoor;

        MissionManager.isInCutscene = true;
    }

    private bool playedRunningSound;
    public DamageManager damageManagerForDoor;
    private float doorHPNum = 30;
    public GameObject structure;
    private float shipAccelleration = .5f;
    public  GameObject escapeParticles;

    private bool firstRun = true;
    // Update is called once per frame
    void Update()
    {
        if (firstRun)
        {
            gridToSetup.player.SetCharacterSheet(0);
            firstRun = false;
        }
        //Run in.
        if (stepNum == 1)
        {
            timeSinceStepStarted += Time.deltaTime;
            float brightness = Mathf.Clamp01(timeSinceStepStarted / 2);
            darkness.color = new Color(0, 0, 0, 1 - brightness);
            float personPosit1time = Mathf.Clamp01((timeSinceStepStarted - 2f) / 2f);
            float personPosit2time = Mathf.Clamp01((timeSinceStepStarted - 2.2f) / 2f);
            Vector3 personPosit1 = Vector3.Lerp(person1Start, person1End, personPosit1time);
            Vector3 personPosit2 = Vector3.Lerp(person2Start, person2End, personPosit2time);
            person1.transform.localPosition = personPosit1;
            person2.transform.localPosition = personPosit2;
            if (timeSinceStepStarted > 2 && !playedRunningSound)
            {
                runningSound.Play();
                playedRunningSound = true;
            }

            if (timeSinceStepStarted > 4.2)
            {
                hackyCallback.enabled = true;
            }
            //firesToTurnOff.gameObject.SetActive(false);
        }

        //Board slides up

        if (stepNum == 5)
        {
            timeSinceStepStarted += Time.deltaTime;
            float t = Mathf.Cos(Mathf.Clamp01(timeSinceStepStarted / 2) * Mathf.PI);
            cameraToMove.localPosition = Vector3.Lerp(Vector3.zero, toMoveCameraTo, t);
        }

        if (stepNum == 15)
        {
            timeSinceStepStarted += Time.deltaTime;
            structure.transform.localPosition = new Vector3(.1f-(timeSinceStepStarted*timeSinceStepStarted*shipAccelleration), 24.77f, -14.93f);
        }
    }
}
