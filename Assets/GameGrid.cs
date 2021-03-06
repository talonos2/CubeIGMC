﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class GameGrid : MonoBehaviour
{
    public bool isRecording = false;
    
    public static Vector2Int numCells = new Vector2Int(15, 18);
    public GameObject strangeFrontPlateThing;
    public AudioSource ominousTick;
    public AudioSource powerDown;
    public AudioSource errorSound;
    public GameCube[,] grid = new GameCube[numCells.x, numCells.y];
    public CellType[,] cellTypes = new CellType[numCells.x, numCells.y];
    public TileFX[,] cellTypeFX = new TileFX[numCells.x, numCells.y];
    public PlayingPiece piecePrefab;
    public PowerupEffect powerUpEffect;
    public CubeConversionManager cubeConversionManager;

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

    internal AIPlayer LoadAI(bool isRobotic, float speed, bool loop, String json)
    {
        string inputJson = json;
        AIPlayer ai = JsonUtility.FromJson<AIPlayer>(inputJson);
        if (isRobotic)
        {
            ai.MakeRobotic(speed);
        }
        if (loop)
        {
            ai.MakeLoop();
        }
        mover = ai;
        return ai;
    }

    internal void SetLocalPVPMover(bool isPlayer1)
    {
        this.mover = new LocalPVPMover(player,ominousTick,isPlayer1);
    }

    internal void getSeedFromServer(int value)
    {
        this.SetSeedAndStart(value);
        player.enemy.ThisGameGrid.SetSeedAndStart(value);
    }

    internal void getCharSheetFromServer(int value)
    {


    }

    public PlayingPiece currentPiece;
    private PlayingPiece nextPiece;

    public SeededRandom dice;
    private GameRecorder recorder;

    internal void SetSeedAndStart(int randomSeed)
    {
        seeded = randomSeed;

        if (isRecording)
        {
            recorder = new GameRecorder(randomSeed);
        }

        dice = new SeededRandom(randomSeed);

        SetupPieceArray();

        currentPiece = MakeAPiece();

        currentPiece.transform.parent = this.transform;
        UpdateCurrentPieceTransform();

        nextPiece = MakeAPiece();
        nextPiece.transform.parent = nextPieceHolder;
        nextPiece.transform.localPosition = Vector3.zero;

        SetGridCellTypeStateAndAttendentVFX();


        if (isPlayerOne)
        {
            string dataPath;


            dataPath = Path.Combine(Application.persistentDataPath, MissionManager.instance.player1CharacterSheetPath);
            if (!File.Exists(dataPath))
            {
                player.SaveCharacterToDisk(MissionManager.instance.player1CharacterSheetPath);
            }

            player.SetCharacterSheet(MissionManager.instance.player1CharacterSheetPath);
        }
    }

    internal void SetRemotePVPPlayer()
    {
        mover = new RemoteNetworkedPVPMover(MissionManager.instance.engineRoomNetworkManager,MissionManager.instance.mission.GameType()==EngineRoomGameType.SERVER_PVP, player);
    }

    internal void SetLocalPVPPlayer()
    {
        mover = new LocalNetworkedPVPMover(player, ominousTick, MissionManager.instance.engineRoomNetworkManager, this, seeded, MissionManager.instance.mission.GameType() == EngineRoomGameType.SERVER_PVP);
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
                    player.RemoveDeathEffectFromDamageManager(cellTypeFX[x, y]);
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

    private Vector2Int prevPiecePosition = new Vector2Int(1, 1);//(7, 16);
    public Vector2Int currentPiecePosition = new Vector2Int(1, 1);//(7, 16);

    public int currentPieceRotation = 0;
    public int prevPieceRotation = 0;

    public Transform nextPieceHolder;

    public float timeSinceLastMove = 0;
    public float timeSinceLastRot = 0;
    private readonly float msNeededToLerp = 62;
    private float tickMove = 0;
    private float tickRot = 0;

    private readonly Quaternion[] orientations = { Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 90, 0), Quaternion.Euler(0, 180, 0), Quaternion.Euler(0, 270, 0) };

    private readonly int[] pieceSizeBag = { 1, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 6, 6, 6, 6, 7, 7, 8 };

    private List<int[,]> forcedPieces = new List<int[,]>();

	// Use this for initialization
	void Start ()
    {
        if (mover == null)
        {
            mover = new SinglePlayerMover(player, ominousTick);
        }
    }

    private PlayingPiece MakeAPiece()
    {

        Debug.Log("Piece Made");
        PlayingPiece toReturn = GameObject.Instantiate(piecePrefab);

        if (forcedPieces.Count > 0)
        {
            int[,] forcedPiece = forcedPieces[0];
            forcedPieces.RemoveAt(0);
            toReturn.Initialize(this.player, dice, forcedPiece);
        }
        else
        {
            int num = dice.NextInt(0, pieceSizeBag.Length);
            int pieceSize = pieceSizeBag[num];
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
        Debug.Log(oldPiece1);
        GameObject.Destroy(oldPiece1.gameObject);
        GameObject.Destroy(oldPiece2.gameObject);

        currentPiece.transform.parent = this.transform;
        currentPiecePosition = new Vector2Int(1, 1);
        UpdateCurrentPieceTransform();

        nextPiece.transform.parent = nextPieceHolder;
        nextPiece.transform.localPosition = Vector3.zero;

        prevPieceRotation = currentPieceRotation = 0;
    }

    /* This moves the graphical representation of the piece.*/
    public void UpdateCurrentPieceTransform()
    {
        if (currentPiece == null)
        {
            return;
        }

        timeSinceLastMove += Time.deltaTime * 1000;
        timeSinceLastRot += Time.deltaTime * 1000;


        tickMove = timeSinceLastMove / msNeededToLerp;
        tickRot = timeSinceLastRot / msNeededToLerp;


        Vector3 futurePos = GetLocalTranslationFromGridLocation(currentPiecePosition.x, currentPiecePosition.y);
        Vector3 oldPos = GetLocalTranslationFromGridLocation(prevPiecePosition.x, prevPiecePosition.y);
        Vector3 betweenPosition = Vector3.Lerp(oldPos, futurePos, tickMove);

        currentPiece.transform.localPosition = betweenPosition;

        Quaternion futureRot = orientations[currentPieceRotation];
        Quaternion oldRot = orientations[prevPieceRotation];
        Quaternion betweenRot = Quaternion.Slerp(oldRot, futureRot, tickRot);

        currentPiece.transform.localRotation = betweenRot;
    }

    private Vector3 GetLocalTranslationFromGridLocation(int x, int y)
    {
        return new Vector3(x - numCells.x / 2.0f + .5f, 0, y - numCells.y / 2.0f + .5f);
    }

    public bool isUpBeingHeld;
    public bool isDownBeingHeld;
    public bool isLeftBeingHeld;
    public bool isRightBeingHeld;

    public float timeSinceLastMoveUpEvent;
    public float timeSinceLastMoveDownEvent;
    public float timeSinceLastMoveLeftEvent;
    public float timeSinceLastMoveRightEvent;

    public bool justPressedUp;
    public bool justPressedDown;
    public bool justPressedLeft;
    public bool justPressedRight;

    private float timeHeldBothRotatesAtOnce;

    private bool hasSaved;

    public InvisibleDelayedChargeGiver chargeGiverPrefab;

    Mover mover;

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
        {
            justExitedMenu = true;
            return;
        }

        justExitedMenu = false;

        if (MissionManager.isInCutscene)
        {
            return;
        }

        //There are five places input could come from.
        // -1: The Single player, which can be keyboard or controller.
        // -2: Local player 1, which is keyboard
        // -3: Local player 2, which is controller
        // -4: The AI
        // -5: The remote player.

        //These sources can all perform the same basic commands: LRUD, CW, CCW, Drop, and Reboot.
        //To figure out which we're doing, we query the "Mover" for a command.

        mover.Tick(justExitedMenu);

        if (mover.GetInput(MoverCommand.UP)) { TryGoUp(); }
        if (mover.GetInput(MoverCommand.DOWN)) { TryGoDown(); }
        if (mover.GetInput(MoverCommand.LEFT)) { TryGoLeft(); }
        if (mover.GetInput(MoverCommand.RIGHT)) { TryGoRight(); }
        if (mover.GetInput(MoverCommand.CW)) { TryRotateCW(); }
        if (mover.GetInput(MoverCommand.CCW)) { TryRotateCCW(); }
        if (mover.GetInput(MoverCommand.DROP)) { TryDrop(); }
        if (mover.GetInput(MoverCommand.REBOOT)) { Reboot(); }

        if (Time.timeSinceLevelLoad > 300 & isRecording & !hasSaved)
        {
            recorder.PrintOut();
            hasSaved = true;
        }

        UpdateCurrentPieceTransform();

    }

    private void TryDrop()
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
                errorSound.Play();
                return;
            }
            forcedPlacements.RemoveAt(0);
        }

        if (isRecording) { recorder.RegisterEvent(GameRecorder.DROP); }
        DropPiece();
    }

    private void TryRotateCCW()
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

    private void TryRotateCW()
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

    public void Reboot()
    {
        powerDown.Play();
        MeltBoard();
        player.DeleteAllEnergy();
        if (MissionManager.triggerCallbacksOnShipReboot)
        {
            MissionManager.instance.grossCallbackHack.enabled = true;
        }
    }



    public void TryGoUp()
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

    public void TryGoDown()
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

    public void TryGoLeft()
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

    public void TryGoRight()
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
//            this.gameObject.name = "GameGrid"+UnityEngine.Random.Range(1, 100);
//            Debug.Log(this.gameObject.name);
            prevPiecePosition = currentPiecePosition;
            currentPiecePosition = currentPiecePosition + new Vector2Int(+1, 0);
            if (isRecording) { recorder.RegisterEvent(GameRecorder.RIGHT); }
            currentPiece.PlaySlideSound();
            timeSinceLastMove = 0f;
        }
    }

    public void DropPiece()
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
            delays.Add(delay);
        }

        delays.Sort();

        player.StartNewParticleBarrage();

        foreach (float delay in delays)
        {
            if (player.HasRoomForMoreEnergy())
            {
                PowerupEffect pe = GameObject.Instantiate<PowerupEffect>(powerUpEffect);
                GameCube sourceCube = cubesToExplode[UnityEngine.Random.Range(0, cubesToExplode.Count)];
                pe.Initialize(sourceCube.transform.position, player.GetTargetOfParticle(PowerupType.ENERGY, 3), delay, PowerupType.ENERGY);
                InvisibleDelayedChargeGiver chargeGiver = GameObject.Instantiate<InvisibleDelayedChargeGiver>(chargeGiverPrefab);
                chargeGiver.target = player;
                chargeGiver.delay = delay + 1;
                chargeGiver.type = PowerupType.ENERGY;
                chargeGiver.SetAmountForOneCube(PowerupType.ENERGY);
                chargeGiver.amount /= 3; //(3 particles per square made);
            }
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

    internal int seeded;

    public bool IsInInvalidArea(float x, float y)
    {
        if (forcedPlacements.Count <= 0)
        {
            bool error = y < 3;
            if (error)
            {
                errorSound.Play();
            }
            return error;
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

    //Is there a block in the square? (Also, is it off the edge of the board?)*/
    public bool IsObstructedAt(int x, int y)
    {

        if (x < 0 || x >= numCells.x)
            return true;
        else if (y < 0 || y >= numCells.y)
            return true;
        else if (grid[x, y] != null)
            return true;
        else
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

    public void ClearBoardSilently()
    {
        for (int x = 0; x < numCells.x; x++)
        {
            for (int y = 0; y < numCells.y; y++)
            {
                if (grid[x, y] != null)
                {
                    GameObject.Destroy(grid[x, y].gameObject);
                    grid[x, y] = null;
                }
            }
        }
    }

    internal void DropNewCubeAt(int x, int y)
    {
        GameCube cube = GameObject.Instantiate<GameCube>(currentPiece.cube); // Probably unsafe.

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