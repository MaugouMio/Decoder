using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class InputBarControl : NetworkBehaviour {
	public main mainScript;
	public InputField inputfield;
	
	void Awake () {
		inputfield.enabled = false;
		inputfield.placeholder.GetComponent<Text>().text = "";
	}
	
	public void setPrompt (string placeholderText) {
		inputfield.placeholder.GetComponent<Text>().text = placeholderText;
		inputfield.text = "";
	}
	
	public void Select () {
		inputfield.ActivateInputField();
	}
	
	public void Deselect () {
		inputfield.DeactivateInputField();
	}
	
	public void Switch (bool status) {
		inputfield.enabled = status;
	}
	
	public void GetInputNumber () {
		if (inputfield.enabled){
			int guess;
			if (System.Int32.TryParse(inputfield.text, out guess)) {
				mainScript.CmdInputResult(guess, false);
			}
			else {
				mainScript.CmdInputResult(0, false); //definitly failed guess number
			}
		}
	}
}
