using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;
using UnityEngine;

/*
    Name: Alec Reyerson
    ID: 1826582
    Email: reyer101@mail.chapman.edu
    Course: CPSC-344-01
    Assignment: Beta Milestone

    Description: Script for managing game state and events
    */

// GameController
public class GameController : MonoBehaviour {
    public static Vector2 currentCheckPoint;
    public GameObject barrierOne, barrierTwo;
    public bool roomOneDone, roomTwoDone;

    GameObject player, dialogueBox;
    Text tutText;
    int currDialogIndex, lastDialogIndex;
    string name;
    bool showingDialogueBox, triggerNextLevel;        

	// Awake
	void Awake () {
        player = GameObject.FindGameObjectWithTag("Player");
        dialogueBox = GameObject.FindGameObjectWithTag("Dialogue");
        name = PlayerPrefs.GetString("Name");
        dialogueBox.SetActive(false);
        currentCheckPoint = player.transform.position;     
        roomOneDone = false;
        roomTwoDone = false;
        showingDialogueBox = false;      
    }
	
	// Update
	void Update () {
        player = GameObject.FindGameObjectWithTag("Player");      
        if(roomOneDone)
        {            
            barrierOne.GetComponent<BoxCollider2D>().enabled = false;
        }    
        if(roomTwoDone)
        {
            barrierTwo.GetComponent<BoxCollider2D>().enabled = false;
        } 
        
        if(showingDialogueBox)
        {
            populateDialogue();
            if(CrossPlatformInputManager.GetButtonDown("Jump"))
            {
                skipDialogue();
            }
        }  		
	}

    /*
   Name: showDialogue
   Parameters: int start, int end
   */
    public void showDialogueBox(int start, int end, bool nextLevel)
    {
        dialogueBox.SetActive(true);
        showingDialogueBox = true;
        player.GetComponent<AlecController>().setCanMove(false);
        currDialogIndex = start;
        lastDialogIndex = end;
        triggerNextLevel = nextLevel;
    }  
    
    // populateDialogue
    void populateDialogue()
    {
        dialogueBox.transform.Find("speakerText")
            .GetComponent<Text>().text = Constants.Dialogue[currDialogIndex][0]
            .Replace(Constants.Player, name);
        dialogueBox.transform.Find("dialogueText")
            .GetComponent<Text>().text = Constants.Dialogue[currDialogIndex][1]
            .Replace(Constants.Player, name); ;
    }

    // skipDialogue
    void skipDialogue()
    {
        currDialogIndex += 1;
        if(currDialogIndex > lastDialogIndex)
        {
            dialogueBox.SetActive(false);
            player.GetComponent<AlecController>().setCanMove(true);
            if(triggerNextLevel)
            {
                SceneManager.LoadScene(Application.loadedLevel + 1);
            }            
        }        
    }
}
