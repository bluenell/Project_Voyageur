using System.Collections;
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

	private void Update()
	{
		
	}

	private void OnTriggerEnter2D(Collider2D other)
	{

		if (other.gameObject.tag == "TriggerTop")
		{
			//Debug.Log(gameObject.name + " has collided with top door mat");

			if (other.gameObject.transform.parent.name == "1")
			{
				transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 10;
			}
			if (other.gameObject.transform.parent.name == "2")
			{
				transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 4;
			}
			if (other.gameObject.transform.parent.name == "3")
			{
				transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 1;
			}
			if (other.gameObject.transform.parent.name == "4")
			{

				Debug.Log("Top 4");
				transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 1;
			}

		}

		if (other.gameObject.tag=="TriggerBottom")
		{
			//Debug.Log(gameObject.name + " has collided with bottom door mat");

			if (other.gameObject.transform.parent.name == "1")
			{
				transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 13;
			}
			if (other.gameObject.transform.parent.name == "2")
			{
				transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 10;
			}
			if (other.gameObject.transform.parent.name == "3")
			{
				Debug.Log("Bottom 3");
				transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 4;
			}
			if (other.gameObject.transform.parent.name == "4")
			{
				Debug.Log("Bottom 4");
				transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 4;
			}



		}

	}



}

