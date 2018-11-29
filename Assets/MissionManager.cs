using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

internal class MissionManager : MonoBehaviour
{
    public static MissionManager instance = null;//the single instance of design manager available
    public HackyCallback grossCallbackHack;
    public Mission mission;
    public String player1CharacterSheetPath;
    public String player2CharacterSheetPath;

    public List<Mission> missions = new List<Mission>();



    public static bool isInCutscene;
    internal static bool triggerCallbacksOnBlockDrop;
    internal static bool triggerCallbacksOnAttackHit;
    internal static bool triggerCallbacksOnShipReboot;
    internal static bool freezeAI;
    internal PointerHolder pointers;
    public EngineRoomNetworkManager engineRoomNetworkManager;

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

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        GameObject p = GameObject.Find("Pointer Holder");
        if (p != null)
        {
            pointers = p.GetComponent<PointerHolder>();
            mission.gameObject.SetActive(true);
            pointers.narrationSystem.characterController = this.grossCallbackHack;
        }
    }

}