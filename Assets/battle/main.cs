using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class main : MonoBehaviour {
	public Text min; //the text display the lower bound
	public Text max; //the text display the upper bound
	public Text HP1; //the text of player 1 HP
	public Text HP2; //the text of player 2 HP
	
	public InputBarControl inputBar; //the API of the input field
	public lightImgControl lightImg; //the API of the light sprite
	public screenControl screen; //the API of the middle screen sprite
	public startButtonControl startButton; //the API of the start button
	
	static readonly int MAX_DAMAGE = 200;
	
	static int point; //the target number
	static int turn; //determine which player to act
	static int startTurn; //determine which player to act at the begining
	static int [] HP = new int [2]; //the health of the players
	static int [] displayHP = new int [2]; //the display health of the players
	static int guess_time;
	static bool HPchange;
	
	static int lastRandomImg; //the image index showed last time, for not repeating
	static int startCounting; //the count down number after pressing the start button
	static int turnCounting; //the count down number for each turn
	
	// Use this for initialization
	void Awake () {
		min.text = "";
		max.text = "";
		HP1.text = "";
		HP2.text = "";
		
		HPchange = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (HPchange) { //dynamicly change the HP number display
			if (displayHP[0] > HP[0]) {
				displayHP[0]--;
				HP1.text = System.Convert.ToString(displayHP[0]);
			}
			else if (displayHP[1] > HP[1]) {
				displayHP[1]--;
				HP2.text = System.Convert.ToString(displayHP[1]);
			}
			else {
				HPchange = false;
			}
		}
	}
	
	public void startGame () {
		HP[0] = 100;
		HP[1] = 100;
		displayHP[0] = 100;
		displayHP[1] = 100;
		startTurn = 1;
		
		startCounting = 4;
		InvokeRepeating("StartCountDown", 0, 1); //the starting count down, called 1 time per second
	}
	
	public void InputResult (int guess) {
		if (guess == point) {
			RoundEnd(); //things to do at the end of a round
		}
		else {
			if (guess < point) {
				if (guess > System.Int32.Parse(min.text)) { //valid guess
					valid_guess(guess, true);
				}
				else {
					screen.setImg(4); //error image
				}
			}
			else { //guess > point
				if (guess < System.Int32.Parse(max.text)) {
					valid_guess(guess, false);
				}
				else {
					screen.setImg(4); //error image
				}
			}
			
			inputBar.setPrompt("Player " + System.Convert.ToString(turn)); //the prompt word
		}
		
		lightImg.setTurn(turn);
	}
	
	void valid_guess (int guess, bool Lower) {
		CancelInvoke("TurnCountDown");
		turnCounting = 6;
		InvokeRepeating("TurnCountDown", 2, 1); //restart the turn counting down after 2 seconds
		inputBar.Switch(false);
		
		int randomImg = Random.Range(1, 4); //1 ~ 3 are troll images
		guess_time++;
		
		turn = turn == 1 ? 2 : 1; //swap turn
		
		if (Lower)
			min.text = System.Convert.ToString(guess);
		else
			max.text = System.Convert.ToString(guess);
		
		while (randomImg == lastRandomImg)
			randomImg = Random.Range(1, 4);
		screen.setImg(randomImg, turn == 1);
		lastRandomImg = randomImg;
	}
	
	void TurnCountDown () {
		turnCounting--;
		
		if (turnCounting > 0) {
			screen.setText(System.Convert.ToString(turnCounting)); //display the rest time
			if (turnCounting == 5) {
				inputBar.Switch(true);
				inputBar.Select(); //automatically select the input field for user convenience
			}
		}
		else { //failed to action in time
			CancelInvoke("TurnCountDown"); //stop the loop timer
			InputResult(point); //let the player guess the number to lose
		}
	}
	
	void RoundEnd () {
		CancelInvoke("TurnCountDown");
		
		HP[turn - 1] -= MAX_DAMAGE / (guess_time + 1) + 10;
		
		screen.setImg(0); //boom image
		min.text = System.Convert.ToString(point);
		max.text = System.Convert.ToString(point);
		
		turn = turn == 1 ? 2 : 1; //swap turn
		if (HP[2 - turn] <= 0) {
			HP[2 - turn] = 0;
			inputBar.setPrompt("PLAYER " + System.Convert.ToString(turn) + " WINS!");
			InvokeRepeating("EndSet", 5, 1); //wait 5 seconds and end
		}
		else { //some reset for the next round
			inputBar.setPrompt("Get ready...");
			startTurn = startTurn == 1 ? 2 : 1; //swap the starting turn
			
			startCounting = 4;
			InvokeRepeating("StartCountDown", 3, 1); //wait 3 seconds and start the new round starting count down
		}
		HPchange = true;
		inputBar.Switch(false); //turn off the interaction of the input field
	}
	
	void StartCountDown () {
		startCounting--;
		
		if (startCounting > 0) {
			screen.setText(System.Convert.ToString(startCounting)); //display the rest time
		}
		else { //the game starts
			if (startCounting == 0) {
				screen.setText("GO!");
			}
			else if (startCounting == -2) { //some initialization
				min.text = "0";
				max.text = "1000";
				HP1.text = System.Convert.ToString(HP[0]);
				HP2.text = System.Convert.ToString(HP[1]);
				
				point = Random.Range(1, 1000);
				turn = startTurn;
				lastRandomImg = 0;
				
				guess_time = 0;
				
				inputBar.setPrompt("Player " + System.Convert.ToString(turn));
				inputBar.Switch(true);
				inputBar.Select();
				
				lightImg.Switch(true);
				lightImg.setTurn(turn);
				
				CancelInvoke("StartCountDown"); //stop the loop timer
				
				turnCounting = 6;
				InvokeRepeating("TurnCountDown", 0, 1); //the turn count down, called 1 time per second
			}
		}
	}
	
	void EndSet () { //when the game ends and go back to menu
		min.text = "";
		max.text = "";
		HP1.text = "";
		HP2.text = "";
		screen.hide();
		lightImg.Switch(false);
		inputBar.setPrompt("");
		
		startButton.reset(); //set the start button back
		CancelInvoke("EndSet"); //stop the loop timer
	}
}
