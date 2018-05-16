using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputBarControl : MonoBehaviour {
	public main mainScript;
	public InputField inputfield;
	
	void Awake () {
		inputfield.enabled = false;
		inputfield.placeholder.GetComponent<Text>().text = "";
	}
	
	public void SetPrompt (string placeholderText) {
		inputfield.placeholder.GetComponent<Text>().text = placeholderText;
		inputfield.text = "";
	}
	
	public void Select () {
		inputfield.ActivateInputField();
	}
	
	public void Deselect () {
		inputfield.ActivateInputField();
	}
	
	public void Switch (bool status) {
		inputfield.enabled = status;
	}
	
	public void GetInputNumber () {
		int guess;
		if (System.Int32.TryParse(inputfield.text, out guess)) {
			mainScript.InputResult(guess);
		}
		else {
			Debug.Log("invalid input");
		}
	}
}
