using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingPiece : MonoBehaviour
{
    private Combatant owner;
    private GameCube[,] piecemap = new GameCube[3, 3];

    public GameCube cube;
    public AudioSource[] slideSounds;

	// Use this for initialization
	public void Start ()
    {
	}

    public void Initialize(Combatant c, SeededRandom dice, int[,] arra)
    {
        owner = c;

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (arra[x, y] == 1)
                {
                    piecemap[x, y] = MakeCube(dice);
                    piecemap[x, y].transform.parent = this.transform;
                    piecemap[x, y].transform.localPosition = new Vector3(x - 1, 0, y - 1);
                }
            }
        }
    }

    private GameCube MakeCube(SeededRandom dice)
    {
        GameCube toReturn = GameObject.Instantiate(cube);
        toReturn.Initialize(owner, dice);
        return toReturn;
    }

    internal bool HasBlockAt(int x, int y)
    {
        return piecemap[x, y] != null;
    }

    /*REPLACE*/
    public bool RotateCW(bool[,] surroundings)
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

    internal GameCube GetCubeAt(int x, int y)
    {
        return piecemap[x, y];
    }

    private int lastPlayed = 0;

    internal void PlaySlideSound()
    {
        slideSounds[lastPlayed++].Play();
        lastPlayed %= slideSounds.Length;
    }
}
