using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayItem : MonoBehaviour {
    public Text TextToDisplay;

	// Use this for initialization
	void Start () {
        TextToDisplay.text = "Welcome to the shop!";

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void DisplayItemDesc(ItemClass anItem) {
        TextToDisplay.text = anItem.GetItemDesc();
    }
}
