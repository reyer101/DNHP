using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Name: Alec Reyerson
    ID: 1826582
    Email: reyer101@mail.chapman.edu
    Course: CPSC-344-01
    Assignment: Beta Milestone

    Description: Script for triggering checkpoint
    */

// Checkpoint
public class Checkpoint : MonoBehaviour {
    public GameController gameController;

	// OnTriggerEnter2D
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag.Equals("Player"))
        {
            GameController.currentCheckPoint = gameObject.transform.position;            
        }
    }
}
