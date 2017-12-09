using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Name: Alec Reyerson
    ID: 1826582
    Email: reyer101@mail.chapman.edu
    Course: CPSC-344-01
    Assignment: Beta Milestone

    Description: Script for triggering player death
    */

// TriggerDeath
public class TriggerDeath : MonoBehaviour {
    GameController gameController;

    // Start
    void Start() {
        gameController = GameObject.FindGameObjectWithTag("GameController")
            .GetComponent<GameController>();
    }

    /*
   Name: OnTriggerEnter2D
   Parameters: Collider2D other
   */
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Kill player");
            gameController.KillPlayer();
        }
    }
}
