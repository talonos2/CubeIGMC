using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour {


    public GameObject pauseMenu;
    bool pause = false;


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
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else if (!pause)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }





}
