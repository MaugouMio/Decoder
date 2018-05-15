using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lightPos : MonoBehaviour {
	public void setPos (float x, float y, float z) {
		this.transform.localPosition = new Vector3(x, y, z);
	}
}
