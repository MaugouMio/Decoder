using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputNumber : MonoBehaviour {
	public main mainScript;
	public InputField inputfield;
	
	void Start () {
		inputfield.ActivateInputField();
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
