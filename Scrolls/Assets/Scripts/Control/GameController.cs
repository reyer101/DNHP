using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/*
    Name: Alec Reyerson
    ID: 1826582
    Email: reyer101@mail.chapman.edu
    Course: CPSC-344-01
    Assignment: Beta Milestone

    Description: Script for managing game state and events
    */

// GameController
public class GameController : MonoBehaviour {
    public static Vector2 currentCheckPoint;
    public GameObject barrierOne, barrierTwo;
    public bool roomOneDone, roomTwoDone;
    private GameObject player;
    private Text tutText;    

	// Awake
	void Awake () {
        player = GameObject.FindGameObjectWithTag("Player");
        currentCheckPoint = player.transform.position;     
        roomOneDone = false;
        roomTwoDone = false;        
    }
	
	// Update
	void Update () {
        player = GameObject.FindGameObjectWithTag("Player");      
        if(roomOneDone)
        {            
            barrierOne.GetComponent<BoxCollider2D>().enabled = false;
        }    
        if(roomTwoDone)
        {
            barrierTwo.GetComponent<BoxCollider2D>().enabled = false;
        }    		
	}

    // KillPlayer
    public void KillPlayer()
    {
        Debug.Log("GameController.KillPlayer");
        //player.transform.position = currentCheckPoint;              
    }
}
