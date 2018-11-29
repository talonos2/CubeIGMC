using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseManager : MonoBehaviour {
    public Transform PlayerStorage;
    public Text DescriptionTextBox;
    public Text ErrorTextBox;
    public Text CurrentFundsBox;
    public Text TransactionDetails;
    public Transform WeaponEquipped;
    public Transform ShieldEquipped;
    public Transform EnginesEquipped;
    public Transform ArmorEquipped;
    public Transform PowerEquipped; 
    
    // Use this for initialization
    void Start () {
        //PlayerCharacterSheet CurrentPlayer = PlayerStorage.GetComponent<PlayerBuyingEquipment>().GetPlayer();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateDescriptionText(ItemClass AnItem)
    {


    }

    public void AttemptToPurchase(ItemClass CurrentlySelectedItem) {
        PlayerCharacterSheet CurrentPlayer = PlayerStorage.GetComponent<PlayerBuyingEquipment>().GetPlayer();
        //ItemClass CurrentlySelectedItem = ItemClass.GetItem(ItemSlot.WEAPON, 3);
        ItemClass OldItem = CurrentPlayer.GetItem(CurrentlySelectedItem.GetItemType());

        DescriptionTextBox.text = CurrentlySelectedItem.GetItemDesc();
        TransactionDetails.text="Purchase Price $" + CurrentlySelectedItem.GetGoldCost() + ", Trade in Value $" + OldItem.GetGoldCost()
            +", \nNet Purchase price $"+(CurrentlySelectedItem.GetGoldCost()- OldItem.GetGoldCost());
        if (CurrentlySelectedItem.GetGoldCost() > (CurrentPlayer.Gold + OldItem.GetGoldCost())) {
            ErrorTextBox.text = "Error, not enough funds.";
            return;
        }
        CurrentPlayer.Gold = CurrentPlayer.Gold - (CurrentlySelectedItem.GetGoldCost() - OldItem.GetGoldCost());
        CurrentPlayer.AddEquipment(CurrentlySelectedItem);
        CurrentFundsBox.text = "Current Funds $" + CurrentPlayer.Gold;
        PlayerCharacterSheet.SaveToDisk(CurrentPlayer, "save1.txt");        
        PlayerStorage.GetComponent<PlayerBuyingEquipment>().UpdateRequired();
        WeaponEquipped.GetComponent<InventoryItemDisplay>().RefreshEquipment();
        ArmorEquipped.GetComponent<InventoryItemDisplay>().RefreshEquipment();
        ShieldEquipped.GetComponent<InventoryItemDisplay>().RefreshEquipment();
        PowerEquipped.GetComponent<InventoryItemDisplay>().RefreshEquipment();
        EnginesEquipped.GetComponent<InventoryItemDisplay>().RefreshEquipment();

    }
}
