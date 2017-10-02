using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

/*
Name: Alec Reyerson
ID: 1826582
Email: reyer101@mail.chapman.edu
Course: CPSC-344-01
Assignment: Alpha Milestone

Description: Script for handling player input
*/

// CharacterController
public class AlecController : MonoBehaviour {
    private PlayerCharacter m_Player;
    private Transform m_CrouchCheck;
    private Vector3 m_CrouchScale, m_NormalScale;
    private LayerMask m_LayerMask;
    private bool m_Jump, m_Crouch;

    private float k_CrouchRadius = 1.5f;

    // Awake
    void Awake () {
        m_Player = GetComponent<PlayerCharacter>();
        m_CrouchCheck = transform.Find("ClimbCheck");
        m_LayerMask = 1;
        m_Crouch = false;                             		
	}
	
	// Update
	void Update () {
        if (!m_Jump)
        {
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }
        
        if(CrossPlatformInputManager.GetAxis("Spell") == 1)
        {                        
            m_Player.castSpell();
        }

        if (CrossPlatformInputManager.GetAxis("ToggleSpell") == 1)
        {
            m_Player.toggleSpell();
        }

        if (CrossPlatformInputManager.GetButtonDown("Crouch"))
        {
            m_Crouch = !m_Crouch;
            Collider2D[] cColliders = Physics2D.OverlapCircleAll(m_CrouchCheck.position, k_CrouchRadius, m_LayerMask);
            for (int i = 0; i < cColliders.Length; i++)
            {                
                if (cColliders[i].gameObject != gameObject)
                {                    
                    m_Crouch = true;
                }
            }                     
        }         
    }

    // FixedUpdate
    void FixedUpdate()
    {
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        m_Player.Move(h, m_Jump, m_Crouch);
        m_Jump = false;

        float v = CrossPlatformInputManager.GetAxis("Vertical");
        m_Player.Climb(v);
    }
}
