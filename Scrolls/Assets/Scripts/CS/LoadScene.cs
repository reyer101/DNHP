//Carissa Smith
//1800292
//smith426@mail.chapman.edu
//CPSC-344-01
//Scrolls
//This script's purpose is to load scenes within the game. 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

	public string sceneName;

	//load
	public void load() {
        GetComponent<AudioSource>().Play();
        if(sceneName == "PlayerInfo")
        {
            PlayerPrefs.DeleteAll();
        }
		SceneManager.LoadScene(sceneName);
	}

    // Quit
    public void Quit()
    {  
        GetComponent<AudioSource>().Play();      
        Application.Quit();
    }

    // Resume
    public void Resume()
    {
        GetComponent<AudioSource>().Play();
        string savedScene = PlayerPrefs.GetString(Constants.Scene, "");
        if(savedScene != "")
        {
            SceneManager.LoadScene(savedScene);
        }
        else
        {
            load();
        }
    }
}

