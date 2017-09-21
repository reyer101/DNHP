using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Name: Alec Reyerson
    ID: 1826582
    Email: reyer101@mail.chapman.edu
    Course: CPSC-344-01
    Assignment: Alpha Milestone

    Description: Script for moving the spell forward according to its current rotation
    */

// SpellMover
public class SpellMover : MonoBehaviour {
    public float m_MovementSpeed;
	
	// Update 
	void Update () {
        transform.position += transform.right * Time.deltaTime * m_MovementSpeed;
    }

    void onTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Other object: " + other.gameObject.name);
        if (!other.gameObject.name.Contains("Player"))
        {
            Destroy(gameObject);
        }
    }
}
