﻿using UnityEngine;
using System.Collections;

public class RotateableGridObject : GridObject {

	// 0 = South, 1 = West, 2 = North, 3 = East
	protected Globals.Direction direction;
	public BoxCollider2D face;
	
	// Use this for initialization
	protected virtual void Start () {
		direction = 0;
	}

	protected virtual void Update() {
		base.Update();
	}

	// Changes direction and direction sprite
	public void Rotate(Globals.Direction rotate)
	{
		direction = rotate;
		//Debug.Log(direction);

	}
}
