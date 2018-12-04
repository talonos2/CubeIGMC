using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Narrate;

public class CommonMissionScriptingTargets : MonoBehaviour
{
    public GameGrid player1Grid;
    public GameGrid player2Grid;
    public SpaceshipPawn ship1;
    public SpaceshipPawn ship2;
    public Camera camera1;
    public Camera camera2;
    public Camera spaceCamera;
    public CameraShake cameraShaker1;
    public CameraShake cameraShaker2;
    public CameraShake spaceCameraShaker;
    public Combatant combatant1;
    public Combatant combatant2;
    public Image daaaaaknesssss;
    public Light spaceLight;
    public NarrationManager narrationSystem;
    public GameObject cameraWrapper1;
    public GameObject cameraWrapper2;
    public Button restartButton1;
    public Button restartButton2;

    public Image levelFinishedImage;
    public Sprite campaignVictory;
    public Sprite campaignDefeat;

    public Button shopButton;
    public Button nextMissionButton;
    public Text replayMissionText;

    public GameObject singlePlayerVictoryOrDefeatSprite;

    public AudioSource gameStartTickSound;
    public GameObject missionFinishedScreen;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
