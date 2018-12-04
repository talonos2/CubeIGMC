using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Narrate;
using UnityEngine.UI;

public class Mission1NarrationIntro : Mission
{
    public TimerNarrationTrigger[] narrations;
    public GameObject[] tutorialTexts;

    private GameObject[] thingsToHide;

    public Vector3 toMoveCameraTo = new Vector3(25,0,0);
    public TextAsset aiThatDoesNothing;

    public AudioSource turnOnLightsSound;
    public AudioSource runningSound;
    public AudioSource openShipSound;
    public AudioSource startShipSound;
    public AudioSource alarmSound;
    public AudioSource wallImpactExplosionSound;

    public KlaxonSpin klaxonToTurnOn;

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
    
    public DamageManager damageManagerForDoor;
    public float doorHPNum = 30;
    public GameObject structure;
    public float shipAccelleration = .5f;

    private GameObject escapeParticles;

    private int stepNum;
    private float timeSinceStepStarted;
    private bool firstRun = true;
    private bool victoryScreen;
    private bool playedRunningSound;


    internal override void Unblock()
    {
        stepNum++;
        switch (stepNum)
        {
            case 1:  // Finish first narration, characters walk in.
                timeSinceStepStarted = 0f;
                pointers.ship1.gameObject.transform.localPosition = new Vector3(-58, 5.67f, 15.38f); //DON'T LOOK! MAGIC NUMBERS!
                pointers.ship1.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(-180, -90, 121.93f));
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
                pointers.ship1.takeoff(5, pointers.ship1.transform.position, pointers.ship1.transform.rotation);
                pointers.ship1.enabled = true;
                pointers.ship1.SetHomePosition(new Vector3(-8, 6.25f, 15f), Quaternion.Euler(new Vector3(-180, -90, 120f)));
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

                GameGrid gridToSetup = pointers.player1Grid;

                gridToSetup.ForcePieces(piecesToForce);

                //This is ugly, and we can do better.

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
                MissionManager.freezePlayerBoard = false;
                MusicManager.instance.music[MusicManager.SKEAKY_SHIP].Play();
                MissionManager.triggerCallbacksOnBlockDrop = true;
                tutorialPlacement1.gameObject.SetActive(true);
                List<Vector2> placements6 = new List<Vector2>(new Vector2[1] { new Vector2(11, 5) });
                List<int> rotations6 = new List<int>(new int[1] { 0 });
                pointers.player1Grid.AddForcedPosition(rotations6, placements6);
                pointers.player1Grid.player.bulletFlightTime = 1;
                pointers.ship1.distanceBetweenShips = 9;
                tutorialTexts[0].gameObject.SetActive(true);
                break;
            case 7: //first block placed, "How to rotate" tutorial plays.
                tutorialPlacement1.gameObject.SetActive(false);
                tutorialPlacement2.gameObject.SetActive(true);
                List<Vector2> placements7 = new List<Vector2>(new Vector2[1] { new Vector2(3, 5) });
                List<int> rotations7 = new List<int>(new int[1] { 2 });
                pointers.player1Grid.AddForcedPosition(rotations7, placements7);
                tutorialTexts[0].gameObject.SetActive(false);
                tutorialTexts[1].gameObject.SetActive(true);
                break;
            case 8: //Second block dropped, "Make big squares" tutorial plays.
                tutorialPlacement2.gameObject.SetActive(false);
                tutorialPlacement3.gameObject.SetActive(true);
                List<Vector2> placements8 = new List<Vector2>(new Vector2[2] { new Vector2(7, 12), new Vector2(7,11 )});
                List<int> rotations8 = new List<int>(new int[2] { 0,2 });
                pointers.player1Grid.AddForcedPosition(rotations8, placements8);
                tutorialTexts[1].gameObject.SetActive(false);
                tutorialTexts[2].gameObject.SetActive(true);
                break;
            case 9: //Third block dropped, "Combo Multiplier" tutorial plays.
                thingsToHide[2].SetActive(true);
                tutorialPlacement3.gameObject.SetActive(false);
                tutorialPlacement4.gameObject.SetActive(true);
                List<Vector2> placements9 = new List<Vector2>(new Vector2[2] { new Vector2(7, 16), new Vector2(7, 16) });
                List<int> rotations9 = new List<int>(new int[2] { 1, 3 });
                pointers.player1Grid.AddForcedPosition(rotations9, placements9);
                tutorialTexts[2].gameObject.SetActive(false);
                tutorialTexts[3].gameObject.SetActive(true);
                break;
            case 10: // Fourth block dropped, cutscene starts.
                MissionManager.freezePlayerBoard = true;
                tutorialPlacement4.gameObject.SetActive(false);
                narrations[5].gameObject.SetActive(true);
                tutorialTexts[3].gameObject.SetActive(false);
                break;
            case 11: //Dialogue finished, attack spots appear.
                pointers.player1Grid.player.GetCharacterSheet().WeaponEquippedID = 1;
                MissionManager.freezePlayerBoard = false;
                pointers.player1Grid.SetGridCellTypeStateAndAttendentVFX();
                MissionManager.triggerCallbacksOnBlockDrop = false;
                MissionManager.triggerCallbacksOnAttackHit = true;
                tutorialTexts[4].gameObject.SetActive(true);
                pointers.player1Grid.player.enemy.health = doorHPNum;
                break;
            case 12: //Wall impacted. Stuff becomes intense. Cutscene starts.
                MissionManager.freezePlayerBoard = true;
                klaxonToTurnOn.TurnOn();
                wallImpactExplosionSound.Play();
                pointers.spaceCameraShaker.ShakeCamera(3, 1);
                MusicManager.instance.StopAllMusic();
                MusicManager.instance.music[MusicManager.BLASTING_THROUGH_WALLS].Play();
                narrations[6].gameObject.SetActive(true);
                MissionManager.triggerCallbacksOnAttackHit = false;
                tutorialTexts[4].gameObject.SetActive(false);
                MissionManager.triggerCallbackOnShipDestroyed = true;
                break;
            case 13: //Cutscene ends.
                MissionManager.freezePlayerBoard = false;
                MissionManager.triggerCallbacksOnAttackHit = true;
                doorHP.transform.GetChild(0).gameObject.GetComponent<Text>().text = "DOOR HP: " + (int)(pointers.combatant2.health / doorHPNum*100)+"%";
                doorHP.SetActive(true);
                break;
            case 14:
                wallImpactExplosionSound.Play();
                pointers.spaceCameraShaker.ShakeCamera(3, 1);
                doorHP.transform.GetChild(0).gameObject.GetComponent<Text>().text = "DOOR HP: " + (int)(pointers.combatant2.health / doorHPNum * 100) + "%";
                if (pointers.combatant2.IsAlive())
                {
                    stepNum--;
                }
                else
                {
                    doorHP.SetActive(false);
                    MissionManager.freezePlayerBoard = true;
                    narrations[7].gameObject.SetActive(true);
                    MissionManager.triggerCallbacksOnAttackHit = false;
                }
                break;
            case 15: // Cutscene ends, door is gone. Fly fly away!
                timeSinceStepStarted = 0f;
                escapeParticles.SetActive(true);
                MissionManager.triggerCallbacksOnAttackHit = false;
                break;
        }
}

    // Use this for initialization
    void OnEnable ()
    {
        pointers = MissionManager.instance.pointers;
        thingsToHide = new GameObject[3];
        thingsToHide[0] = pointers.combatant1.healthBar.gameObject;
        thingsToHide[1] = pointers.ship2.gameObject;
        thingsToHide[2] = pointers.combatant1.multiplierText.gameObject;
        escapeParticles = pointers.ship1.engineParticles;
        pointers.restartButton1.gameObject.SetActive(true);
        pointers.restartButton2.gameObject.SetActive(true);

        narrations[0].gameObject.SetActive(true);
        foreach (GameObject go in thingsToHide)
        {
            go.SetActive(false);
        }
        pointers.camera2.enabled = false;
        pointers.camera1.transform.localPosition = toMoveCameraTo;
        pointers.spaceLight.enabled = false;
        pointers.ship1.enabled = false;
        pointers.player1Grid.player.enemy.damageManager = damageManagerForDoor;

        MissionManager.freezePlayerBoard = true;
    }

    void Update()
    {
        if (firstRun)
        {
            pointers.player1Grid.player.SetCharacterSheet(0);
            firstRun = false;
        }
        //Run in.
        if (stepNum == 1)
        {
            timeSinceStepStarted += Time.deltaTime;
            float brightness = Mathf.Clamp01(timeSinceStepStarted / 2);
            pointers.daaaaaknesssss.color = new Color(0, 0, 0, 1 - brightness);
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
        }

        //Board slides up

        if (stepNum == 5)
        {
            timeSinceStepStarted += Time.deltaTime;
            float t = Mathf.Cos(Mathf.Clamp01(timeSinceStepStarted / 2) * Mathf.PI);
            pointers.camera1.transform.localPosition = Vector3.Lerp(Vector3.zero, toMoveCameraTo, t);
        }

        if (stepNum == 15)
        {
            timeSinceStepStarted += Time.deltaTime;
            structure.transform.localPosition = new Vector3(.1f-(timeSinceStepStarted*timeSinceStepStarted*shipAccelleration), 24.77f, -14.93f);
            if (timeSinceStepStarted > 1.5 && !victoryScreen)
            {
                Win(false);
                victoryScreen = true;
            }
        }
    }

    internal override AIParams GetAIParams()
    {
        return new AIParams(aiThatDoesNothing.text, false, 0, false);
    }

    internal override EngineRoomGameType GameType()
    {
        return EngineRoomGameType.SINGLE_PLAYER;
    }
}
