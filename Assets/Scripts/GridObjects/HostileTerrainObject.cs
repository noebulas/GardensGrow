﻿using UnityEngine;
using System.Collections;

public class HostileTerrainObject : TerrainObject {
	public int damage = 12;
	private int framesPerHit = 10;
	private int currentFrame = 0;

	public bool activeCollider;

	// Use this for initialization
	protected override void Start () {
		base.Start();

		if (!activeCollider) {
			BoxCollider2D thisCollider = this.gameObject.GetComponent<BoxCollider2D>();
			thisCollider.enabled = false;
			Destroy(transform.GetComponent<Rigidbody>());
		}
	}
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			PlayerGridObject player = other.GetComponent<PlayerGridObject>();
			if (player.onPlatform == false) {
				player.TakeDamage(damage);
				player.gameObject.transform.position = Globals.spawnLocation;
			}
		}
	}
}
