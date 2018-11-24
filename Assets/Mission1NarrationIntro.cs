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

    public ParticleSystem tutorialPlacement1;
    public ParticleSystem tutorialPlacement2;
    public ParticleSystem tutorialPlacement3;

    public GameGrid gridToSetup;

    public AudioSource sneakyShipMusic;
    public AudioSource blastingTHroughWallsMusic;


    internal override void Unblock()
    {
        stepNum++;
        switch (stepNum)
        {
            case 1:
                timeSinceStepStarted = 0f;
                turnOnLightsSound.Play();
                //Turn on lights
                //Tiny figures walk up to ship.
                break;
            case 2:
                narrations[1].gameObject.SetActive(true);
                break;
            case 3:
                openShipSound.Play();
                narrations[2].gameObject.SetActive(true);
                break;
            case 4:
                startShipSound.Play();
                shipToMakeNotWiggle.enabled = true;
                shipToMakeNotWiggle.takeoff(5, ship.transform.position, ship.transform.rotation);
                narrations[3].gameObject.SetActive(true);
                break;
            case 5:
                //Setup board.
                List<int[,]> piecesToForce = new List<int[,]>();
                piecesToForce.Add(new int[3,3] { { 0,1,0},{ 0,1,1},{ 0,0,0} });
                piecesToForce.Add(new int[3, 3] { { 0, 1, 0 }, { 1, 1, 0 }, { 0, 0, 0 } });
                piecesToForce.Add(new int[3, 3] { { 1, 1, 0 }, { 1, 1, 0 }, { 1, 1, 0 } });

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

                gridToSetup.player.howManyItemsIHave = 0;
                gridToSetup.SetGridCellTypeStateAndAttendentVFX();

                timeSinceStepStarted = 0f;
                narrations[4].gameObject.SetActive(true);
                break;
            case 6:
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
            case 7:
                tutorialPlacement1.gameObject.SetActive(false);
                tutorialPlacement2.gameObject.SetActive(true);
                List<Vector2> placements7 = new List<Vector2>(new Vector2[1] { new Vector2(3, 5) });
                List<int> rotations7 = new List<int>(new int[1] { 2 });
                gridToSetup.AddForcedPosition(rotations7, placements7);
                tutorialTexts[0].gameObject.SetActive(false);
                tutorialTexts[1].gameObject.SetActive(true);
                break;
            case 8:
                tutorialPlacement2.gameObject.SetActive(false);
                tutorialPlacement3.gameObject.SetActive(true);
                List<Vector2> placements8 = new List<Vector2>(new Vector2[2] { new Vector2(7, 12), new Vector2(7,11 )});
                List<int> rotations8 = new List<int>(new int[2] { 0,2 });
                gridToSetup.AddForcedPosition(rotations8, placements8);
                tutorialTexts[1].gameObject.SetActive(false);
                tutorialTexts[2].gameObject.SetActive(true);
                break;
            case 9:
                MissionManager.isInCutscene = true;
                tutorialPlacement3.gameObject.SetActive(false);
                narrations[5].gameObject.SetActive(true);
                tutorialTexts[2].gameObject.SetActive(false);
                break;
            case 10:
                MissionManager.isInCutscene = false;
                gridToSetup.player.howManyItemsIHave = 1;
                gridToSetup.SetGridCellTypeStateAndAttendentVFX();
                MissionManager.triggerCallbacksOnBlockDrop = false;
                MissionManager.triggerCallbacksOnAttackHit = true;
                tutorialTexts[3].gameObject.SetActive(true);
                break;
            case 11:
                MissionManager.isInCutscene = true;
                klaxonToTurnOn.TurnOn();
                alarmSound.Play();
                wallImpactExplosionSound.Play();
                cameraToShake.ShakeCamera(3, 1);
                sneakyShipMusic.Stop();
                blastingTHroughWallsMusic.Play();
                narrations[6].gameObject.SetActive(true);
                MissionManager.triggerCallbacksOnAttackHit = false;
                tutorialTexts[3].gameObject.SetActive(false);
                break;
            case 12:
                MissionManager.isInCutscene = false;
                MissionManager.triggerCallbacksOnAttackHit = true;
                tutorialTexts[4].gameObject.SetActive(true);
                break;
            case 13:
                wallImpactExplosionSound.Play();
                cameraToShake.ShakeCamera(3, 1);
                tutorialTexts[5].gameObject.SetActive(true);
                tutorialTexts[4].gameObject.SetActive(false);
                thingsToHide[2].SetActive(true);
                break;
            case 14:
                wallImpactExplosionSound.Play();
                cameraToShake.ShakeCamera(3, 1);
                tutorialTexts[5].gameObject.SetActive(false);
                break;
            case 15:
                wallImpactExplosionSound.Play();
                cameraToShake.ShakeCamera(3, 1);
                if (gridToSetup.player.enemy.IsAlive())
                {
                    stepNum--;
                }
                else
                {
                    MissionManager.isInCutscene = true;
                    narrations[7].gameObject.SetActive(true);
                }
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

        gridToSetup.player.enemy.howManyItemsIHave = -1;
        gridToSetup.player.enemy.health = 50;
        gridToSetup.player.energy = 39;
        gridToSetup.player.enemy.damageManager = damageManagerForDoor;

        MissionManager.isInCutscene = true;
    }

    private bool playedRunningSound;
    public DamageManager damageManagerForDoor;

    // Update is called once per frame
    void Update ()
    {

        //Run in.
        if (stepNum == 1)
        {
            timeSinceStepStarted += Time.deltaTime;
            float brightness = Mathf.Clamp01(timeSinceStepStarted/2);
            darkness.color = new Color(0, 0, 0, 1-brightness);
            float personPosit1time = Mathf.Clamp01((timeSinceStepStarted-2f) / 2f);
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
            float t = Mathf.Cos(Mathf.Clamp01(timeSinceStepStarted / 2)*Mathf.PI);
            cameraToMove.localPosition = Vector3.Lerp(Vector3.zero, toMoveCameraTo, t);
        }
    }
}
