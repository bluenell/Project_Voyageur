using System.Collections;
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

	[Header("Canoe")]
	public GameObject canoe;
	public GameObject canoeTarget;
	public GameObject putDownTarget;
	public bool canPickUp;

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


	void Awake()
	{

	}

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
			sprite.flipX = true;
			torch.transform.rotation = Quaternion.Euler(0, 0, 90);
		}
		if (moveX > 0f)
		{
			sprite.flipX = false;
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
	void Interact()
	{

	}

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

	/*
	//Handles the picking up/placing down the canoe
	void HandleCanoe()
	{
		if (Input.GetButtonDown("Joystick X") && canPickUp)
		{
			canoe.transform.position = canoeTarget.transform.position;
			canoe.transform.SetParent(canoeTarget.transform);
			hasCanoe = true;
			canPickUp = false;

		}
		*/

	/*
	if (Input.GetButtonDown("Joystick X") && hasCanoe)
	{
		canoe.transform.position = putDownTarget.transform.position;
		canoe.transform.SetParent(null);
		hasCanoe = false;
		canPickUp = true;
	}
	*/

	void CycleInventory()
	{
		if (Input.GetButtonDown("InventoryRight"))
		{
			currentInventoryIndex++;

			if (currentInventoryIndex == inventory.tools.Count)
			{
				currentInventoryIndex = 0;
			}
			Debug.Log(inventory.tools[currentInventoryIndex]);
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
			Debug.Log(inventory.tools[currentInventoryIndex]);

		}
	}

}

		





