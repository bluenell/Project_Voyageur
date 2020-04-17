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
	public float fishResponseTime, fishBagTime;
	float timer;
	int buttonPresses = 0;
	bool fishBite;
	bool catchSucces;
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
		if (!fishBite && !catchSucces)
		{
			playerAnimator.SetTrigger("cast");

			timer += Time.deltaTime;

			Debug.Log((int)timer);

			if (timer >= Random.Range(fishCatchTime.x, fishCatchTime.y))
			{
				timer = 0;
				fishBite = true;
				Debug.Log("Bite");
			}

		}
		else if (fishBite && !catchSucces)
		{
			timer += Time.deltaTime;
			Debug.Log((int)timer);

			if (timer < 2 && (Input.GetButtonDown("Button A")) || Input.GetKeyDown(KeyCode.E))
			{			
				Debug.Log("Caugt");
				catchSucces = true;
				timer = 0;

			}
			else
			{
				fishBite = false;
				catchSucces = false;
			}


		}
		else if (fishBite && catchSucces)
		{
			timer += Time.deltaTime;
			Debug.Log((int)timer);

			if (timer < 2 && (Input.GetButtonDown("Button A")) || Input.GetKeyDown(KeyCode.E)) 
			{
				Debug.Log("Fish bagged");
			}
			else
			{
				Debug.Log("Fish thrown away");
			}

			fishBite = false;
			catchSucces = false;
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
