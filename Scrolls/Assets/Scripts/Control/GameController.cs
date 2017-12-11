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
    Assignment: Gold Milestone

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
    string name, scene;
    bool showingDialogueBox, triggerNextLevel;        

	// Awake
	void Awake () {
        scene = SceneManager.GetActiveScene().name;
        dialogueBox = GameObject.FindGameObjectWithTag("Dialogue");
        name = PlayerPrefs.GetString("Name");
        dialogueBox.SetActive(false);             
        roomOneDone = false;
        roomTwoDone = false;
        showingDialogueBox = false;
        
        if(scene == "EndScene")
        {
            showDialogueBox(33, 35, false);
        }  
        else
        {
            player = GameObject.FindGameObjectWithTag("Player");
            currentCheckPoint = player.transform.position;
        }    
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
        currDialogIndex = start;
        lastDialogIndex = end;
        triggerNextLevel = nextLevel;

        if(scene != "EndScene")
        {
            player.GetComponent<AlecController>().setCanMove(false);
        }
    }  
    
    // populateDialogue
    void populateDialogue()
    {
        try
        {
            dialogueBox.transform.Find("speakerText")
            .GetComponent<Text>().text = Constants.Dialogue[currDialogIndex][0]
            .Replace(Constants.Player, name);
            dialogueBox.transform.Find("dialogueText")
                .GetComponent<Text>().text = Constants.Dialogue[currDialogIndex][1]
                .Replace(Constants.Player, name);

        } catch (KeyNotFoundException e) { }         
    }

    // skipDialogue
    void skipDialogue()
    {
        currDialogIndex += 1;
        if(currDialogIndex > lastDialogIndex)
        {
            dialogueBox.SetActive(false);
            if (scene != "EndScene") 
            {
                player.GetComponent<AlecController>().setCanMove(true);
            }
            
            if(triggerNextLevel)
            {
                SceneManager.LoadScene(Application.loadedLevel + 1);
            }            
        }        
    }
}
