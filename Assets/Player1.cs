using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player1 : NetworkBehaviour
{
    GameObject home;
    GameObject guest;

    GameGrid gameGridHome;
    GameGrid gameGridGuest;

    bool started = false;

	// Use this for initialization
	void Start ()
    {
        //Time.timeScale = 0;
        home = GameObject.Find("GameGridHome");
        guest = GameObject.Find("GameGridGuest");
        gameGridHome = home.GetComponent<GameGrid>();
        gameGridGuest = guest.GetComponent<GameGrid>();
        Sharedgamedata.issingleplayer = false;
        if (isLocalPlayer)
        {
            gameGridHome.isPlayerOne = true;
        }


    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isLocalPlayer)
        {
            //            Debug.Log(gameGridHome);
                        Debug.Log("I do one");
            gameGridHome.proxyUpdate();
            //            Debug.Log("local");
        }
        else
        {
            if (!started)
            {
                Debug.Log("timestarts");
                Time.timeScale = 1;
                started = true;
            }
            Debug.Log("I do the other");
            gameGridGuest.proxyUpdate();
//            Debug.Log("not local");

        }

    }
}
