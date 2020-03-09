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
	bool hasCanoe, inRangeOfCanoe, inCanoeZone, canoeTargetFound, parkingSpaceFound, movementStopped;
	Transform canoePutDownTarget;
	Transform canoePickUpTarget;
	Transform currentParkingZone;
	Transform playerTarget;
	Vector2 targetY;
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

	Vector3 rightTorchTransform = new Vector3(0.608f, 1.642f,0);
	Vector3 leftTorchTransform = new Vector3(-0.608f,1.642f,0);
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

		canoePickUpTarget = canoe.transform.GetChild(0).transform;

		montyObj = GameObject.Find("Monty");
		montyStateActions = montyObj.GetComponent<MontyStateActions>();
		montyStateManager = montyObj.GetComponent<MontyStateManager>();
		montyStateVariables = montyObj.GetComponent<MontyStateVariables>();


		inCanoeZone = false;
		hasCanoe = false;
		inRangeOfCanoe = false;

		//canoe = GameObject.Find("Canoe");
		//canoeTarget = GameObject.Find("canoeTarget");

		xSpeed = defaultXSpeed;
		ySpeed = defaultYSpeed;
	}
	void FixedUpdate()
	{
		Move();
	}

	private void Update()
	{
		HandleCanoe();

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

		if (Input.GetButtonDown("Button X"))
		{
			UseItem();
		}

		if (Input.GetButtonDown("Button A"))
		{
			HandleMonty();
		}
		

	}


	//Controls the player movment when not holding the canoe
	void Move()
	{
		float moveX = Input.GetAxisRaw("Horizontal");
		float moveY = Input.GetAxisRaw("Vertical");

		//Debug.Log("X Input " + moveX + " Y Input " + moveY);

		if (hasCanoe)
		{
			rb.velocity = new Vector2(moveX * canoeWalkSpeed, moveY * canoeWalkSpeed);
		}
		else
		{
			rb.velocity = new Vector2(moveX * xSpeed, moveY * ySpeed);
		}
		

		float yPos = transform.position.y;

		if (!canoeTargetFound || !parkingSpaceFound)
		{
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

	void HandleCanoe()
	{
		float moveX = Input.GetAxis("Horizontal");
		float moveY = Input.GetAxis("Vertical");


		if (Input.GetButtonDown("Button B") && inRangeOfCanoe && !hasCanoe)
		{
			currentInventoryIndex = 0;
			anim.SetInteger("inventoryIndex", 0);

			canoeTargetFound = true;
			StartCoroutine(EnablePlayerInput(0));
			xSpeed = 0;
			ySpeed = 0;
			canoeWalkSpeed = 0;
		}

		if (canoeTargetFound)
		{
			
			transform.position = Vector2.MoveTowards(transform.position, canoePickUpTarget.transform.position, defaultXSpeed * Time.deltaTime);
			anim.SetBool("isMoving", true);

		
			if (transform.position.x > canoePickUpTarget.transform.position.x)
			{
				facingRight = false;
				anim.SetBool("facingRight", false);
			}
			else
			{
				facingRight = true;
				anim.SetBool("facingRight", true);
			}

			if (transform.position == canoePickUpTarget.transform.position)
			{
				StartCoroutine(EnablePlayerInput(0.8f));
				Debug.Log("At Canoe");
				
				canoe.transform.SetParent(transform);
				anim.SetTrigger("PickUp");
				anim.SetBool("isCarrying", true);
				canoe.SetActive(false);
				hasCanoe = true;
				canoeTargetFound = false;
				anim.SetBool("isMoving", false);
				StartCoroutine(EnablePlayerInput(0));
				
			}

		}

		if (Input.GetButtonDown("Button B") && hasCanoe && inCanoeZone)
		{

			parkingSpaceFound = true;
			parkingSpaceFound = true;
			DisablePlayerInput();

		}



		if (parkingSpaceFound)
		{
			
			targetY = new Vector2(transform.position.x, currentParkingZone.position.y);
			Debug.DrawLine(transform.position, targetY);
			transform.position = Vector2.MoveTowards(transform.position, targetY, defaultCanoeWalkSpeed * Time.deltaTime);


			anim.SetBool("isMoving", true);

			if (transform.position.x > currentParkingZone.transform.position.x)
			{
				facingRight = false;
				anim.SetBool("facingRight", false);
			}
			else
			{
				facingRight = true;
				anim.SetBool("facingRight", true);
			}


			if (transform.position.y == currentParkingZone.transform.position.y)
			{
				StartCoroutine(EnablePlayerInput(0.8f));
				Debug.Log("At Parking Space");
				StartCoroutine(RevealCanoe(0.8f));				

				anim.SetTrigger("PutDown");
				anim.SetBool("isCarrying", false);

				hasCanoe = false;
				parkingSpaceFound = false;
				anim.SetBool("isMoving", false);
				canoe.transform.SetParent(null);
				canoe.transform.position = new Vector2(transform.position.x,canoePutDownTarget.transform.position.y);
				transform.position = new Vector2(transform.position.x,playerTarget.transform.position.y);
				StartCoroutine(EnablePlayerInput(0));
				
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

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(transform.position, armsReach);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "CanoePickUpRange") 
		{
			inRangeOfCanoe = true;
			//canoePickUpTarget = other.gameObject.transform.GetChild(0).transform;
			
		}

		if (other.gameObject.tag == "PutDownZone")
		{
			inCanoeZone = true;
			canoePutDownTarget = other.gameObject.transform.GetChild(0).transform;
			playerTarget = other.gameObject.transform.GetChild(1).transform;
			currentParkingZone = other.gameObject.transform.GetChild(0).transform;
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
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "CanoePickUpRange") 
		{
			inRangeOfCanoe = false;
		}
		if (other.gameObject.tag == "PutDownZone")
		{
			inCanoeZone = false;
			canoePutDownTarget = null;
			currentParkingZone = null;
		}
	}	
}

	




		





