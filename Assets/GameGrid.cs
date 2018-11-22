﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public bool isRecording = false;
    
    public static Vector2Int numCells = new Vector2Int(15, 18);
    public GameObject strangeFrontPlateThing;
    public AudioSource ominousTick;
    public AudioSource powerDown;
    public GameCube[,] grid = new GameCube[numCells.x, numCells.y];
    public CellType[,] cellTypes = new CellType[numCells.x, numCells.y];
    public TileFX[,] cellTypeFX = new TileFX[numCells.x, numCells.y];
    public PlayingPiece piecePrefab;
    public PowerupEffect powerUpEffect;
    public TextAsset aIText;
    public CubeConversionManager cubeConversionManager;
    private AIPlayer aIPlayer = new AIPlayer();

    public GameObject attackCellPrefab;
    public GameObject shieldCellPrefab;
    public GameObject psiCellPrefab;
    public ComboParticleHolder comboParticleHolder;

    int[][][,] pieceArray = new int[10][][,];

    public AudioSource dropSound;
    public AudioSource matchSound;

    public GameObject pauseMenu;
    bool pause = false;

    public bool justExitedMenu;

    internal void LoadAI()
    {
        string inputJson = aIText.text;
        aIPlayer = JsonUtility.FromJson<AIPlayer>(inputJson);
    }

    internal int GetAISeed()
    {
        if (!isPlayedByAI)
        {
            Debug.LogError("Warning: Request for the seed of an AI that shouldn't exist!");
        }
        return aIPlayer.seed;
    }

    private PlayingPiece currentPiece;
    private PlayingPiece nextPiece;

    private SeededRandom dice;
    private GameRecorder recorder;

    internal void SetSeedAndStart(int randomSeed)
    {
        if (isRecording)
        {
            recorder = new GameRecorder(randomSeed);
        }

        dice = new SeededRandom(randomSeed);

        SetupPieceArray();

        currentPiece = MakeAPiece();
        currentPiece.transform.parent = this.transform;
        UpdateCurrentPieceTransform();

        nextPieceHolder = transform.Find("PieceHolder");
        nextPiece = MakeAPiece();
        nextPiece.transform.parent = nextPieceHolder;
        nextPiece.transform.localPosition = Vector3.zero;

        SetGridCellTypeStateAndAttendentVFX();
    }

    public void SetGridCellTypeStateAndAttendentVFX()
    {
        for (int x = 0; x < numCells.x; x++)
        {
            for (int y = 0; y < numCells.y; y++)
            {
                cellTypes[x, y] = CellType.NORMAL;
             }
        }
                player.SetGridcellsStartingState(cellTypes);

        for (int x = 0; x < numCells.x; x++)
        {
            for (int y = 0; y < numCells.y; y++)
            {
                if (cellTypeFX[x,y] != null)
                {
                    GameObject.Destroy(cellTypeFX[x, y].gameObject);
                    cellTypeFX[x, y] = null;
        }
                switch (cellTypes[x, y])
                {
                    case CellType.ATTACK:
                        cellTypeFX[x, y] = GameObject.Instantiate(attackCellPrefab).GetComponent<TileFX>();
                        cellTypeFX[x, y].transform.parent = this.transform;
                        cellTypeFX[x, y].transform.localPosition = GetLocalTranslationFromGridLocation(x, y);
                        player.AddDeathEffectToDamageManager(cellTypeFX[x, y]);
                        break;
                    case CellType.SHIELD:
                        cellTypeFX[x, y] = GameObject.Instantiate(shieldCellPrefab).GetComponent<TileFX>();
                        cellTypeFX[x, y].transform.parent = this.transform;
                        cellTypeFX[x, y].transform.localPosition = GetLocalTranslationFromGridLocation(x, y);
                        player.AddDeathEffectToDamageManager(cellTypeFX[x, y]);
                        break;
                    case CellType.PSI:
                        cellTypeFX[x, y] = GameObject.Instantiate(psiCellPrefab).GetComponent<TileFX>();
                        cellTypeFX[x, y].transform.parent = this.transform;
                        cellTypeFX[x, y].transform.localPosition = GetLocalTranslationFromGridLocation(x, y);
                        player.AddDeathEffectToDamageManager(cellTypeFX[x, y]);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public Combatant player;
    public bool isPlayerOne = true;

    internal void SetEnemy()
    {

    }

    private Vector2Int prevPiecePosition = new Vector2Int(1, 1);//(7, 16);
    private Vector2Int currentPiecePosition = new Vector2Int(1, 1);//(7, 16);

    private int currentPieceRotation = 0;
    private int prevPieceRotation = 0;

    private Transform nextPieceHolder;

    private float timeSinceLastMove = 0;
    private float timeSinceLastRot = 0;
    private readonly float msNeededToLerp = 62;

    private readonly Quaternion[] orientations = { Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 90, 0), Quaternion.Euler(0, 180, 0), Quaternion.Euler(0, 270, 0) };

    private readonly int[] pieceSizeBag = { 1, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 6, 6, 6, 6, 7, 7, 8 };

    private List<int[,]> forcedPieces = new List<int[,]>();

	// Use this for initialization
	void Start ()
    {

    }

    private PlayingPiece MakeAPiece()
    {
        PlayingPiece toReturn = GameObject.Instantiate(piecePrefab);

        int pieceSize = pieceSizeBag[dice.NextInt(0, pieceSizeBag.Length)];

        if (forcedPieces.Count > 0)
        {
            int[,] forcedPiece = forcedPieces[0];
            forcedPieces.RemoveAt(0);
            toReturn.Initialize(this.player, dice, forcedPiece);
        }
        else
        {
            toReturn.Initialize(this.player, dice, pieceArray[pieceSize][dice.NextInt(0, pieceArray[pieceSize].Length)]);
        }

        return toReturn;
    }

    public void ForcePieces(List <int[,]> forcedPieces)
    {
        this.forcedPieces = forcedPieces;
        PlayingPiece oldPiece1 = currentPiece;
        PlayingPiece oldPiece2 = nextPiece;
        currentPiece = MakeAPiece();
        nextPiece = MakeAPiece();
        GameObject.Destroy(oldPiece1.gameObject);
        GameObject.Destroy(oldPiece2.gameObject);

        currentPiece.transform.parent = this.transform;
        currentPiecePosition = new Vector2Int(1, 1);
        UpdateCurrentPieceTransform();

        nextPiece.transform.parent = nextPieceHolder;
        nextPiece.transform.localPosition = Vector3.zero;

        prevPieceRotation = currentPieceRotation = 0;
    }

    /*REPLACE: This moves the graphical representation of the piece.*/
    private void UpdateCurrentPieceTransform()
    {
        timeSinceLastMove += Time.deltaTime * 1000;
        timeSinceLastRot += Time.deltaTime * 1000;
        float prop = timeSinceLastMove / msNeededToLerp;
        Vector3 targetPosition = GetLocalTranslationFromGridLocation(currentPiecePosition.x, currentPiecePosition.y);
        Vector3 oldPosition = GetLocalTranslationFromGridLocation(prevPiecePosition.x, prevPiecePosition.y);
        Vector3 animatedPosition = Vector3.Lerp(oldPosition, targetPosition, prop);

        currentPiece.transform.localPosition = animatedPosition;

        prop = timeSinceLastRot / msNeededToLerp;

        Quaternion targetRotation = orientations[currentPieceRotation];
        Quaternion oldRotation = orientations[prevPieceRotation];
        Quaternion animatedRotation = Quaternion.Slerp(oldRotation, targetRotation, prop);

        currentPiece.transform.localRotation = animatedRotation;
    }

    private Vector3 GetLocalTranslationFromGridLocation(int x, int y)
    {
        return new Vector3(x - numCells.x / 2.0f + .5f, 0, y - numCells.y / 2.0f + .5f);
    }

    bool isUpBeingHeld;
    bool isDownBeingHeld;
    bool isLeftBeingHeld;
    bool isRightBeingHeld;

    float fastButtonMashSpeed = (1f / 8f);
    float buttonMashDebounceInput = .2f;

    float timeSinceLastMoveUpEvent;
    float timeSinceLastMoveDownEvent;
    float timeSinceLastMoveLeftEvent;
    float timeSinceLastMoveRightEvent;

    bool justPressedUp;
    bool justPressedDown;
    bool justPressedLeft;
    bool justPressedRight;

    private float timeHeldBothRotatesAtOnce;

    private bool hasSaved;

    public bool isPlayedByAI;
    public InvisibleDelayedChargeGiver chargeGiverPrefab;

    // Update is called once per frame
    void Update ()
    {
        if (Time.timeScale == 0)
        {
            justExitedMenu = true;
            return;
        }

        if (MissionManager.isInCutscene)
        {
            return;
        }
        justExitedMenu = false;

        if (Time.timeSinceLevelLoad > 300 & isRecording & !hasSaved)
        {
            recorder.PrintOut();
            hasSaved = true;
        }

        if (isPlayedByAI)
        {
            aIPlayer.TickAI();
        }

        if (isPlayerOne?(Input.GetButton("Rotate1_P1")&& Input.GetButton("Rotate2_P1")): (Input.GetButton("Rotate1_P2") && Input.GetButton("Rotate2_P2")))
        {
            float oldTimeHeld = timeHeldBothRotatesAtOnce;
            timeHeldBothRotatesAtOnce += Time.deltaTime;
            if ((int)timeHeldBothRotatesAtOnce != (int)oldTimeHeld)
            {
                ominousTick.Play();
            }
            if (timeHeldBothRotatesAtOnce > 5)
            {
                Reboot();
                timeHeldBothRotatesAtOnce = 0;
                if (isRecording) { recorder.RegisterEvent(GameRecorder.REBOOT); }
            }
        }
        else
        {
            timeHeldBothRotatesAtOnce = 0;
        }


        if (isPlayedByAI)
        {
            if (aIPlayer.GetButtonDown("UP")) { TryGoUp(); }
            if (aIPlayer.GetButtonDown("DOWN")) { TryGoDown(); }
            if (aIPlayer.GetButtonDown("LEFT")) { TryGoLeft(); }
            if (aIPlayer.GetButtonDown("RIGHT")) { TryGoRight(); }
            if (aIPlayer.GetButtonDown("REBOOT")) {Reboot(); }
        }
        else
        {
            HandleUpMovement();
            HandleDownMovement();
            HandleLeftMovement();
            HandleRightMovement();
        }

        if (isPlayerOne ? Input.GetButtonDown("Place_P1") && justExitedMenu == false  : (Input.GetButtonDown("Place_P2")|| aIPlayer.GetButtonDown("Place")) && justExitedMenu == false)
        {
            //Fallback: If there's somebohw someting directly underneath you, do not place. Should never happen in practice.
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (currentPiece.HasBlockAt(x, y) && IsInInvalidArea(currentPiecePosition.x + x - 1, currentPiecePosition.y + y - 1))
                    {
                        return;
                    }
                }
            }

            if (forcedPlacements.Count > 0)
            {
                bool inGoodPosition = false;
                ForcedPlacementOptions forced = forcedPlacements[0];
                for (int i = 0; i < forced.placements.Count; i++)
                {
                    Vector3 posit = forced.placements[i];
                    if (currentPieceRotation == forced.rotations[i] &&
                        currentPiecePosition.x == posit.x &&
                        currentPiecePosition.y == posit.y)
                    {
                        inGoodPosition = true;
                    }
                }
                if (!inGoodPosition)
                {
                    return;
                }
                forcedPlacements.RemoveAt(0);
            }

            if (isRecording) { recorder.RegisterEvent(GameRecorder.DROP); }
            DropPiece();
        }

        if (isPlayerOne ? Input.GetButtonDown("Rotate1_P1") : (Input.GetButtonDown("Rotate1_P2") || aIPlayer.GetButtonDown("Rotate1")))
        {
            bool[,] surroundings = new bool[3, 3];
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    surroundings[x, y] = IsObstructedAt(currentPiecePosition.x + x - 1, currentPiecePosition.y + y - 1);
                }
            }
            if (currentPiece.rotateCCW(surroundings))
            {
                prevPieceRotation = currentPieceRotation;
                currentPieceRotation = (currentPieceRotation + 5) % 4;
                timeSinceLastRot = 0f;
                currentPiece.PlaySlideSound();
                if (isRecording) { recorder.RegisterEvent(GameRecorder.CCW_ROTATE); }
            }
            else
            {
                //Make some sort of sound.
            }
        }

        if (isPlayerOne ? Input.GetButtonDown("Rotate2_P1") : (Input.GetButtonDown("Rotate2_P2") || aIPlayer.GetButtonDown("Rotate2")))
        {
            bool[,] surroundings = new bool[3, 3];
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    surroundings[x, y] = IsObstructedAt(currentPiecePosition.x + x - 1, currentPiecePosition.y + y - 1);
                }
            }
            if (currentPiece.RotateCW(surroundings))
            {
                prevPieceRotation = currentPieceRotation;
                currentPieceRotation = (currentPieceRotation + 3) % 4;
                timeSinceLastRot = 0f;
                currentPiece.PlaySlideSound();
                if (isRecording) { recorder.RegisterEvent(GameRecorder.CW_ROTATE); }
            }
            else
            {
                //Make some sort of sound.
            }
        }

        UpdateCurrentPieceTransform();

        for (int x = 0; x < numCells.x; x++)
        {
            for (int y = 0; y < numCells.y; y++)
            {
                if (grid[x, y] != null)
                {
                    Debug.DrawLine(new Vector3(this.transform.position.x + x - numCells.x / 2, 0, this.transform.position.y + y - numCells.y / 2), new Vector3(this.transform.position.x + x - numCells.x / 2, 4, this.transform.position.y + y - numCells.y / 2));
                }
            }
        }


    }

    private void Reboot()
    {
        powerDown.Play();
        MeltBoard();
        player.DeleteAllEnergy();
    }

    private void HandleUpMovement()
    {
        if ((isPlayerOne ? Input.GetAxis("Vertical_P1") > 0 : Input.GetAxis("Vertical_P2") > 0 || aIPlayer.getButtonPressed("Up")) && !isUpBeingHeld)
        {
            isUpBeingHeld = true;
            timeSinceLastMoveUpEvent = fastButtonMashSpeed * -buttonMashDebounceInput;
            justPressedUp = true;
        }
        if (isUpBeingHeld)
        {
            if (timeSinceLastMoveUpEvent > fastButtonMashSpeed || justPressedUp)
            {
                justPressedUp = false;
                TryGoUp();

                timeSinceLastMoveUpEvent %= fastButtonMashSpeed;
            }
            timeSinceLastMoveUpEvent += Time.deltaTime;
        }
        if (!(isPlayerOne ? Input.GetAxis("Vertical_P1") > 0 : Input.GetAxis("Vertical_P2") > 0 || aIPlayer.getButtonPressed("Up")))
        {
            isUpBeingHeld = false;
        }
    }

    private void TryGoUp()
    {
        bool isBlocked = false;
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (currentPiece.HasBlockAt(x, y) && IsObstructedAt(currentPiecePosition.x + x - 1, currentPiecePosition.y + y - 1 + 1))
                {
                    isBlocked = true;
                }
            }
        }
        if (!isBlocked)
        {
            prevPiecePosition = currentPiecePosition;
            currentPiecePosition = currentPiecePosition + new Vector2Int(0, 1);
            currentPiece.PlaySlideSound();
            if (isRecording) { recorder.RegisterEvent(GameRecorder.UP); }
            timeSinceLastMove = 0f;
        }
    }

    private void HandleDownMovement()
    {
        if ((isPlayerOne ? Input.GetAxis("Vertical_P1") < 0 : Input.GetAxis("Vertical_P2") < 0 || aIPlayer.getButtonPressed("Down")) && !isDownBeingHeld)
        {
            isDownBeingHeld = true;
            timeSinceLastMoveDownEvent = fastButtonMashSpeed * -buttonMashDebounceInput;
            justPressedDown = true;
        }
        if (isDownBeingHeld)
        {
            if (timeSinceLastMoveDownEvent > fastButtonMashSpeed || justPressedDown)
            {
                justPressedDown = false;
                TryGoDown();

                timeSinceLastMoveDownEvent %= fastButtonMashSpeed;
            }
            timeSinceLastMoveDownEvent += Time.deltaTime;
        }
        if (!(isPlayerOne ? Input.GetAxis("Vertical_P1") < 0 : Input.GetAxis("Vertical_P2") < 0 || aIPlayer.getButtonPressed("Down")))
        {
            isDownBeingHeld = false;
        }
    }

    private void TryGoDown()
    {
        bool isBlocked = false;
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (currentPiece.HasBlockAt(x, y) && IsObstructedAt(currentPiecePosition.x + x - 1, currentPiecePosition.y + y - 1 - 1))
                {
                    isBlocked = true;
                }
            }
        }
        if (!isBlocked)
        {
            prevPiecePosition = currentPiecePosition;
            currentPiecePosition = currentPiecePosition + new Vector2Int(0, -1);
            currentPiece.PlaySlideSound();
            if (isRecording) { recorder.RegisterEvent(GameRecorder.DOWN); }
            timeSinceLastMove = 0f;
        }
    }

    private void HandleLeftMovement()
    {
        if ((isPlayerOne ? Input.GetAxis("Horizontal_P1") < 0 : Input.GetAxis("Horizontal_P2") < 0 || aIPlayer.getButtonPressed("Left")) && !isLeftBeingHeld)
        {
            isLeftBeingHeld = true;
            timeSinceLastMoveLeftEvent = fastButtonMashSpeed * -buttonMashDebounceInput;
            justPressedLeft = true;
        }
        if (isLeftBeingHeld)
        {
            if (timeSinceLastMoveLeftEvent > fastButtonMashSpeed || justPressedLeft)
            {
                justPressedLeft = false;
                TryGoLeft();

                timeSinceLastMoveLeftEvent %= fastButtonMashSpeed;
            }
            timeSinceLastMoveLeftEvent += Time.deltaTime;
        }
        if (!(isPlayerOne ? Input.GetAxis("Horizontal_P1") < 0 : Input.GetAxis("Horizontal_P2") < 0 || aIPlayer.getButtonPressed("Left")))
        {
            isLeftBeingHeld = false;
        }
    }

    private void TryGoLeft()
    {
        bool isBlocked = false;
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (currentPiece.HasBlockAt(x, y) && IsObstructedAt(currentPiecePosition.x + x - 1 - 1, currentPiecePosition.y + y - 1))
                {
                    isBlocked = true;
                }
            }
        }
        if (!isBlocked)
        {
            prevPiecePosition = currentPiecePosition;
            currentPiecePosition = currentPiecePosition + new Vector2Int(-1, 0);
            if (isRecording) { recorder.RegisterEvent(GameRecorder.LEFT); }
            currentPiece.PlaySlideSound();
            timeSinceLastMove = 0f;
        }
    }

    private void HandleRightMovement()
    {
        if ((isPlayerOne ? Input.GetAxis("Horizontal_P1") > 0 : Input.GetAxis("Horizontal_P2") > 0 || aIPlayer.getButtonPressed("Right")) && !isRightBeingHeld)
        {
            isRightBeingHeld = true;
            timeSinceLastMoveRightEvent = fastButtonMashSpeed * -buttonMashDebounceInput;
            justPressedRight = true;
        }
        if (isRightBeingHeld)
        {
            if (timeSinceLastMoveRightEvent > fastButtonMashSpeed || justPressedRight)
            {
                justPressedRight = false;
                TryGoRight();

                timeSinceLastMoveRightEvent %= fastButtonMashSpeed;
            }
            timeSinceLastMoveRightEvent += Time.deltaTime;
        }
        if (!(isPlayerOne ? Input.GetAxis("Horizontal_P1") > 0 : Input.GetAxis("Horizontal_P2") > 0 || aIPlayer.getButtonPressed("Right")))
        {
            isRightBeingHeld = false;
        }
    }

    private void TryGoRight()
    {
        bool isBlocked = false;
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (currentPiece.HasBlockAt(x, y) && IsObstructedAt(currentPiecePosition.x + x - 1 + 1, currentPiecePosition.y + y - 1))
                {
                    isBlocked = true;
                }
            }
        }
        if (!isBlocked)
        {
            prevPiecePosition = currentPiecePosition;
            currentPiecePosition = currentPiecePosition + new Vector2Int(+1, 0);
            if (isRecording) { recorder.RegisterEvent(GameRecorder.RIGHT); }
            currentPiece.PlaySlideSound();
            timeSinceLastMove = 0f;
        }
    }

    private void DropPiece()
    {
        //Place the cubes from the piece to the board.
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (currentPiece.HasBlockAt(x,y))
                {
                    GameCube cube = currentPiece.GetCubeAt(x, y);
                    grid[currentPiecePosition.x+x-1, currentPiecePosition.y+y-1] = cube;
                    cube.transform.parent = this.transform;
                    cube.transform.localPosition = new Vector3(currentPiecePosition.x - numCells.x / 2f + x - 1+.5f, 0, currentPiecePosition.y - numCells.y / 2f + y - 1+.5f);
                }
            }
        }

        //Check for squares
        List<GameCube> cubesToExplode = new List<GameCube>();

        int numberOfSquaresMade = 0;

        for (int x = 0; x < numCells.x-2; x++)
        {
            for (int y = 0; y < numCells.y-2; y++)
            {
                if (IsCornerOfSquare(x, y))
                {
                    AddCubesFromSquareToList(x, y, cubesToExplode);
                    numberOfSquaresMade++;
                }
            }
        }

        int numberOfParticles = numberOfSquaresMade * 3;

        List<float> delays = new List<float>();
        for (int x = 0; x < numberOfParticles; x++)
        {
            float delay = UnityEngine.Random.Range(0, Mathf.Sqrt(numberOfSquaresMade/2));
            Debug.Log(numberOfSquaresMade + ", " + delay);
            delays.Add(delay);
        }

        delays.Sort();

        player.StartNewParticleBarrage();

        foreach (float delay in delays)
        {
            PowerupEffect pe = GameObject.Instantiate<PowerupEffect>(powerUpEffect);
            GameCube sourceCube = cubesToExplode[UnityEngine.Random.Range(0,cubesToExplode.Count)];
            pe.Initialize(sourceCube.transform.position, player.GetTargetOfParticle(PowerupType.ENERGY, 3), delay, PowerupType.ENERGY);
            InvisibleDelayedChargeGiver chargeGiver = GameObject.Instantiate<InvisibleDelayedChargeGiver>(chargeGiverPrefab);
            chargeGiver.target = player;
            chargeGiver.delay = delay + 1;
            chargeGiver.type = PowerupType.ENERGY;
            chargeGiver.SetAmountForOneCube(PowerupType.ENERGY);
            chargeGiver.amount /= 3; //(3 particles per square made);
        }

        if (numberOfSquaresMade != 0)
        {
            matchSound.Play();
            if (numberOfSquaresMade >= 2)
            {
                Vector3 centroid = FindCentroid(cubesToExplode);
                GameObject comboParticles = comboParticleHolder.comboParticles[numberOfSquaresMade].gameObject;
                GameObject go = GameObject.Instantiate(comboParticles);
                go.transform.position = centroid;
            }
        }
        else
        {
            dropSound.Play();
        }

        //This is where we handle explosions based on tile color.
        for (int x = 0; x < numCells.x; x++)
        {
            for (int y = 0; y < numCells.y; y++)
            {
                if (grid[x, y] != null && cubesToExplode.Contains(grid[x,y]))
                {
                    //A cube that has exploded is on a tile. What kind?
                    switch (cellTypes[x,y])
                    {
                        case CellType.ATTACK:
                            cubeConversionManager.QueueCube(grid[x, y], PowerupType.ATTACK);
                            cubesToExplode.Remove(grid[x, y]);
                            grid[x, y] = null;
                            break;
                        case CellType.SHIELD:
                            cubeConversionManager.QueueCube(grid[x, y], PowerupType.SHIELDS);
                            cubesToExplode.Remove(grid[x, y]);
                            grid[x, y] = null;
                            break;
                    }
                }
            }
        }

        foreach (GameCube cube in cubesToExplode)
        {
            RemoveCubeFromGrid(cube);
            cube.Sink(UnityEngine.Random.Range(0, Mathf.Sqrt(numberOfSquaresMade)));
        }

        Destroy(currentPiece.gameObject);

        currentPiece = nextPiece;
        currentPiece.transform.parent = this.transform;
        currentPiecePosition = new Vector2Int(1, 1);
        UpdateCurrentPieceTransform();

        nextPiece = MakeAPiece();
        nextPiece.transform.parent = nextPieceHolder;
        nextPiece.transform.localPosition = Vector3.zero;

        prevPieceRotation = currentPieceRotation = 0;

        //For tutorials; hack in a callback;
        if (MissionManager.triggerCallbacksOnBlockDrop)
        {
            MissionManager.instance.grossCallbackHack.enabled = true;
        }
    }

    private Vector3 FindCentroid(List<GameCube> cubesToExplode)
    {
        float x = 0;
        float y = 0;
        float z = 0;
        foreach (GameCube cube in cubesToExplode)
        {
            x += cube.transform.position.x;
            y += cube.transform.position.y;
            z += cube.transform.position.z;
        }
        x /= cubesToExplode.Count;
        y /= cubesToExplode.Count;
        z /= cubesToExplode.Count;
        Vector3 centroid = new Vector3(x, y+1.5f, z-.5f);
        Debug.Log("Centroid is at " + centroid);
        return centroid;
    }

    private int GetCubesInSquare(int xCorner, int yCorner, PowerupType type)
    {
        int toReturn = 0;
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                GameCube cube = grid[x + xCorner, y + yCorner];
                if (cube == null)
                {
                    throw new InvalidOperationException("Somehow we're trying to check the type of a null cube in GameGrid.getCubesInSquare");
                }
                if (type == cube.type)
                {
                    toReturn += 1;
                }
            }
        }
        return toReturn;
    }

    private void RemoveCubeFromGrid(GameCube cube)
    {
        for (int x = 0; x < numCells.x; x++)
        {
            for (int y = 0; y < numCells.y; y++)
            {
                if (grid[x,y]==cube)
                {
                    grid[x, y] = null;
                    return;
                }
            }
        }
        Debug.LogWarning("Warning! Attempt to remove Cube that doesn't exist!");
    }

    private bool IsCornerOfSquare(int xCorner, int yCorner)
    {
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (grid[x+xCorner,y+yCorner]==null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void AddCubesFromSquareToList(int xCorner, int yCorner, List<GameCube> list)
    {
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (grid[x+xCorner, y+yCorner] == null)
                {
                    throw new InvalidOperationException("Somehow we're trying to add a null cube to the destruction list.");
                }
                if (!list.Contains(grid[x + xCorner, y + yCorner]))
                {
                    list.Add(grid[x + xCorner, y + yCorner]);
                }
            }
        }
    }

    private List<ForcedPlacementOptions> forcedPlacements = new List<ForcedPlacementOptions>();

    private bool IsInInvalidArea(float x, float y)
    {
        if (forcedPlacements.Count <= 0)
        {
            return y < 3;
        }

        return false;
    }

    /// <summary>
    /// Adds a "Forced Placement" to the queue, preventing the player from placing the next piece
    /// anywhere but that spot.
    /// </summary>
    /// <param name="rotations">Possible orientations of the piece.</param>
    /// <param name="placements">Possible positions of tne piece.</param>
    public void AddForcedPosition(List<int> rotations, List<Vector2> placements)
    {
        forcedPlacements.Add(new ForcedPlacementOptions(rotations, placements));
    }

    /*REPLACE: Is there a block in the square? (Also, is it off the edge of the board?)*/
    private bool IsObstructedAt(int x, int y)
    {
        if (x < 0||x>=numCells.x||y<0||y>=numCells.y)
        {
            return true;
        }
        if (grid[x,y] != null)
        {
            return true;
        }
        return false;
    }

    internal void DeathClear()
    {
        currentPiece.SinkBlocksAndTurnInvisible(1.5f);
        nextPiece.SinkBlocksAndTurnInvisible(1.5f);
        strangeFrontPlateThing.SetActive(false);
        MeltBoard();
    }

    private void MeltBoard()
    {
        for (int x = 0; x < numCells.x; x++)
        {
            for (int y = 0; y < numCells.y; y++)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y].Sink(UnityEngine.Random.Range(0, 1.5f));
                    grid[x, y] = null;
                }
            }
        }
    }

    internal void DropNewCubeAt(int x, int y)
    {
        GameCube cube = GameObject.Instantiate<GameCube>(currentPiece.cube); // Probbably unsafe.
        cube.Initialize(player, dice);
        grid[currentPiecePosition.x + x - 1, currentPiecePosition.y + y - 1] = cube;
        cube.transform.parent = this.transform;
        cube.transform.localPosition = new Vector3(currentPiecePosition.x - numCells.x / 2f + x - 1 + .5f, 0, currentPiecePosition.y - numCells.y / 2f + y - 1 + .5f);
    }

    private void SetupPieceArray()
    {
        pieceArray[0] = new int[1][,];

        pieceArray[1] = new int[1][,];
        pieceArray[1][0] = new int[3, 3] { { 0, 0, 0 }, { 0, 1, 0 }, { 0, 0, 0 } };

        pieceArray[2] = new int[1][,];
        pieceArray[2][0] = new int[3, 3] { { 0, 1, 0 }, { 0, 1, 0 }, { 0, 0, 0 } };

        pieceArray[3] = new int[3][,];
        pieceArray[3][0] = new int[3, 3] { { 0, 1, 0 }, { 0, 1, 1 }, { 0, 0, 0 } };
        pieceArray[3][1] = new int[3, 3] { { 0, 1, 0 }, { 1, 1, 0 }, { 0, 0, 0 } };
        pieceArray[3][2] = new int[3, 3] { { 0, 1, 0 }, { 0, 1, 0 }, { 0, 1, 0 } };

        pieceArray[4] = new int[6][,];
        pieceArray[4][0] = new int[3, 3] { { 0, 1, 1 }, { 1, 1, 0 }, { 0, 0, 0 } };
        pieceArray[4][1] = new int[3, 3] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 0, 0 } };
        pieceArray[4][2] = new int[3, 3] { { 1, 1, 0 }, { 0, 1, 1 }, { 0, 0, 0 } };
        pieceArray[4][3] = new int[3, 3] { { 1, 1, 0 }, { 0, 1, 0 }, { 0, 1, 0 } };
        pieceArray[4][4] = new int[3, 3] { { 0, 1, 1 }, { 0, 1, 0 }, { 0, 1, 0 } };
        pieceArray[4][5] = new int[3, 3] { { 1, 1, 0 }, { 1, 1, 0 }, { 0, 0, 0 } };

        pieceArray[5] = new int[9][,];
        pieceArray[5][0] = new int[3, 3] { { 1, 1, 0 }, { 1, 1, 1 }, { 0, 0, 0 } };
        pieceArray[5][1] = new int[3, 3] { { 1, 1, 1 }, { 1, 1, 0 }, { 0, 0, 0 } };
        pieceArray[5][2] = new int[3, 3] { { 1, 0, 1 }, { 1, 1, 1 }, { 0, 0, 0 } };
        pieceArray[5][3] = new int[3, 3] { { 1, 0, 0 }, { 1, 1, 1 }, { 0, 0, 1 } };
        pieceArray[5][4] = new int[3, 3] { { 0, 0, 1 }, { 1, 1, 1 }, { 1, 0, 0 } };
        pieceArray[5][5] = new int[3, 3] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 1, 0 } };
        pieceArray[5][6] = new int[3, 3] { { 0, 1, 0 }, { 1, 1, 1 }, { 1, 0, 0 } };
        pieceArray[5][7] = new int[3, 3] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 0, 1 } };
        pieceArray[5][8] = new int[3, 3] { { 1, 1, 1 }, { 0, 1, 0 }, { 0, 1, 0 } };

        pieceArray[6] = new int[10][,];
        pieceArray[6][0] = new int[3, 3] { { 1, 1, 1 }, { 1, 1, 0 }, { 1, 0, 0 } };
        pieceArray[6][1] = new int[3, 3] { { 1, 1, 0 }, { 1, 1, 1 }, { 1, 0, 0 } };
        pieceArray[6][2] = new int[3, 3] { { 1, 1, 0 }, { 1, 1, 1 }, { 0, 1, 0 } };
        pieceArray[6][3] = new int[3, 3] { { 1, 1, 1 }, { 1, 1, 0 }, { 0, 1, 0 } };
        pieceArray[6][4] = new int[3, 3] { { 1, 1, 0 }, { 1, 1, 0 }, { 0, 1, 1 } };
        pieceArray[6][5] = new int[3, 3] { { 1, 1, 0 }, { 1, 1, 1 }, { 0, 0, 1 } };
        pieceArray[6][6] = new int[3, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 0, 0, 0 } };
        pieceArray[6][7] = new int[3, 3] { { 1, 0, 1 }, { 1, 1, 1 }, { 0, 1, 0 } };
        pieceArray[6][8] = new int[3, 3] { { 1, 0, 1 }, { 1, 1, 1 }, { 1, 0, 0 } };
        pieceArray[6][9] = new int[3, 3] { { 1, 0, 1 }, { 1, 1, 1 }, { 0, 0, 1 } };

        pieceArray[7] = new int[7][,];
        pieceArray[7][0] = new int[3, 3] { { 0, 0, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
        pieceArray[7][1] = new int[3, 3] { { 0, 1, 0 }, { 1, 1, 1 }, { 1, 1, 1 } };
        pieceArray[7][2] = new int[3, 3] { { 1, 0, 0 }, { 1, 1, 1 }, { 1, 1, 1 } };
        pieceArray[7][3] = new int[3, 3] { { 1, 1, 0 }, { 1, 1, 1 }, { 0, 1, 1 } };
        pieceArray[7][4] = new int[3, 3] { { 1, 1, 1 }, { 1, 1, 0 }, { 0, 1, 1 } };
        pieceArray[7][5] = new int[3, 3] { { 1, 1, 1 }, { 0, 1, 0 }, { 1, 1, 1 } };
        pieceArray[7][6] = new int[3, 3] { { 1, 1, 0 }, { 1, 1, 1 }, { 1, 0, 1 } };

        pieceArray[8] = new int[2][,];
        pieceArray[8][0] = new int[3, 3] { { 0, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
        pieceArray[8][1] = new int[3, 3] { { 1, 0, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };


        pieceArray[9] = new int[1][,];
        pieceArray[9][0] = new int[3, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
    }



}

[Serializable]
public enum CellType
{
    NORMAL,
    ATTACK,
    SHIELD,
    PSI,
    BROKEN,
    ENERGY
}

internal class ForcedPlacementOptions
{
    public List<int> rotations = new List<int>();
    public List<Vector2> placements = new List<Vector2>();

    public ForcedPlacementOptions(List<int> rotations, List<Vector2> placements)
    {
        this.rotations = rotations;
        this.placements = placements;
    }
}