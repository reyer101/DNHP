using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Name: Alec Reyerson
    ID: 1826582
    Email: reyer101@mail.chapman.edu
    Course: CPSC-344-01
    Assignment: Alpha Milestone

    Description: Script for practicing using input from right analog. Script doesn't do anything in the actual 
    game currently but will most likely be used as a reference in the future for rotating objects w/ right analog.
    */

// PlayerCharacter
public class RightAnalogRotator : MonoBehaviour {
    Vector2 inputDirection;
    public float smooth;

	// Start
	void Start () {
        inputDirection = Vector2.zero;		
	}
	
	// Update
	void Update () {        
        float x = Input.GetAxis("RightAnalogHori");
        float y = Input.GetAxis("RightAnalogVert");
        float angle = Mathf.Atan2(-y, x) * (180f / Mathf.PI);           
        transform.rotation = Quaternion.Slerp(
            transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), 10f * smooth * Time.deltaTime);
	}
}
