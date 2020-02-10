﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLayerManager : MonoBehaviour
{
	//on whatever object the script is on


	PlayerDogLayerManager manager;

	private void Start()
	{
		manager = GameObject.Find("LayerManager").GetComponent<PlayerDogLayerManager>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{

		if (other.gameObject.tag == "TriggerTop")
		{
			//Debug.Log(gameObject.name + " has collided with top door mat");
			transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 1;
		}

		if (other.gameObject.tag=="TriggerBottom")
		{
			//Debug.Log(gameObject.name + " has collided with bottom door mat");

			transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 4;

		}
	}



}

