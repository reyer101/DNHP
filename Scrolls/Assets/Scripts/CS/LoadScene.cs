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
		SceneManager.LoadScene(sceneName);
	}
<<<<<<< HEAD

	public void quit(){

		Application.Quit();
	}

}


=======
}

>>>>>>> origin/kj_dev
