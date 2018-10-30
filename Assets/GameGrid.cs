﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public static Vector2Int numCells = new Vector2Int(15, 18);
    public GameCube[,] grid = new GameCube[numCells.x, numCells.y];
    public PlayingPiece piecePrefab;

    private PlayingPiece currentPiece;
    private PlayingPiece nextPiece;
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
        currentPiece = GameObject.Instantiate(piecePrefab);
        currentPiece.transform.parent = this.transform;
        updateCurrentPieceTransform();

        nextPieceHolder = transform.Find("PieceHolder");
        nextPiece = GameObject.Instantiate(piecePrefab);
        nextPiece.transform.parent = nextPieceHolder;
        nextPiece.transform.localPosition = Vector3.zero;
    }

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

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (currentPiece.hasBlockAt(x, y)&&IsObstructedAt(currentPiecePosition.x+x-1, currentPiecePosition.y + y - 1 + 1))
                    {
                        return;
                    }
                }
            }
            prevPiecePosition = currentPiecePosition;
            currentPiecePosition = currentPiecePosition + new Vector2Int(0, 1);
            timeSinceLastMove = 0f;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (currentPiece.hasBlockAt(x, y) && IsObstructedAt(currentPiecePosition.x + x - 1, currentPiecePosition.y + y - 1 - 1))
                    {
                        return;
                    }
                }
            }
            prevPiecePosition = currentPiecePosition;
            currentPiecePosition = currentPiecePosition + new Vector2Int(0, -1);
            timeSinceLastMove = 0f;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (currentPiece.hasBlockAt(x, y) && IsObstructedAt(currentPiecePosition.x + x - 1 - 1, currentPiecePosition.y + y - 1))
                    {
                        return;
                    }
                }
            }
            prevPiecePosition = currentPiecePosition;
            currentPiecePosition = currentPiecePosition + new Vector2Int(-1, 0);
            timeSinceLastMove = 0f;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (currentPiece.hasBlockAt(x, y) && IsObstructedAt(currentPiecePosition.x + x - 1 + 1, currentPiecePosition.y + y - 1))
                    {
                        return;
                    }
                }
            }
            prevPiecePosition = currentPiecePosition;
            currentPiecePosition = currentPiecePosition + new Vector2Int(1,0);
            timeSinceLastMove = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Fallback: If there's somebohw someting directly underneath you, do not place. Should never happen in practice.
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (currentPiece.hasBlockAt(x, y) && IsInInvalidArea(currentPiecePosition.x + x - 1, currentPiecePosition.y + y - 1))
                    {
                        return;
                    }
                }
            }
            DropPiece();
        }

        if (Input.GetKeyDown(KeyCode.Z))
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
            }
            else
            {
                //Make some sort of sound.
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
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
                if (currentPiece.hasBlockAt(x,y))
                {
                    GameCube cube = currentPiece.getCubeAt(x, y);
                    grid[currentPiecePosition.x+x-1, currentPiecePosition.y+y-1] = cube;
                    cube.transform.parent = this.transform;
                    cube.transform.localPosition = new Vector3(currentPiecePosition.x - numCells.x / 2f + x - 1+.5f, 0, currentPiecePosition.y - numCells.y / 2f + y - 1+.5f);
                }
            }
        }
        Destroy(currentPiece.gameObject);

        //Check for squares
        List<GameCube> cubesToExplode = new List<GameCube>();
        for (int x = 0; x < numCells.x-2; x++)
        {
            for (int y = 0; y < numCells.y-2; y++)
            {
                if (IsCornerOfSquare(x, y))
                {
                    AddCubesFromSquareToList(x, y, cubesToExplode);
                }
            }
        }

        foreach (GameCube cube in cubesToExplode)
        {
            RemoveCubeFromGrid(cube);
            GameObject.Destroy(cube.gameObject);
        }

        currentPiece = nextPiece;
        currentPiece.transform.parent = this.transform;
        currentPiecePosition = new Vector2Int(1, 1);
        updateCurrentPieceTransform();

        nextPiece = GameObject.Instantiate(piecePrefab);
        nextPiece.transform.parent = nextPieceHolder;
        nextPiece.transform.localPosition = Vector3.zero;

        prevPieceRotation = currentPieceRotation = 0;
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
                list.Add(grid[x+xCorner, y+yCorner]);
            }
        }
    }

    private bool IsInInvalidArea(float x, float y)
    {
        return y < 3;
    }

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

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(.5f, 0, 1f, .5f);
        for (int x = 0; x <= numCells.x; x++)
        {
            Vector3 from = transform.position + (new Vector3(numCells.x*.5f-x, 0, numCells.y * .5f));
            Vector3 to = transform.position + (new Vector3(numCells.x*.5f-x, 0, numCells.y * -.5f));
            Gizmos.DrawLine(to, from);
        }
        for (int y = 0; y <= numCells.y; y++)
        {
            Vector3 from = transform.position + (new Vector3(numCells.x * .5f, 0, numCells.y * .5f-y));
            Vector3 to = transform.position + (new Vector3(numCells.x * -.5f, 0, numCells.y * .5f-y));
            Gizmos.DrawLine(to, from);
        }
    }
}
