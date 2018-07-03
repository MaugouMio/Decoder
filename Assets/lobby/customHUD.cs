using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class customHUD : MonoBehaviour {
	public NetworkManager manager;
	
	public void startHost () {
		manager.StartHost();
	}
	
	public void connectTo () {
		manager.StartClient();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
