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
