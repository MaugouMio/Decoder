using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour {
	
	public main mainScript;
	public Button swapButton;
	public Button hackButton;
	public Button challengeButton;

   
	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		//thisButton.SetActive(true);
	}

	public void swap()
    {
		mainScript.use_skill("swap");   
		swapButton.interactable = false;
    }
	public void hack()
    {
        mainScript.use_skill("hack");
		hackButton.interactable = false;
    }
	public void challenge()
    {
		mainScript.use_skill("challenge");
		challengeButton.interactable = false;
        
    }
    public void on()
    {
		//thisButton.SetActive(false);
		swapButton.interactable = true;
		hackButton.interactable = true;
		challengeButton.interactable = true;
    }
	public void off()
	{
		swapButton.interactable = false;
		hackButton.interactable = false;
        challengeButton.interactable = false;
	}
}
