using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	#region Player Variables
	[Header("Player")]
	public float stamina;
	public float defaultXSpeed, defaultYSpeed;
	[HideInInspector]
	public bool isMoving;
	[HideInInspector]
	public bool facingRight;
	public float armsReach;

	float xSpeed, ySpeed;
	[HideInInspector]
	public bool movementDisabled;

	#endregion

	#region Interaction Variables
	[Header("Canoe")]
	public GameObject canoe;
	public float defaultCanoeWalkSpeed;
	float canoeWalkSpeed;


	Transform pickUpTarget;
	Transform putDownTarget;
	Transform spawnTarget;

	bool inRangeOfCanoe;
	bool inRangeParkingSpace;
	bool inRangeOfLaunchingZone;

	bool targetFound;

	bool canPickUp;
	bool canPutDown;
	bool canLaunch;
	public int pushCounter;

	bool carryingCanoe;

	public string interactionType;
	bool playingFetch = false;
	public bool usingAxe = false;

	#endregion

	#region Monty Variables
	MontyStateManager montyStateManager;
	MontyStateVariables montyStateVariables;
	MontyStateActions montyStateActions;
	GameObject montyObj;

	#endregion

	#region Inventory
	[Header("Items)")]
	public GameObject torch;
	bool torchOn = false;

	Vector3 rightTorchTransform = new Vector3(0.608f, 1.642f, 0);
	Vector3 leftTorchTransform = new Vector3(-0.608f, 1.642f, 0);
	Quaternion leftTorchRot;
	Quaternion rightTorchRot;


	PlayerInventory inventory;
	public int currentInventoryIndex;

	public float switchRate = 2f;
	float nextSwitchTime = 0f;
	#endregion

	#region Components
	Rigidbody2D rb;
	SpriteRenderer sprite;
	Animator anim;

	DayNightCycleManager nightCycle;
	BoxCollider2D parkingCollider;
	PlayerSoundManager playerSoundManager;
	CameraHandler cameraHandler;
	TransitionHandler transitionHandler;
	InteractionsManager interactionsManager;
	IndividualInteractions individualInteractions;
	#endregion

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
		anim = transform.GetChild(0).GetComponent<Animator>();
		inventory = GetComponent<PlayerInventory>();
		nightCycle = GameObject.Find("Global Light (Sun)").GetComponent<DayNightCycleManager>();
		playerSoundManager = GetComponent<PlayerSoundManager>();
		cameraHandler = GameObject.Find("Camera Manager").GetComponent<CameraHandler>();
		transitionHandler = GameObject.Find("Transition Handler").GetComponent<TransitionHandler>();
		interactionsManager = GetComponent<InteractionsManager>();
		individualInteractions = GameObject.Find("Interactions Manager").GetComponent<IndividualInteractions>();

		pickUpTarget = canoe.transform.GetChild(0).transform;

		montyObj = GameObject.Find("Monty");
		montyStateActions = montyObj.GetComponent<MontyStateActions>();
		montyStateManager = montyObj.GetComponent<MontyStateManager>();
		montyStateVariables = montyObj.GetComponent<MontyStateVariables>();



		//canoe = GameObject.Find("Canoe");
		//canoeTarget = GameObject.Find("canoeTarget");

		xSpeed = defaultXSpeed;
		ySpeed = defaultYSpeed;
		canoeWalkSpeed = defaultCanoeWalkSpeed;
	}
	private void FixedUpdate()
	{
		Move();
	}

	private void Update()
	{

		HandleInput();


		if (targetFound)
		{
			HandleCanoe(interactionType);
		}
		else if (playingFetch)
		{
			HandleMonty();
		}
		else if (usingAxe && !CheckIfAtTarget(interactionsManager.interaction.transform.GetChild(0), false) && !interactionsManager.interaction.complete)
		{
			MoveTowardsTarget(interactionsManager.interaction.transform.GetChild(0), false);
		}
	}

	void HandleInput()
	{
		if (Time.time >= nextSwitchTime)
		{
			if (Input.GetButtonDown("InventoryLeft") || Input.GetAxis("Mouse ScrollWheel") < 0)
			{
				playerSoundManager.PlayItemSwitch();
				CycleInventory("left");
				nextSwitchTime = Time.time + 1f / switchRate;
			}
			if (Input.GetButtonDown("InventoryRight") || Input.GetAxis("Mouse ScrollWheel") > 0)
			{
				playerSoundManager.PlayItemSwitch();
				CycleInventory("right");
				nextSwitchTime = Time.time + 1f / switchRate;

			}
		}

		if (Input.GetButtonDown("Button A") || Input.GetKeyDown(KeyCode.E))
		{
			if (!usingAxe)
			{
				if (carryingCanoe)
				{
					if (inRangeOfLaunchingZone)
					{
						targetFound = true;
						interactionType = "beginLaunch";
					}
					else if (inRangeParkingSpace)
					{

						targetFound = true;
						interactionType = "putdown";
					}
				}
				else if (!carryingCanoe && montyStateManager.inFetch)
				{
					if (montyStateVariables.GetPlayerNearStick())
					{
						playingFetch = true;
						interactionType = "pickUpStick";
					}
					else if (montyStateVariables.playerHasStick)
					{
						playingFetch = true;
						interactionType = "throw";
					}
					else if (inRangeOfCanoe && !montyStateVariables.GetPlayerNearStick())
					{
						targetFound = true;
						interactionType = "pickUpCanoe";
					}
					else
					{
						targetFound = false;
						playingFetch = false;
						interactionType = "";
					}

				}

				else if (!carryingCanoe && interactionsManager.inRange && !interactionsManager.interaction.complete)
				{
					if (currentInventoryIndex == 1)
					{
						usingAxe = true;
					}
				}

				else if (!carryingCanoe)
				{
					if (inRangeOfCanoe && !canLaunch)
					{
						targetFound = true;
						interactionType = "pickUpCanoe";
					}
					else if (inRangeOfCanoe && canLaunch)
					{
						targetFound = true;
						interactionType = "launch";
					}
					else
					{
						targetFound = false;
						interactionType = "";
					}
				}
			}
		}
		else
		{
			playingFetch = false;
			//targetFound = false;

		}


		if (Input.GetButtonDown("Button X") || Input.GetKeyDown(KeyCode.Q))
		{
			WhistleMonty();
		}

		if (Input.GetButtonDown("Button Select") || Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Button Start"))
		{
			Debug.Log("Open Journal");
		}

	}

	void Move()
	{
		float moveX = Input.GetAxisRaw("Horizontal");
		float moveY = Input.GetAxisRaw("Vertical");

		//Debug.Log("X Input " + moveX + " Y Input " + moveY);

		if (carryingCanoe)
		{
			rb.velocity = new Vector2(moveX * canoeWalkSpeed, moveY * canoeWalkSpeed);
		}
		else
		{
			rb.velocity = new Vector2(moveX * xSpeed, moveY * ySpeed);
		}


		float yPos = transform.position.y;


		if (moveX < 0f)
		{
			facingRight = false;
			anim.SetBool("facingRight", false);
			//sprite.flipX = true;
			torch.transform.rotation = Quaternion.Euler(0, 0, 90);
			torch.transform.localPosition = leftTorchTransform;
		}
		if (moveX > 0f)
		{
			facingRight = true;
			anim.SetBool("facingRight", true);

			//sprite.flipX = false;
			torch.transform.rotation = Quaternion.Euler(0, 0, -90);
			torch.transform.localPosition = rightTorchTransform;
		}


		if (moveX != 0 || moveY != 0)
		{
			isMoving = true;
			anim.SetBool("isMoving", true);
		}
		else
		{
			isMoving = false;
			anim.SetBool("isMoving", false);

		}



	}

	void UseItem()
	{
		if (currentInventoryIndex == 3)
		{
			if (torchOn)
			{
				torchOn = false;
				torch.gameObject.SetActive(false);
				playerSoundManager.PlayTorchClickOff();
			}
			else
			{
				torchOn = true;
				torch.gameObject.SetActive(true);
				playerSoundManager.PlayTorchClickOn();
			}
		}
	}

	void HandleCanoe(string type)
	{

		currentInventoryIndex = 0;
		anim.SetInteger("inventoryIndex", 0);

		if (type == "pickUpCanoe")
		{
			DisablePlayerInput();
			MoveTowardsTarget(pickUpTarget, false);

			if (CheckIfAtTarget(pickUpTarget, false))
			{
				targetFound = false;
				carryingCanoe = true;


				anim.SetTrigger("PickUp");
				canoe.SetActive(false);
				StartCoroutine(EnablePlayerInput(0.8f));
			}

		}
		else if (type == "putdown")
		{
			DisablePlayerInput();
			MoveTowardsTarget(spawnTarget, true);

			if (CheckIfAtTarget(spawnTarget, true))
			{
				targetFound = false;
				carryingCanoe = false;

				anim.SetTrigger("PutDown");
				canoe.transform.position = new Vector3(transform.position.x, spawnTarget.position.y, 0);
				transform.position = canoe.transform.GetChild(0).transform.position;
				StartCoroutine(RevealCanoe(0.8f));
				StartCoroutine(EnablePlayerInput(0.8f));

			}
		}
		else if (type == "beginLaunch")
		{
			DisablePlayerInput();
			MoveTowardsTarget(putDownTarget, false);
			currentInventoryIndex = 0;
			anim.SetInteger("inventoryIndex", 0);

			if (CheckIfAtTarget(putDownTarget, false))
			{
				targetFound = false;
				carryingCanoe = false;

				anim.SetTrigger("PutDown");

				canoe.transform.position = spawnTarget.position;

				StartCoroutine(RevealCanoe(0.8f));
				StartCoroutine(EnablePlayerInput(0.8f));
				canLaunch = true;
				Debug.Log("Launched");
			}
		}
		else if (type == "launch")
		{
			if (pushCounter >= 3)
			{
				transitionHandler.PreLaunch();
			}
			else
			{
				DisablePlayerInput();
				MoveTowardsTarget(pickUpTarget, false);

				if (CheckIfAtTarget(pickUpTarget, false))
				{
					pushCounter++;
					canoe.GetComponent<Animator>().SetInteger("pushCounter", pushCounter);
					anim.SetTrigger("pushCanoe");


					StartCoroutine(RevealCanoe(0.8f));
					StartCoroutine(EnablePlayerInput(0.8f));

					canLaunch = true;
					targetFound = false;
					carryingCanoe = false;
				}
			}

			
			

			
		}
	}

	void HandleMonty()
	{
		//checking if monty has the stick and the player is within range of picking it up
		if (interactionType == "pickUpStick")
		{
			montyStateVariables.GetFetchStick().transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
			DisablePlayerInput();
			currentInventoryIndex = 0;
			anim.SetInteger("inventoryIndex", currentInventoryIndex);
			anim.SetTrigger("pickUpStick");

			//setting the position of the stick object to around the players arm height (can easily be changed by moving the target object in the scene)
			montyStateVariables.GetFetchStick().transform.position = transform.GetChild(3).transform.position;
			montyStateVariables.playerHasStick = true;
			montyStateVariables.montyHasStick = false;
		}
		if (interactionType == "throw")
		{
			anim.SetTrigger("throwStick");
			Debug.Log("Throw Stick");
		}
	}

	void WhistleMonty()
	{
		if (!montyStateManager.inFetch)
		{
			playerSoundManager.PlayWhistle();
			montyStateVariables.movingTowardsPlayer = true;
		}
	}

	void CycleInventory(string dir)
	{

		if (dir == "right")
		{
			currentInventoryIndex++;
			if (currentInventoryIndex == inventory.tools.Count)
			{
				currentInventoryIndex = 0;
			}

			//Debug.Log(inventory.tools[currentInventoryIndex]);
			anim.SetInteger("inventoryIndex", currentInventoryIndex);
		}
		else if (dir == "left")
		{
			if (currentInventoryIndex == 0)
			{
				currentInventoryIndex = inventory.tools.Count;
			}
			currentInventoryIndex--;
			anim.SetInteger("inventoryIndex", currentInventoryIndex);
			//Debug.Log(inventory.tools[currentInventoryIndex]);
		}
	}

	public void ThrowStick()
	{
		montyStateVariables.GetFetchStick().transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
		montyStateVariables.playerHasStick = false;
		montyStateVariables.montyHasStick = false;
		montyStateVariables.stickThrown = true;
		montyStateVariables.GetFetchStick().GetComponent<Rigidbody2D>().gravityScale = 1;
		montyStateVariables.GetFetchStick().GetComponent<Rigidbody2D>().velocity = montyStateVariables.CalculateThrowVelocity();
		montyStateVariables.GetFetchStick().GetComponent<Rigidbody2D>().freezeRotation = false;

	}

	public void DisablePlayerInput()
	{
		xSpeed = 0;
		ySpeed = 0;
		canoeWalkSpeed = 0;
		movementDisabled = true;

	}

	IEnumerator RevealCanoe(float time)
	{
		yield return new WaitForSeconds(time);
		canoe.SetActive(true);
	}

	public IEnumerator EnablePlayerInput(float time)
	{
		yield return new WaitForSeconds(time);
		xSpeed = defaultXSpeed;
		ySpeed = defaultYSpeed;
		canoeWalkSpeed = defaultCanoeWalkSpeed;
		movementDisabled = false;

	}

	public void MoveTowardsTarget(Transform target, bool onlyY)
	{
		anim.SetBool("isMoving", true);

		if (onlyY)
		{
			transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, target.position.y), defaultXSpeed * Time.deltaTime);
		}
		else
		{
			transform.position = Vector2.MoveTowards(transform.position, target.position, defaultXSpeed * Time.deltaTime);

		}


		if (transform.position.x > target.transform.position.x)
		{
			facingRight = false;
			anim.SetBool("facingRight", facingRight);
		}
		else
		{
			facingRight = true;
			anim.SetBool("facingRight", facingRight);
		}

	}

	public bool CheckIfAtTarget(Transform target, bool onlyY)
	{
		if (onlyY)
		{
			if (transform.position.y == target.position.y)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			if (transform.position == target.position)
			{

				return true;
			}
			else
			{
				return false;
			}
		}

	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(transform.position, armsReach);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "CanoePickUpRange")
		{
			inRangeOfCanoe = true;
		}

		if (other.gameObject.tag == "PutDownZone")
		{
			putDownTarget = other.gameObject.transform.GetChild(1).transform;
			spawnTarget = other.gameObject.transform.GetChild(0).transform;
			inRangeParkingSpace = true;

		}
		if (other.gameObject.tag == "FetchZoneExit")
		{
			playingFetch = false;
			montyStateManager.inFetch = false;
			montyStateManager.currentState = "roam";
			montyStateManager.SwitchState();
		}

		if (other.gameObject.tag == "FetchZoneSpawner")
		{
			montyStateVariables.GetFetchZoneExits(4).SetActive(true);
			montyStateVariables.GetFetchZoneExits(5).SetActive(true);
		}

		if (other.gameObject.tag == "LaunchingZone")
		{
			inRangeOfLaunchingZone = true;
			putDownTarget = other.gameObject.transform.GetChild(1).transform;
			spawnTarget = other.gameObject.transform.GetChild(0).transform;

		}

	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "CanoePickUpRange")
		{
			inRangeOfCanoe = false;
		}
		if (other.gameObject.tag == "PutDownZone")
		{
			inRangeParkingSpace = false;
		}
		if (other.gameObject.tag == "LaunchingZone")
		{
			inRangeOfLaunchingZone = false;

		}
	}

}












