using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemClass {

    private string ItemName="Default";
    private string ItemDescription="Default desc";
    private int ItemId=0;
    private int GoldCost=1000;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
                        break;
                    case 2:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 3:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 4:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 5:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 6:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 7:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 8:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 9:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
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
                        break;
                    case 2:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 3:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 4:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 5:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 6:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 7:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 8:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 9:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                }
                break;

            case ItemSlot.ENGINES:
                switch (ItemId) {
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
                        break;
                    case 2:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 3:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 4:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 5:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 6:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 7:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 8:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 9:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
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
                        break;
                    case 2:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 3:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 4:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 5:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 6:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 7:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 8:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 9:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
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
                        break;
                    case 2:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 3:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 4:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 5:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 6:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 7:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 8:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                    case 9:
                        newItem.ItemId = ItemId;
                        newItem.GoldCost = 1000;
                        newItem.ItemName = "tbi";
                        newItem.ItemDescription = "tbi desc";
                        break;
                }
                break;
        }


        return newItem;
    }


}

[System.Serializable]
public enum ItemSlot
{
    WEAPON,
    SHIELDS,
    ARMOR,
    ENGINES,
    MISC
}

