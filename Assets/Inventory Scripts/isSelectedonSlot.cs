using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class isSelectedonSlot : MonoBehaviour {

    public Transform ImageToTurnOn;
    public Transform GreyoutImage;
    public int Character;
    public Transform CharacterManager; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CharacterGotClicked() {
        CharacterManager.GetComponent<CharacterSelectManager>().CurrentlySelectedPosition = Character;
        CharacterManager.GetComponent<CharacterSelectManager>().CharacterIsSelected();


    }

    public void CharacterHighlighted() {
        CharacterManager.GetComponent<CharacterSelectManager>().ClearCharacterHighlighted();
        CharacterManager.GetComponent<CharacterSelectManager>().CurrentlySelectedPosition = Character;       
        GreyoutImage.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.392f);
    }

    public void CharacterUnHighlighted() {

        GreyoutImage.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.392f);
    }


}
