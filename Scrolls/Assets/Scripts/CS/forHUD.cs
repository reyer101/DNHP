using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Carissa Smith / Alec Reyerson
//1800292
//smith426@mail.chapman.edu
//CPSC-344-01
//Scrolls
//This script's purpose is to load scenes within the game. 
public class forHUD : MonoBehaviour {

	public Toggle wizTog;
	public Toggle witTog;

	public Dropdown nameSelect;

	public int nameIndex;
	public int spriteIndex;

    private List<Dropdown.OptionData> choices;    

    // Awake
    void Awake()
    {
        wizTog = GameObject.FindGameObjectWithTag("WizardToggle").GetComponent<Toggle>();
        witTog = GameObject.FindGameObjectWithTag("WitchToggle").GetComponent<Toggle>();
        nameSelect = GameObject.FindGameObjectWithTag("NameDrop").GetComponent<Dropdown>();
        choices = nameSelect.options;       
    }

    // getName
	public void getName()
	{
		nameIndex = nameSelect.value;
		PlayerPrefs.SetString ("Name", choices[nameIndex].text);
		
	}

    // getSprite
	public void getSprite()
	{
		if (wizTog.isOn == true) 
		{
			spriteIndex = 0;
			PlayerPrefs.SetInt ("Sprite", spriteIndex);
		} 

		else if (witTog.isOn == true)
		{
			spriteIndex = 1;
			PlayerPrefs.SetInt ("Sprite", spriteIndex);
		}
	}
}