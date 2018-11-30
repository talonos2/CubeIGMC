using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour
{
    public static MissionManager instance = null;

    //These correspond to the missions, in whatever way they're slotted into the MissionManager in GameScreen.
    //NOTE: If you change the order of the levels, it's your responsibility to update these numbers!
    public const int LOCAL_MULTIPLAYER = 0;
    public const int ONLINE_MULTIPLAYER_HOST = 1;
    public const int ONLINE_MULTIPLAYER_GUEST = 2;
    public const int RANDOM_SINGLE_PLAYER_MATCH = 3;
    public const int MISSION_1 = 4;
    public const int MISSION_2 = 5;
    public const int MISSION_3 = 6;

    //This is necessary to interact with the dialogue system, I think, but the least we can do is hide it.
    private HackyCallback grossCallbackHack;

    //Needed to know which mission to unblock.
    private Mission mission;

    //A list of missions. MissionManager queries the PlayerData to find out what mission to run, then runs it on awake. TODO
    public List<Mission> missions = new List<Mission>();

    //COntrols to freeze boards.
    internal static bool freezePlayerBoard;
    internal static bool freezeAI;

    public CommonMissionScriptingTargets pointers;

    //Callback booleans: Enable to "subscribe" to that callback and call Unblock when that event triggers.
    public static bool triggerCallbackOnShipDestroyed;
    public static bool triggerCallbacksOnBlockDrop;
    public static bool triggerCallbacksOnAttackHit;
    public static bool triggerCallbacksOnShipReboot;
    public static bool triggerCallbackOnRemotePlayerConnected;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        grossCallbackHack = transform.Find("This is a Callback").gameObject.GetComponent<HackyCallback>();

        //I *think* this is the right place to call this:
        if (CrossScenePlayerData.instance.missionNumToLoad != -1)
        {
            mission = missions[CrossScenePlayerData.instance.missionNumToLoad];
        }
    }

    public void Update()
    {
        //I have no idea how to do callbacks, so let's pretend this is how I'm supposed to do it.
        if (grossCallbackHack.enabled)
        {
            mission.Unblock();
            grossCallbackHack.enabled = false;
        }
    }

    public void DelayedCallback()
    {
        grossCallbackHack.enabled = true;
    }

    internal EngineRoomGameType GameType()
    {
        return mission.GameType();
    }

    internal AIParams GetAIParams()
    {
        return mission.GetAIParams();
    }
}