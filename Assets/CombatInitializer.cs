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

    public Text p1Text;
    public Text p2Text;

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
            //There are three scenarios: Either we're against an AI, in local co-op, or online. Each one needs a different seed.
            //If against an AI, look at its info and extract a scene from it.
            AIParams pars = MissionManager.instance.mission.GetAIParams();
            if (pars != null)
            {
                AIPlayer ai = grid2.LoadAI(pars.robotic, pars.robotSpeed, pars.loop, pars.text);
                randomSeed = ai.seed;
            }
            //elseif connecting then get the seed from the host. (To Be Implemented)
            else //Single player
            {
                randomSeed = UnityEngine.Random.Range(1, 65535);
            }
            if (MissionManager.instance.mission.GameType() == EngineRoomGameType.LOCAL_PVP)
            {
                grid1.SetLocalPVPMover(true);
                grid2.SetLocalPVPMover(false);
            }
            if (MissionManager.instance.mission.GameType() == EngineRoomGameType.SERVER_PVP)
            {
                EngineRoomNetworkManager ernm = MissionManager.instance.engineRoomNetworkManager;
                ernm.SetupLocalClient();
                ernm.SetupServer();
                grid2.SetRemotePVPPlayer();
                grid1.SetLocalPVPPlayer();
            }
            if (MissionManager.instance.mission.GameType() == EngineRoomGameType.CLIENT_PVP)
            {
                EngineRoomNetworkManager ernm = MissionManager.instance.engineRoomNetworkManager;
                ernm.SetupClient();
                grid2.SetRemotePVPPlayer();
                grid1.SetLocalPVPPlayer();
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
