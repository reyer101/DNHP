using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Name: Alec Reyerson
ID: 1826582
Email: reyer101@mail.chapman.edu
Course: CPSC-344-01
Assignment: Gold Milestone

Description: Script for controlling boss 
*/

// BossController
public class BossController : MonoBehaviour {
    GameObject player;
    float lastSpellTime = -999f;
    public float m_SpellCD;    

	// Awake
	void Awake () {
        player = GameObject.FindGameObjectWithTag("Player");        

    }
	
	// Update 
	void Update () {
        if(Time.time - lastSpellTime > m_SpellCD)
        {
            Fire();
        }		
	}

    // Fire
    void Fire() {
        Vector3 diff = player.transform.position - transform.position;        
        float angle = Mathf.Atan2(diff.x, diff.y) * Mathf.Rad2Deg;
        Quaternion fireRotation = Quaternion.Euler(new Vector3(0, 0, -angle + 70));

        GameObject spell = (GameObject)Instantiate(Resources.Load(
               "Spells/Hairball"), new Vector3(transform.position.x - 3, transform.position.y, transform.position.z), transform.rotation);
        spell.transform.localRotation = fireRotation;
        lastSpellTime = Time.time;
    }
}
