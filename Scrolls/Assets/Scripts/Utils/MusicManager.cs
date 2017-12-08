using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Name: Alec Reyerson
    ID: 1826582
    Email: reyer101@mail.chapman.edu
    Course: CPSC-344-01
    Assignment: Beta Milestone

    Description: Script ensures that background music loops between scenes.
    */

// MusicManager
public class MusicManager : MonoBehaviour {

    // Awake
	void Awake()
    {
        DontDestroyOnLoad(GetComponent<AudioSource>());
    }	
}
