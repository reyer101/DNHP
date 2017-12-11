using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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
    GameObject player, hpBar;
    Animator m_Animator; 
    float lastSpellTime, lastPhaseSwitch, lastSpawnTime, fightTriggerTime, kittyDieTime;
    int totalHp;
    bool hairBallPhase, fightTriggered, fightOver;
    Vector3 minionSpawnPosition;

    public GameObject minion;
    public float m_SpellCD, m_SpawnCD, m_PhaseDuration, m_TransformTime,
        m_MinionSpeed;
    public int hp; 

	// Awake
	void Awake () {        
        lastSpellTime = -999f;
        lastSpawnTime = -999f;
        fightTriggerTime = 999f;
        kittyDieTime = 999f;
        totalHp = hp;        
        hairBallPhase = true;
        fightTriggered = false;
        minionSpawnPosition = new Vector3(transform.position.x - 10f,
            transform.position.y, transform.position.z);
        m_Animator = GetComponent<Animator>();        
        player = GameObject.FindGameObjectWithTag("Player");
        hpBar = GameObject.FindGameObjectWithTag("BossHP");
        hpBar.SetActive(false);
    }
	
	// Update 
	void Update () {

        if(fightTriggered)
        {
            if(kittyTransformed())
            {
                if(hp > 0)
                {
                    hpBar.SetActive(true);
                    player.GetComponent<AlecController>().setCanMove(true);
                    m_Animator.runtimeAnimatorController = Resources.Load(Constants.Idle)
                        as RuntimeAnimatorController;

                    if (Time.time - lastPhaseSwitch > m_PhaseDuration)
                    {
                        Debug.Log("Phase switch");
                        hairBallPhase = !hairBallPhase;

                        if (hairBallPhase)
                        {
                            clearHairballs();
                        }
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
                            //minionClone.GetComponent<Rigidbody2D>().velocity = new Vector2(-m_MinionSpeed, 0);
                            lastSpawnTime = Time.time;
                        }
                    }

                }
                else
                {
                    if(Time.time - kittyDieTime > m_TransformTime)
                    {
                        if(!fightOver)
                        {
                            fightOver = true;
                            player.GetComponent<AlecController>().setCanMove(true);
                            GetComponent<BoxCollider2D>().enabled = false;
                            GameObject.FindGameObjectWithTag("MusicManager")
                                .GetComponent<AudioSource>().Stop();
                        }                        
                    }
                    else
                    {
                        transform.position = new Vector3(
                            transform.position.x, transform.position.y - Time.deltaTime * 1.15f,
                            transform.position.y);
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
               "Spells/Hairball"), new Vector3(transform.position.x - 4,
               transform.position.y -1, transform.position.z), transform.rotation);
               spell.transform.localRotation = fireRotation;
        lastSpellTime = Time.time;
    }

    // kittyTransformed
    bool kittyTransformed()
    {
        return Time.time - fightTriggerTime > m_TransformTime;
    }

    // triggerFight
    public void triggerFight()
    {
        // Do trasnforation anmation here
        fightTriggerTime = Time.time;
        lastPhaseSwitch = Time.time;
        m_Animator.enabled = true;
        fightTriggered = true;
    }

    // clearHairballs
    void clearHairballs()
    {
        GameObject[] hairballs = GameObject.FindGameObjectsWithTag("Liftable");
        foreach(GameObject hairball in hairballs)
        {
            Destroy(hairball);
        }
    }

    // takeDamage
    void takeDamage()
    {
        hp -= 1;
        hpBar.transform.Find("HP").GetComponent<Image>().fillAmount = (float)hp / totalHp;
        if(hp <= 0)
        {            
            m_Animator.runtimeAnimatorController = Resources.Load(
                Constants.Return) as RuntimeAnimatorController;
            kittyDieTime = Time.time;
            player.GetComponent<AlecController>().setCanMove(false);
        }
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
        else if (other.gameObject.name.Contains("Fire"))
        {           
            if(kittyTransformed() && hp > 0)
            {                
                Destroy(other.gameObject);
                takeDamage();
            }
        }
    }
}
