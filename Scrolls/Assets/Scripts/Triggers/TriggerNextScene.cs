using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

/*
    Name: Alec Reyerson
    ID: 1826582
    Email: reyer101@mail.chapman.edu
    Course: CPSC-344-01
    Assignment: Gold Milestone

    Description: Script for triggering next scene
    */

// TriggerNextScene
public class TriggerNextScene : MonoBehaviour {

    /*
   Name: OnTriggerEnter2D
   Parameters: Collider2D other
   */
   void OnTriggerEnter2D(Collider2D other)
   {
        if(other.tag == "Player")
        {
            SceneManager.LoadScene(Application.loadedLevel + 1);
        }        
   }
}
