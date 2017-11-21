using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Name: Alec Reyerson
ID: 1826582
Email: reyer101@mail.chapman.edu
Course: CPSC-344-01
Assignment: Gold Milestone

Description: Script that ties CD Wheel follow player
*/

// CDWheelFollow
public class CDWheelFollow : MonoBehaviour {
    Transform player, camera;
    Vector3 initialPosition;
    float baseDiffY, baseDiffX;

    public float followStrength, headOffset, screenXRatio, screenYRatio;  

	// Awake
	void Awake () { 
        // 522, 326       
        /*player = GameObject.FindGameObjectWithTag("Player").transform;
        camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        initialPosition = transform.position;
        baseDiffY = camera.position.y - player.position.y;
        baseDiffX = camera.position.x - player.position.x;
        screenXRatio = Screen.width / 522;
        screenYRatio = Screen.height / 326;
        Debug.Log("Screen  height: " + Screen.height);
        Debug.Log("Screen width: " + Screen.width);*/
          		
	}
	
	// Update
	void Update () {
        /*float newDiffX = camera.position.x - player.position.x;
        float newDiffY = camera.position.y - player.position.y;        
        transform.position = new Vector3(screenXRatio * (initialPosition.x - followStrength * newDiffX),
            screenYRatio * (initialPosition.y + headOffset - followStrength * newDiffY));*/		
	}
}
