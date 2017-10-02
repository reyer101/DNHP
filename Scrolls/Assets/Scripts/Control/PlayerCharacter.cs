using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

/*
    Name: Alec Reyerson
    ID: 1826582
    Email: reyer101@mail.chapman.edu
    Course: CPSC-344-01
    Assignment: Alpha Milestone

    Description: Script for performing actions based on character input.
    */

// PlayerCharacter
public class PlayerCharacter : MonoBehaviour {
    public float m_MaxSpeed, m_ClimbSpeed, m_CrouchSpeed, m_JumpForce, m_FireSpellCD;
    public int HP;   
    private bool m_Grounded, m_CanClimb;
    private Rigidbody2D m_Rigidbody2D;
    private Transform m_GroundCheck, m_ClimbCheck;
    private CircleCollider2D m_CircleCollider2D;
    private LayerMask m_LayerMask;
    Vector3 m_NormalScale, m_CrouchScale, m_SpellSpawnPosition;
    Quaternion m_ForwardRotation, m_BackRotation;
    private Text m_SpellText;

    private LinkedList<string> m_SpellList;
    private int currentSpellIdx;
    private float lastSpellTime, lastToggleTime;
    private float k_GroundedRadius = .5f;
    private float k_ClimbRadius = 1.0f;    

    // Awake
    void Awake () {        
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_GroundCheck = transform.Find("GroundCheck");
        m_SpellText = GameObject.FindGameObjectWithTag("SpellText").GetComponent<Text>();
        m_ClimbCheck = transform.Find("ClimbCheck");
        m_NormalScale = gameObject.transform.localScale;
        m_CrouchScale = new Vector3(m_NormalScale[0], m_NormalScale[1] * .667f, m_NormalScale[2]);
        m_SpellSpawnPosition = transform.Find("SpellSpawner").transform.position;
        m_ForwardRotation = transform.rotation;        
        m_BackRotation = new Quaternion(0, m_ForwardRotation.y - 1, 0, 0);          
        m_LayerMask = 1;
        lastSpellTime = -100f;
        lastToggleTime = -100f;
        m_SpellList = new LinkedList<string>();         
        currentSpellIdx = 0;
	}
	
	// FixedUpdate
	void FixedUpdate () {
        m_Grounded = false;
        m_CanClimb = false;        
        m_Rigidbody2D.gravityScale = 2;
        m_SpellSpawnPosition = transform.Find("SpellSpawner").transform.position;

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
        if(horizontal < 0)
        {            
            transform.rotation = m_BackRotation;
        } 
        else if(horizontal > 0)
        {            
            transform.rotation = m_ForwardRotation;
        }      

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
        if (m_CanClimb)
        {            
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, vertical * m_ClimbSpeed);
        }
    }
    
    // castSpell
    public void castSpell()
    {
        if(m_SpellList.Count > 0)
        {
            switch (m_SpellList.ElementAt(currentSpellIdx))
            {
                case "Fire":
                    if ((Time.time - lastSpellTime) > m_FireSpellCD)
                    {
                        Quaternion spawnRotation = new Quaternion(0, transform.rotation.y, 0, 0);
                        GameObject spell = (GameObject)Instantiate(Resources.Load(
                            "Spells/FireSpell"), m_SpellSpawnPosition, spawnRotation);
                        lastSpellTime = Time.time;
                    }
                    break;
                case "Water":
                    Debug.Log("Cast water spell");
                    break;
                // Other spell cases 
            }
        }          
    } 

    // toggleSpell
    public void toggleSpell()
    {
        if((Time.time - lastToggleTime) > .1f && m_SpellList.Count != 0)
        {
            if(currentSpellIdx + 1 <= m_SpellList.Count -1)
            {
                ++currentSpellIdx;
            }
            else
            {
                currentSpellIdx = 0;                
            }
            m_SpellText.text = "Spell: " + m_SpellList.ElementAt(currentSpellIdx);
        }
        lastToggleTime = Time.time;        
    }
    
    // OnTriggerEnter2D
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Contains("EnemyProjectile"))
        {
            HP -= 1;
            Destroy(other.gameObject);
            if (HP == 0)
            {
                SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
                //respawn at checkpoint
            }
        }
        else if (other.gameObject.name == "FireScroll")
        {
            Destroy(other.gameObject);
            m_SpellList.AddLast("Fire");
            m_SpellList.AddLast("Water");
            m_SpellText.text = "Spell: " + m_SpellList.ElementAt(0);
        }
    }  
}
