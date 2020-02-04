using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{

	#region Public Variables
	[Header("Player Stats")]
	public float stamina;
	public float speed;

	[Header("Player Inventory")]
	public bool hasWood;
	public bool hasStick;
	public bool hasFish;
	public bool hasCanoe;
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
    }
	
    // Update is called once per frame
    void FixedUpdate()
    {
		Move();
    }

	//Controls the player movment when not holding the canoe
	void Move()
	{

		float moveX = Input.GetAxis("Horizontal");
		float moveY = Input.GetAxis("Vertical");
		

		rb.velocity = new Vector2(moveX, moveY) * speed;

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

		if (moveX != 0 || moveY != 0)
		{
			anim.SetBool("isMoving", true);
		}
		else
		{
			anim.SetBool("isMoving", false);

		}



		#endregion




		


	}
	//Detects input and what interacted with
	void Interact()
	{

	}
	//Handles the picking up/placing down the canoe
	void HandleCanoe()
	{

	}

}
