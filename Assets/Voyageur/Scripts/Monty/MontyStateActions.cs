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

	bool targetFound;
	Vector2 target;
	float stuckTimer;

	private void Start()
	{
		stateVariables = GetComponent<MontyStateVariables>();
		anim = transform.GetChild(0).GetComponent<Animator>();
		player = GameObject.Find("Player");
		followTarget = player.transform.GetChild(3).gameObject;
		followTargetCollider = followTarget.GetComponent<BoxCollider2D>();
		rb = GetComponent<Rigidbody2D>();
	}

	public void Roam()
	{
		SpriteRenderer sprite;
		sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
		
		//Debug.Log("Monty is following");
		anim.SetBool("isMoving", true);
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
				Debug.Log("Monty Stuck");
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
		//Debug.Log("Monty is playing fetch");

	}

	public void Wait()
	{
		Debug.Log("Waiting");
		anim.SetBool("isMoving", false);
		anim.SetBool("isSitting", false);
	}

	public void MoveTowards()
	{
		Debug.Log("Moving Towards");
		transform.position = Vector2.MoveTowards(transform.position, player.transform.position, stateVariables.montySpeed * Time.deltaTime);		
	}

	public void Follow()
	{
		transform.position = Vector2.MoveTowards(transform.position, player.transform.position, stateVariables.montySpeed * Time.deltaTime);
	}


	public void Canoe()
	{
		Debug.Log("monty is in the canoe");
	}
}
