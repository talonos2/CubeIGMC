using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseManager : MonoBehaviour {
    public Transform PlayerStorage;
    public Text DescriptionTextBox;
    public Text ErrorTextBox;
    public Text CurrentFundsBox;
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

    public void AttemptToPurchase() {
        PlayerCharacterSheet CurrentPlayer = PlayerStorage.GetComponent<PlayerBuyingEquipment>().GetPlayer();
        ItemClass CurrentlySelectedItem = ItemClass.GetItem(ItemSlot.WEAPON, 3);
        DescriptionTextBox.text = CurrentlySelectedItem.GetItemDesc();
        if (CurrentlySelectedItem.GetGoldCost() > CurrentPlayer.Gold) {
            ErrorTextBox.text = "Error, not enough funds.";
            return;
        }
        CurrentPlayer.Gold = CurrentPlayer.Gold - CurrentlySelectedItem.GetGoldCost();
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
