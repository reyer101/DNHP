using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Name: Alec Reyerson
    ID: 1826582
    Email: reyer101@mail.chapman.edu
    Course: CPSC-344-01
    Assignment: Beta Milestone

    Description: Script for moving the spell forward according to its current rotation
    */

// SpellMover
public class SpellMover : MonoBehaviour {
    public float m_MovementSpeed, m_RPM;    

    // Start
    void Start()
    {
        if(!gameObject.tag.Contains("Enemy"))
        {
            GetComponent<Rigidbody2D>().velocity = transform.right * m_MovementSpeed;
        }                 
    }
	
	// Update 
	void Update () {
        if (!gameObject.tag.Contains("Enemy"))
        {
            transform.Rotate(0, 0, -6.0f * m_RPM * Time.deltaTime);
        }
        else
        {
            transform.position += transform.right * Time.deltaTime * m_MovementSpeed;
        }      
    }

    void onTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
    }
}
