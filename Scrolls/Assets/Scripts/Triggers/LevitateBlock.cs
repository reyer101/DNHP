using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Name: Alec Reyerson
    ID: 1826582
    Email: reyer101@mail.chapman.edu
    Course: CPSC-344-01
    Assignment: Gold Milestone

    Description: Script for allowing levitating objects to block enemy projectiles
    */

// LevitateBlock
public class LevitateBlock : MonoBehaviour {

    /*
    Name: OnTriggerEnter2D
    Parameters: Collider2D other
    */
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "EnemyProjectile")
        {
            Destroy(other.gameObject);
        }
    }
}
