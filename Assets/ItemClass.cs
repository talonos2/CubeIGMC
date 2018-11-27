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
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 1:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 1;
                        break;
                    case 2:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 2;
                        break;
                    case 3:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 3;
                        break;
                    case 4:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 4;
                        break;
                    case 5:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 1;
                        break;
                    case 6:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 2;
                        break;
                    case 7:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 3;
                        break;
                    case 8:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 4;
                        break;
                    case 9:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 2;
                        break;
                    case 10:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 4;
                        break;
                    case 11://Boss Weapon 1
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 5;
                        break;

                }
                break;

            case ItemSlot.ARMOR:
                switch (ItemId)
                {
                    case 0:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 1:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 1;
                        break;
                    case 2:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 2;
                        break;
                    case 3:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 3;
                        break;
                    case 4:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 4;
                        break;
                    case 5:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 1;
                        break;
                    case 6:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 2;
                        break;
                    case 7:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 3;
                        break;
                    case 8:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 4;
                        break;
                    case 9:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 2;
                        break;
                    case 10:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 4;
                        break;
                }
                break;

            case ItemSlot.ENGINES:
                switch (ItemId)
                {
                    case 0:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 1:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 1;
                        break;
                    case 2:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 2;
                        break;
                    case 3:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 3;
                        break;
                    case 4:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 4;
                        break;
                    case 5:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 1;
                        break;
                    case 6:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 2;
                        break;
                    case 7:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 3;
                        break;
                    case 8:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 4;
                        break;
                    case 9:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 2;
                        break;
                    case 10:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 4;
                        break;
                }
                break;

            case ItemSlot.SHIELDS:
                switch (ItemId)
                {
                    case 0:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 1:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 1;
                        break;
                    case 2:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 2;
                        break;
                    case 3:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 3;
                        break;
                    case 4:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 4;
                        break;
                    case 5:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 1;
                        break;
                    case 6:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 2;
                        break;
                    case 7:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 3;
                        break;
                    case 8:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 4;
                        break;
                    case 9:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 2;
                        break;
                    case 10:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 4;
                        break;
                }
                break;

            case ItemSlot.MISC:
                switch (ItemId)
                {
                    case 0:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 1:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 1;
                        break;
                    case 2:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 2;
                        break;
                    case 3:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 3;
                        break;
                    case 4:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 4;
                        break;
                    case 5:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 1;
                        break;
                    case 6:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 2;
                        break;
                    case 7:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 3;
                        break;
                    case 8:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 4;
                        break;
                    case 9:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 2;
                        break;
                    case 10:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        newItem.ItemTier = 4;
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
               

                break;
            case 6:
                
                break;
            case 7:
                
                break;
            case 8:
               
                break;
            case 9:
                for (int x = 0; x < 5; x++)
                {
                    for (int y = 0; y < 9; y++)
                    {
                        cellTypes[x + 2, y + 7] = CellType.ATTACK;
                    }
                }
                break;
            case 10:
                for (int x = 0; x < 6; x++)
                {
                    for (int y = 0; y < 9; y++)
                    {
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

