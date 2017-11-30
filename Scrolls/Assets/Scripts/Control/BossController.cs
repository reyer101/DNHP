using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
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
    float lastSpellTime, lastPhaseSwitch;
    bool hairBallPhase, fightTriggered;    
    public float m_SpellCD, m_PhaseDuration;    

	// Awake
	void Awake () {
        lastSpellTime = -999f;
        lastPhaseSwitch = 0f;
        hairBallPhase = true;
        fightTriggered = false;
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update 
	void Update () {

        if(fightTriggered)
        {
            if (Time.time - lastPhaseSwitch > m_PhaseDuration)
            {
                Debug.Log("Phase switch");
                hairBallPhase = !hairBallPhase;
                lastPhaseSwitch = Time.time;
            }

            if (hairBallPhase)
            {
                if (Time.time - lastSpellTime > m_SpellCD)
                {
                    Fire();
                }
            }
            else
            {
                Debug.Log("Not hairball phase");
            }
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

    public void triggerFight()
    {
        // Do trasnforation anmation here
        fightTriggered = true;
    }

   /*
   Name: OnTriggerEnter2D
   Parameters: Collider2D other
   */
   void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);            
        }
    }
}
