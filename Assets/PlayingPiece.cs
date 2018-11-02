using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingPiece : MonoBehaviour
{
    private Combatant owner;
    private GameCube[,] piecemap = new GameCube[3, 3];

    public GameCube cube;

	// Use this for initialization
	public void Start ()
    {
	}

    public void Initialize(Combatant c)
    {
        owner = c;
        CreateRandomPiece();
    }


    /*REPLACE*/
    public void CreateRandomPiece ()
    {
        Debug.Log("Making a Piece");
        piecemap[1, 1] = MakeCube();
        if (UnityEngine.Random.Range(0,2) == 0) { piecemap[0, 1] = MakeCube(); }
        if (UnityEngine.Random.Range(0, 2) == 0) { piecemap[2, 1] = MakeCube(); }
        if (UnityEngine.Random.Range(0, 2) == 0) { piecemap[1, 0] = MakeCube(); }
        if (UnityEngine.Random.Range(0, 2) == 0) { piecemap[1, 2] = MakeCube(); }
        if ((piecemap[0, 1] != null || piecemap[1, 0] != null) && UnityEngine.Random.Range(0, 2) == 0) { piecemap[0, 0] = MakeCube(); }
        if ((piecemap[2, 1] != null || piecemap[1, 2] != null) && UnityEngine.Random.Range(0, 2) == 0) { piecemap[2, 2] = MakeCube(); }
        if ((piecemap[0, 1] != null || piecemap[1, 2] != null) && UnityEngine.Random.Range(0, 2) == 0) { piecemap[0, 2] = MakeCube(); }
        if ((piecemap[2, 1] != null || piecemap[1, 0] != null) && UnityEngine.Random.Range(0, 2) == 0) { piecemap[2, 0] = MakeCube(); }
        PlaceCubesCorrectly();
    }

    private GameCube MakeCube()
    {
        GameCube toReturn = GameObject.Instantiate(cube);
        toReturn.Initialize(owner);
        return toReturn;
    }

    internal bool HasBlockAt(int x, int y)
    {
        return piecemap[x, y] != null;
    }

    /*REPLACE: This is only ever called by CreateRandomPiece, and can probably be folded into it.*/
    private void PlaceCubesCorrectly()
    {
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (piecemap[x,y]!= null)
                {
                    piecemap[x, y].transform.parent = this.transform;
                    piecemap[x, y].transform.localPosition = new Vector3(x - 1, 0, y - 1);
                }
            }
        }
    }

    /*REPLACE*/
    public bool rotateCW(bool[,] surroundings)
    {
        GameCube[,] newPiecemap = new GameCube[3,3];
        newPiecemap[0,0] = piecemap[0,2];
        newPiecemap[0,1] = piecemap[1,2];
        newPiecemap[0,2] = piecemap[2,2];
        newPiecemap[1,0] = piecemap[0,1];
        newPiecemap[1,1] = piecemap[1,1];
        newPiecemap[1,2] = piecemap[2,1];
        newPiecemap[2,0] = piecemap[0,0];
        newPiecemap[2,1] = piecemap[1,0];
        newPiecemap[2,2] = piecemap[2,0];

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (newPiecemap[x,y]!=null&&surroundings[x,y]==true)
                {
                    return false;
                }
            }
        }
        piecemap = newPiecemap;
        return true;
        //placeCubesCorrectly();
    }

    /*REPLACE*/
    public bool rotateCCW(bool[,] surroundings)
    {
        GameCube[,] newPiecemap = new GameCube[3,3];
        newPiecemap[0,2] = piecemap[0,0];
        newPiecemap[1,2] = piecemap[0,1];
        newPiecemap[2,2] = piecemap[0,2];
        newPiecemap[0,1] = piecemap[1,0];
        newPiecemap[1,1] = piecemap[1,1];
        newPiecemap[2,1] = piecemap[1,2];
        newPiecemap[0,0] = piecemap[2,0];
        newPiecemap[1,0] = piecemap[2,1];
        newPiecemap[2,0] = piecemap[2,2];

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (newPiecemap[x, y] != null && surroundings[x, y] == true)
                {
                    return false;
                }
            }
        }
        piecemap = newPiecemap;
        return true;
        //placeCubesCorrectly();
    }

    // Update is called once per frame
    void Update ()
    {
		
	}

    internal GameCube getCubeAt(int x, int y)
    {
        return piecemap[x, y];
    }
}
