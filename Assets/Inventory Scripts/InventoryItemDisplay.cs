using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemDisplay : MonoBehaviour {

    public int ItemId;
    public ItemClass Item;
    public Text textName;
    public Text ItemPrice;
    public bool Weapon;
    public bool Shields;
    public bool Engines;
    public bool Armor;
    public bool Power;
    public PlayerCharacterSheet CurrentPlayer=new PlayerCharacterSheet();
    public Transform PlayerStorage;
    
    //public Text ItemDescription;
    //public Image ItemSprite;


    // Use this for initialization
    void Start () {

        RefreshEquipment();
       


        //Item = ItemClass.GetItem(ItemSlot.WEAPON, 3);
        //Debug.Log("Item Received " + Item.GetItemDesc());
        //    Prime(Item);

    }

    public void RefreshEquipment()
    {
        if (PlayerStorage != null)
        {
            CurrentPlayer = PlayerStorage.GetComponent<PlayerBuyingEquipment>().GetPlayer();
        }
        //CurrentPlayer = PlayerBuyingEquipment.GetPlayer();
        if (Weapon)
        {
            Item = ItemClass.GetItem(ItemSlot.WEAPON, CurrentPlayer.WeaponEquippedID);
            Prime(Item);
        }
        if (Shields)
        {
            Item = ItemClass.GetItem(ItemSlot.SHIELDS, CurrentPlayer.ShieldEquippedID);
            Prime(Item);
        }
        if (Engines)
        {
            Item = ItemClass.GetItem(ItemSlot.ENGINES, CurrentPlayer.EngineEquippedID);
            Prime(Item);
        }
        if (Armor)
        {
            Item = ItemClass.GetItem(ItemSlot.ARMOR, CurrentPlayer.ArmorEquippedID);
            Prime(Item);

        }
        if (Power)
        {
            Item = ItemClass.GetItem(ItemSlot.MISC, CurrentPlayer.MiscEquippedID);
            Prime(Item);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
    public void Prime(ItemClass item) {
            this.Item = item;
        
            textName.text = item.GetItemName();
        //if (ItemSprite != null)
        //    ItemSprite.sprite = item.sprite;
  
            ItemPrice.text = "$"+item.GetGoldCost();
        //ItemDescription.text = item.GetItemDesc();
    }
}
