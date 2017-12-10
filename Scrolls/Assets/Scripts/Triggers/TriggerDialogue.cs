using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Name: Alec Reyerson
    ID: 1826582
    Email: reyer101@mail.chapman.edu
    Course: CPSC-344-01
    Assignment: Gold Milestone

    Description: Script for triggering dialogue
    */

// TriggerDialogue
public class TriggerDialogue : MonoBehaviour {
    public int start, end;
    public bool triggersNextLevel;

    private GameController gameController;
    private bool triggered;

	// Start
	void Start () {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        triggered = false;	
	}

    /*
    Name: OnTriggerEnter2D
    Parameters: Collider2D other
    */
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && !triggered)
        {
            Debug.Log("Triggered Dialogue");
            gameController.showDialogueBox(start, end, triggersNextLevel);
            triggered = true;
        }
    }
}
