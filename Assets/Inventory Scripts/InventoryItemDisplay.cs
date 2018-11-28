using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemDisplay : MonoBehaviour {

    public int ItemId;
    public ItemClass Item;
    public Text textName;
    public Text ItemPrice;
    //public Text ItemDescription;
    //public Image ItemSprite;
    

	// Use this for initialization
	void Start () {

        //Item = ItemClass.GetItem(ItemSlot.WEAPON, 3);
        //Debug.Log("Item Received " + Item.GetItemDesc());
        //    Prime(Item);
		
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
