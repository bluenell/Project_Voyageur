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

	[Header("General")]
	bool movingTowards = false;
	public bool targetFound;

	
	[HideInInspector]
	public float timer;
	[HideInInspector]
	public int fishStage;
	public int logStage;

	[Header("Fishing")]
	public Vector2 randomWaitTime;
	[HideInInspector]
	public bool generated;
	[HideInInspector]
	public float random;
	[HideInInspector]
	public bool fishing;
	[HideInInspector]
	public bool lineCast;
	[HideInInspector]
	public bool firstPress;

	[HideInInspector]
	public int chopCount = 0;
	bool animTriggered;


	public bool hasLogs;
	public bool pickedUp;
	private void Start()
	{
		player = GameObject.Find("Player");
		manager = player.GetComponent<InteractionsManager>();
		playerController = player.GetComponent<PlayerController>();
		spritesManager = GameObject.Find("ExtraSpritesManager").GetComponent<AdditionalSpritesManager>();
		playerAnimator = player.transform.GetChild(0).GetComponent<Animator>();
		montyStateManager = GameObject.Find("Monty").GetComponent<MontyStateManager>();
	}


	private void Update()
	{
		if (fishing)
		{
			timer += Time.deltaTime;
		}
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

	public void Fishing()
	{
		fishing = true;

		if (playerController.usingRod)
		{
			playerController.DisablePlayerInput();

			if (fishStage == 0)
			{
				if (!lineCast)
				{
					playerAnimator.SetTrigger("fishing_cast");
					lineCast = true;
				}

				Debug.Log("waiting for bite");

				if (!generated)
				{
					random = Random.Range(randomWaitTime.x, randomWaitTime.y);
					generated = true;
				}

				if (timer >= random)
				{
					Debug.Log("Bite");
					playerAnimator.SetTrigger("fishing_bite");
					timer = 0;
					generated = false;
					fishStage = 1;
				}
				else if ((Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Button A")) && timer < random)
				{
					playerAnimator.SetTrigger("fishing_fail");
					Debug.Log("reel too early");
				}

			}
			else if (fishStage == 1)
			{

				if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Button A"))
				{
					if (timer < 1f)
					{
						playerAnimator.SetInteger("fishing_randomIndex", Random.Range(1, 5));
						Debug.Log("caught");
						timer = 0;
						fishStage = 2;
					}
				}
				else if (timer > 1f)
				{
					Debug.Log("Didn't reel");
					playerAnimator.SetTrigger("fishing_fail");

				}

			}
			else if (fishStage == 2)
			{
				if (timer <= 5f && (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Button A")))
				{
					playerAnimator.SetInteger("fishing_randomIndex", 0);
					playerAnimator.SetTrigger("fishing_keep");


				}
				else if (timer >= 5f)
				{
					playerAnimator.SetInteger("fishing_randomIndex", 0);
					playerAnimator.SetTrigger("fishing_throw");
					
				}
			}
		}
	}




	public void Chop()
	{
		if (chopCount >2)
		{
			chopCount = 0;
			Debug.Log("Chopped");
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
		Debug.Log("Chopping");
		if (chopCount > 3)
		{
			Debug.Log("done");
		}
		else
		{
			playerController.DisablePlayerInput();
			if (playerController.CheckIfAtTarget(manager.interaction.transform.GetChild(0), false))
			{
				manager.interaction.transform.GetChild(1).gameObject.SetActive(false);
				manager.interaction.transform.GetChild(2).gameObject.SetActive(false);

				targetFound = false;
				playerController.usingAxe = false;
				chopCount++;
				playerAnimator.SetInteger("choppingBlockCounter", chopCount);
				Debug.Log(chopCount);
			}
		}
	}


	public void LogPile()
	{
		playerController.DisablePlayerInput();


		if (playerController.CheckIfAtTarget(manager.interaction.transform.GetChild(0), false))
		{
			if (!pickedUp)
			{
				playerAnimator.SetTrigger("logs_PickUp");
				pickedUp = true;
			}

		}


		if (hasLogs)
		{
			playerController.facingRight = false;
			playerAnimator.SetTrigger("logs_Walk");
			playerController.MoveTowardsTarget(manager.interaction.transform.GetChild(1), false);
			logStage = 2;
		}


		if (playerController.CheckIfAtTarget(manager.interaction.transform.GetChild(1), false))
		{
			playerAnimator.SetTrigger("logs_Drop");
		}



	}


	public void FallenBridge()
	{
		Debug.Log("Fallen Bridge");

		playerController.DisablePlayerInput();
		if (playerController.CheckIfAtTarget(manager.interaction.transform.GetChild(0), false))
		{
			playerController.facingRight = false;
			playerAnimator.SetTrigger("fallTree");
			targetFound = false;
			playerController.usingAxe = false;		
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

		if (timer >= 10)
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
