using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Menus : NetworkBehaviour {


    public GameObject theMenu;



    bool pause = false;

    GameObject home;
    GameObject guest;

    GameGrid gameGridHome;
    GameGrid gameGridGuest;

    GameObject networker;


    // Use this for initialization
    void Start ()
    {



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
        Time.timeScale = 1;
        Sharedgamedata.issingleplayer = true;
        Debug.Log("crash?");
        SceneManager.LoadScene("SampleScene");
    }

    public void ToMultiplayer()
    {
        Time.timeScale = 1;
        Sharedgamedata.issingleplayer = false;
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

    public void QuitGame()
    {
        Application.Quit();
    }





}


