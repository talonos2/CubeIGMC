using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public static Vector2Int numCells = new Vector2Int(15, 18);
    public GameCube[,] grid = new GameCube[numCells.x, numCells.y];
    public PlayingPiece piecePrefab;
    public PowerupEffect powerUpEffect;

    int[][][,] arra = new int[10][][,];

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
        setupArra();

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

        switch (UnityEngine.Random.Range(0, 9))
        {
            case 0:
                toReturn.Initialize(this.player, arra[1][0]);
                break;
            case 1:
                toReturn.Initialize(this.player, arra[2][0]);
                break;
            case 2:
                toReturn.Initialize(this.player, arra[3][UnityEngine.Random.Range(0, 3)]);
                break;
            case 3:
                toReturn.Initialize(this.player, arra[4][UnityEngine.Random.Range(0, 6)]);
                break;
            case 4:
                toReturn.Initialize(this.player, arra[5][UnityEngine.Random.Range(0, 9)]);
                break;
            case 5:
                toReturn.Initialize(this.player, arra[6][UnityEngine.Random.Range(0, 10)]);
                break;
            case 6:
                toReturn.Initialize(this.player, arra[7][UnityEngine.Random.Range(0, 7)]);
                break;
            case 7:
                toReturn.Initialize(this.player, arra[8][UnityEngine.Random.Range(0, 2)]);
                break;
            case 8:
                toReturn.Initialize(this.player, arra[9][0]);
                break;
            default:
                throw new InvalidOperationException("unknown item type");
        }

        //PlayingPiece toReturn = GameObject.Instantiate(piecePrefab);
        //toReturn.Initialize(this.player);
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

    // Update is called once per frame
    void Update ()
    {
        /*REPLACE: All the movement commands. Rotation and dropping is fine to stay.*/
        if ((isPlayerOne?Input.GetAxis("Vertical_P1") > 0: Input.GetAxis("Vertical_P2") > 0) && !isUpBeingHeld)
        {
            isUpBeingHeld = true;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (currentPiece.HasBlockAt(x, y)&&IsObstructedAt(currentPiecePosition.x+x-1, currentPiecePosition.y + y - 1 + 1))
                    {
                        return;
                    }
                }
            }
            prevPiecePosition = currentPiecePosition;
            currentPiecePosition = currentPiecePosition + new Vector2Int(0, 1);
            timeSinceLastMove = 0f;
            currentPiece.PlaySlideSound();
        }

        if (!(isPlayerOne ? Input.GetAxis("Vertical_P1") > 0 : Input.GetAxis("Vertical_P2") > 0))
        {
            isUpBeingHeld = false;
        }

        if ((isPlayerOne ? Input.GetAxis("Vertical_P1") < 0 : Input.GetAxis("Vertical_P2") < 0) && !isDownBeingHeld)
        {
            isDownBeingHeld = true;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (currentPiece.HasBlockAt(x, y) && IsObstructedAt(currentPiecePosition.x + x - 1, currentPiecePosition.y + y - 1 - 1))
                    {
                        return;
                    }
                }
            }
            prevPiecePosition = currentPiecePosition;
            currentPiecePosition = currentPiecePosition + new Vector2Int(0, -1);
            timeSinceLastMove = 0f;
            currentPiece.PlaySlideSound();
        }

        if (!(isPlayerOne ? Input.GetAxis("Vertical_P1") < 0 : Input.GetAxis("Vertical_P2") < 0))
        {
            isDownBeingHeld = false;
        }

        if ((isPlayerOne ? Input.GetAxis("Horizontal_P1") < 0 : Input.GetAxis("Horizontal_P2") < 0) && !isLeftBeingHeld)
        {
            isLeftBeingHeld = true;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (currentPiece.HasBlockAt(x, y) && IsObstructedAt(currentPiecePosition.x + x - 1 - 1, currentPiecePosition.y + y - 1))
                    {
                        return;
                    }
                }
            }
            prevPiecePosition = currentPiecePosition;
            currentPiecePosition = currentPiecePosition + new Vector2Int(-1, 0);
            timeSinceLastMove = 0f;
            currentPiece.PlaySlideSound();
        }

        if (!(isPlayerOne ? Input.GetAxis("Horizontal_P1") < 0 : Input.GetAxis("Horizontal_P2") < 0))
        {
            isLeftBeingHeld = false;
        }

        if ((isPlayerOne ? Input.GetAxis("Horizontal_P1") > 0 : Input.GetAxis("Horizontal_P2") > 0) && !isRightBeingHeld)
        {
            isRightBeingHeld = true;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (currentPiece.HasBlockAt(x, y) && IsObstructedAt(currentPiecePosition.x + x - 1 + 1, currentPiecePosition.y + y - 1))
                    {
                        return;
                    }
                }
            }
            prevPiecePosition = currentPiecePosition;
            currentPiecePosition = currentPiecePosition + new Vector2Int(1,0);
            timeSinceLastMove = 0f;
            currentPiece.PlaySlideSound();
        }

        if (!(isPlayerOne ? Input.GetAxis("Horizontal_P1") > 0 : Input.GetAxis("Horizontal_P2") > 0))
        {
            isRightBeingHeld = false;
        }

        if (isPlayerOne?Input.GetButtonDown("Place_P1"): Input.GetButtonDown("Place_P2"))
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
            if (currentPiece.rotateCW(surroundings))
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
                if (grid[x,y]!=null)
                {
                    Debug.DrawLine(new Vector3(this.transform.position.x + x - numCells.x / 2, 0, this.transform.position.y + y - numCells.y / 2), new Vector3(this.transform.position.x + x - numCells.x / 2, 4, this.transform.position.y + y - numCells.y / 2));
                }
            }
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

    /*REPLACE: Checks to see if the piece is even partially on the red line.
    
        Come to think of it, this is wrong anyway. (Consider a 1x1 piece, for instance.)*/
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

    private void setupArra()
    {
        arra[0] = new int[1][,];

        arra[1] = new int[1][,];
        arra[1][0] = new int[3, 3] { { 0, 0, 0 }, { 0, 1, 0 }, { 0, 0, 0 } };

        arra[2] = new int[1][,];
        arra[2][0] = new int[3, 3] { { 0, 1, 0 }, { 0, 1, 0 }, { 0, 0, 0 } };

        arra[3] = new int[3][,];
        arra[3][0] = new int[3, 3] { { 0, 1, 0 }, { 0, 1, 1 }, { 0, 0, 0 } };
        arra[3][1] = new int[3, 3] { { 0, 1, 0 }, { 1, 1, 0 }, { 0, 0, 0 } };
        arra[3][2] = new int[3, 3] { { 0, 1, 0 }, { 0, 1, 0 }, { 0, 1, 0 } };

        arra[4] = new int[6][,];
        arra[4][0] = new int[3, 3] { { 0, 1, 1 }, { 1, 1, 0 }, { 0, 0, 0 } };
        arra[4][1] = new int[3, 3] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 0, 0 } };
        arra[4][2] = new int[3, 3] { { 1, 1, 0 }, { 0, 1, 1 }, { 0, 0, 0 } };
        arra[4][3] = new int[3, 3] { { 1, 1, 0 }, { 0, 1, 0 }, { 0, 1, 0 } };
        arra[4][4] = new int[3, 3] { { 0, 1, 1 }, { 0, 1, 0 }, { 0, 1, 0 } };
        arra[4][5] = new int[3, 3] { { 1, 1, 0 }, { 1, 1, 0 }, { 0, 0, 0 } };

        arra[5] = new int[9][,];
        arra[5][0] = new int[3, 3] { { 1, 1, 0 }, { 1, 1, 1 }, { 0, 0, 0 } };
        arra[5][1] = new int[3, 3] { { 1, 1, 1 }, { 1, 1, 0 }, { 0, 0, 0 } };
        arra[5][2] = new int[3, 3] { { 1, 0, 1 }, { 1, 1, 1 }, { 0, 0, 0 } };
        arra[5][3] = new int[3, 3] { { 1, 0, 0 }, { 1, 1, 1 }, { 0, 0, 1 } };
        arra[5][4] = new int[3, 3] { { 0, 0, 1 }, { 1, 1, 1 }, { 1, 0, 0 } };
        arra[5][5] = new int[3, 3] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 1, 0 } };
        arra[5][6] = new int[3, 3] { { 0, 1, 0 }, { 1, 1, 1 }, { 1, 0, 0 } };
        arra[5][7] = new int[3, 3] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 0, 1 } };
        arra[5][8] = new int[3, 3] { { 1, 1, 1 }, { 0, 1, 0 }, { 0, 1, 0 } };

        arra[6] = new int[10][,];
        arra[6][0] = new int[3, 3] { { 1, 1, 1 }, { 1, 1, 0 }, { 1, 0, 0 } };
        arra[6][1] = new int[3, 3] { { 1, 1, 0 }, { 1, 1, 1 }, { 1, 0, 0 } };
        arra[6][2] = new int[3, 3] { { 1, 1, 0 }, { 1, 1, 1 }, { 0, 1, 0 } };
        arra[6][3] = new int[3, 3] { { 1, 1, 1 }, { 1, 1, 0 }, { 0, 1, 0 } };
        arra[6][4] = new int[3, 3] { { 1, 1, 0 }, { 1, 1, 0 }, { 0, 1, 1 } };
        arra[6][5] = new int[3, 3] { { 1, 1, 0 }, { 1, 1, 1 }, { 0, 0, 1 } };
        arra[6][6] = new int[3, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 0, 0, 0 } };
        arra[6][7] = new int[3, 3] { { 1, 0, 1 }, { 1, 1, 1 }, { 0, 1, 0 } };
        arra[6][8] = new int[3, 3] { { 1, 0, 1 }, { 1, 1, 1 }, { 1, 0, 0 } };
        arra[6][9] = new int[3, 3] { { 1, 0, 1 }, { 1, 1, 1 }, { 0, 0, 1 } };

        arra[7] = new int[7][,];
        arra[7][0] = new int[3, 3] { { 0, 0, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
        arra[7][1] = new int[3, 3] { { 0, 1, 0 }, { 1, 1, 1 }, { 1, 1, 1 } };
        arra[7][2] = new int[3, 3] { { 1, 0, 0 }, { 1, 1, 1 }, { 1, 1, 1 } };
        arra[7][3] = new int[3, 3] { { 1, 1, 0 }, { 1, 1, 1 }, { 0, 1, 1 } };
        arra[7][4] = new int[3, 3] { { 1, 1, 1 }, { 1, 1, 0 }, { 0, 1, 1 } };
        arra[7][5] = new int[3, 3] { { 1, 1, 1 }, { 0, 1, 0 }, { 1, 1, 1 } };
        arra[7][6] = new int[3, 3] { { 1, 1, 0 }, { 1, 1, 1 }, { 1, 0, 1 } };

        arra[8] = new int[2][,];
        arra[8][0] = new int[3, 3] { { 0, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
        arra[8][1] = new int[3, 3] { { 1, 0, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };


        arra[9] = new int[1][,];
        arra[9][0] = new int[3, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
    }



}
