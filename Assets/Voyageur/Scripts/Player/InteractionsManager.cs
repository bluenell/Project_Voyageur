﻿using System.Collections;
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
			if (interaction.GetInteractable())
			{
				canInteract = true;
			}
			else
			{
				canInteract = false;
			}

			if (!interaction.GetComplete() && !interaction.GetCancelled())
			{
				if (interaction.interactionName == "Squirrel")
				{
					indivInteractions.Squirrel();
				}
				else if (interaction.interactionName == "Fetch")
				{
					indivInteractions.Fetch();

				}
				else if (interaction.interactionName == "BlueJay")
				{
					indivInteractions.BlueJay();
				}
				else if (interaction.interactionName == "Deer")
				{
					indivInteractions.Deer();
				}

				else if (playerController.usingAxe)
				{
					if (playerController.currentInventoryIndex == 1)
					{
						if (interaction.interactionName == "TreeChop1")
						{
							indivInteractions.Chop();
						}

						if (interaction.interactionName == "CampWood")
						{
							indivInteractions.CampsiteChop();
						}

						if (interaction.interactionName == "FallenTree")
						{
							indivInteractions.FallenBridge();
						}
					}
				}
				else if (playerController.usingRod)
				{
					if (playerController.currentInventoryIndex == 2)
					{
						if (interaction.interactionName == "Fishing")
						{
							indivInteractions.Fishing();
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



	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Interaction")
		{
			interaction = collision.gameObject.GetComponent<Interaction>();
			if (interaction.interactionName != "fetch")
			{
				inRange = false;
				if (interaction.GetCanBeCancelled())
				{
					interaction.CancelInteraction();
				}
				interaction = null;
			}




		}
	}
}
	
	

