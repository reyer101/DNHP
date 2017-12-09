using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

/*
    Name: Alec Reyerson
    ID: 1826582
    Email: reyer101@mail.chapman.edu
    Course: CPSC-344-01
    Assignment: Beta Milestone

    Description: Script for enemy behavior
    */

// Enemy
public class Enemy : MonoBehaviour {
    public float killVelocity;
    GameObject player;    
    private Vector2 moveDirection;
    private Quaternion fireRotation, forwardRotation, backRotation;
    private GameController gameController;
    private AudioSource audio;
    private SpriteRenderer sr;
    private LayerMask m_LayerMask;
    private float initialPosition, lastAttackTime;
    private bool playerInRange;
    public float patrolRadius, playerFollowRadius, moveSpeed, attackCD;   
    public int hitPoints;

	// Awake
	void Awake () {
        lastAttackTime = -999f;
        fireRotation = new Quaternion();
        forwardRotation = transform.rotation;
        backRotation = new Quaternion(0, forwardRotation.y - 1, 0, 0);
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        player = GameObject.FindGameObjectWithTag("Player");
        audio = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        m_LayerMask = 1;
        moveDirection = Vector2.right;
        playerInRange = false;
        initialPosition = transform.position.x;
        transform.rotation = forwardRotation;
    }
    
    // FixedUpdate
    void FixedUpdate()
    {
        playerInRange = false;
        Collider2D[] pColliders = Physics2D.OverlapCircleAll(transform.position, playerFollowRadius, m_LayerMask);
        for (int i = 0; i < pColliders.Length; i++)
        {
            if (pColliders[i].gameObject.tag == "Player")
            {
                playerInRange =  true;
                Vector3 diff = pColliders[i].gameObject.transform.position - transform.position;
                //Quaternion lookAt = Quaternion.LookRotation(
                //   );
                float angle = Mathf.Atan2(diff.x, diff.y) * Mathf.Rad2Deg;
                fireRotation = Quaternion.Euler(new Vector3(0, 0, -angle + 90f));              
            }                             
        }
    }
	
	// Update
	void Update () {
        player = GameObject.FindGameObjectWithTag("Player");
        if(!playerInRange)
        {
            Patrol();
        }
        else
        {
            FacePlayer();
            Attack();
        }       
    }

    // Patrol
    void Patrol()
    {
        if(moveDirection == Vector2.right)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }    
           
        if (Mathf.Abs(transform.position.x - initialPosition) > patrolRadius)
        {            
            if (moveDirection == Vector2.right)
            {
                moveDirection = Vector2.left;                
            }
            else
            {
                moveDirection = Vector2.right;                
            }
        }       
        transform.Translate(moveDirection * (moveSpeed / 50f));
    }

    // Attack
    void Attack()
    {
        if(Time.time - lastAttackTime > attackCD)
        {
            GameObject spell = (GameObject)Instantiate(Resources.Load(
                "Spells/EnemyProjectile"), transform.position, transform.rotation);
            spell.transform.localRotation = fireRotation;
            lastAttackTime = Time.time;
            audio.clip = (AudioClip)Resources.Load(Constants.EnemySpellAudio);
            audio.Play();
        }
    }

    // FacePlayer
    void FacePlayer()
    {
        Vector3 diff = player.transform.position - transform.position;
        diff.Normalize();        
        if(diff.x < 0)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX = true;
        }        
    }

    // OnTriggerEnter2D
    void OnTriggerEnter2D(Collider2D other)
    {        
        if (other.gameObject.name.Contains("Fire"))
        {
            Destroy(other.gameObject);
            hitPoints -= 1;
            if(hitPoints == 0)
            {
                if (SceneManager.GetActiveScene().name.Contains("Turtorial"))
                {
                    gameController.roomTwoDone = true;
                }                
                Destroy(gameObject);
            }
        } 
        else if (other.gameObject.tag.Equals("Liftable") 
            || other.gameObject.tag.Contains("Boulder"))
        {
            float velocity = other.gameObject.GetComponent<Rigidbody2D>().velocity.y;
            if (velocity <= -killVelocity)
            {
                if(gameObject.name.Contains("Trigger"))
                {
                    GameObject.FindGameObjectWithTag("Checkpoint")
                        .GetComponent<BoxCollider2D>().enabled = false;
                }                
                Destroy(gameObject);                
            } 
            else if (velocity <= (.5f * -killVelocity))
            {                
                hitPoints -= 2;                
            } 
            else
            {                
                hitPoints -= 1;                
            }

            if (hitPoints <= 0)
            {                
                Destroy(gameObject);                
            }

            player.GetComponent<PlayerCharacter>().toggleTarget();                   
            Destroy(other.gameObject);
        }
    }
}
