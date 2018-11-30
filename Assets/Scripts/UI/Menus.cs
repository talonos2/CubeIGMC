using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Menus : MonoBehaviour
{


    public GameObject theMenu;

    public MissionManager getReady;
    public Mission campaign;
    public Mission localMulti;
    public NetworkedMission onlineMulti;

    public EngineRoomNetworkManager ernm;

    public GameObject Primary;
    public GameObject loginPage;
    public GameObject infoPage;
    public GameObject LoadCharacterPage;

    public InputField inputIP;


    bool pause = false;

    GameObject home;
    GameObject guest;

    GameGrid gameGridHome;
    GameGrid gameGridGuest;

    GameObject networker;


    // Use this for initialization
    void Start ()
    {
        //Time.timeScale = 0;

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Escape"))
        {
            Pause();
        }
    }



    public void Pause()  //called by escape key and "resume" button
    {
        pause = !pause;


        if (pause)
        {
            theMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else if (!pause)
        {
            theMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void SinglePlayer()
    {
        getReady.mission = campaign;
        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
    }

    public void ToMultiplayerOptions()
    {
        Primary.SetActive(false);
        loginPage.SetActive(true);
        infoPage.SetActive(true);

//        getReady.mission = onlineMulti;
//        Time.timeScale = 1;
//        SceneManager.LoadScene("SampleScene");

    }

    public void hostMultiplayer()
    {
        //Time.timeScale = 0;
        getReady.mission = onlineMulti;
        onlineMulti.isHost = true;

        SceneManager.LoadScene("SampleScene");
    }

    public void joinMultiplayer()
    {
       // Time.timeScale = 1;
        getReady.mission = onlineMulti;
        if (inputIP.text == "")
        {
            return;
        }
        ernm.loadIPSlug(inputIP.text);
        onlineMulti.isHost = false;

        SceneManager.LoadScene("SampleScene");
    }


    public void ToLocalMultiplayer()
    {
        getReady.mission = localMulti;
        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void BackFromMulti()
    {
        loginPage.SetActive(false);
        infoPage.SetActive(false);
        Primary.SetActive(true);


    }

    public void BackFromCharacterSelect()
    {
        LoadCharacterPage.SetActive(false);
        Primary.SetActive(true);

    }

    public void QuitGame()
    {
        Application.Quit();
    }





}


