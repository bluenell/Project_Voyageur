using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualInteractions : MonoBehaviour
{
	InteractionsManager manager;
	PlayerController playerController;
	AdditionalSpritesManager spritesManager;
	Animator playerAnimator;
	GameObject player;

	private void Start()
	{
		player = GameObject.Find("Player");
		manager = player.GetComponent<InteractionsManager>();
		playerController = player.GetComponent<PlayerController>();
		spritesManager = GameObject.Find("ExtraSpritesManager").GetComponent<AdditionalSpritesManager>();
		playerAnimator = player.transform.GetChild(0).GetComponent<Animator>();

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
		int chopCounter = 0;
		Vector3 walkTarget = manager.interaction.transform.GetChild(2).transform.position;
		bool movingTowards = false;

		if (Input.GetButtonDown("Button A") && playerController.currentInventoryIndex == 1)
		{
			playerController.DisablePlayerInput();
			movingTowards = true;
		}

		if (movingTowards)
		{
			player.transform.position = Vector2.MoveTowards(player.transform.position, walkTarget, playerController.defaultXSpeed * Time.deltaTime);

		}

		if (player.transform.position == walkTarget)
		{
			chopCounter++;
			manager.interaction.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = spritesManager.sprites[chopCounter - 1];
			playerAnimator.SetInteger("chopCounter", chopCounter);

			do
			{
				if (Input.GetButtonDown("Button X"))
				{
					chopCounter++;
					manager.interaction.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = spritesManager.sprites[chopCounter - 1];
					playerAnimator.SetInteger("chopCounter", chopCounter);

				}
			} while (chopCounter <= 3);

			manager.interaction.transform.GetChild(1).gameObject.SetActive(true);
			StartCoroutine(playerController.EnablePlayerInput(0.5f));
		}
	}

	void Collect()
	{
		//play animation
		//play sound
		//increase counter
		//if counter = max, destroy object
		//add to inventory

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

		anim = manager.interaction.gameObject.transform.GetChild(0).GetComponent<Animator>() ;
		
		anim.SetTrigger("Play");



	}

	public void BlueJay()
	{
		anim = manager.interaction.gameObject.transform.GetChild(0).GetComponent<Animator>();

		anim.SetTrigger("Play");
	}


	public void Fetch()
	{
		Debug.Log("Interation with Fetch");
	}




}
