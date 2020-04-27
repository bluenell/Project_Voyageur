using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLayerManager : MonoBehaviour
{
   
	Doormat currentDoormat;
	SpriteRenderer sprite;


	private void Start()
	{
		sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();	
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Door mat")
		{
			currentDoormat = other.GetComponent<Doormat>();
			sprite.sortingOrder = currentDoormat.GetSortingOrder();
		}
		
	}
}

