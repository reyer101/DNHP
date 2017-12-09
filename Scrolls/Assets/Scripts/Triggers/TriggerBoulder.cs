using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Name: Alec Reyerson
    ID: 1826582
    Email: reyer101@mail.chapman.edu
    Course: CPSC-344-01
    Assignment: Gold Milestone

    Description: Script for performing actions based on character input.
*/

// TriggerBoulder
public class TriggerBoulder : MonoBehaviour {
    bool triggered = false;

    // OnTriggerEnter2D
	void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && !triggered)
        {
            GameObject.FindGameObjectWithTag("TutBoulder").GetComponent<Rigidbody2D>().gravityScale = 1;
            triggered = true;
        }
    }
}
