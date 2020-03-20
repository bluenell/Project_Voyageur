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
	CameraHandler cameraHandler;


	bool targetFound;
	Vector2 target;
	float stuckTimer;

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
		cameraHandler = GameObject.Find("Camera Manager").GetComponent<CameraHandler>();
	}

	


	public void Roam()
	{				
		//Debug.Log("Monty is following");
		anim.SetBool("isRunning", false);
		anim.SetBool("isSitting", false);
		anim.SetBool("isWalking", true);

		if (!targetFound)
		{
			target = stateVariables.GetRandomPointInBounds(followTargetCollider.bounds);
			targetFound = true;
		}
	   
		transform.position = Vector2.MoveTowards(transform.position,target, stateVariables.walkSpeed * Time.deltaTime);

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
		//checking if the stick hasn't been thrown yet, or monty is bringing the stick back (when to move monty to the start point)
		if (!stateVariables.stickThrown || stateVariables.montyReturningStick)
		{
			transform.position = Vector2.MoveTowards(transform.position, stateVariables.GetFetchStartingPoint().position, stateVariables.runSpeed*Time.deltaTime);
			anim.SetBool("isWalking", false);
			anim.SetBool("isSitting", false);
			anim.SetBool("isRunning", true);
			stateVariables.montyHasStick = false;

			if (stateVariables.montyReturningStick)
			{
				sprite.flipX = true;
			}
			else
			{
				sprite.flipX = false;
			}
		}
		//checking if the stick has been thrown (when to move monty towards the stick after being thrown)
		else if (stateVariables.stickThrown)
		{
			cameraHandler.SwitchToMonty();
			transform.position = Vector2.MoveTowards(transform.position, stateVariables.GetThrowTarget().position, stateVariables.runSpeed * Time.deltaTime);
			anim.SetBool("isWalking", false);
			anim.SetBool("isSitting", false);
			anim.SetBool("isRunning", true);
			sprite.flipX = false;

		}

		//checking if monty is at the starting point
		if (transform.position == stateVariables.GetFetchStartingPoint().position)
		{
			cameraHandler.SwitchToPlayer();
			//Resetting animator and facing in the correct spot
			Debug.Log("Monty at stick");
			anim.SetBool("isWalking", false);
			anim.SetBool("isRunning", false);
			anim.SetBool("isSitting", true);
			sprite.flipX = true;

			stateVariables.stickThrown = false;
			stateVariables.montyHasStick = true;
			stateVariables.montyReturningStick = false;

			if (stateVariables.playerHasStick)
			{
				Debug.Log("Disable input"); 
				playerController.DisablePlayerInput();				
			}
			else
			{
				Debug.Log("enable input");
				StartCoroutine(playerController.EnablePlayerInput(0));
			}

			if (stateVariables.playerHasStick)
			{
				stateVariables.montyHasStick = false;
				stateVariables.GetFetchStick().transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
			}
			else
			{
				stateVariables.montyHasStick = true;

				stateVariables.GetFetchStick().transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;

			}
		}

		//checking if monty is at the stick after being thrown
		if (transform.position == stateVariables.GetThrowTarget().position)
		{
			Debug.Log("At thrown stick");
			anim.SetBool("isRunning", false);
			StartCoroutine(WaitForTime(2));

			if (stateVariables.waitedAtStick)
			{
				stateVariables.waitedAtStick = false;
				sprite.flipX = true;
				stateVariables.montyReturningStick = true;
				stateVariables.GetFetchStick().position = stateVariables.GetStickSpawnLocation().position;
				stateVariables.GetFetchStick().transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
			}
			
		}
	}
	
	public void Wait()
	{
		//Debug.Log("Waiting");
		anim.SetBool("isRunning", false);
		anim.SetBool("isWalking", false);
		anim.SetBool("isSitting", false);
	}

	public void MoveTowards()
	{
			anim.SetBool("isSitting", false);
			anim.SetBool("isWalking", false);
			anim.SetBool("isRunning", true);
			//Debug.Log("Moving Towards");
			transform.position = Vector2.MoveTowards(transform.position, player.transform.position - new Vector3(playerController.armsReach, 0, 0), stateVariables.runSpeed * Time.deltaTime);
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
		anim.SetBool("isRunning", false);
		anim.SetBool("isSitting", false);
		anim.SetBool("isWalking", true);

		if (playerController.facingRight)
		{
			transform.position = Vector2.MoveTowards(transform.position, player.transform.position - new Vector3(playerController.armsReach, 0, 0), stateVariables.walkSpeed * Time.deltaTime);
		}
		else
		{
			sprite.flipX = true;
			transform.position = Vector2.MoveTowards(transform.position, player.transform.position + new Vector3(playerController.armsReach, 0, 0), stateVariables.walkSpeed * Time.deltaTime);
		}
		
	}

	public void Canoe()
	{
		Debug.Log("monty is in the canoe");
	}


	IEnumerator WaitForTime(int time)
	{
		yield return new WaitForSeconds(time);
		stateVariables.waitedAtStick = true;
	}
}
