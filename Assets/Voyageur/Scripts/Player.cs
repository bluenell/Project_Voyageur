using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
	

	[Header("Player Stats")]
	public float stamina;
	public float speed;

	[Header("Player Inventory")]
	public bool hasWood;
	public bool hasStick;
	public bool hasFish;
	public bool hasCanoe;
	
	
	Rigidbody2D rb;
	SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
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
		float yScaleFactor = 10;
		float scaleMax = 0.9f;

		
		



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
