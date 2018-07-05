using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class startButtonControl : NetworkBehaviour {
	public main mainScript;
	public GameObject thisButton;
	
	public void hide () {
		thisButton.SetActive(false);
	}
	
	public void click () {
		thisButton.SetActive(false);
		mainScript.CmdStartGame();
	}
	
	public void reset () {
		thisButton.SetActive(true);
	}
}
