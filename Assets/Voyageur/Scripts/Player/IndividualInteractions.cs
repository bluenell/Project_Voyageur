﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualInteractions : MonoBehaviour
{
	InteractionsManager manager;
	PlayerController playerController;
	AdditionalSpritesManager spritesManager;
	Animator playerAnimator;
	GameObject player;
	MontyStateManager montyStateManager;

	public bool xPressed;
	bool movingTowards = false;
	public bool targetFound;

	float timer;

	public int chopCount = 0;
	bool animTriggered;

	private void Start()
	{
		player = GameObject.Find("Player");
		manager = player.GetComponent<InteractionsManager>();
		playerController = player.GetComponent<PlayerController>();
		spritesManager = GameObject.Find("ExtraSpritesManager").GetComponent<AdditionalSpritesManager>();
		playerAnimator = player.transform.GetChild(0).GetComponent<Animator>();
		montyStateManager = GameObject.Find("Monty").GetComponent<MontyStateManager>();
	}

	Animator anim;

	#region InputInteractions
	void Sleep()
	{
		//check time
		//play animations
		//play sounds
		//advance time to 06:00
	}

	void Eat()
	{
		 //check fish
		 //play animation
		 //stamina = max
	}

	void Harmonica()
	{
		//play clip at index
		//advance index
		//play animation
		//exit


	}

	void PetDog()
	{
		//play animation
		//play sound
	}

	public void Chop()
	{
		if (chopCount >2)
		{
			chopCount = 0;
			manager.interaction.MarkAsComplete();
		}
		else
		{
			playerController.DisablePlayerInput();
			if (playerController.CheckIfAtTarget(manager.interaction.transform.GetChild(0), false))
			{
				targetFound = false;
				playerController.usingAxe = false;
				chopCount++;
				playerAnimator.SetInteger("chopCounter", chopCount);				
				Debug.Log(chopCount);
			}

		}		
	}

	public void CampsiteChop()
	{
		if (chopCount >3)
		{
			chopCount = 0;
			manager.interaction.MarkAsComplete();
			StartCoroutine(playerController.EnablePlayerInput(0));
		}
		else
		{
			manager.interaction.transform.GetChild(1).gameObject.SetActive(false);
			manager.interaction.transform.GetChild(2).gameObject.SetActive(false);

			playerController.DisablePlayerInput();
			if (playerController.CheckIfAtTarget(manager.interaction.transform.GetChild(0), false))
			{
				targetFound = false;
				playerController.usingAxe = false;
				chopCount++;
				playerAnimator.SetInteger("choppingBlockCounter", chopCount);
			}
		}





	}

	void LightFire()
	{
		//check if wood
		//play animation
		//play sound
		//create fire object
		
	}

	void InvestigateMine()
	{
		//play animation
	}

	#endregion 

	public void Squirrel()
	{
		manager.interaction.gameObject.transform.GetChild(0).gameObject.SetActive(true);
		manager.interaction.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);

		anim = manager.interaction.gameObject.transform.GetChild(0).GetComponent<Animator>() ;
		
		anim.SetTrigger("Play");
		manager.interaction.MarkAsComplete();

	}

	public void BlueJay()
	{
		anim = manager.interaction.gameObject.transform.GetChild(0).GetComponent<Animator>();
		manager.interaction.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
		anim.SetTrigger("Play");
		manager.interaction.MarkAsComplete();
	}


	public void Deer()
	{
		anim = manager.interaction.gameObject.transform.GetChild(0).GetComponent<Animator>();



		if (manager.interaction.GetCancelled())
		{
			timer = 0;
			manager.interaction.MarkAsComplete();
		}

		if (!animTriggered)
		{
			anim.SetTrigger("rustle");
			animTriggered = true;
		}

		timer += Time.deltaTime;

		if (timer >= 5)
		{
			timer = 0;
			anim.SetTrigger("reveal");
			manager.interaction.MarkAsComplete();
		}
	}


	public void Fetch()
	{
		//Debug.Log("enter fetch");
		montyStateManager.inFetch = true;
		montyStateManager.currentState = "fetch";
		montyStateManager.SwitchState();
	}


}
