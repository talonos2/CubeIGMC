using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuyingEquipment : MonoBehaviour {

    public PlayerCharacterSheet APlayer = new PlayerCharacterSheet();
    private bool InitializePlayer = true;

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
            APlayer = PlayerCharacterSheet.LoadFromDisk("save1.txt");
            InitializePlayer = false;
        }
        return APlayer;

    }

    internal void UpdateRequired()
    {
        InitializePlayer = true;
    }
}
