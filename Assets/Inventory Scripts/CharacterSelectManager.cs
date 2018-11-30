using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour {

    public Transform SaveSlot1;
    public Transform SaveSlot2;
    public Transform SaveSlot3;
    public Transform isSelected1;
    public Transform isSelected2;
    public Transform isSelected3;
    public Transform SelectedImage1;
    public Transform SelectedImage2;
    public Transform SelectedImage3;
    public Transform MissionManaged;
    public Transform ACanvas;
    public GameObject MainMenu;
    public GameObject LoadCharMenu;
    public Text LoadText;

    public int CurrentlySelectedPosition=1;
    public string SaveFile1 = "Save1.txt";
    public string SaveFile2 = "Save2.txt";
    public string SaveFile3 = "Save3.txt";
    private int TimeSinceLastInput = 11;
    private int ScrollingRight = 0;
    //private Mission missionWeAreGoingTo;
    public PlayerCharacterSheet Character1;
    public PlayerCharacterSheet Character2;
    public PlayerCharacterSheet Character3;
    private int GameType =0;

    private int ScrollingLeft = 0;




    // Use this for initialization
    void Start () {
        //PlayerCharacterSheet.LoadFromDisk("Save1.txt")
        string dataPath = Path.Combine(Application.persistentDataPath, SaveFile1);
        if (!File.Exists(dataPath))
        {
            PlayerCharacterSheet.SaveToDisk(new PlayerCharacterSheet(), dataPath);
        }
        Character1 = PlayerCharacterSheet.LoadFromDisk(SaveFile1);

        dataPath = Path.Combine(Application.persistentDataPath, SaveFile2);
        if (!File.Exists(dataPath))
        {
            PlayerCharacterSheet.SaveToDisk(new PlayerCharacterSheet(), dataPath);
        }
        Character2 = PlayerCharacterSheet.LoadFromDisk(SaveFile2);

        dataPath = Path.Combine(Application.persistentDataPath, SaveFile3);
        if (!File.Exists(dataPath))
        {
            PlayerCharacterSheet.SaveToDisk(new PlayerCharacterSheet(), dataPath);
        }

        Character3 = PlayerCharacterSheet.LoadFromDisk(SaveFile3);

    }

    private void OnEnable()
    {
        CurrentlySelectedPosition = 1;
        isSelected1.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.392f);
        //missionWeAreGoingTo = MissionManaged.GetComponent<MissionManager>().mission;
    }
    // Update is called once per frame
    void Update () {


        if (Input.GetKeyUp(KeyCode.Return)) {
            CharacterIsSelected();
        }
        if (Input.GetAxis("Submit")>.1) {
            CharacterIsSelected();
        }




        if (TimeSinceLastInput < 40)
            TimeSinceLastInput++;

        if (TimeSinceLastInput > 20)
        {
            ScrollingRight = 0;
            ScrollingLeft = 0;
        }

       

        if (Input.GetAxis("Horizontal") > .1f  && TimeSinceLastInput > 10)
        {
            if (ScrollingRight == 0 || ScrollingRight > 1)
            {
                if (CurrentlySelectedPosition == 1)
                {
                    isSelected1.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.392f);
                    isSelected2.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.392f);
                    CurrentlySelectedPosition = 2;
                }
                else if (CurrentlySelectedPosition == 2)
                {
                    isSelected2.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.392f);
                    isSelected3.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.392f);
                    CurrentlySelectedPosition = 3;
                }
                else if (CurrentlySelectedPosition == 3)
                {
                    isSelected3.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.392f);
                    isSelected1.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.392f);
                    CurrentlySelectedPosition = 1;

                }
            }
            TimeSinceLastInput = 0;
            ScrollingRight++;
        }

        if (Input.GetAxis("Horizontal") < -.1f  && TimeSinceLastInput > 10)
        {
            if (ScrollingLeft == 0 || ScrollingLeft > 1)
            {

                if (CurrentlySelectedPosition == 1) {
                    isSelected1.GetComponent<Image>().color=new Color(1f, 1f, 1f, 0.392f);
                    isSelected3.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.392f);
                    CurrentlySelectedPosition = 3;
                }
                else if (CurrentlySelectedPosition == 2)
                {
                    isSelected2.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.392f);
                    isSelected1.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.392f);
                    CurrentlySelectedPosition = 1;
                }
                else if (CurrentlySelectedPosition == 3)
                {
                    isSelected3.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.392f);
                    isSelected2.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.392f);
                    CurrentlySelectedPosition = 2;

                }

                
          
            }
            TimeSinceLastInput = 0;
            ScrollingLeft++;
        }


    }

    internal void ClearCharacterHighlighted()
    {
        if (CurrentlySelectedPosition == 1)
        {
            isSelected1.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.392f);
        }
        if (CurrentlySelectedPosition == 2)
        {
            isSelected2.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.392f);
        }
        if (CurrentlySelectedPosition == 3)
        {
            isSelected3.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.392f);
        }
    }

    public void CharacterIsSelected()
    {
        string SelectedCharacterPath = "";
        if (CurrentlySelectedPosition == 1) {
            //SelectedImage1.GetComponent<Image>().enabled = true;
            SelectedCharacterPath = SaveFile1;
        }
        if (CurrentlySelectedPosition == 2)
        {
            //SelectedImage2.GetComponent<Image>().enabled = true;
            SelectedCharacterPath = SaveFile2;
        }
        if (CurrentlySelectedPosition == 3)
        {
            //SelectedImage3.GetComponent<Image>().enabled = true;
            SelectedCharacterPath = SaveFile3;
        }

        LoadCharMenu.SetActive(false);
        MainMenu.SetActive(true);

        if (GameType == 1) {
            MissionManaged.GetComponent<MissionManager>().player1CharacterSheetPath = SelectedCharacterPath;
            ACanvas.GetComponent<Menus>().SinglePlayer();
        }
        else if (GameType == 2) {
            MissionManaged.GetComponent<MissionManager>().player1CharacterSheetPath = SelectedCharacterPath;
            MainMenu.SetActive(false);
            LoadCharMenu.SetActive(true);
            LoadText.text = "Select Player 2:";
            GameType = 3;

            //RefreshCharacterSheet();
        }
        else if (GameType == 3) {
            Debug.Log("Did I get here");
            MissionManaged.GetComponent<MissionManager>().player2CharacterSheetPath = SelectedCharacterPath;
            ACanvas.GetComponent<Menus>().ToLocalMultiplayer();
        }
        else if (GameType == 4) {
            MissionManaged.GetComponent<MissionManager>().player1CharacterSheetPath = SelectedCharacterPath;
            ACanvas.GetComponent<Menus>().ToMultiplayerOptions();
        }




    }

    public void SinglePlayerGetChar() {
      //  LoadCharMenu.SetActive(true);
       // MainMenu.SetActive(false);
        GameType = 1;
        ACanvas.GetComponent<Menus>().SinglePlayer();
    }

    public void MultiPlayerGetChar() {
      //  LoadCharMenu.SetActive(true);
     //   MainMenu.SetActive(false);
     //   GameType = 2;
     //   LoadText.text = "Select Player 1:";
       // if (GameType == 2) {
           // GameType = 3;
           // ResetCharacterSelect();
        //}
        ACanvas.GetComponent<Menus>().ToLocalMultiplayer();
    }

    public void NetworkMultiplayerGetChar() {
        //LoadCharMenu.SetActive(true);
        //MainMenu.SetActive(false);
        GameType = 4;
        ACanvas.GetComponent<Menus>().ToMultiplayerOptions();
        //ACanvas.GetComponent<Menus>().ToMultiplayerOptions();
    }

}
