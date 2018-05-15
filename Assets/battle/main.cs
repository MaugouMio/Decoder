using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class main : MonoBehaviour {
	public Text min;
	public Text max;
	public InputField inputfield;
	public lightPos lightSprite;
	public screen mainScreen;
	public int turn = 1;
	
	static int point;
	
	// Use this for initialization
	void Start () {
		point = Random.Range(1, 1000);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void InputResult (int guess) {
		if (guess == point) {
			turn = turn == 1 ? 2 : 1;
			
			mainScreen.setImg(0);
			inputfield.placeholder.GetComponent<Text>().text = "PLAYER " + System.Convert.ToString(turn) + " WINS!";
			inputfield.enabled = false;
		}
		else {
			if (guess < point) {
				if (guess > System.Int32.Parse(min.text)) {
					turn = turn == 1 ? 2 : 1;
					min.text = System.Convert.ToString(guess);
				}
				else {
					Debug.Log("invalid number");
				}
			}
			else {
				if (guess < System.Int32.Parse(max.text)) {
					turn = turn == 1 ? 2 : 1;
					max.text = System.Convert.ToString(guess);
				}
				else {
					Debug.Log("invalid number");
				}
			}
			
			inputfield.placeholder.GetComponent<Text>().text = "Player " + System.Convert.ToString(turn);
			inputfield.ActivateInputField();
		}
		
		
		
		if (turn == 1) {
			lightSprite.setPos(-144, 40, 0);
		}
		else {
			lightSprite.setPos(142, 40, 0);
		}
		inputfield.text = "";
	}
}
