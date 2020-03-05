using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MontyStateActions : MonoBehaviour
{
	Animator anim;
	MontyStateVariables stateVariables;
	GameObject player;
	GameObject followTarget;
	BoxCollider2D followTargetCollider;
	Rigidbody2D rb;
	SpriteRenderer sprite;
	PlayerController playerController;


	bool targetFound;
	Vector2 target;
	float stuckTimer;

	bool fetchTargetFound;
	Vector2 fetchWalkTarget;
	bool montyHasStick;
	public bool playerHasStick = false;
	bool playerCanPickUpStick = false;
	bool pickedUp = false;
	Rigidbody2D stickRb;
	private void Start()
	{
		sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
		stateVariables = GetComponent<MontyStateVariables>();
		anim = transform.GetChild(0).GetComponent<Animator>();
		player = GameObject.Find("Player");
		followTarget = player.transform.GetChild(2).gameObject;
		followTargetCollider = followTarget.GetComponent<BoxCollider2D>();
		rb = GetComponent<Rigidbody2D>();
		playerController = player.GetComponent<PlayerController>();
	}

	private void FixedUpdate()
	{
		

	}


	public void Roam()
	{				
		//Debug.Log("Monty is following");
		anim.SetBool("isRunning", true);
		anim.SetBool("isSitting", false);

		if (!targetFound)
		{
			target = stateVariables.GetRandomPointInBounds(followTargetCollider.bounds);
			targetFound = true;
		}
	   
		transform.position = Vector2.MoveTowards(transform.position,target, stateVariables.montySpeed* Time.deltaTime);

		if (transform.position.x == target.x && transform.position.y == target.y)
		{
			stateVariables.desintationReached = true;
			//Debug.Log("Destination Reached");
		}

		if (stateVariables.desintationReached)
		{
			targetFound = false;			
		}

		if (target.x < transform.position.x)
		{
			sprite.flipX = true;
		}
		else
		{
			sprite.flipX = false;
		}

		if (targetFound && (transform.position.x != target.x && transform.position.y != target.y))
		{

			stuckTimer += Time.deltaTime;
			//Debug.Log(stuckTimer);

			if (stuckTimer >= 3)
			{
				//Debug.Log("Monty Stuck");
				target = stateVariables.GetRandomPointInBounds(followTargetCollider.bounds);
				stuckTimer = 0;
			}
		}
	}
	public void Sit()
	{
		anim.SetBool("isSitting", true);
		Debug.Log("Monty is sitting");
	}
	public void Fetch()
	{
		Transform stick = stateVariables.GetFetchStick();
		fetchWalkTarget = stateVariables.GetFetchStartingPoint();
		Stick stickRange = stick.GetComponent<Stick>();
		stickRb = stick.GetComponent<Rigidbody2D>();

		stick.gameObject.SetActive(false);

		if (fetchWalkTarget != null)
		{
			fetchTargetFound = true;
		}
		else
		{
			fetchTargetFound = false;
		}

		if (fetchTargetFound)
		{
			transform.position = Vector2.MoveTowards(transform.position, fetchWalkTarget, stateVariables.montySpeed * Time.deltaTime);
			anim.SetBool("isRunning", true);
			anim.SetBool("isSitting", false);
		}

		if (transform.position.x == fetchWalkTarget.x && transform.position.y == fetchWalkTarget.y)
		{
			Debug.Log("at fetch location");
			anim.SetBool("isRunning", false);
			anim.SetBool("isSitting", true);
			sprite.flipX = true;
			stick.gameObject.SetActive(true);
			stateVariables.montyHasStick = true;
		}
	}
	
	public void Wait()
	{
		//Debug.Log("Waiting");
		anim.SetBool("isMoving", false);
		anim.SetBool("isSitting", false);
	}

	public void MoveTowards()
	{
		//Debug.Log("Moving Towards");
		transform.position = Vector2.MoveTowards(transform.position, player.transform.position - new Vector3(playerController.armsReach,0,0), stateVariables.montySpeed * Time.deltaTime);
		if (player.transform.position.x < transform.position.x)
		{
			sprite.flipX = true;
		}
		else
		{
			sprite.flipX = false;
		}
	}

	public void Follow()
	{
		anim.SetBool("isMoving", true);
		anim.SetBool("isSitting", false);
		if (playerController.facingRight)
		{
			transform.position = Vector2.MoveTowards(transform.position, player.transform.position - new Vector3(playerController.armsReach, 0, 0), stateVariables.montySpeed * Time.deltaTime);
		}
		else
		{
			sprite.flipX = true;
			transform.position = Vector2.MoveTowards(transform.position, player.transform.position + new Vector3(playerController.armsReach, 0, 0), stateVariables.montySpeed * Time.deltaTime);
		}
		
	}

	public void Canoe()
	{
		Debug.Log("monty is in the canoe");
	}
}
