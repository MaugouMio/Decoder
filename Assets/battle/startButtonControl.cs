using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startButtonControl : MonoBehaviour {
	
	public main mainScript;
	public GameObject thisButton;

	public void click () {
		thisButton.SetActive(false);
		mainScript.startGame();
	}
	
	public void reset () {
		thisButton.SetActive(true);
	}
}
