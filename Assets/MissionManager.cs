using System;
using UnityEngine;

internal class MissionManager : MonoBehaviour
{
    public static MissionManager instance = null;//the single instance of design manager available
    public HackyCallback grossCallbackHack;
    public Mission mission;

    public static bool isInCutscene;
    internal static bool triggerCallbacksOnBlockDrop;
    internal static bool triggerCallbacksOnAttackHit;
    internal static bool triggerCallbacksOnShipReboot;
    internal static bool freezeAI;

    public static bool TriggerCallbackOnShipDestroyed { get; internal set; }

    /// <summary>
    /// Sets up the NarrationManager's singleton design pattern - only one instance of
    /// the manager is allowed to exist and is referenced by the variable "instance"
    /// </summary>
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
        DontDestroyOnLoad(this);
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
}