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
		if (!stateVariables.stickThrown || stateVariables.montyReturningStick)
		{
			transform.position = Vector2.MoveTowards(transform.position, stateVariables.GetFetchStartingPoint(), stateVariables.montySpeed*Time.deltaTime);
			anim.SetBool("isSitting", false);
			anim.SetBool("isRunning", true);

			if (stateVariables.montyReturningStick)
			{
				sprite.flipX = true;
			}
			else
			{
				sprite.flipX = false;
			}
		}
		else if (stateVariables.stickThrown)
		{
			transform.position = Vector2.MoveTowards(transform.position, stateVariables.GetThrowTarget().position, stateVariables.montySpeed * Time.deltaTime);
			anim.SetBool("isSitting", false);
			anim.SetBool("isRunning", true);
			sprite.flipX = false;

		}

		if (transform.position.x == stateVariables.GetFetchStartingPoint().x && transform.position.y == stateVariables.GetFetchStartingPoint().y)
		{
			//Resetting animator and facing in the correct spot
			Debug.Log("Monty at stick");
			anim.SetBool("isRunning", false);
			anim.SetBool("isSitting", true);
			sprite.flipX = true;

			stateVariables.stickThrown = false;
			stateVariables.montyHasStick = true;
			stateVariables.montyReturningStick = false;			

			if (stateVariables.playerHasStick)
			{
				stateVariables.montyHasStick = false;
				stateVariables.GetFetchStick().transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
			}
			else
			{
				montyHasStick = true;
				stateVariables.GetFetchStick().transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
			}
		}

		if (transform.position.x == stateVariables.GetThrowTarget().position.x && transform.position.y == stateVariables.GetThrowTarget().position.y)
		{
			Debug.Log("At thrown stick");
			anim.SetBool("isRunning", false);
			sprite.flipX = true;
			stateVariables.montyReturningStick = true;
			stateVariables.GetFetchStick().position = stateVariables.GetStickSpawnLocation().position;
			stateVariables.GetFetchStick().transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
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
