using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CombatInitializer : MonoBehaviour
{

    public GameGrid grid1;
    public GameGrid grid2;
    public GameObject gameGrid1;
    public GameObject gameGrid2;
    private bool isDying;
    private float deathExplosionTime = 0;

    public Image p1Text;
    public Image p2Text;

    public Sprite victory;
    public Sprite defeat;

    public GameObject p1Menu;
    public GameObject p2Menu;
    public GameObject endMenu;

    public GameObject netWindow;

    public GameObject darkCanvas;
    public GameObject campaign;

    public int randomSeed;

    // Use this for initialization
    void Start ()
    {
        gameGrid1.SetActive(true);
        gameGrid2.SetActive(true);
    }

    bool setup = false;
	// Update is called once per frame
	void Update ()
    {
        if (!setup)
        {
            bool doStart = true;
            //There are three scenarios: Either we're against an AI, in local co-op, or online. Each one needs a different seed.
            //If against an AI, look at its info and extract a scene from it.
            AIParams pars = MissionManager.instance.GetAIParams();
            if (pars != null)
            {
                AIPlayer ai = grid2.LoadAI(pars.robotic, pars.robotSpeed, pars.loop, pars.text);
                randomSeed = ai.seed;
            }
            else //If you are a guest player, this seed will be immediately replaced when you finish connecting.
            {
                randomSeed = UnityEngine.Random.Range(1, 65535);
            }
            EngineRoomGameType gameType = MissionManager.instance.GameType();
            switch (gameType)
            {
                case EngineRoomGameType.LOCAL_PVP:
                    grid1.SetLocalPVPMover(true);
                    grid2.SetLocalPVPMover(false);
                    break;
                case EngineRoomGameType.SERVER_PVP:
                    EngineRoomNetworkManager.instance.SetupLocalClient();
                    EngineRoomNetworkManager.instance.SetupServer();
                    grid2.SetRemotePVPPlayer();
                    grid1.SetLocalPVPPlayer();
                    break;
                case EngineRoomGameType.CLIENT_PVP:
                    EngineRoomNetworkManager.instance.SetupClient();
                    grid2.SetRemotePVPPlayer();
                    grid1.SetLocalPVPPlayer();
                    doStart = false; //Client waits to start until server says what the seed is.
                    break;
                case EngineRoomGameType.SINGLE_PLAYER:
                    //NOOP
                    break;
            }

            if (doStart)
            {
                grid1.SetSeedAndStart(randomSeed);
                grid2.SetSeedAndStart(randomSeed);
            }

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
                    p1Text.sprite = victory;
                    p2Text.sprite = defeat;
                }
                if (grid2.player.IsAlive() && !grid1.player.IsAlive())
                {
                    p2Text.sprite = victory;
                    p1Text.sprite = defeat;
                }
                if (!grid1.player.IsAlive() && !grid2.player.IsAlive())
                {
                    p1Text.sprite = defeat;
                    p2Text.sprite = defeat;
                }
                if (grid1.player.IsAlive() && grid2.player.IsAlive())
                {
                    p1Text.sprite = victory;
                    p2Text.sprite = victory;
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
