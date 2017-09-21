using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Name: Alec Reyerson
    ID: 1826582
    Email: reyer101@mail.chapman.edu
    Course: CPSC-344-01
    Assignment: Alpha Milestone

    Description: Script for triggering the Torch object on and off
    */

// TorchTrigger
public class TorchTrigger : MonoBehaviour {
    SpriteRenderer m_OnRenderer;

    // Awake
    void Awake()
    {
        m_OnRenderer = transform.Find("lightOn").GetComponent<SpriteRenderer>();        
    }

    /*
    Name: Climb
    Parameters: Collider2D other
    */
    void OnTriggerEnter2D(Collider2D other)
    {        
        if(other.gameObject.name.Contains("Fire"))
        {
            m_OnRenderer.enabled = true;
            Destroy(other.gameObject);
        }
    }
}
