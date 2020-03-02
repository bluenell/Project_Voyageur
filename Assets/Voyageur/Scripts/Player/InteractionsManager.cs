using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionsManager : MonoBehaviour
{

	public Interaction interaction;
	IndividualInteractions indivInteractions;


	private void Start()
	{
		indivInteractions = GameObject.Find("Interactions Manager").GetComponent<IndividualInteractions>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Interaction")
		{
			//Debug.Log("Interact");
			interaction = collision.gameObject.GetComponent<Interaction>();

			if (!interaction.complete)
			{
				switch (interaction.interactionName)
				{
					case "Squirrel":
						indivInteractions.Squirrel();
						interaction.MarkAsComplete();
						break;

					case "Fetch":
						indivInteractions.Fetch();
						interaction.MarkAsComplete();
						break;

					case "BlueJay":
						indivInteractions.BlueJay();
						interaction.MarkAsComplete();
						break;

					

				}

				
			}
		}

	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Interaction")
		{
			if (!interaction.complete)
			{
				switch (interaction.interactionName)
				{

					case "TreeChop1":
						Debug.Log("Tree");
						indivInteractions.Chop();
						break;
				}
			}

			
   			
		}
	}
}
