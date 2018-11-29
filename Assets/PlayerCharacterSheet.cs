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
    public int StartingEnergy=50;
    private int MaxEnergy = 200;
    private float StartingShields = 0;
    private float MaxShields = 100;
    private float ShieldDecayFactor = .8f;
    private float Psi=0;
    private float BaseEngineSpeed = 8f;
    public bool UnusedSave = true;
    public int WeaponEquippedID = 3;
    public int ArmorEquippedID = 0;
    public int ShieldEquippedID = 3;
    public int EngineEquippedID = 0;
    public int MiscEquippedID = 0;
    public int lastMissionBeaten = 0;



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
        if (BaseHealth > 100 || Level >5 || ArmorEquippedID >5)//to counter filthy cheaters
            return 10;
        return BaseHealth+Level*10+ArmorEquippedID*10;
    }

    internal int GetStartingEnergy()
    {
        if (StartingEnergy > 75)
            return 0;
        if (MiscEquippedID == 1)
            return StartingEnergy + 25;
        return StartingEnergy;
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

    internal void GetWeaponPositions(CellType[,] cellTypes)
    {
        ItemClass.GetItem(ItemSlot.WEAPON, WeaponEquippedID).GetWeaponPositions(cellTypes);
        //Pull slot locations from class next
    }

    internal void GetShieldPositions(CellType[,] cellTypes)
    {
        ItemClass.GetItem(ItemSlot.SHIELDS, ShieldEquippedID).GetShieldPositions(cellTypes);
        //Pull slot locations from class next
    }

    internal void GetPsiPositions(CellType[,] cellTypes)
    {
        ItemClass.GetItem(ItemSlot.MISC, MiscEquippedID).GetPsiPositions(cellTypes);
        //Pull slot locations from class next
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

    internal static PlayerCharacterSheet GetNPC(int NPCReference)
    {
        PlayerCharacterSheet tempSheet = new PlayerCharacterSheet();



        switch (NPCReference) { 
            case 0:
                tempSheet.Level = 0;
                tempSheet.BaseHealth = 70;
                tempSheet.WeaponEquippedID = 0;
                tempSheet.ShieldEquippedID = 0;
                break;
            case 1:
                break;
            case 2:
                tempSheet.Level = 0;
                tempSheet.BaseHealth = 25;
                tempSheet.WeaponEquippedID = 11;
                tempSheet.ShieldEquippedID = 0;
                break;

        }
        return tempSheet;



    }

    internal static PlayerCharacterSheet GetRandomNPC(int level)
    {
        PlayerCharacterSheet tempSheet = new PlayerCharacterSheet();

        tempSheet.Level = level;
        tempSheet.WeaponEquippedID = 1;
        tempSheet.ShieldEquippedID = 1;



        switch (level) {
            case 0:
                tempSheet.BaseHealth = tempSheet.BaseHealth/2;
                tempSheet.WeaponEquippedID = 1;
                tempSheet.ShieldEquippedID = 1;
                break;
            case 1:
                tempSheet.BaseHealth = 2*(tempSheet.BaseHealth / 3);
                tempSheet.WeaponEquippedID = 1;
                tempSheet.ShieldEquippedID = 1;
                break;
            case 2:
                tempSheet.WeaponEquippedID = 2;
                tempSheet.ShieldEquippedID = 2;
                break;
            case 3:
                tempSheet.WeaponEquippedID = 3;
                tempSheet.ShieldEquippedID = 3;
                break;
            case 4:
                tempSheet.WeaponEquippedID = 3;
                tempSheet.ShieldEquippedID = 3;
                break;
            case 5:
                tempSheet.WeaponEquippedID = 4;
                tempSheet.ShieldEquippedID = 4;
                break;


        }
        return tempSheet;
    }

    internal void AddEquipment(ItemClass currentlySelectedItem)
    {
        if (currentlySelectedItem.GetItemType() == ItemSlot.WEAPON)
            this.WeaponEquippedID = currentlySelectedItem.GetItemId();
        if (currentlySelectedItem.GetItemType() == ItemSlot.SHIELDS)
            this.ShieldEquippedID = currentlySelectedItem.GetItemId();
        if (currentlySelectedItem.GetItemType() == ItemSlot.ARMOR)
            this.ArmorEquippedID = currentlySelectedItem.GetItemId();
        if (currentlySelectedItem.GetItemType() == ItemSlot.ENGINES)
            this.EngineEquippedID = currentlySelectedItem.GetItemId();
        if (currentlySelectedItem.GetItemType() == ItemSlot.MISC)
            this.MiscEquippedID = currentlySelectedItem.GetItemId();
    }
}


