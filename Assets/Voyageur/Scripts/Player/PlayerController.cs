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
	public bool isMoving;
	public bool facingRight;
	public float armsReach;
	float xSpeed, ySpeed;
	public bool movementDisabled;

	#endregion

	#region Canoe Variables
	[Header("Canoe")]
	public GameObject canoe;
	public float defaultCanoeWalkSpeed;
	float canoeWalkSpeed;


	Transform pickUpTarget;
	Transform putDownTarget;
	Transform spawnTarget;


	public bool inRangeOfCanoe;
	public bool inRangeParkingSpace;
	public bool inRangeOfLaunchingZone;

	public bool targetFound;

	public bool canPickUp;
	public bool canPutDown;
	public bool canLaunch;

	public bool carryingCanoe;

	public string canoeHandleType;
	

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
	void FixedUpdate()
	{
		Move();
	}

	private void Update()
	{

		if (Time.time >= nextSwitchTime)
		{
			if (Input.GetButtonDown("InventoryLeft"))
			{
				playerSoundManager.PlayItemSwitch();
				CycleInventory("left");
				nextSwitchTime = Time.time + 1f / switchRate;
			}
			if (Input.GetButtonDown("InventoryRight"))
			{
				playerSoundManager.PlayItemSwitch();
				CycleInventory("right");
				nextSwitchTime = Time.time + 1f / switchRate;

			}
		}

		if (Input.GetButtonDown("Button A"))
		{
			if (carryingCanoe && inRangeOfLaunchingZone)
			{
				targetFound = true;
				canoeHandleType = "launch";
			}
			else if (carryingCanoe && inRangeParkingSpace)
			{
				targetFound = true;
				canoeHandleType = "putdown";
			}
			else if (!carryingCanoe && inRangeOfCanoe)
			{
				targetFound = true;
				canoeHandleType = "pickup";
			}
			else
			{
				targetFound = false;
				canoeHandleType = "";
			}
		}

		if (targetFound)
		{
			HandleCanoe(canoeHandleType);
		}


		if (Input.GetButtonDown("Button X"))
		{
			WhistleMonty();
		}


	}


	//Controls the player movment when not holding the canoe
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
	//Detects input and what interacted with

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
		if (type == "pickup")
		{
			DisablePlayerInput();
			MoveTowardsTarget(pickUpTarget);

			if (CheckIfAtTarget(pickUpTarget))
			{
				targetFound = false;
				carryingCanoe = true;

				canoe.SetActive(false);
				anim.SetTrigger("PickUp");
				StartCoroutine(EnablePlayerInput(0.8f));
			}	
			
		}
		else if (type ==  "putdown")
		{
			DisablePlayerInput();
			MoveTowardsTarget(putDownTarget);

			if (CheckIfAtTarget(putDownTarget))
			{
				targetFound = false;
				carryingCanoe = false;

				anim.SetTrigger("PutDown");
				canoe.transform.position = spawnTarget.position;

				StartCoroutine(RevealCanoe(0.8f));
				StartCoroutine(EnablePlayerInput(0.8f));

			}

		
		}
		else if (type == "launch")
		{
			DisablePlayerInput();
			MoveTowardsTarget(putDownTarget);

			if (CheckIfAtTarget(putDownTarget))
			{
				targetFound = false;
				carryingCanoe = false;

				anim.SetTrigger("PutDown");

				canoe.transform.position = spawnTarget.position;

				StartCoroutine(RevealCanoe(0.8f));
				StartCoroutine(EnablePlayerInput(0.8f));
				Debug.Log("Launched");


			}
		}
	}

	void HandleMonty()
	{
		if (montyStateManager.inFetch)
		{
			//checking if monty has the stick and the player is within range of picking it up
			if (montyStateVariables.montyHasStick && montyStateVariables.GetPlayerDistanceFromStick() <= montyStateVariables.GetFetchStick().GetComponent<Stick>().range)
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
			else if (montyStateVariables.playerHasStick)
			{
				anim.SetTrigger("throwStick");
				Debug.Log("Throw Stick");

			}
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
		montyStateVariables.throwCount++;
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

	void MoveTowardsTarget(Transform target)
	{
		anim.SetBool("isMoving", true);
		transform.position = Vector2.MoveTowards(transform.position, target.position, defaultXSpeed * Time.deltaTime);


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

	bool CheckIfAtTarget(Transform target)
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

	




		





