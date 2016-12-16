﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {

	public UnityEngine.UI.Image[] uiPlants;
	public UnityEngine.UI.Text[] uiPlantCounters;

	public Sprite[] seedPackets;

	// Use this for initialization
	void Start () {
		UpdateUI();
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateUI () {
		for (int i = 0; i < 8; i++)
		{
			uiPlantCounters[i].text = "";
			uiPlantCounters[i].color = new Color (1,1,1,1);
			if (Globals.unlockedSeeds[i] == true || Globals.inventory[i] > 0)
			{
				Globals.unlockedSeeds[i] = true;
				uiPlants[i].sprite = seedPackets[i];
				uiPlantCounters[i].text = Globals.inventory[i].ToString();
				if (Globals.inventory[i] == 0) {
					uiPlantCounters[i].color = new Color (0.92f, 0.42f, 0.01f, 1);
				}
				if (Globals.inventory[i] == 9) {
					uiPlantCounters[i].color = new Color (0.35f, 0.73f, 0.13f, 1);
				}
			}
		}
	}
}
