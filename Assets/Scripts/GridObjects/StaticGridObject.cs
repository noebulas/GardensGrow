﻿using UnityEngine;

public class StaticGridObject : GridObject {

	public bool isBarrier;
	public BoxCollider2D barrier;

	// Use this for initialization
	protected virtual void Start () {
		barrier.enabled = false;

		if (!isBarrier)
		{
			barrier.enabled = false;
		}
		else 
		{
			barrier.enabled = true;
		}
	}
}