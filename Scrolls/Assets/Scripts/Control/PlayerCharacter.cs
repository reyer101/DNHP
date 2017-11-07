using System.Collections;
using System;
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
    Assignment: Beta Milestone

    Description: Script for performing actions based on character input.
    */

// PlayerCharacter
public class PlayerCharacter : MonoBehaviour {
    public GameObject m_LevitateTarget;
    public LinkedList<GameObject> m_LevitateTargets;    
    public float m_MaxSpeed, m_ClimbSpeed, m_CrouchSpeed, m_JumpForce, 
        m_FireSpellCD, m_LevitateRadius, m_LevitateSpeed;
    public int HP;
    public bool m_DropWhenOutOfRange, m_CanLevitateAndMove;    
    private bool m_Grounded, m_CanClimb, m_HasSpell, m_LeviateDisabled, m_;
    private AudioSource m_Audio;
    private Animator m_Animator;  
    private Rigidbody2D m_Rigidbody2D;
    private Transform m_GroundCheck, m_ClimbCheck;
    private CircleCollider2D m_CircleCollider2D;
    private LayerMask m_LayerMask;    
    private Vector3 m_NormalScale, m_CrouchScale, m_SpellSpawnPosition;
    private Quaternion m_ForwardRotation, m_BackRotation;
    private Color m_Highlight;
    private Text m_SpellText, m_HPText, m_CDText, m_NameText;
    private Image m_Witch, m_Wizard;

    private LinkedList<string> m_SpellList;    
    private int currentSpellIdx, spriteIndex, targetIndex;
    private float lastFireSpellTime, lastLevitateTime, lastToggleTime;
    private float k_GroundedRadius = .5f;   
    private float k_ClimbRadius = 1.0f;
    private float k_UnderRadius = 1f;
    private float m_LevitateCD = 1f;
    public float k_GroundDistance;
         

    // Awake
    void Awake () {
        m_HasSpell = false;        
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Audio = GetComponent<AudioSource>();
        m_Animator = GetComponent<Animator>();
        m_GroundCheck = transform.Find("GroundCheck");
        m_Witch = GameObject.FindGameObjectWithTag("WitchToggle").GetComponent<Image>();
        m_Wizard = GameObject.FindGameObjectWithTag("WizardToggle").GetComponent<Image>();
        m_NameText = GameObject.FindGameObjectWithTag("NameText").GetComponent<Text>();
        m_NameText.text = PlayerPrefs.GetString("Name");
        m_SpellText = GameObject.FindGameObjectWithTag("SpellText").GetComponent<Text>();       
        m_HPText = GameObject.FindGameObjectWithTag("HPText").GetComponent<Text>();
        m_HPText.text = "HP: " + HP;
        m_CDText = GameObject.FindGameObjectWithTag("CDText").GetComponent<Text>();
        ColorUtility.TryParseHtmlString("#c156f7", out m_Highlight);
        m_ClimbCheck = transform.Find("ClimbCheck");
        m_NormalScale = gameObject.transform.localScale;
        m_CrouchScale = new Vector3(m_NormalScale[0], m_NormalScale[1] * .667f, m_NormalScale[2]);
        m_SpellSpawnPosition = transform.Find("SpellSpawner").transform.position;
        m_ForwardRotation = transform.rotation;              
        m_BackRotation = new Quaternion(0, m_ForwardRotation.y - 1, 0, 0);          
        m_LayerMask = -1;
        lastFireSpellTime = -100f;
        lastLevitateTime = -100f;
        lastToggleTime = -100f;
        m_LevitateTargets = new LinkedList<GameObject>();        
        m_SpellList = new LinkedList<string>();              
        currentSpellIdx = 0;
        targetIndex = 0;

        if(SceneManager.GetActiveScene().name == "1-1KH")
        {
            m_SpellList.AddLast("Fire");
            m_SpellList.AddLast("Earth");
            m_SpellText.text = "Spell: Fire";
            m_HasSpell = true;
        }

        if(PlayerPrefs.GetInt("Sprite") == 0)
        {
            m_Witch.enabled = false;
        }
        else
        {
            m_Wizard.enabled = true;
        }
	}
	
	// FixedUpdate
	void FixedUpdate () {        
        m_Grounded = false;
        m_CanClimb = false;
        m_LeviateDisabled = false;       
        m_Rigidbody2D.gravityScale = 2;
        checkCanLevitateVert();
        m_SpellSpawnPosition = transform.Find("SpellSpawner").transform.position;       

        // Check if player is standing on ground by searching for colliders overlapping radius at bottom of player
        Collider2D[] gColliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_LayerMask);
        for (int i = 0; i < gColliders.Length; i++)
        {                      
            if (gColliders[i].gameObject != gameObject)  // If a collider besides the one attatched to the player is found
            {                                            // then the player is considerd to be grounded
                m_Grounded = true;               
            }                   
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

        if(m_Grounded)
        {
            m_Animator.speed = Mathf.Abs(.2f * m_Rigidbody2D.velocity.x);                    
        }
        else
        {            
            m_Animator.speed = 0;
        }          

        if (m_HasSpell)
        {
            switch (m_SpellList.ElementAt(currentSpellIdx))
            {
                case "Fire":
                    if (m_FireSpellCD - (Time.time - lastFireSpellTime) >= 0)
                    {
                        m_CDText.text = "Spell Cooldown: " + (m_FireSpellCD - (
                            Time.time - lastFireSpellTime)).ToString("0.0");                        
                    }
                    else
                    {
                        m_CDText.text = "Spell Cooldown: 0";
                    }
                    break;
                case "Earth":
                    if (m_LevitateCD - (Time.time - lastLevitateTime) >= 0)
                    {
                        m_CDText.text = "Spell Cooldown: " + (m_LevitateCD - (
                            Time.time - lastLevitateTime)).ToString("0.0");
                    }
                    else
                    {
                        m_CDText.text = "Spell Cooldown: 0";
                    }
                    break;
            }
        }        
    }  

    /*
    Name: Move
    Parameters: float horizontal, bool jump, bool crouch
    */
    public void Move(float horizontal, bool jump, bool crouch)
    {
        if(m_CanLevitateAndMove || (!m_CanLevitateAndMove && m_LevitateTarget == null))
        {
            m_Rigidbody2D.constraints &= ~RigidbodyConstraints2D.FreezePositionX;

            if (horizontal < 0)
            {
                transform.rotation = m_BackRotation;
            }
            else if (horizontal > 0)
            {
                transform.rotation = m_ForwardRotation;
            }
            
            if(Mathf.Abs(horizontal) < .1)
            {                
                // Prevents the player from being pushed by levitating object
                m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX
                      | RigidbodyConstraints2D.FreezeRotation;
            }            

            if(horizontal <= 0 && m_Rigidbody2D.velocity.x > 0
                || horizontal >= 0 && m_Rigidbody2D.velocity.x < 0)
            {
                m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX
                     | RigidbodyConstraints2D.FreezeRotation;
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
        else
        {
            m_Rigidbody2D.velocity = Vector2.zero;
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

    /*
    Name: MoveLevitationTarget
    Parameters: float h, float v
    */
    public void MoveLevitationTarget(float h, float v)
    {                
        if(m_LevitateTarget != null && !m_LeviateDisabled)
        {            
            Rigidbody2D rb = m_LevitateTarget.GetComponent<Rigidbody2D>();
            Vector3 direction = new Vector3(h, -v, 0);
            if (!(Mathf.Abs(m_LevitateTarget.transform.position.y - transform.position.y) >= m_LevitateRadius
                || Mathf.Abs(m_LevitateTarget.transform.position.x - transform.position.x) >= m_LevitateRadius))
            {
                if(m_Grounded)
                {
                    rb.velocity = direction * 20f * m_LevitateSpeed * Time.deltaTime;                    
                }                                                                
            }
            else
            {
                rb.velocity = Vector2.zero;
                Vector3 position1 = m_LevitateTarget.transform.position;
                Vector3 position2 = position1 - .01f * (position1 - transform.position);
                m_LevitateTarget.transform.position = position2;                                
                if (m_DropWhenOutOfRange)
                {
                    rb.gravityScale = 1;
                    m_LevitateTarget.GetComponent<SpriteRenderer>().material.color = Color.white;                    
                    m_LevitateTarget = null;
                }                
            }
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
                    if ((Time.time - lastFireSpellTime) > m_FireSpellCD)
                    {
                        Quaternion spawnRotation = new Quaternion(0, transform.rotation.y, 0, 0);
                        GameObject spell = (GameObject)Instantiate(Resources.Load(
                            "Spells/FireSpell"), m_SpellSpawnPosition, spawnRotation);
                        lastFireSpellTime = Time.time;
                        m_Audio.clip = (AudioClip)Resources.Load(Constants.FireSpellAudio);
                        m_Audio.Play();
                    }
                    break;
                case "Earth":                    
                    if((Time.time - lastLevitateTime) > m_LevitateCD)
                    {
                        if(m_LevitateTarget == null)
                        {
                            findLevitateTargets();                            
                        }
                        else
                        {                            
                            m_LevitateTarget.GetComponent<Rigidbody2D>().gravityScale = 1;
                            m_LevitateTarget.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                            m_LevitateTarget.GetComponent<SpriteRenderer>().material.color = Color.white;                            
                            m_LevitateTarget = null;
                            m_LevitateTargets.Clear();
                            targetIndex = 0;
                            lastLevitateTime = Time.time;                            
                        }                        
                    }                    
                    break;                 
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
                if(m_LevitateTarget != null)
                {
                    m_LevitateTarget.GetComponent<Rigidbody2D>().gravityScale = 1;
                    m_LevitateTarget.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    m_LevitateTarget.GetComponent<SpriteRenderer>().material.color = Color.white;
                    m_LevitateTarget = null;
                    m_LevitateTargets.Clear();
                    targetIndex = 0;
                }                           
            }
            m_SpellText.text = "Spell: " + m_SpellList.ElementAt(currentSpellIdx);            
        }
        lastToggleTime = Time.time;        
    }

    // toggleTarget
    public void toggleTarget()
    {
        if(m_LevitateTargets.Count > 0)
        {
            if(m_LevitateTarget != null)
            {
                m_LevitateTarget.GetComponent<Rigidbody2D>().gravityScale = 1;
                m_LevitateTarget.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                m_LevitateTarget.GetComponent<SpriteRenderer>().material.color = Color.white;
                m_LevitateTargets.ElementAt(targetIndex).GetComponent<Rigidbody2D>().constraints
                        &= ~RigidbodyConstraints2D.FreezePositionY;
            }
            else
            {
                m_LevitateTargets.Remove(m_LevitateTarget);
            }                               

            try
            {                
                m_LevitateTarget = m_LevitateTargets.ElementAt(targetIndex + 1);                
                m_LevitateTarget.GetComponent<Rigidbody2D>().gravityScale = 0;
                m_LevitateTarget.GetComponent<SpriteRenderer>().material.color = m_Highlight;
                targetIndex = targetIndex + 1;
            }
            catch(ArgumentOutOfRangeException e)
            {                
                targetIndex = 0;                
                m_LevitateTarget = m_LevitateTargets.ElementAt(targetIndex);                
                m_LevitateTarget.GetComponent<Rigidbody2D>().gravityScale = 0;
                m_LevitateTarget.GetComponent<SpriteRenderer>().material.color = m_Highlight;
            }

            if (Mathf.Abs(m_LevitateTarget.transform.position.y - transform.position.y) >= m_LevitateRadius
                || Mathf.Abs(m_LevitateTarget.transform.position.x - transform.position.x) >= m_LevitateRadius)
            {                
                m_LevitateTargets.Remove(m_LevitateTarget);
                m_LevitateTarget.GetComponent<Rigidbody2D>().gravityScale = 1;
                m_LevitateTarget.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                m_LevitateTarget.GetComponent<SpriteRenderer>().material.color = Color.white;
                toggleTarget();
            }
        }
        else
        {
            m_LevitateTarget = null;
        }       
    }

    // checkCanLevitateVert
    private void checkCanLevitateVert()
    {
        if(m_LevitateTarget != null)
        {
            m_LevitateTarget.GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            Vector2 checkPosition = m_LevitateTarget.transform.position + new Vector3(0, 1.4f);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(checkPosition, k_UnderRadius, m_LayerMask);
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject != m_LevitateTarget.gameObject && (collider.gameObject.tag == "Liftable"
                    || collider.gameObject.tag == "Player"))
                {                    
                    m_LevitateTarget.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY
                       | RigidbodyConstraints2D.FreezeRotation;
                }
            }            
        }
    }

    // findLevitateTargets
    public void findLevitateTargets()
    {
        m_LevitateTargets = new LinkedList<GameObject>();
        targetIndex = 0;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
                                transform.position, m_LevitateRadius, m_LayerMask);
        for (int i = 0; i < colliders.Length; ++i)
        {
            if (colliders[i].gameObject.tag == "Liftable"
                && !m_LevitateTargets.Contains(colliders[i].gameObject))
            {
                m_LevitateTargets.AddLast(colliders[i].gameObject);
                lastLevitateTime = Time.time;
            }
        }

        if (m_LevitateTargets.Count > 0)
        {
            m_LevitateTarget = m_LevitateTargets.ElementAt(targetIndex);
            m_LevitateTarget.GetComponent<Rigidbody2D>().gravityScale = 0;
            m_LevitateTarget.GetComponent<SpriteRenderer>().material.color = m_Highlight;
        }
    }
    
    // OnTriggerEnter2D
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Contains("EnemyProjectile"))
        {
            HP -= 1;
            m_HPText.text = "HP: " + HP;
            Destroy(other.gameObject);
            if (HP == 0)
            {
                SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
                //respawn at checkpoint
            }
        }
        else if (other.gameObject.name == "FireScroll")
        {
            m_HasSpell = true;
            Destroy(other.gameObject);
            m_SpellList.AddLast("Fire");            
            m_SpellText.text = "Spell: " + m_SpellList.ElementAt(0);
        }
        else if (other.gameObject.name == "EarthScroll")
        {
            Destroy(other.gameObject);
            m_SpellList.AddLast("Earth");
            m_SpellText.text = "Spell: " + m_SpellList.ElementAt(1);
        }
    }  
}
