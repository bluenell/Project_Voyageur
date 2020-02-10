using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerMananger : MonoBehaviour
{

	PlayerController playerController;
	GameObject interactedObject;



	// Start is called before the first frame update
	void Start()
	{
		playerController = GetComponent<PlayerController>();

	}


	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Canoe")
		{
			playerController.canPickUp = true;
		}

		if (other.gameObject.tag == "Observation")
		{
			interactedObject = other.gameObject; 
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Canoe")
		{
			playerController.canPickUp = false;
		}
	}

	



}
