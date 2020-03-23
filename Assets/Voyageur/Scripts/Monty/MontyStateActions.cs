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
	Vector3 target;
	float stuckTimer;

	Vector3[] path;
	int targetIndex;
	bool firstPathGenerated;

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

		if (!firstPathGenerated)
		{
			target = stateVariables.GetRandomPointInBounds(followTargetCollider.bounds);
			PathRequestManager.RequestPath(transform.position, target, OnPathFound);
			firstPathGenerated = true;
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
			stateVariables.waitedAtStick = false;

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
			transform.position = Vector2.MoveTowards(transform.position, stateVariables.GetThrowTarget().position, (stateVariables.runSpeed - 2) * Time.deltaTime);
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

			if (!stateVariables.waitedAtStick)
			{
				StartCoroutine(WaitForTime(2));
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
		sprite.flipX = true;
		stateVariables.montyReturningStick = true;
		stateVariables.GetFetchStick().position = stateVariables.GetStickSpawnLocation().position;
		stateVariables.GetFetchStick().transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
	}


	public void OnPathFound(Vector3[] newPath, bool pathSucessfull)
	{
		if (pathSucessfull)
		{
			path = newPath;
			
			StartCoroutine(FollowPath());
			StopCoroutine(FollowPath());
		}
	}

	IEnumerator FollowPath()
	{
		Vector3 currentWaypoint = path[0];

		while (true)
		{
			if (transform.position == currentWaypoint)
			{
				//Debug.Log("At waypoint: " + path[targetIndex]);
				targetIndex++;
				if (targetIndex >= path.Length)
				{
					target = stateVariables.GetRandomPointInBounds(followTargetCollider.bounds);
					PathRequestManager.RequestPath(transform.position, target, OnPathFound);
					stateVariables.desintationReached = true;
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}

			transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, stateVariables.walkSpeed * Time.deltaTime);
			yield return null;
		}
	}



	public void OnDrawGizmos()
	{
		if (path != null)
		{
			for (int i = targetIndex; i < path.Length; i++)
			{
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], Vector3.one);

				if (i == targetIndex)
				{
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else
				{
					Gizmos.DrawLine(path[i - 1], path[i]);
				}
			}
		}
	}

}
