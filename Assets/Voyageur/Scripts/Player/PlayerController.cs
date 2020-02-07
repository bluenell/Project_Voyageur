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

	[Header("Player Inventory")]
	public bool hasWood;
	public bool hasStick;
	public bool hasFish;
	public bool hasCanoe, canPickUp;

	[Header("Canoe")]
	public GameObject canoe;
	public GameObject canoeTarget;
	public GameObject putDownTarget;

	#endregion



	#region Private Variables

	Rigidbody2D rb;
	SpriteRenderer sprite;
	Animator anim;
	

	#endregion

	// Start is called before the first frame update
	void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
		anim = transform.GetChild(0).GetComponent<Animator>();


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
		HandleCanoe();
	}


	//Controls the play  er movment when not holding the canoe
	void Move()
	{

		float moveX = Input.GetAxis("Horizontal");
		float moveY = Input.GetAxis("Vertical");
		

		rb.velocity = new Vector2(moveX * xSpeed, moveY* ySpeed);

		float yPos = transform.position.y;

		#region FlipCharacter
		if (moveX < 0f)
		{
			sprite.flipX = true;
		}
		if (moveX > 0f)
		{
		 sprite.flipX = false;
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

		/*
		if (Input.GetButtonDown("Joystick X") && hasCanoe)
		{
			canoe.transform.position = putDownTarget.transform.position;
			canoe.transform.SetParent(null);
			hasCanoe = false;
			canPickUp = true;
		}
		*/

		
	}



}
