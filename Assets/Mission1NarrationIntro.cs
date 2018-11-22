using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Narrate;
using UnityEngine.UI;

public class Mission1NarrationIntro : Mission
{
    public TimerNarrationTrigger[] narrations;
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
                timeSinceStepStarted = 0f;
                narrations[4].gameObject.SetActive(true);
                break;
            case 6:
                //narrations[3].gameObject.SetActive(true);
                break;
        //Game starts.
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
    }

    private bool playedRunningSound;

	// Update is called once per frame
	void Update ()
    {

        //Run in.
        if (stepNum == 1)
        {
            timeSinceStepStarted += Time.deltaTime;
            float brightness = Mathf.Clamp01(timeSinceStepStarted/2);
            Debug.Log(brightness);
            darkness.color = new Color(0, 0, 0, 1-brightness);
            float personPosit1time = Mathf.Clamp01((timeSinceStepStarted-2f) / 2f);
            float personPosit2time = Mathf.Clamp01((timeSinceStepStarted - 2.2f) / 2f);
            Vector3 personPosit1 = Vector3.Lerp(person1Start, person1End, personPosit1time);
            Vector3 personPosit2 = Vector3.Lerp(person2Start, person2End, personPosit2time);
            Debug.Log(personPosit1);
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
