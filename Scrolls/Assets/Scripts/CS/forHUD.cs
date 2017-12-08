using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class forHUD : MonoBehaviour {

	public Toggle wizTog;
	public Toggle witTog;

	public Dropdown nameSelect;

	public int nameIndex;
	public int spriteIndex;

	public void getName()
	{
		nameIndex = nameSelect.value;
		PlayerPrefs.SetInt ("Name", nameIndex);
		
	}

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