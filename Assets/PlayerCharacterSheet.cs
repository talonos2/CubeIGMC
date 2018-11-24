using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class PlayerCharacterSheet 
{
    /* 
     * This class is designed to hold all possible ship variables to determine the base
     * statistics of a ship/player.
     * This class has a save and load function attached to it as static methods.
     * Not all game affecting attributes have been added yet, but so far this script 
     * controls: health, max energy, starting shields, max shields, and engine speed. 
     * Combatant.cs class is to actually control a battle, the above is just initial stats. 
     * 
     * Later plans include more item implementations and class implementations. 
     * 
     * There is also an example in Combatant.cs Start function on how to differentiate between
     * player 1 and 2 in terms of stats. Later we will need to add differntation for different 
     * NPC's, local multiplayer, and internet multiplayer. 
     */

    public string PlayerName ="Unknown Captain";
    public float Level = 1f;
    public int Exp=0;
    public int Gold=0;
    public float BaseHealth=70;
    public int StartingEnergy=25;
    public int MaxEnergy = 200;
    public float StartingShields = 0;
    public float MaxShields = 100;
    public float ShieldDecayFactor = .8f;
    public float Psi=0;
    public float BaseEngineSpeed = 8f;
    public bool UnusedSave = true;
    public int WeaponEquippedID = 0;
    public int ArmorEquippedID = 0;
    public int ShieldEquippedID = 0;
    public int EngineEquippedID = 0;
    public int MiscEquippedID = 0;



    // Use this for initialization
    void Start () {
        //Use: if (transform.parent.parent.parent.name.Equals("Player2")) {
        //  ThisPlayer.BaseHealth = 30;
        //}
       //Debug.Log(transform.parent.name);
        
}
	
	// Update is called once per frame
	void Update () {
		
	}

    public float GetMovementSpeed() {
        return BaseEngineSpeed + (float)EngineEquippedID;
    }

    public float GetMaxHealth() {
        return BaseHealth+Level*10;
    }

    public int GetMaxEnergy() {
        return MaxEnergy;
    }

    public float GetStartingShields() {
        return StartingShields;
    }

    internal float GetMaxShields()
    {
        return MaxShields;
    }

    internal float GetShieldDecayFactor()
    {
        return ShieldDecayFactor;
    }


    public static void SaveToDisk(PlayerCharacterSheet data, string savedFileName) {

        //Usage: 
        // string FileName = "SaveFile3.txt";
        // PlayerCharacterSheet.SaveToDisk(ThisPlayer, FileName);
        //ThisPlayer = PlayerCharacterSheet.LoadFromDisk(FileName);

        string dataPath;
        dataPath = Path.Combine(Application.persistentDataPath, savedFileName);
        Debug.Log(dataPath);
        string savedString = JsonUtility.ToJson(data);
        using (StreamWriter streamWriter = File.CreateText(dataPath)) {
            streamWriter.Write(savedString);
        }
    }

    public static PlayerCharacterSheet LoadFromDisk(string savedFileName) {
        string dataPath;
        dataPath=Path.Combine(Application.persistentDataPath, savedFileName);
        Debug.Log(dataPath);
        using (StreamReader streamReader = File.OpenText(dataPath)) {
            string loadedString = streamReader.ReadToEnd();
            return JsonUtility.FromJson<PlayerCharacterSheet>(loadedString);
        }
    }

   
}
