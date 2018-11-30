using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Menus : MonoBehaviour
{
    public GameObject theMenu;

    public GameObject Primary;
    public GameObject loginPage;
    public GameObject infoPage;
    public GameObject LoadCharacterPage;

    public InputField inputIP;

    bool pause = false;

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
        CrossScenePlayerData.instance.missionNumToLoad = MissionManager.MISSION_1;
        SceneManager.LoadScene("SampleScene");
    }

    public void ToMultiplayerOptions()
    {
        Primary.SetActive(false);
        loginPage.SetActive(true);
        infoPage.SetActive(true);
    }

    public void HostMultiplayer()
    {
        CrossScenePlayerData.instance.missionNumToLoad = MissionManager.ONLINE_MULTIPLAYER_HOST;
        CrossScenePlayerData.isEnteringAsHost = true;
        SceneManager.LoadScene("SampleScene");
    }

    public void joinMultiplayer()
    {
        CrossScenePlayerData.instance.missionNumToLoad = MissionManager.ONLINE_MULTIPLAYER_GUEST;
        if (inputIP.text == "")
        {
            return;
        }
        EngineRoomNetworkManager.instance.loadIPSlug(inputIP.text);
        SceneManager.LoadScene("SampleScene");
    }


    public void ToLocalMultiplayer()
    {
        CrossScenePlayerData.instance.missionNumToLoad = MissionManager.LOCAL_MULTIPLAYER;
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


