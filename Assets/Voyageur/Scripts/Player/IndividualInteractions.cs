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

	public bool xPressed;
	bool movingTowards = false;
	public bool targetFound;

	[Header("Chopping")]
	public int chopCount = 0;


	[Header("Fishing")]
	public Vector2 fishCatchTime;
	int r;
	bool generated;
	public float fishResponseTime, fishBagTime;
	float timer;
	int buttonPresses = 0;
	bool fishBite;
	bool catchSuccess;
	bool casting;

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


	public void Fish()
	{
		if (!fishBite && !catchSuccess)
		{
			if (!generated)
			{
				r = (int)Random.Range(fishCatchTime.x, fishCatchTime.y);
				generated = true;
			}
			playerAnimator.SetTrigger("cast");

			timer += Time.deltaTime;

			Debug.Log("Time til catch: " + (int)timer);

			if (Input.GetButtonDown("Button A") || Input.GetKeyDown(KeyCode.E) && timer < r)
			{
				Debug.Log("Fail");
				generated = false;
				timer = 0;
				playerAnimator.SetTrigger("fail");
			}

			if (timer >= r)
			{
				generated = false;
				timer = 0;
				fishBite = true;
				Debug.Log("Bite");
			}

		}
		else if (fishBite && !catchSuccess)
		{
			timer += Time.deltaTime;
			Debug.Log("Time to reel: " + (int)timer);

			playerAnimator.SetTrigger("bite");

			if (Input.GetButtonDown("Button A") || Input.GetKeyDown(KeyCode.E))
			{
				if (timer < fishResponseTime)
				{
					playerAnimator.SetInteger("fishIndex", 1);
					Debug.Log("Fish Caught");
					timer = 0;
					catchSuccess = true;
				}
				else
				{
					//playerAnimator.SetTrigger("fail");
					Debug.Log("Fish lost");
					timer = 0;
					catchSuccess = false;
					fishBite = false;
				}
			}
			else if (timer > fishResponseTime)
			{
				Debug.Log("Fish lost");
				timer = 0;
				catchSuccess = false;
				fishBite = false;
			}
		}
		else if (fishBite && catchSuccess)
		{
			timer += Time.deltaTime;
			Debug.Log("Time to bag: " + (int)timer);

			if (Input.GetButtonDown("Button A") || Input.GetKeyDown(KeyCode.E))
			{
				if (timer < fishBagTime)
				{
					Debug.Log("Fish bagged");
					timer = 0;

					fishBite = false;
					catchSuccess = false;
				}
			}
			else if (timer > fishResponseTime)
			{
				Debug.Log("Fish thrown");
				timer = 0;

				fishBite = false;
				catchSuccess = false;
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
