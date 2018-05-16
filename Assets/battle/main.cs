using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class main : MonoBehaviour {
	public Text min;
	public Text max;
	
	public InputBarControl inputBar;
	public lightImgControl lightImg;
	public screenControl screen;
	public startButtonControl startButton;
	
	static int point;
	static int turn;
	static int startCounting;
	
	// Use this for initialization
	void Awake () {
		min.text = "";
		max.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void startGame () {
		point = Random.Range(1, 1000);
		turn = 1;
		startCounting = 4;
		InvokeRepeating("StartCountDown", 0, 1);
	}
	
	public void InputResult (int guess) {
		if (guess == point) {
			turn = turn == 1 ? 2 : 1;
			
			screen.setImg(0);
			min.text = System.Convert.ToString(point);
			max.text = System.Convert.ToString(point);
			
			inputBar.SetPrompt("PLAYER " + System.Convert.ToString(turn) + " WINS!");
			inputBar.Switch(false);
			
			InvokeRepeating("EndSet", 5, 1);
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
			
			inputBar.SetPrompt("Player " + System.Convert.ToString(turn));
			inputBar.Select();
		}
		
		
		
		if (turn == 1) {
			lightImg.SetPos(-144, 40, 0);
		}
		else {
			lightImg.SetPos(142, 40, 0);
		}
	}
	
	void StartCountDown () {
		startCounting--;
		
		if (startCounting > 0) {
			screen.setText(System.Convert.ToString(startCounting));
		}
		else {
			screen.setText("GO!");
			
			min.text = "0";
			max.text = "1000";
			
			inputBar.SetPrompt("Player " + System.Convert.ToString(turn));
			inputBar.Switch(true);
			inputBar.Select();
			
			lightImg.Switch(true);
			
			CancelInvoke("StartCountDown");
		}
	}
	
	void EndSet () {
		min.text = "";
		max.text = "";
		screen.hide();
		lightImg.Switch(false);
		inputBar.SetPrompt("");
		
		startButton.reset();
		CancelInvoke("EndSet");
	}
}
