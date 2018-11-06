﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public static Vector2Int numCells = new Vector2Int(15, 18);
    public GameCube[,] grid = new GameCube[numCells.x, numCells.y];
    public PlayingPiece piecePrefab;
    public PowerupEffect powerUpEffect;

    int[][][,] pieceArray = new int[10][][,];

    public AudioSource dropSound;
    public AudioSource matchSound;

    private UnityEngine.Random gameplayDice;
    private PlayingPiece currentPiece;
    private PlayingPiece nextPiece;

    internal void SetSeedAndStart(int randomSeed)
    {
        this.gameplayDice = new UnityEngine.Random();
        //...uh... can I actually *do* anything with that? Doesn't seem like it...

    }

    public Combatant player;
    public bool isPlayerOne = true;
    private Combatant enemy;

    internal void SetEnemy(GameGrid other)
    {
        this.enemy = other.player;
    }

    private Vector2Int prevPiecePosition = new Vector2Int(1, 1);//(7, 16);
    private Vector2Int currentPiecePosition = new Vector2Int(1, 1);//(7, 16);

    private int currentPieceRotation = 0;
    private int prevPieceRotation = 0;

    private Transform nextPieceHolder;

    private float timeSinceLastMove = 0;
    private float timeSinceLastRot = 0;
    private float msNeededToLerp = 62;

    private Quaternion[] orientations = { Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 90, 0), Quaternion.Euler(0, 180, 0), Quaternion.Euler(0, 270, 0) };

	// Use this for initialization
	void Start ()
    {
        SetupPieceArray();

        currentPiece = MakeAPiece();
        currentPiece.transform.parent = this.transform;
        updateCurrentPieceTransform();

        nextPieceHolder = transform.Find("PieceHolder");
        nextPiece = MakeAPiece();
        nextPiece.transform.parent = nextPieceHolder;
        nextPiece.transform.localPosition = Vector3.zero;
    }

    private PlayingPiece MakeAPiece()
    {
        PlayingPiece toReturn = GameObject.Instantiate(piecePrefab);

        int pieceSize = UnityEngine.Random.Range(1, 10);
        toReturn.Initialize(this.player, pieceArray[pieceSize][UnityEngine.Random.Range(0, pieceArray[pieceSize].Length)]);

        return toReturn;
    }

    /*REPLACE: This moves the graphical representation of the piece.*/
    private void updateCurrentPieceTransform()
    {
        timeSinceLastMove += Time.deltaTime * 1000;
        timeSinceLastRot += Time.deltaTime * 1000;
        float prop = timeSinceLastMove / msNeededToLerp;

        Vector3 targetPosition = new Vector3(currentPiecePosition.x - numCells.x / 2.0f + .5f, 0, currentPiecePosition.y - numCells.y / 2.0f + .5f);
        Vector3 oldPosition = new Vector3(prevPiecePosition.x - numCells.x / 2.0f + .5f, 0, prevPiecePosition.y - numCells.y / 2.0f + .5f);
        Vector3 animatedPosition = Vector3.Lerp(oldPosition, targetPosition, prop);

        currentPiece.transform.localPosition = animatedPosition; 

        prop = timeSinceLastRot / msNeededToLerp;

        Quaternion targetRotation = orientations[currentPieceRotation];
        Quaternion oldRotation = orientations[prevPieceRotation];
        Quaternion animatedRotation = Quaternion.Slerp(oldRotation, targetRotation, prop);

        currentPiece.transform.localRotation = animatedRotation;
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

    // Update is called once per frame
    void Update ()
    {
        HandleUpMovement();
        HandleDownMovement();
        HandleLeftMovement();
        HandleRightMovement();

        if (isPlayerOne ? Input.GetButtonDown("Place_P1") : Input.GetButtonDown("Place_P2"))
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
            DropPiece();
        }

        if (isPlayerOne ? Input.GetButtonDown("Rotate1_P1") : Input.GetButtonDown("Rotate1_P2"))
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
            }
            else
            {
                //Make some sort of sound.
            }
        }

        if (isPlayerOne ? Input.GetButtonDown("Rotate2_P1") : Input.GetButtonDown("Rotate2_P2"))
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
            }
            else
            {
                //Make some sort of sound.
            }
        }

        updateCurrentPieceTransform();

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

    private void HandleUpMovement()
    {
        if ((isPlayerOne ? Input.GetAxis("Vertical_P1") > 0 : Input.GetAxis("Vertical_P2") > 0) && !isUpBeingHeld)
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
                    timeSinceLastMove = 0f;
                }

                timeSinceLastMoveUpEvent %= fastButtonMashSpeed;
            }
            timeSinceLastMoveUpEvent += Time.deltaTime;
        }
        if (!(isPlayerOne ? Input.GetAxis("Vertical_P1") > 0 : Input.GetAxis("Vertical_P2") > 0))
        {
            isUpBeingHeld = false;
        }
    }

    private void HandleDownMovement()
    {
        if ((isPlayerOne ? Input.GetAxis("Vertical_P1") < 0 : Input.GetAxis("Vertical_P2") < 0) && !isDownBeingHeld)
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
                    timeSinceLastMove = 0f;
                }

                timeSinceLastMoveDownEvent %= fastButtonMashSpeed;
            }
            timeSinceLastMoveDownEvent += Time.deltaTime;
        }
        if (!(isPlayerOne ? Input.GetAxis("Vertical_P1") < 0 : Input.GetAxis("Vertical_P2") < 0))
        {
            isDownBeingHeld = false;
        }
    }

    private void HandleLeftMovement()
    {
        if ((isPlayerOne ? Input.GetAxis("Horizontal_P1") < 0 : Input.GetAxis("Horizontal_P2") < 0) && !isLeftBeingHeld)
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
                    currentPiece.PlaySlideSound();
                    timeSinceLastMove = 0f;
                }

                timeSinceLastMoveLeftEvent %= fastButtonMashSpeed;
            }
            timeSinceLastMoveLeftEvent += Time.deltaTime;
        }
        if (!(isPlayerOne ? Input.GetAxis("Horizontal_P1") < 0 : Input.GetAxis("Horizontal_P2") < 0))
        {
            isLeftBeingHeld = false;
        }
    }

    private void HandleRightMovement()
    {
        if ((isPlayerOne ? Input.GetAxis("Horizontal_P1") > 0 : Input.GetAxis("Horizontal_P2") > 0) && !isRightBeingHeld)
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
                    currentPiece.PlaySlideSound();
                    timeSinceLastMove = 0f;
                }

                timeSinceLastMoveRightEvent %= fastButtonMashSpeed;
            }
            timeSinceLastMoveRightEvent += Time.deltaTime;
        }
        if (!(isPlayerOne ? Input.GetAxis("Horizontal_P1") > 0 : Input.GetAxis("Horizontal_P2") > 0))
        {
            isRightBeingHeld = false;
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
        int attackCubes = 0;
        int energyCubes = 0;
        int shieldCube = 0;
        int psiCubes = 0;

        int numberOfExplosions = 0;

        for (int x = 0; x < numCells.x-2; x++)
        {
            for (int y = 0; y < numCells.y-2; y++)
            {
                if (IsCornerOfSquare(x, y))
                {
                    AddCubesFromSquareToList(x, y, cubesToExplode);

                    attackCubes += GetCubesInSquare(x, y, CubeType.ATTACK);
                    energyCubes += GetCubesInSquare(x, y, CubeType.ENERGY);
                    shieldCube += GetCubesInSquare(x, y, CubeType.SHIELDS);
                    psiCubes += GetCubesInSquare(x, y, CubeType.PSI);
                    numberOfExplosions++;
                }
            }
        }

        float maxDelay = (float)Math.Sqrt(numberOfExplosions / 9f);

        List<ExplosionWrapper> allExplosions = new List<ExplosionWrapper>();
        for (int x = 0; x < numCells.x - 2; x++)
        {
            for (int y = 0; y < numCells.y - 2; y++)
            {
                if (IsCornerOfSquare(x, y))
                {
                    allExplosions.AddRange(ExplodeCubesInSquare(x, y, maxDelay));
                }
            }
        }

        //All explosions are in a list. Explode them I guess.
        {
            allExplosions.Sort(new ExplosionSorter());
        }

        player.StartNewParticleBarrage();

        foreach (ExplosionWrapper ew in allExplosions)
        {
            ew.Explode(powerUpEffect, player);
        }

        foreach (GameCube cube in cubesToExplode)
        {
            RemoveCubeFromGrid(cube);
            cube.Sink(UnityEngine.Random.Range(0, maxDelay));
        }
        if (allExplosions.Count!=0)
        {
            matchSound.Play();
        }
        else
        {
            dropSound.Play();
        }

        Destroy(currentPiece.gameObject);

        currentPiece = nextPiece;
        currentPiece.transform.parent = this.transform;
        currentPiecePosition = new Vector2Int(1, 1);
        updateCurrentPieceTransform();

        nextPiece = MakeAPiece();
        nextPiece.transform.parent = nextPieceHolder;
        nextPiece.transform.localPosition = Vector3.zero;

        prevPieceRotation = currentPieceRotation = 0;
    }

    private List<ExplosionWrapper> ExplodeCubesInSquare(int xCorner, int yCorner, float maxDelay)
    {
        List<ExplosionWrapper> toReturn = new List<ExplosionWrapper>();
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                GameCube cube = grid[x + xCorner, y + yCorner];
                if (cube == null)
                {
                    throw new InvalidOperationException("Somehow we're trying to check the type of a null cube in GameGrid.ExplodeCubesInSquare");
                }
                toReturn.Add(new ExplosionWrapper(cube, UnityEngine.Random.Range(0, maxDelay)));
            }
        }
        return toReturn;
    }

    private int GetCubesInSquare(int xCorner, int yCorner, CubeType type)
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

    /*REPLACE: Checks to see if this block is in an invalid position. Right now, this means "In the Red Area.*/
    private bool IsInInvalidArea(float x, float y)
    {
        return y < 3;
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

    private class ExplosionWrapper
    {
        private GameCube cube;
        internal float delay;

        internal ExplosionWrapper(GameCube cube, float delay)
        {
            this.cube = cube;
            this.delay = delay;
        }

        internal void Explode(PowerupEffect powerupEffect, Combatant player)
        {
            PowerupEffect p = GameObject.Instantiate(powerupEffect);
            p.transform.position = cube.transform.position;
            Color toInitialize = Color.white;

            p.Initialize(p.transform.position, player.GetTargetOfParticle(cube.type), delay, cube.type, player);
        }
    }

    private class ExplosionSorter : IComparer<ExplosionWrapper>
    {
        int IComparer<ExplosionWrapper>.Compare(ExplosionWrapper a, ExplosionWrapper b)
        {

            if (a.delay > b.delay)
                return 1;

            if (a.delay < b.delay)
                return -1;

            else
                return 0;
        }
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
