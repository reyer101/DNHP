using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Name: Alec Reyerson
    ID: 1826582
    Email: reyer101@mail.chapman.edu
    Course: CPSC-344-01
    Assignment: Gold Milestone

    Description: Script for triggering boss fight
    */

// TriggerFight
public class TriggerFight : MonoBehaviour {
    BoxCollider2D barrier;
    bool triggered = false;

    // Awake
    void Awake()
    {
        barrier = GameObject.FindGameObjectWithTag("Barrier").GetComponent<BoxCollider2D>();
    }

    /*
    Name: OnTriggerEnter2D
    Parameters: Collider2D other
    */
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && !triggered)
        {
            Debug.Log("Trigger fight");
            GameObject.FindGameObjectWithTag(
                "Enemy").GetComponent<BossController>().triggerFight();
            barrier.enabled = true;
            triggered = true;
        }
    }
}
