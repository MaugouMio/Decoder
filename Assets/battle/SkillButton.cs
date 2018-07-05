using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SkillButton : NetworkBehaviour {
	
	public main mainScript;
	public GameObject swapButton;
	public GameObject hackButton;
	public GameObject challengeButton;

   
	// Use this for initialization
	void Awake () {
		swapButton.SetActive(false);
		hackButton.SetActive(false);
        challengeButton.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
		
	}
	
	public void spell (string type) {
		mainScript.Cmd_use_skill(type);
		off();
	}
	
    public void on (int nowMemory) {
		if (nowMemory >= 128) {
			swapButton.SetActive(true);
		}
		if (nowMemory >= 256) {
			challengeButton.SetActive(true);
		}
		if (nowMemory >= 512) {
			hackButton.SetActive(true);
		}
    }
	public void off () {
		swapButton.SetActive(false);
		hackButton.SetActive(false);
        challengeButton.SetActive(false);
	}
}
