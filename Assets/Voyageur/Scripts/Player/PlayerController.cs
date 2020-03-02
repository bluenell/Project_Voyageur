﻿using System.Collections;
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

	#region Inventory
	[Header("Items)")]
	public GameObject torch;
	PlayerInventory inventory;
	public int currentInventoryIndex;
	#endregion

	#region Components
	Rigidbody2D rb;
	SpriteRenderer sprite;
	Animator anim;

	DayNightCycleManager nightCycle;
	BoxCollider2D parkingCollider;
	#endregion

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
		anim = transform.GetChild(0).GetComponent<Animator>();
		inventory = GetComponent<PlayerInventory>();
		nightCycle = GameObject.Find("Global Light (Sun)").GetComponent<DayNightCycleManager>();

		canoePickUpTarget = canoe.transform.GetChild(0).transform;


		inCanoeZone = false;
		hasCanoe = false;
		inRangeOfCanoe = false;

		//canoe = GameObject.Find("Canoe");
		//canoeTarget = GameObject.Find("canoeTarget");

		xSpeed = defaultXSpeed;
		ySpeed = defaultYSpeed;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		Move();
	}

	private void Update()
	{
		CycleInventory();
		UseItem();
		HandleCanoe();

		if (movementStopped)
		{
			DisablePlayerInput();
		}
		else
		{
			EnablePlayerInput(0.0f);
		}


	}


	//Controls the player movment when not holding the canoe
	void Move()
	{
		float moveX = Input.GetAxis("Horizontal");
		float moveY = Input.GetAxis("Vertical");

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
			}
			if (moveX > 0f)
			{
				facingRight = true;
				anim.SetBool("facingRight", true);

				//sprite.flipX = false;
				torch.transform.rotation = Quaternion.Euler(0, 0, -90);
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
		if ((nightCycle.colourArrayIndex >= 0 && nightCycle.colourArrayIndex <= 9) || (nightCycle.colourArrayIndex >= 18 && nightCycle.colourArrayIndex <= 23))
		{
			if (currentInventoryIndex == 3)
			{
				torch.SetActive(true);
			}
			else
			{
				torch.SetActive(false);
			}
		}		
		else
		{
			torch.SetActive(false);
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
			movementStopped = true;
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
				movementStopped = false;
				
			}

		}

		if (Input.GetButtonDown("Button B") && hasCanoe && inCanoeZone)
		{

			parkingSpaceFound = true;
			movementStopped = true;

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
				movementStopped = false;
				
			}
		}
		
	} 

	public void DisablePlayerInput()
	{
		xSpeed = 0;
		ySpeed = 0;
		canoeWalkSpeed = 0;
		
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

	}



	void CycleInventory()
	{
		if (!hasCanoe)
		{
			if (Input.GetButtonDown("InventoryRight"))
			{
				currentInventoryIndex++;

				if (currentInventoryIndex == inventory.tools.Count)
				{
					currentInventoryIndex = 0;
				}
				//Debug.Log(inventory.tools[currentInventoryIndex]);
				anim.SetInteger("inventoryIndex", currentInventoryIndex);
			}

			if (Input.GetButtonDown("InventoryLeft"))
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

	




		





