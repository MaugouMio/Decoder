using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Audio;

public class main : NetworkBehaviour {
	public Text min; //the text display the lower bound
	public Text max; //the text display the upper bound
	public Text HP1; //the text of player 1 HP
	public Text HP2; //the text of player 2 HP
	
	public InputBarControl inputBar; //the API of the input field
	public lightImgControl lightImg; //the API of the light sprite
	public screenControl screen; //the API of the middle screen sprite
	public startButtonControl startButton; //the API of the start button
	public SkillButton skillButton;
	
	public AudioSource bang;
	
	const int MAX_DAMAGE = 200;
	
	int point; //the target number
	[SyncVar]
	int turn; //determine which player to act
	[SyncVar]
	int startTurn; //determine which player to act at the begining

	[SyncVar]
	Vector2 HP; //the health of the players
	Vector2 displayHP; //the display health of the players
	[SyncVar]
	Vector2 memory_space; 

	int guess_time;
	bool HPchange;
	
	[SyncVar]
	int randomImg;
	int lastRandomImg; //the image index showed last time, for not repeating
	int startCounting; //the count down number after pressing the start button
	int turnCounting; //the count down number for each turn

	int your_turn;
	bool challenge;
	
	// Use this for initialization
	void Awake () {
		min.text = "";
		max.text = "";
		HP1.text = "";
		HP2.text = "";
		bang = this.GetComponent<AudioSource>();
		HPchange = false;
		
		if (isServer) {
			your_turn = 1;
			startButton.hide();
		}
		else {
			your_turn = 2;
		}
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
	
	[Command]
	public void CmdStartGame () {
		if (isServer) {
			HP[0] = 100;
			HP[1] = 100;
			startTurn = 1;
		}
		
		displayHP[0] = 100;
		displayHP[1] = 100;
		
		startCounting = 4;
		InvokeRepeating("StartCountDown", 0, 1); //the starting count down, called 1 time per second
	}
	
	[Command]
	public void CmdInputResult (int guess, bool timeout) {
		if (isServer) {
			if (timeout) {
				guess = point;
				challenge = false;
			}
			
			if (guess == point) {
				RoundEnd(challenge); //things to do at the end of a round
			}
			else {
				if (guess < point) {
					if (guess > System.Int32.Parse(min.text)) { //valid guess
						valid_guess(guess, true);
					}
					else {
						RpcWrongInput();
					}
				}
				else { //guess > point
					if (guess < System.Int32.Parse(max.text)) {
						valid_guess(guess, false);
					}
					else {
						RpcWrongInput();
					}
				}
			}
		}
	}
	
	[ClientRpc]
	void RpcWrongInput () {
		screen.setImg(4); //error image
		if (turn == your_turn) {
			inputBar.Select();
		}
	}
	
	[Command]
	public void Cmd_use_skill(string skill) {
		if(skill == "hack"){
			if (isServer) {
				memory_space[turn - 1] -= 512;
				int display_bit = Random.Range(0 ,2);
				int display_num = Mathf.FloorToInt(point / Mathf.Pow(10, display_bit) % 10);
				char[] tmp = "XXX".ToCharArray();
				tmp[display_bit] = display_num.ToString()[0];
				string display_result = new string(tmp);
				
				RpcSkillDisplay(display_result, "???", 136);
			}
		}
		else if(skill == "swap") {
			if (isServer) {
				memory_space[turn - 1] -= 256;
				turn = turn == 1 ? 2 : 1; //swap turn
				
				RpcSkillDisplay("SWAP", "SWAP", 96);
			}
		}
		else if(skill == "challenge") {
			if (isServer) {
				memory_space[turn - 1] -= 128;
				challenge = true;
				
				RpcSkillDisplay("CHALLENGE", "CHALLENGE", 45);
			}
		}
	}
	
	[ClientRpc]
	void RpcSkillDisplay (string result, string unknown, int size) {
		if (turn == your_turn) {
			screen.setText(result, size);
		}
		else {
			screen.setText(unknown, size);
		}
		
		CancelInvoke("TurnCountDown");
		turnCounting = 6;
		InvokeRepeating("TurnCountDown", 2, 1);
		inputBar.Switch(false);
		lightImg.setTurn(turn);
	}

	void valid_guess (int guess, bool Lower) {
		guess_time++;
		turn = turn == 1 ? 2 : 1; //swap turn
		while (randomImg == lastRandomImg)
			randomImg = Random.Range(1, 4); //1 ~ 3 are troll images
		lastRandomImg = randomImg;
		
		RpcValidGuess(guess, Lower);
	}
	
	[ClientRpc]
	void RpcValidGuess (int guess, bool Lower) {
		CancelInvoke("TurnCountDown");
		turnCounting = 6;
		InvokeRepeating("TurnCountDown", 2, 1); //restart the turn counting down after 2 seconds
		inputBar.Switch(false);
		
		if (Lower)
			min.text = System.Convert.ToString(guess);
		else
			max.text = System.Convert.ToString(guess);
		
		screen.setImg(randomImg, turn == 1);
		lightImg.setTurn(turn);
		inputBar.setPrompt("Player " + System.Convert.ToString(turn)); //the prompt word
	}
	
	void TurnCountDown () {
		turnCounting--;
		
		if (turnCounting > 0) {
			screen.setText(System.Convert.ToString(turnCounting)); //display the rest time
			if (turnCounting == 5) {
				if (turn == your_turn) { //Turn on the Button
					skillButton.on((int)memory_space[your_turn - 1]);
					inputBar.Switch(true);
					inputBar.Select(); //automatically select the input field for user convenience
				}
			}
		}
		else { //failed to action in time
			CancelInvoke("TurnCountDown"); //stop the loop timer
			if (turn == your_turn) {
				CmdInputResult(0, true); //time out input
			}
		}
	}
	
	void RoundEnd (bool isChallenge) {
		if (isChallenge) {
			turn = turn == 1 ? 2 : 1; //swap turn
		}
		
		HP[turn - 1] -= MAX_DAMAGE / (guess_time + 1) + 10;
		if (HP[turn - 1] <= 0) {
			HP[turn - 1] = 0;
		}
		turn = turn == 1 ? 2 : 1; //swap turn
		
		if (HP[2 - turn] == 0) {
			RpcRoundEnd(true);
		}
		else { //some reset for the next round
			startTurn = startTurn == 1 ? 2 : 1; //swap the starting turn
			RpcRoundEnd(false);
		}
	}
	
	[ClientRpc]
	void RpcRoundEnd (bool endGame) {
		bang.Play();
		CancelInvoke("TurnCountDown");
		
		screen.setImg(0); //boom image
		min.text = System.Convert.ToString(point);
		max.text = System.Convert.ToString(point);
		lightImg.setTurn(turn);
		
		HPchange = true;
		inputBar.Switch(false); //turn off the interaction of the input field
		
		if (endGame) {
			inputBar.setPrompt("PLAYER " + System.Convert.ToString(turn) + " WINS!");
			InvokeRepeating("EndSet", 5, 1); //wait 5 seconds and end
		}
		else {
			inputBar.setPrompt("Get ready...");
			startCounting = 4;
			InvokeRepeating("StartCountDown", 3, 1); //wait 3 seconds and start the new round starting count down
		}
	}
	
	void StartCountDown () {
		startCounting--;
		
		if (startCounting > 0) {
			screen.setText(System.Convert.ToString(startCounting)); //display the rest time
		}
		else { //the game starts
			if (startCounting == 0) {
				screen.setText("GO!");
				if (isServer) {
					point = Random.Range(1, 1000);
					turn = startTurn;
					lastRandomImg = 0;
					
					guess_time = 0;
					
					memory_space[0] = 1024;
					memory_space[1] = 1024;
					challenge = false;
				}
			}
			else if (startCounting == -2) { //some initialization
				min.text = "0";
				max.text = "1000";
				HP1.text = System.Convert.ToString(HP[0]);
				HP2.text = System.Convert.ToString(HP[1]);
				
				inputBar.setPrompt("Player " + System.Convert.ToString(turn));
				if (turn == your_turn) {
					inputBar.Switch(true);
					inputBar.Select();
					skillButton.on(1024);
				}
				
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
		
		if (isClient) {
			startButton.reset(); //set the start button back
		}
		CancelInvoke("EndSet"); //stop the loop timer
	}
}
