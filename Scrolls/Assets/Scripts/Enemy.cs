using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Name: Alec Reyerson
    ID: 1826582
    Email: reyer101@mail.chapman.edu
    Course: CPSC-344-01
    Assignment: Alpha Milestone

    Description: Script for enemy behavior
    */

// Enemy
public class Enemy : MonoBehaviour {
    GameObject player;    
    private Vector2 moveDirection;
    private Quaternion fireRotation;
    private GameController gameController;
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
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        player = GameObject.FindGameObjectWithTag("Player");
        sr = GetComponent<SpriteRenderer>();
        m_LayerMask = 1;
        moveDirection = Vector2.right;
        playerInRange = false;
        initialPosition = transform.position.x;	
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
                Vector2 targetDirection = pColliders[i].gameObject.transform.position - transform.position;
                Quaternion lookAt = Quaternion.LookRotation(targetDirection);
                fireRotation = new Quaternion(lookAt.x, lookAt.y, 0, 0);
                Debug.Log("Player in range");
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
            Attack();
        }  
        
    }

    // Patrol
    void Patrol()
    {
        if (Mathf.Abs(transform.position.x - initialPosition) > patrolRadius)
        {
            if (moveDirection == Vector2.right)
            {
                moveDirection = Vector2.left;
                sr.flipX = false;
            }
            else
            {
                moveDirection = Vector2.right;
                sr.flipX = true;
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
                "Spells/EnemyProjectile"), transform.position, fireRotation);
            lastAttackTime = Time.time;
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
                gameController.roomTwoDone = true;
                Destroy(gameObject);
            }
        }
    }
}
