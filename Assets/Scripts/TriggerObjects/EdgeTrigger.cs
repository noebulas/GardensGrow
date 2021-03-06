﻿using UnityEngine;
using System.Collections.Generic;

public class EdgeTrigger : MonoBehaviour {
    public bool isTriggered;
    public Collider2D other;    // the other collider
    private List<KillableGridObject> killList = new List<KillableGridObject>();

    void Update() {
        if (isTriggered && (!other || !(other.isActiveAndEnabled))) {
            isTriggered = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!other.isTrigger) {
            isTriggered = true;
            this.other = other;
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if (!other.isTrigger) {
            isTriggered = true;
            this.other = other;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (!other.isTrigger) {
            isTriggered = false;
            this.other = other;
        }
    }

    public List<KillableGridObject> getKillList() {
        return killList;
    }

}
