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
    private bool m_Jump, m_Crouch;
    private Vector3 m_CrouchScale, m_NormalScale;

    // Awake
    void Awake () {
        m_Player = GetComponent<PlayerCharacter>();
        m_Crouch = false;                             		
	}
	
	// Update
	void Update () {
        if (!m_Jump)
        {
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }

        if (CrossPlatformInputManager.GetButtonDown("Crouch"))
        {
            m_Crouch = !m_Crouch;         
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
