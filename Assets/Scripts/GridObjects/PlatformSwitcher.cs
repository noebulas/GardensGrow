﻿using UnityEngine;
using System.Collections;

public class PlatformSwitcher : MonoBehaviour {
	public Globals.Direction directionToSwitchTo;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("Platform")) {
			PlatformGridObject platform = other.GetComponent<PlatformGridObject>();
			platform.changeDirection(directionToSwitchTo);
		}
	}
}
