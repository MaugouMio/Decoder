using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class lightImgControl : NetworkBehaviour {
	void Awake () {
		this.GetComponent<Renderer>().enabled = false;
	}
	
	public void setPos (float x, float y, float z) {
		this.transform.localPosition = new Vector3(x, y, z);
	}
	
	public void Switch (bool status) {
		this.GetComponent<Renderer>().enabled = status;
	}
	
	public void setTurn (int turn) {
		if (turn == 1) { //set the spot light position
			setPos(-445, 128, 0);
		}
		else {
			setPos(440, 128, 0);
		}
	}
}
