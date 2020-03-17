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
		if (Input.GetButtonDown("Button X") || (Input.GetKeyDown(KeyCode.Space)))
		{
			xPressed = true;
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

	public void Chop()
	{
		bool chopping;
		int chopCounter = 0;
		GameObject walkTarget = manager.interaction.transform.GetChild(2).gameObject;
		GameObject treeMain = manager.interaction.transform.GetChild(0).gameObject;
		

		if (xPressed && playerController.currentInventoryIndex == 1)
		{
			chopping = true;
			xPressed = false;
			Debug.Log("Sup");
			playerController.DisablePlayerInput();
			movingTowards = true;			
		}
		else if (xPressed && playerController.currentInventoryIndex != 1)
		{

			xPressed = false;
		}

		if (movingTowards)
		{
			player.transform.position = Vector2.MoveTowards(player.transform.position, walkTarget.transform.position, playerController.defaultXSpeed* Time.deltaTime);
		}


		if (player.transform.position == walkTarget.transform.position)
		{
			movingTowards = false;
			Debug.Log("At Target");
			treeMain.GetComponent<SpriteRenderer>().sprite = spritesManager.sprites[0];
			playerAnimator.SetInteger("chopCounter", 1);

			for (int i = 1; i < 4; i++)
			{
				if (Input.GetButtonDown("Button X"))
				{
					treeMain.GetComponent<SpriteRenderer>().sprite = spritesManager.sprites[i];
					playerAnimator.SetInteger("chopCounter", i + 1);
				}
			}

			treeMain.SetActive(false);
			manager.interaction.transform.GetChild(3).gameObject.SetActive(false);
			manager.interaction.transform.GetChild(1).gameObject.SetActive(true);
			StartCoroutine(playerController.EnablePlayerInput(0.5f));
			playerAnimator.SetInteger("chopCounter", 3);
			manager.interaction.gameObject.GetComponent<Interaction>().MarkAsComplete();
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
		Debug.Log("enter fetch");
		montyStateManager.inFetch = true;
		montyStateManager.currentState = "fetch";
		montyStateManager.SwitchState();
	}


}
