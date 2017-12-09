using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Name: Alec Reyerson
    ID: 1826582
    Email: reyer101@mail.chapman.edu
    Course: CPSC-344-01
    Assignment: Gold Milestone

    Description: Script for controlling ghost kitties
    */

// GhostKittyController
public class GhostKittyController : MonoBehaviour {
    public float speed;
    private GameObject player;
    
    // Start
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
    }  
	
	// Update 
	void Update () {
        transform.position = Vector2.MoveTowards(transform.position,
            player.transform.position, (speed/2f) * Time.deltaTime);       
    }

    /*
   Name: OnTriggerEnter2D
   Parameters: Collider2D other
   */
    void OnTriggerEnter2D(Collider2D other)
    {        
        if (other.gameObject.tag == "Liftable")
        {
            Vector2 hairballVelocity = other.gameObject.GetComponent<Rigidbody2D>().velocity;
            float maxVelocity = Mathf.Max(
                Mathf.Abs(hairballVelocity.x), Mathf.Abs(hairballVelocity.y));
            Debug.Log("Velocity: " + hairballVelocity);

            if (maxVelocity > 10f)
            {
                Destroy(other.gameObject);
                Destroy(gameObject);                
            }           
        }
        else if(other.gameObject.name.Contains("Fire"))
        {
            Destroy(other.gameObject);
        }
    }
}
