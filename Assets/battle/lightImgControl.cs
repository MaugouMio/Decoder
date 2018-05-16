using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lightImgControl : MonoBehaviour {
	void Awake () {
		this.GetComponent<Renderer>().enabled = false;
	}
	
	public void setPos (float x, float y, float z) {
		this.transform.localPosition = new Vector3(x, y, z);
	}
	
	public void Switch (bool status) {
		this.GetComponent<Renderer>().enabled = status;
	}
}
