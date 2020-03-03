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
	bool canThrowStick;

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

		GameObject stick = stateVariables.GetFetchStick();
		fetchWalkTarget = stateVariables.GetFetchStartingPoint();
		Rigidbody2D stickRb = stick.GetComponent<Rigidbody2D>();
		Stick stickRange = stick.GetComponent<Stick>();


		stick.SetActive(false);

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
			stick.SetActive(true);
			canThrowStick = true;

			float dist = Vector2.Distance(player.transform.position, stick.transform.position);

			if (canThrowStick && dist <= stickRange.range)
			{
				if (Input.GetButtonDown("Button A"))
				{
					stickRb.gravityScale = 1;
					stickRb.AddForce(new Vector2(stateVariables.throwForce, stateVariables.throwForce/2) * Time.deltaTime, ForceMode2D.Impulse);
				}
			}

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
