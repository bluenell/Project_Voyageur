﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

	#region Public Variables
	[Header("Player Stats")]
	public float stamina;
	public float xSpeed, ySpeed;
	public bool isMoving;
	public bool facingRight;

	public float armsReach;

	[Header("Canoe")]
	public GameObject canoe;
	public GameObject pickUpTarget;
	public bool canPickUp, hasCanoe, inRangeOfCanoe;

	[Header("Items)")]

	public GameObject torch;

	#endregion

	#region Private Variables

	Rigidbody2D rb;
	SpriteRenderer sprite;
	Animator anim;
	PlayerInventory inventory;
	DayNightCycleManager nightCycle;

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


		//canoe = GameObject.Find("Canoe");
		//canoeTarget = GameObject.Find("canoeTarget");
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

		rb.velocity = new Vector2(moveX * xSpeed, moveY * ySpeed);

		float yPos = transform.position.y;

		#region FlipCharacter
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
		if (Input.GetButtonDown("Button B") && inRangeOfCanoe)
		{
			//pick up canoe
			transform.position = pickUpTarget.transform.position;
			canoe.SetActive(false);
			canoe.transform.SetParent(transform);
			anim.SetTrigger("PickUp");
			anim.SetBool("isCarrying", true);
			hasCanoe = true;


		}
		else if(Input.GetButtonDown("Button B") && hasCanoe)
		{
			StartCoroutine(RevealCanoe());
			canoe.transform.SetParent(null);
			anim.SetTrigger("PutDown");
			anim.SetBool("isCarrying", false);
			hasCanoe = false;

		}
	}

	void CycleInventory()
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


	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(transform.position, armsReach);
	}


	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "CanoePickUpRange") 
		{
			canPickUp = true;
			inRangeOfCanoe = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "CanoePickUpRange") 
		{
			canPickUp = false;
			inRangeOfCanoe = false;
		}
	}

	IEnumerator RevealCanoe()
	{
		yield return new WaitForSeconds(0.8f);
		canoe.SetActive(true);
	}

}

		





