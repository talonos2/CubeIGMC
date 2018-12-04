using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScreenMenus : MonoBehaviour {

    public GameObject pauseMenu;
    public AudioSource nopeSound;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Escape"))
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void MenuQuit()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void MenuResume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void MenuNextMission()
    {
        CrossScenePlayerData.instance.missionNumToLoad++;
        SceneManager.LoadScene("SampleScene");
    }

    public void MenuRestart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
    }

    public void MenuBuyParts()
    {
        //CrossScenePlayerData.instance.missionNumToLoad++;
        //SceneManager.LoadScene("ShopMenu");
        nopeSound.Play();
    }
}
