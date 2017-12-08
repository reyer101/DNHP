using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/*
    Name: Alec Reyerson
    ID: 1826582
    Email: reyer101@mail.chapman.edu
    Course: CPSC-344-01
    Assignment: Alpha Milestone

    Description: Script for managing game state and events
    */

// GameController
public class GameController : MonoBehaviour {
    public Vector2 currentCheckPoint;
    public GameObject barrierOne, barrierTwo;
    public bool roomOneDone, roomTwoDone;
    private Text tutText;    

	// Awake
	void Awake () {        
        roomOneDone = false;
        roomTwoDone = false;	
	}
	
	// Update
	void Update () {        
        if(roomOneDone)
        {            
            barrierOne.GetComponent<BoxCollider2D>().enabled = false;
        }    
        if(roomTwoDone)
        {
            barrierTwo.GetComponent<BoxCollider2D>().enabled = false;
        }    		
	}
}
