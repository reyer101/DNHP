using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Name: Alec Reyerson
    ID: 1826582
    Email: reyer101@mail.chapman.edu
    Course: CPSC-344-01
    Assignment: Alpha Milestone

    Description: Script for performing actions based on character input.
    */

// CharacterController
public class PlayerCharacter : MonoBehaviour {
    public float m_MaxSpeed, m_ClimbSpeed, m_CrouchSpeed, m_JumpForce;    
    private bool m_Grounded, m_CanClimb;
    private Rigidbody2D m_Rigidbody2D;
    private Transform m_GroundCheck, m_ClimbCheck;
    private CircleCollider2D m_CircleCollider2D;
    private LayerMask m_LayerMask;
    Vector3 m_NormalScale, m_CrouchScale;

    private float k_GroundedRadius = .5f;
    private float k_ClimbRadius = .15f;

    // Awake
    void Awake () {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_GroundCheck = transform.Find("GroundCheck");
        m_ClimbCheck = transform.Find("ClimbCheck");
        m_NormalScale = gameObject.transform.localScale;
        m_CrouchScale = new Vector3(m_NormalScale[0], m_NormalScale[1] * .667f, m_NormalScale[2]);
        m_LayerMask = 1;        	
	}
	
	// FixedUpdate
	void FixedUpdate () {
        m_Grounded = false;
        m_CanClimb = false;
        m_Rigidbody2D.gravityScale = 2;       

        // Check if player is standing on ground by searching for colliders overlapping radius at bottom of player
        Collider2D[] gColliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_LayerMask);
        for (int i = 0; i < gColliders.Length; i++)
        {            
            if (gColliders[i].gameObject != gameObject)  // If a collider besides the one attatched to the player is found
                m_Grounded = true;                      // then the player is considerd to be grounded           
        }

        Collider2D[] cColliders = Physics2D.OverlapCircleAll(m_ClimbCheck.position, k_ClimbRadius, m_LayerMask);
        for (int i = 0; i < cColliders.Length; i++)
        {
            if (cColliders[i].gameObject.tag.Contains("Climb"))
            {  
                Debug.Log("Overlapping object: " + cColliders[i].gameObject.name);
                m_CanClimb = true;
                m_Rigidbody2D.gravityScale = 0;
            }               
        }
    }

    /*
    Name: Move
    Parameters: float horizontal, bool jump, bool crouch
    */
    public void Move(float horizontal, bool jump, bool crouch)
    {
        if (!crouch)
        {
            m_Rigidbody2D.velocity = new Vector2(horizontal * m_MaxSpeed, m_Rigidbody2D.velocity.y);
            gameObject.transform.localScale = m_NormalScale;
        }  
        else
        {
            m_Rigidbody2D.velocity = new Vector2(horizontal * m_CrouchSpeed, m_Rigidbody2D.velocity.y);
            gameObject.transform.localScale = m_CrouchScale;            
        }      
             
        if (m_Grounded && jump)
        {            
            m_Grounded = false;                
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }

    /*
    Name: Climb
    Parameters: float vertical
    */
    public void Climb(float vertical)
    {
        Debug.Log("Can climb: " + m_CanClimb);
        if (m_CanClimb)
        {            
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, vertical * m_ClimbSpeed);
        }
    }

    /* 
    Name: OnTriggerEnter2D
    Parameters: Collider 
    */
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag.Contains("Climb")) {
            m_CanClimb = true;
        }
    }

    /* 
   Name: OnTriggerExit2D
   Parameters: Collider 
   */
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag.Contains("Climb")) {
            m_CanClimb = false;
        }
    }
}
