using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemDisplay : MonoBehaviour {

    public int LocationInList;
    public ItemClass Item;
    public Text textName;
    public Text ItemPrice;
    public Transform ShadingPanel;
    public Sprite WpnSprite;
    public Sprite ShldSprite;
    public Sprite EngineSprite;
    public Sprite ArmorSprite;
    public Sprite PowerSprite;
    public Transform SpriteToSet;
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

    // Update is called once per frame
    void Update()
    {

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

  
    public void Prime(ItemClass item) {
            this.Item = item;
        
            textName.text = item.GetItemName();
        //if (ItemSprite != null)
        //    ItemSprite.sprite = item.sprite;
  
            ItemPrice.text = "$"+item.GetGoldCost();
        if (item.GetItemType()==ItemSlot.WEAPON)
            SpriteToSet.GetComponent<Image>().sprite = WpnSprite;
        if (item.GetItemType() == ItemSlot.SHIELDS)
            SpriteToSet.GetComponent<Image>().sprite = ShldSprite;
        if (item.GetItemType() == ItemSlot.ENGINES)
            SpriteToSet.GetComponent<Image>().sprite = EngineSprite;
        if (item.GetItemType() == ItemSlot.ARMOR)
            SpriteToSet.GetComponent<Image>().sprite = ArmorSprite;
        if (item.GetItemType() == ItemSlot.MISC)
            SpriteToSet.GetComponent<Image>().sprite = PowerSprite;
        //ItemDescription.text = item.GetItemDesc();
    }

    public void ShadeItem() {
        ShadingPanel.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.392f);
    }

    public void UnShadeItem() {
        ShadingPanel.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.392f); ;
    }

    public void ClickedItem() {
        GameObject selectionManager = GameObject.Find("SelectionManager");
        selectionManager.GetComponent<ItemSelectionManager>().ClickedItem(this.LocationInList);
        
    }

}
