using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuyingEquipment : MonoBehaviour {

    public PlayerCharacterSheet APlayer = new PlayerCharacterSheet();
    public Transform TemporaryPlayerStore;
    private bool InitializePlayer = true;
    public string CharacterSheetLocation;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public PlayerCharacterSheet GetPlayer()
    {
        if (InitializePlayer)
        {
            Transform EngineRoomManager = GameObject.Find("EngineRoomMissionManager").transform;
            CharacterSheetLocation = EngineRoomManager.GetComponent<MissionManager>().player1CharacterSheetPath;
            APlayer = PlayerCharacterSheet.LoadFromDisk(CharacterSheetLocation);
            InitializePlayer = false;
        }
        return APlayer;

    }

    internal void UpdateRequired()
    {
        InitializePlayer = true;
    }
}
