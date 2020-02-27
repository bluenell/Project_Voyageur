﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{ 
	#region Public Variables
	[Header("Player Stats")]
	public float stamina;
	public float defaultXSpeed, defaultYSpeed;
	public float canoeWalkSpeed;
	public bool isMoving;
	public bool facingRight;

	public float armsReach;

	[Header("Canoe")]
	public GameObject canoe;

	public bool hasCanoe, inRangeOfCanoe, inCanoeZone, canoeTargetFound, parkingSpaceFound;
	Transform canoePutDownTarget;
	Transform canoePickUpTarget;
	Transform playerTarget;

	[Header("Items)")]

	public GameObject torch;

	#endregion

	#region Private Variables

	Rigidbody2D rb;
	SpriteRenderer sprite;
	Animator anim;
	PlayerInventory inventory;
	DayNightCycleManager nightCycle;

	float xSpeed, ySpeed;
	

	int currentInventoryIndex;

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
			rb.velocity = new Vector2(moveX * defaultXSpeed, moveY * defaultYSpeed);
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
		#region FlipCharacter
		

		#endregion

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
		if (Input.GetButtonDown("Button B") && inRangeOfCanoe && !hasCanoe)
		{
			canoeTargetFound = true; 
		}

		if (canoeTargetFound)
		{
			DisablePlayerInput();
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
				Debug.Log("At Canoe");
				canoe.SetActive(false);
				canoe.transform.SetParent(transform);
				anim.SetTrigger("PickUp");
				anim.SetBool("isCarrying", true);
				hasCanoe = true;
				canoeTargetFound = false;
				anim.SetBool("isMoving", false);
				EnablePlayerInput();
			}

		}

		if (Input.GetButtonDown("Button B") && hasCanoe && inCanoeZone)
		{

			parkingSpaceFound = true;
		}

		if (parkingSpaceFound)
		{
			DisablePlayerInput();
			transform.position = Vector2.MoveTowards(transform.position, canoePutDownTarget.transform.position, canoeWalkSpeed * Time.deltaTime);
			anim.SetBool("isMoving", true);

			if (transform.position.x > canoePutDownTarget.transform.position.x)
			{
				facingRight = false;
				anim.SetBool("facingRight", false);
			}
			else
			{
				facingRight = true;
				anim.SetBool("facingRight", true);
			}


			if (transform.position == canoePutDownTarget.transform.position)
			{
				Debug.Log("At Parking Space");
				StartCoroutine(RevealCanoe());				

				anim.SetTrigger("PutDown");
				anim.SetBool("isCarrying", false);

				hasCanoe = false;
				parkingSpaceFound = false;
				anim.SetBool("isMoving", false);
				canoe.transform.SetParent(null);
				canoe.transform.position = canoePutDownTarget.transform.position;
				transform.position = playerTarget.transform.position;
				EnablePlayerInput();
				
			}
		}
		
	} 

	void DisablePlayerInput()
	{
		xSpeed = 0;
		ySpeed = 0;
		
	}

	void EnablePlayerInput()
	{
		xSpeed = defaultXSpeed;
		ySpeed = defaultYSpeed;

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
		}
	}

	IEnumerator RevealCanoe()
	{
		yield return new WaitForSeconds(0.8f);
		canoe.SetActive(true);
	}

	



}

		





