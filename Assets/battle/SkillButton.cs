using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour {
	
	public main mainScript;
    public GameObject thisButton;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void click()
    {
		thisButton.SetActive(false);
		mainScript.startGame();
    }

    public void reset()
    {
        thisButton.SetActive(true);
    }
}
