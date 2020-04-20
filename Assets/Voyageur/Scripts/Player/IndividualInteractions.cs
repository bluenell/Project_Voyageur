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
	MontyStateManager montyStateManager;
	PlayerInventory inventory;

	public bool xPressed;
	bool movingTowards = false;
	public bool targetFound;

	[Header("Chopping")]
	public int chopCount = 0;


	[Header("Fishing")]
	public Vector2 fishCatchTime;
	bool lineCast;
	int r;
	bool generated;
	public float fishResponseTime, fishBagTime;
	float timer;
	int buttonPresses = 0;
	bool fishBite;
	bool catchSuccess;
	bool casting;
	public string currentFishingState;

	private void Start()
	{
		player = GameObject.Find("Player");
		manager = player.GetComponent<InteractionsManager>();
		playerController = player.GetComponent<PlayerController>();
		spritesManager = GameObject.Find("ExtraSpritesManager").GetComponent<AdditionalSpritesManager>();
		playerAnimator = player.transform.GetChild(0).GetComponent<Animator>();
		montyStateManager = GameObject.Find("Monty").GetComponent<MontyStateManager>();
		inventory = player.GetComponent<PlayerInventory>();

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


	public void Fish()
	{
		if (!fishBite && !catchSuccess)
		{

			playerController.DisablePlayerInput();

			currentFishingState = "casting line";

			if (!generated)
			{
				r = (int)Random.Range(fishCatchTime.x, fishCatchTime.y);
				generated = true;
			}

			if (!lineCast)
			{
				playerAnimator.SetTrigger("fishing_cast");
				lineCast = true;
			}


			timer += Time.deltaTime;

			//Debug.Log("Time til catch: " + (int)timer);

			if (timer >= r)
			{
				generated = false;
				timer = 0;
				fishBite = true;


				Debug.Log("Bite");
			}

			if (Input.GetButtonDown("Button A") || Input.GetKeyDown(KeyCode.E) && timer < r)
			{
				Debug.Log("Fail");
				generated = false;
				timer = 0;
				playerAnimator.SetTrigger("fishing_reset");
			}



		}
		else if (fishBite && !catchSuccess)
		{
			currentFishingState = "fish bite";
			timer += Time.deltaTime;
			//Debug.Log("Time to reel: " + (int)timer);

			playerAnimator.SetTrigger("fishing_bite");

			if (timer < fishResponseTime)
			{
				// IF THE PLAYER HAS CAUGHT THE FISH

				if (Input.GetButtonDown("Button A") || Input.GetKeyDown(KeyCode.E))
				{
					currentFishingState = "reeling fish";
					playerAnimator.SetInteger("fishing_randomIndex", Random.Range(1, 5));
					Debug.Log("Fish Caught");
					timer = 0;
					catchSuccess = true;
					lineCast = true;

				}
			}
			else
			{
				// IF THE PLAYER PRESSED THE BUTTON BUT WAS TOO LATE

				if (Input.GetButton("Button A") || Input.GetKeyDown(KeyCode.E))
				{
					currentFishingState = "reeling no fish";
					playerAnimator.SetTrigger("fishing_reset");
					Debug.Log("Fish lost");
					timer = 0;
					playerController.usingRod = false;

					manager.interaction = null;
					lineCast = false;

					catchSuccess = false;
					fishBite = false;

				}

				// IF THE PLAYER DIDN'T PRESS THE BUTTON
				else
				{
					currentFishingState = "fish lost";
					//playerAnimator.SetTrigger("fishing_reset");
					timer = 0;
					playerController.usingRod = false;
					catchSuccess = false;
					fishBite = false;
				}
			}

		}

		else if (catchSuccess)
		{
			currentFishingState = "deciding";
			timer += Time.deltaTime;

			if (timer > 0.3f && timer < fishBagTime)
			{
				if (Input.GetButtonDown("Button A") || Input.GetKeyDown(KeyCode.E))
				{
					timer = 0;
					playerAnimator.SetTrigger("fishing_keep");
					Debug.Log("Bag");
					inventory.AddFish();
					fishBite = false;
					catchSuccess = false;
					generated = false;
					lineCast = false;
					playerController.usingRod = false;
					manager.interaction = null;

					playerAnimator.SetInteger("fishing_randomIndex", 0);

					StartCoroutine(playerController.EnablePlayerInput(0));

				}
			}

			else if (timer > fishBagTime)
			{
				timer = 0;
				playerAnimator.SetTrigger("fishing_reset");
				playerController.usingRod = false;
				manager.interaction = null;

				fishBite = false;
				catchSuccess = false;
				generated = false;
				lineCast = false;
				StartCoroutine(playerController.EnablePlayerInput(0));

			}
		}
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
		if (buttonPresses >2)
		{
			manager.interaction.MarkAsComplete();

		}
		else
		{
			playerController.DisablePlayerInput();
			if (playerController.CheckIfAtTarget(manager.interaction.transform.GetChild(0), false))
			{
				targetFound = false;
				playerController.usingAxe = false;
				buttonPresses++;
				playerAnimator.SetInteger("chopCounter", buttonPresses);				
				Debug.Log(buttonPresses);
			}

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
		manager.interaction.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);

		anim = manager.interaction.gameObject.transform.GetChild(0).GetComponent<Animator>() ;
		
		anim.SetTrigger("Play");

		//I wish that bill wasn't recording right now because it's awkward

	}

	public void BlueJay()
	{
		anim = manager.interaction.gameObject.transform.GetChild(0).GetComponent<Animator>();
		manager.interaction.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
		anim.SetTrigger("Play");
	}


	public void Fetch()
	{
		//Debug.Log("enter fetch");
		montyStateManager.inFetch = true;
		montyStateManager.currentState = "fetch";
		montyStateManager.SwitchState();
	}


}
