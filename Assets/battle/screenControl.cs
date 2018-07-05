using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class screenControl : NetworkBehaviour {
	public Text screen_text;
	Sprite[] all_img;
	
	void Awake () {
		all_img = Resources.LoadAll<Sprite>("battle/img/screen");
		this.GetComponent<Renderer>().enabled = false;
		screen_text.text = "";
	}
	
	public void hide () {
		this.GetComponent<Renderer>().enabled = false;
		screen_text.text = "";
	}
	
	public void setImg (int index, bool flipX = false) {
		screen_text.text = "";
		this.GetComponent<Renderer>().enabled = true;
		this.GetComponent<SpriteRenderer>().flipX = flipX;
		this.GetComponent<SpriteRenderer>().sprite = all_img[index];
	}
	
	public void setText (string display_text, int size = 136) {
		this.GetComponent<Renderer>().enabled = false;
		screen_text.fontSize = size;
		screen_text.text = display_text;
	}
}
