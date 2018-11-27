using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemClass {

    private string ItemName = "Default";
    private string ItemDescription = "Default desc";
    private int ItemId = 0;
    private int ItemTier = 0;
    private int GoldCost = 1000;


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public string GetItemName() {
        return this.ItemName;
    }

    public string GetItemDesc()
    {
        return this.ItemDescription;
    }

    public int GetGoldCost() {
        return this.GoldCost;
    }
    public int GetItemId()
    {
        return this.ItemId;
    }

    public int GetItemTier() {
        return this.ItemTier;
    }

    static public ItemClass GetItem(ItemSlot thisItem, int ItemId) {
        ItemClass newItem = new ItemClass();

        switch (thisItem) {
            case ItemSlot.WEAPON:
                switch (ItemId)
                {
                    case 0:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 0;
                        newItem.ItemName = "Rock thrower";
                        newItem.ItemDescription = "Stick your head out the airlock and throw rocks. ";
                        break;
                    case 1:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "Laser Cannon, Red";
                        newItem.ItemDescription = "A large Red flashlight taped to the outer hull of the ship";
                        newItem.ItemTier = 1;
                        break;
                    case 2:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 2000;
                        newItem.ItemName = "Laser Cannon, Yellow";
                        newItem.ItemDescription = "Two flashlights this time, yellow colored. Double the power!*";
                        newItem.ItemTier = 2;
                        break;
                    case 3:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 4000;
                        newItem.ItemName = "Laser Cannon, Blue";
                        newItem.ItemDescription = "4 out of 5 doctors reccomend this blue flashlight to vaporize your enemies!";
                        newItem.ItemTier = 3;
                        break;
                    case 4:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 8000;
                        newItem.ItemName = "Laser Cannon, Violet";
                        newItem.ItemDescription = "An honest to goodness laser weapon. Can burn holes in ships and hearts.";
                        newItem.ItemTier = 4;
                        break;
                    case 5:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1500;
                        newItem.ItemName = "Catapult Cannon";
                        newItem.ItemDescription = "They thought I was crazy to build a Catapult on the top of my ship! Crazy!";
                        newItem.ItemTier = 1;
                        break;
                    case 6:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 3000;
                        newItem.ItemName = "Trebuchet Cannon";
                        newItem.ItemDescription = "When the Catapult just couldn't lob rocks fast enough!";
                        newItem.ItemTier = 2;
                        break;
                    case 7:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 6000;
                        newItem.ItemName = "Cannon Cannon";
                        newItem.ItemDescription = "The magic of Gunpowder to lob projectiles at your enemies!";
                        newItem.ItemTier = 3;
                        break;
                    case 8:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 12000;
                        newItem.ItemName = "Rail Cannon";
                        newItem.ItemDescription = "Load it with a Railway spike! See how far it goes! Newton says forever!";
                        newItem.ItemTier = 4;
                        break;
                    case 9:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 3000;
                        newItem.ItemName = "Tommy Gun";
                        newItem.ItemDescription = "I found a pair of old Tommy Guns. Taped to the side of the ship and ready to go!";
                        newItem.ItemTier = 2;
                        break;
                    case 10:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 12000;
                        newItem.ItemName = "Gatlin Gun";
                        newItem.ItemDescription = "Pew Pew Pew Pew Pew Pew Pew Pew Pew Pew... Whee!";
                        newItem.ItemTier = 4;
                        break;
                    case 11://Boss Weapon 1
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 100000;
                        newItem.ItemName = "Death Cannon";
                        newItem.ItemDescription = "Add 30% salt";
                        newItem.ItemTier = 5;
                        break;

                }
                break;

            case ItemSlot.ARMOR:
                switch (ItemId)
                {
                    case 0:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 0;
                        newItem.ItemName = "Wood Hull";
                        newItem.ItemDescription = "There may be leaks.";
                        break;
                    case 1:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "Tinfoil Hull";
                        newItem.ItemDescription = "Gaurenteed to not have holes for long!";
                        newItem.ItemTier = 1;
                        break;
                    case 2:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 2000;
                        newItem.ItemName = "Aluminum Hull";
                        newItem.ItemDescription = "Holds the ship together just fine, thank you very much!";
                        newItem.ItemTier = 2;
                        break;
                    case 3:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 4000;
                        newItem.ItemName = "Steel Hull";
                        newItem.ItemDescription = "Yes, you still have to pay for it.";
                        newItem.ItemTier = 3;
                        break;
                    case 4:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 8000;
                        newItem.ItemName = "Titanium Hull";
                        newItem.ItemDescription = "Now that's some fine grade metal.";
                        newItem.ItemTier = 4;
                        break;
                    case 5:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 12000;
                        newItem.ItemName = "Carbon Threaded Hull";
                        newItem.ItemDescription = "A girl's best friend.";
                        newItem.ItemTier = 1;
                        break;
                    
                }
                break;

            case ItemSlot.ENGINES:
                switch (ItemId)
                {
                    case 0:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 0;
                        newItem.ItemName = "Sputterig Engine";
                        newItem.ItemDescription = "Barley gets you by.";
                        break;
                    case 1:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "Jury Rigged Engine";
                        newItem.ItemDescription = "Not coughing out smoke anymore! BlockSpeed+1";
                        newItem.ItemTier = 1;
                        break;
                    case 2:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 2000;
                        newItem.ItemName = "Reparied Engine";
                        newItem.ItemDescription = "Finely tuned kinomatic machine! BlockSpeed+2";
                        newItem.ItemTier = 2;
                        break;
                    case 3:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 4000;
                        newItem.ItemName = "Firecraker Engine";
                        newItem.ItemDescription = "Now we are getting somewhere! BlockSpeed+3";
                        newItem.ItemTier = 3;
                        break;
                    case 4:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 8000;
                        newItem.ItemName = "Plasma Engine";
                        newItem.ItemDescription = "Time to roast Marshmellows! BlockSpeed+4";
                        newItem.ItemTier = 4;
                        break;
                    case 5:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 12000;
                        newItem.ItemName = "Nuclear Plasma Engine";
                        newItem.ItemDescription = "May not be safe. BlockSpeed+5";
                        newItem.ItemTier = 5;
                        break;                    
                }
                break;

            case ItemSlot.SHIELDS:
                switch (ItemId)
                {
                    case 0:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 0;
                        newItem.ItemName = "Imaginary Shield";
                        newItem.ItemDescription = "Just believe!";
                        break;
                    case 1:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "Tiny Shield";
                        newItem.ItemDescription = "It's there I promise.";
                        newItem.ItemTier = 1;
                        break;
                    case 2:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 2000;
                        newItem.ItemName = "Small Shield";
                        newItem.ItemDescription = "Now protecting the entire ship!";
                        newItem.ItemTier = 2;
                        break;
                    case 3:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 4000;
                        newItem.ItemName = "Average Shield";
                        newItem.ItemDescription = "Average Shield for the Average Ship.";
                        newItem.ItemTier = 3;
                        break;
                    case 4:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 8000;
                        newItem.ItemName = "Large Shield";
                        newItem.ItemDescription = "Protect everything with this handy dandy shield!";
                        newItem.ItemTier = 4;
                        break;
                    case 5:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1500;
                        newItem.ItemName = "Tiny Diffraction Shield";
                        newItem.ItemDescription = "Smoke and Mirrors to the rescue! Mostly Smoke.";
                        newItem.ItemTier = 1;
                        break;
                    case 6:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 3000;
                        newItem.ItemName = "Small Diffraction Shield";
                        newItem.ItemDescription = "Small Vanity mirros taped to the outside of the ship. Looking good!";
                        newItem.ItemTier = 2;
                        break;
                    case 7:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 6000;
                        newItem.ItemName = "Medium Diffraction Shield";
                        newItem.ItemDescription = "What, you thought it would be an average Mirror?";
                        newItem.ItemTier = 3;
                        break;
                    case 8:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 12000;
                        newItem.ItemName = "Large Diffraction Shield";
                        newItem.ItemDescription = "Lithographic Diffratction along a tangental lines and other gibberish math. It works I promise.";
                        newItem.ItemTier = 4;
                        break;
                    case 9:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 3000;
                        newItem.ItemName = "Drone Defence Field";
                        newItem.ItemDescription = "Launch a bunch of drone! Make them jump in the way of enemy fire!";
                        newItem.ItemTier = 2;
                        break;
                    case 10:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 12000;
                        newItem.ItemName = "Advanced Drone Defence Field";
                        newItem.ItemDescription = "Now with more drones!";
                        newItem.ItemTier = 4;
                        break;
                }
                break;

            case ItemSlot.MISC:
                switch (ItemId)
                {
                    case 0:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 0;
                        newItem.ItemName = "Generic Power Core";
                        newItem.ItemDescription = "Generic Power Core";
                        break;
                    case 1:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 2000;
                        newItem.ItemName = "Battery Reserves";
                        newItem.ItemDescription = "Load a nice large pile of batteries. Starting Energy+25";
                        newItem.ItemTier = 1;
                        break;
                    case 2:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 6000;
                        newItem.ItemName = "Passive Shield Generator";
                        newItem.ItemDescription = "Generates a small starting shield that always recovers between shots.";
                        newItem.ItemTier = 3;
                        break;                   
                }

                break;
        }


        return newItem;
    }

    internal void GetPsiPositions(CellType[,] cellTypes)
    {
        switch (ItemId)
        {
            case 0:

                break;
            case 1:

                break;
            case 2:

                break;
            case 3:

                break;
            case 4:

                break;
            case 5:

                break;
            case 6:

                break;
            case 7:

                break;
            case 8:

                break;
            case 9:

                break;
            case 10:

                break;
            case 11:

                break;
        }
    }

    internal void GetShieldPositions(CellType[,] cellTypes)
    {
        switch (ItemId)
        {
            case 0:
                break;
            case 1://2x4
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        cellTypes[12 - x, y + 10] = CellType.SHIELD;
                    }
                }               
                break;
            case 2://3/4
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        cellTypes[12 - x, y + 10] = CellType.SHIELD;
                    }
                }
                break;
            case 3://4/4
                for (int x = 0; x < 4; x++)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        cellTypes[12- x, y+10]  = CellType.SHIELD;
                    }
                }
                break;
            case 4://5/4
                for (int x = 0; x < 5; x++)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        cellTypes[12 - x, y + 10] = CellType.SHIELD;
                    }
                }
                break;
            case 5:
                for (int x = 0; x < 4; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        if (y == 0)
                            cellTypes[x+9, 13 - y] = CellType.SHIELD;
                        if (y == 1 && x < 3)
                            cellTypes[x + 9, 13 - y] = CellType.SHIELD;
                        if (y == 2 && x < 2)
                            cellTypes[x + 9, 13 - y] = CellType.SHIELD;

                    }
                }
                break;
            case 6:
                for (int x = 0; x < 5; x++)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        if (y == 0)
                            cellTypes[x + 9, 13 - y] = CellType.SHIELD;
                        if (y == 1 && x < 4)
                            cellTypes[x + 9, 13 - y] = CellType.SHIELD;
                        if (y == 2 && x < 3)
                            cellTypes[x + 9, 13 - y] = CellType.SHIELD;
                        if (y == 3 && x < 2)
                            cellTypes[x + 9, 13 - y] = CellType.SHIELD;

                    }
                }
                break;
            case 7:
                for (int x = 0; x < 6; x++)
                {
                    for (int y = 0; y < 5; y++)
                    {
                        if (y == 0)
                            cellTypes[x + 8, 14 - y] = CellType.SHIELD;
                        if (y == 1 && x < 5)
                            cellTypes[x + 8, 14 - y] = CellType.SHIELD;
                        if (y == 2 && x < 4)
                            cellTypes[x + 8, 14 - y] = CellType.SHIELD;
                        if (y == 3 && x < 3)
                            cellTypes[x + 8, 14 - y] = CellType.SHIELD;
                        if (y == 4 && x < 2)
                            cellTypes[x + 8, 14 - y] = CellType.SHIELD;

                    }
                }

                break;
            case 8:
                for (int x = 0; x < 7; x++)
                {
                    for (int y = 0; y < 6; y++)
                    {
                        if (y == 0 && x < 6)
                            cellTypes[x + 8, 14 - y] = CellType.SHIELD;
                        if (y == 1 && x < 6)
                            cellTypes[x + 8, 14 - y] = CellType.SHIELD;
                        if (y == 2 && x < 5)
                            cellTypes[x + 8, 14 - y] = CellType.SHIELD;
                        if (y == 3 && x < 4)
                            cellTypes[x + 8, 14 - y] = CellType.SHIELD;
                        if (y == 4 && x < 3)
                            cellTypes[x + 8, 14 - y] = CellType.SHIELD;
                        if (y == 5 && x < 2)
                            cellTypes[x + 8, 14 - y] = CellType.SHIELD;

                    }
                }

                break;

            case 9:
                for (int x = 0; x < 5; x++)
                {
                    for (int y = 0; y < 9; y++)
                    {
                        if (y % 2 == 0 && x % 2 == 0)
                            cellTypes[x + 8, y + 7] = CellType.SHIELD;
                    }
                }
                break;            
            case 10:
                for (int x = 0; x < 5; x++)
                {
                    for (int y = 0; y < 9; y++)
                    {
                        if (y % 2 == 0 && x % 2 == 0)
                            cellTypes[x + 8, y + 7] = CellType.SHIELD;
                        if (y % 2 == 1 && x % 2 == 1)
                            cellTypes[x + 8, y + 7] = CellType.SHIELD;
                    }
                }
                break;
            case 11:

                break;
        }

    }

    internal void GetWeaponPositions(CellType[,] cellTypes)
    {
        switch (ItemId)
        {
            case 0:
                break;
            case 1://2/4
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        cellTypes[x + 2, y + 10] = CellType.ATTACK;
                    }
                }
                break;
            case 2://3/4
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        cellTypes[x + 2, y + 10] = CellType.ATTACK;
                    }
                }
                break;
            case 3://4/4
                for (int x = 0; x < 4; x++)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        cellTypes[x + 2, y + 10] = CellType.ATTACK;
                    }
                }
                break;
            case 4://5/4
                for (int x = 0; x < 5; x++)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        cellTypes[x + 2, y + 10] = CellType.ATTACK;
                    }
                }
                break;
            case 5:
                for (int x = 0; x < 4; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        if (y==0)
                            cellTypes[6-x, 13-y] = CellType.ATTACK;
                        if (y==1&&x<3)
                            cellTypes[6 - x, 13 - y] = CellType.ATTACK;
                        if (y==2&&x<2)
                            cellTypes[6 - x, 13 - y] = CellType.ATTACK;

                    }
                }
                break;
            case 6:
                for (int x = 0; x < 5; x++)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        if (y == 0)
                            cellTypes[6 - x, 13 - y] = CellType.ATTACK;
                        if (y == 1 && x < 4)
                            cellTypes[6 - x, 13 - y] = CellType.ATTACK;
                        if (y == 2 && x < 3)
                            cellTypes[6 - x, 13 - y] = CellType.ATTACK;
                        if (y == 3 && x < 2)
                            cellTypes[6 - x, 13 - y] = CellType.ATTACK;

                    }
                }
                break;
            case 7:
                for (int x = 0; x < 6; x++)
                {
                    for (int y = 0; y < 5; y++)
                    {
                        if (y == 0)
                            cellTypes[6 - x, 14 - y] = CellType.ATTACK;
                        if (y == 1 && x < 5)
                            cellTypes[6 - x, 14 - y] = CellType.ATTACK;
                        if (y == 2 && x < 4)
                            cellTypes[6 - x, 14 - y] = CellType.ATTACK;
                        if (y == 3 && x < 3)
                            cellTypes[6 - x, 14 - y] = CellType.ATTACK;
                        if (y == 4 && x < 2)
                            cellTypes[6 - x, 14 - y] = CellType.ATTACK;

                    }
                }

                break;
            case 8:
                for (int x = 0; x < 7; x++)
                {
                    for (int y = 0; y < 6; y++)
                    {
                        if (y == 0 && x<6)
                            cellTypes[6 - x, 14 - y] = CellType.ATTACK;
                        if (y == 1 && x < 6)
                            cellTypes[6 - x, 14 - y] = CellType.ATTACK;
                        if (y == 2 && x < 5)
                            cellTypes[6 - x, 14 - y] = CellType.ATTACK;
                        if (y == 3 && x < 4)
                            cellTypes[6 - x, 14 - y] = CellType.ATTACK;
                        if (y == 4 && x < 3)
                            cellTypes[6 - x, 14 - y] = CellType.ATTACK;
                        if (y == 5 && x < 2)
                            cellTypes[6 - x, 14 - y] = CellType.ATTACK;

                    }
                }

                break;
            case 9:
                for (int x = 0; x < 5; x++)
                {
                    for (int y = 0; y < 9; y++)
                    {
                        if (y % 2 == 0 && x % 2 == 0)
                            cellTypes[x + 2, y + 7] = CellType.ATTACK;
                    }
                }
                break;
            case 10:
                for (int x = 0; x < 5; x++)
                {
                    for (int y = 0; y < 9; y++)
                    {
                        if (y % 2 == 0 && x % 2 == 0)
                            cellTypes[x + 2, y + 7] = CellType.ATTACK;
                        if (y % 2 == 1 && x % 2 == 1)
                            cellTypes[x + 2, y + 7] = CellType.ATTACK;
                    }
                }
                break;
            case 11:
                for (int x = 0; x < 5; x++)
                {
                    for (int y = 0; y <11; y++)
                    {
                        cellTypes[x + 5, y + 5] = CellType.ATTACK;
                    }
                }
                break;
        }
       

    }
}

//Alternate Weapons/shields: slots 5-8
/*
 
     aaaa
     aaa
     aa

    aaaaa
    aaaa
    aaa
    aa

    aaaaaa
    aaaaa
    aaaa
    aaa
    aa

    aaaaaaa
    aaaaaa
    aaaaa
    aaaa
    aaa
    aa

     */

/* Slot 9,10
Tier 2
 a-a-a
-----
a-a-a
-----
a-a-a
-----
a-a-a
-----
a-a-a

Tier 4
a-a-a-
-a-a-a
a-a-a-
-a-a-a
a-a-a-
-a-a-a
a-a-a-
-a-a-a
a-a-a-

 */

[System.Serializable]
public enum ItemSlot
{
    WEAPON,
    SHIELDS,
    ARMOR,
    ENGINES,
    MISC
}

