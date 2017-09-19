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
    private bool m_Jump, m_Crouched;
    private Vector3 m_CrouchScale, m_NormalScale;

    // Awake
    void Awake () {
        m_Player = GetComponent<PlayerCharacter>();       
        m_NormalScale = gameObject.transform.localScale;
        m_CrouchScale = gameObject.transform.localScale * .667f;                       		
	}
	
	// Update
	void Update () {
        if (!m_Jump)
        {
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }

        if(CrossPlatformInputManager.GetButtonDown("Crouch"))
        {
            Debug.Log("Crouch pressed");
            if(m_Crouched)
            {
                gameObject.transform.localScale = m_NormalScale;
                m_Crouched = false;
            } 
            else
            {
                gameObject.transform.localScale = m_CrouchScale;
                m_Crouched = true;
            }                        
        }
    }

    // FixedUpdate
    void FixedUpdate()
    {
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        m_Player.Move(h, m_Jump);
        m_Jump = false;

        float v = CrossPlatformInputManager.GetAxis("Vertical");
        m_Player.Climb(v);
    }
}
