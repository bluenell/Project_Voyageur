using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionsManager : MonoBehaviour
{

	public Interaction interaction;
	IndividualInteractions indivInteractions;
	public bool canInteract;
	public bool inRange;
	PlayerController playerController;

	private void Start()
	{
		indivInteractions = GameObject.Find("Interactions Manager").GetComponent<IndividualInteractions>();
		playerController = GetComponent<PlayerController>();
	}

	private void Update()
	{
		if (interaction != null)
		{
			if (interaction.isInteractable)
			{
				canInteract = true;
			}
			else
			{
				canInteract = false;
			}

			if (!interaction.complete)
			{
				if (interaction.interactionName == "Squirrel")
				{
					indivInteractions.Squirrel();
					interaction.MarkAsComplete();
				}
				else if (interaction.interactionName == "Fetch")
				{
					indivInteractions.Fetch();
					
				}
				else if (interaction.interactionName == "BlueJay")
				{
					indivInteractions.BlueJay();
					interaction.MarkAsComplete();
				}

				else if (playerController.usingAxe)
				{
					if (playerController.currentInventoryIndex == 1)
					{
						if (interaction.interactionName == "TreeChop1")
						{
							indivInteractions.Chop();
						}
					}
				}

			
				
			}
		}
	}



	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Interaction")
		{
			inRange = true;
			//Debug.Log("Interact");
			interaction = collision.gameObject.GetComponent<Interaction>();
		}

	}
}
