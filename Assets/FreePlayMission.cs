using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreePlayMission : Mission {

    internal override void Unblock()
    {
        
    }

    // Use this for initialization
    void Start () {
        PointerHolder p = MissionManager.instance.pointers;
        darkness = p.daaaaaknesssss;
    }

    public GameGrid gridToCheatWith;
    private float timeSinceStepStarted;
    public Image darkness;

    public bool cheaty;

	// Update is called once per frame
	void Update ()
    {
        if (timeSinceStepStarted == 0)
        {
            if (cheaty)
            {
                gridToCheatWith.SetGridCellTypeStateAndAttendentVFX();
                gridToCheatWith.player.energy = 0;

                List<int[,]> piecesToForce = new List<int[,]>
                {
                    new int[3, 3] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 0, 0 } }, //1-1
                    new int[3, 3] { { 0, 1, 0 }, { 1, 1, 1 }, { 1, 0, 1 } }, //1-2
                    new int[3, 3] { { 0, 0, 0 }, { 1, 1, 1 }, { 1, 0, 1 } }, //1-3
                    new int[3, 3] { { 0, 0, 0 }, { 1, 1, 1 }, { 0, 0, 0 } }, //1-4

                    new int[3, 3] { { 0, 1, 1 }, { 1, 1, 1 }, { 0, 0, 0 } }, //5-1

                    new int[3, 3] { { 1, 0, 0 }, { 1, 1, 1 }, { 0, 0, 0 } }, //2-1
                    new int[3, 3] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 1, 0 } }, //2-2
                    new int[3, 3] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 0, 0 } }, //2-3

                    new int[3, 3] { { 1, 0, 0 }, { 1, 1, 0 }, { 0, 1, 0 } }, //5-2
                    new int[3, 3] { { 1, 1, 1 }, { 0, 1, 1 }, { 0, 0, 1 } }, //5-3

                    new int[3, 3] { { 0, 1, 0 }, { 1, 1, 1 }, { 1, 0, 1 } }, //2-4
                    new int[3, 3] { { 0, 0, 0 }, { 1, 1, 1 }, { 1, 0, 1 } }, //2-5

                    new int[3, 3] { { 1, 0, 0 }, { 1, 1, 1 }, { 0, 1, 0 } }, //4-1
                    new int[3, 3] { { 1, 0, 1 }, { 1, 1, 1 }, { 0, 0, 0 } }, //4-2

                    new int[3, 3] { { 1, 0, 0 }, { 1, 1, 1 }, { 0, 0, 0 } }, //3-1
                    new int[3, 3] { { 0, 0, 1 }, { 1, 1, 1 }, { 1, 1, 0 } }, //3-2
                    new int[3, 3] { { 0, 0, 0 }, { 1, 1, 1 }, { 1, 1, 0 } }, //3-3

                    new int[3, 3] { { 0, 1, 0 }, { 1, 1, 0 }, { 1, 1, 0 } }, //5-4
                    new int[3, 3] { { 0, 1, 0 }, { 0, 1, 0 }, { 0, 0, 0 } }, //5-5

                    new int[3, 3] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 0, 0 } }, //3-4
                    new int[3, 3] { { 0, 1, 1 }, { 1, 1, 1 }, { 0, 0, 0 } }, //3-5

                    new int[3, 3] { { 0, 0, 1 }, { 0, 1, 1 }, { 1, 1, 1 } }, //4-3
                    new int[3, 3] { { 0, 0, 0 }, { 0, 1, 0 }, { 0, 0, 0 } }, //4-4
                    new int[3, 3] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 0, 0 } }, //4-5

                    new int[3, 3] { { 0, 1, 0 }, { 0, 1, 1 }, { 0, 1, 1 } }, //5-6
                    new int[3, 3] { { 1, 0, 1 }, { 1, 1, 1 }, { 1, 1, 1 } }, //5-7
                };
                gridToCheatWith.ForcePieces(piecesToForce);
            }
        }

        timeSinceStepStarted += Time.deltaTime;
        float brightness = Mathf.Clamp01(timeSinceStepStarted / 2);
        darkness.color = new Color(0, 0, 0, 1 - brightness);
    }

    internal override AIParams GetAIParams()
    {
        return null;
    }

    internal override EngineRoomGameType GameType()
    {
        return EngineRoomGameType.LOCAL_PVP;
    }
}
