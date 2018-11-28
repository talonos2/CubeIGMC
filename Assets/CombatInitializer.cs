using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CombatInitializer : NetworkBehaviour {

    public GameGrid grid1;
    public GameGrid grid2;
    public GameObject gameGrid1;
    public GameObject gameGrid2;
    private bool isDying;
    private float deathExplosionTime = 0;

    public Text p1Text;
    public Text p2Text;

    public GameObject p1Menu;
    public GameObject p2Menu;
    public GameObject endMenu;

    public GameObject netWindow;

    public GameObject darkCanvas;
    public GameObject campaign;



    // Use this for initialization
    void Start ()
    {
        //       if (Sharedgamedata.issingleplayer == false)
        //       {
        //           netWindow.SetActive(false);
        //       }



        Debug.Log("does combat initializing");
        Debug.Log(Sharedgamedata.issingleplayer);
        if (Sharedgamedata.issingleplayer == true)
        {
            Debug.Log("is singleplayer");
            //            netWindow.SetActive(false);
            gameGrid1.SetActive(true);
            gameGrid2.SetActive(true);
            darkCanvas.SetActive(true);
            campaign.SetActive(true);
        }
        else
        {
            Debug.Log("is multiplayer");
            darkCanvas.SetActive(false);
            campaign.SetActive(false);
//            netWindow.GetComponent<networkManager>();
        }


    }

    bool setup = false;
	// Update is called once per frame
	void Update ()
    {
        if (!setup)
        {

            int randomSeed = UnityEngine.Random.Range(1, 65535);
            if (grid2.isPlayedByAI)
            {
                grid2.LoadAI(false, 0, false);
                randomSeed = grid2.GetAISeed();
            }

            grid1.SetSeedAndStart(randomSeed);
            grid2.SetSeedAndStart(randomSeed);
            setup = true;
        }

        if (isDying)
        {
            deathExplosionTime += Time.deltaTime;
            if (deathExplosionTime > 4)
            {
                p1Menu.SetActive(true);
                p2Menu.SetActive(true);
                endMenu.SetActive(true);


                if (grid1.player.IsAlive()&&!grid2.player.IsAlive())
                {
                    p1Text.text = "You Won";
                    p2Text.text = "You Lost";
                }
                if (grid2.player.IsAlive() && !grid1.player.IsAlive())
                {
                    p2Text.text = "You Won";
                    p1Text.text = "You Lost";
                }
                if (!grid1.player.IsAlive() && !grid2.player.IsAlive())
                {
                    p1Text.text = "Pyrrhic Draw!";
                    p2Text.text = "Pyrrhic Draw!";
                }
                if (grid1.player.IsAlive() && grid2.player.IsAlive())
                {
                    p1Text.text = "...peace and love?";
                    p2Text.text = "...more likely a bug...";
                }
                //Time.timeScale = 0;
                isDying = false;
            }
        }

    }

    public void StartDeathSequence()
    {
        this.isDying = true;
        grid1.enabled = false;
        grid2.enabled = false;
    }
}
