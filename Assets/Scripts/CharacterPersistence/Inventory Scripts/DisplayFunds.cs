using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayFunds : MonoBehaviour {

    public Text FundLocation;
    public Transform PlayerStorage;
    // Use this for initialization
    void Start () {
        PlayerCharacterSheet CurrentPlayer = PlayerStorage.GetComponent<PlayerBuyingEquipment>().GetPlayer();
        FundLocation.text = "Current Funds $" + CurrentPlayer.Gold;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
