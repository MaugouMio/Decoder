using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class screen : MonoBehaviour {
	Sprite[] all_img;
	
	void Start () {
		all_img = Resources.LoadAll<Sprite>("battle/img/screen");
		this.GetComponent<Renderer>().enabled = false;
	}
	
	public void hide () {
		this.GetComponent<Renderer>().enabled = false;
	}
	
	public void setImg (int index) {
		this.GetComponent<Renderer>().enabled = true;
		this.GetComponent<SpriteRenderer>().sprite = all_img[index];
	}
}
