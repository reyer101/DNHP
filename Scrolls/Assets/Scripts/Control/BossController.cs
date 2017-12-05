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
    Animator m_Animator;
    float lastSpellTime, lastPhaseSwitch, lastSpawnTime, fightTriggerTime;
    bool hairBallPhase, fightTriggered;
    Vector3 minionSpawnPosition;

    public GameObject minion;
    public float m_SpellCD, m_SpawnCD, m_PhaseDuration, m_TransformTime,
        m_MinionSpeed;    

	// Awake
	void Awake () {
        lastSpellTime = -999f;
        lastSpawnTime = -999f;
        fightTriggerTime = 999f;
        lastPhaseSwitch = 0f;
        hairBallPhase = false;
        fightTriggered = false;
        minionSpawnPosition = new Vector3(transform.position.x - 10f,
            transform.position.y - .25f, transform.position.z);
        m_Animator = GetComponent<Animator>();        
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update 
	void Update () {

        if(fightTriggered)
        {
            if(Time.time - fightTriggerTime > m_TransformTime)
            {
                Debug.Log("Animation over");
                player.GetComponent<AlecController>().setCanMove(true);
                m_Animator.runtimeAnimatorController = Resources.Load(Constants.Idle)
                    as RuntimeAnimatorController;

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
                    // Should spawn minions here
                    Debug.Log("Should be spawning minions");
                    if (Time.time - lastSpawnTime > m_SpawnCD)
                    {
                        GameObject minionClone = Instantiate(minion, minionSpawnPosition, transform.rotation);
                        minionClone.GetComponent<Rigidbody2D>().velocity = new Vector2(-m_MinionSpeed, 0);
                        lastSpawnTime = Time.time;
                    }                    
                }
            }
            else
            {
                // Kitty is transforming during this time
                player.GetComponent<AlecController>().setCanMove(false);
                transform.position = new Vector3(
                transform.position.x, transform.position.y + Time.deltaTime * 1.15f,
                transform.position.y);
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
        fightTriggerTime = Time.time;
        m_Animator.enabled = true;
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
