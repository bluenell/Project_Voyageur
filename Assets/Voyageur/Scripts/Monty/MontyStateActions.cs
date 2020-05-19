using System.Collections;
using UnityEngine;

public class MontyStateActions : MonoBehaviour
{
	Animator anim, parentAnim;
	MontyStateVariables stateVariables;
	MontyStateManager stateManager;
	GameObject player;
	GameObject followTarget;
	BoxCollider2D followTargetCollider;
	Rigidbody2D rb;
	SpriteRenderer sprite;
	PlayerController playerController;
	CameraHandler cameraHandler;
	public Journal journal;
	

	Transform canoeSeat;

	bool targetFound;
	Vector3 target;
	float stuckTimer;
	int maxStuckTime;
	bool checkingIfStuck;
	float newSearchTimer;

	Vector3[] path;
	int targetIndex;
	public bool currentlyOnPath;
	bool pathCancelled;

	private void Start()
	{
		sprite = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
		sprite.enabled = false;
		stateVariables = GetComponent<MontyStateVariables>();
		stateManager = GetComponent<MontyStateManager>();
		anim = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
		parentAnim = transform.GetChild(0).GetComponent<Animator>();
		player = GameObject.Find("Player");
		followTarget = player.transform.GetChild(2).gameObject;
		followTargetCollider = followTarget.GetComponent<BoxCollider2D>();
		rb = GetComponent<Rigidbody2D>();
		playerController = player.GetComponent<PlayerController>();
		cameraHandler = GameObject.Find("Camera Manager").GetComponent<CameraHandler>();
		canoeSeat = GameObject.Find("Monty Seat").transform;
		
	}


	public void Roam()
	{ 
		//Debug.Log("Monty is following");
		anim.SetBool("isRunning", false);
		anim.SetBool("isSitting", false);
		anim.SetBool("isWalking", true);

		if (!currentlyOnPath && !stateVariables.callRequestMade)
		{
			anim.SetBool("isWalking", false);
			stateVariables.desintationReached = false;

			newSearchTimer += Time.deltaTime;

			if (newSearchTimer >= Random.Range(stateVariables.newSearchTime.x, stateVariables.newSearchTime.y))
			{
				newSearchTimer = 0;
				target = stateVariables.GetRandomPointInBounds(followTargetCollider.bounds);
				PathRequestManager.RequestPath(transform.position, target, OnPathFound);
				currentlyOnPath = true;
			}
		}
	}

	public void Sit()
	{	
		anim.SetBool("isWalking", false);
		anim.SetBool("isRunning", false);
		anim.SetBool("isSitting", true);


	}

	public void Launch()
	{
		if (stateVariables.montyReadyToGetIn && !stateVariables.jumping)
		{
			sprite.enabled = true;
			transform.GetChild(0).GetComponent<CircleCollider2D>().enabled = false;

			anim.SetBool("isWalking", false);
			anim.SetBool("isRunning", true);			

			transform.position = Vector3.MoveTowards(transform.position, playerController.montyWalkTarget.position, stateVariables.runSpeed * Time.deltaTime);
						
		}

		if (transform.position == playerController.montyWalkTarget.position)
		{
			stateVariables.jumping = true;
			anim.SetTrigger("jump");
			parentAnim.SetTrigger("jump");
		}

		if (stateVariables.montyInCanoe)
		{
			journal.journalPages[2].SetActive(true);
			//transform.SetParent(GameObject.Find("Canoe Single").transform);
			transform.position = canoeSeat.position;
			sprite.flipX = true;
			anim.SetBool("isRunning", false);
			anim.SetBool("isSitting", true);
		}

	}

	public void Fetch()
	{
		if (!pathCancelled)
		{
			PathRequestManager.RequestPath(transform.position, transform.position, OnPathFound);
			PathRequestManager.ClearRequests();
			StopAllCoroutines();
			
			pathCancelled = true;
		}
		
		//checking if the stick hasn't been thrown yet, or monty is bringing the stick back (when to move monty to the start point)
		if (!stateVariables.stickThrown || stateVariables.montyReturningStick)
		{
			transform.position = Vector3.MoveTowards(transform.position, stateVariables.GetFetchStartingPoint().position, stateVariables.runSpeed * Time.deltaTime);
			
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
			transform.position = Vector3.MoveTowards(transform.position, stateVariables.GetThrowTarget().position, stateVariables.runSpeed * Time.deltaTime);

			cameraHandler.SwitchToMonty();
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
			//Debug.Log("Monty at stick");
			anim.SetBool("isWalking", false);
			anim.SetBool("isRunning", false);
			anim.SetBool("isSitting", true);
			sprite.flipX = true;

			stateVariables.stickThrown = false;
			stateVariables.montyHasStick = true;
			stateVariables.montyReturningStick = false;

			if (stateVariables.playerHasStick)
			{
				//Debug.Log("Disable input");
				playerController.DisablePlayerInput();
				stateVariables.montyHasStick = false;
				stateVariables.GetFetchStick().transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
			}
			else
			{
				//Debug.Log("enable input");
				StartCoroutine(playerController.EnablePlayerInput(0));
				stateVariables.montyHasStick = true;
				stateVariables.GetFetchStick().transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
			}
		}

		//checking if monty is at the stick after being thrown
		if (transform.position == stateVariables.GetThrowTarget().position)
		{
			//Debug.Log("At thrown stick");
			anim.SetBool("isRunning", false);

			if (!stateVariables.waitedAtStick)
			{
				StartCoroutine(WaitForTime(2));
			}
		}
	}

	public void Wait()
	{
		currentlyOnPath = false;
		PathRequestManager.ClearRequests();
		stateVariables.callRequestMade = false;
		stateVariables.movingTowardsPlayer = false;

		anim.SetBool("isRunning", false);
		anim.SetBool("isWalking", false);
		anim.SetBool("isSitting", false);


		if (transform.position.x > player.transform.position.x)
		{
			sprite.flipX = true;
		}
		else
		{
			sprite.flipX = false;
		}


	}

	public void MoveTowards()
	{		
		if (!stateVariables.movingTowardsPlayer)
		{
			Debug.Log("Call Request");
			stateVariables.movingTowardsPlayer = true;
			currentlyOnPath = false;
			PathRequestManager.ClearRequests();
			StopAllCoroutines();
		}

		if (!currentlyOnPath && !stateVariables.desintationReached)
		{
			if (transform.position.x > player.transform.position.x)
			{
				// Right walk target
				PathRequestManager.RequestPath(transform.position, player.transform.GetChild(4).transform.position, OnPathFound);
			}
			else
			{
				// Left walk target
				PathRequestManager.RequestPath(transform.position, player.transform.GetChild(5).transform.position, OnPathFound);
			}

			currentlyOnPath = true;
		}

	}
	public void Canoe()
	{
		Debug.Log("monty is in the canoe");
	}


	IEnumerator WaitForTime(int time)
	{
		Debug.Log("Waiting");
		yield return new WaitForSeconds(time);

		stateVariables.waitedAtStick = true;
		sprite.flipX = true;
		stateVariables.montyReturningStick = true;
		stateVariables.GetFetchStick().position = stateVariables.GetStickSpawnLocation().position;
		stateVariables.GetFetchStick().transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
	}


    #region Pathfinding

    public void OnPathFound(Vector3[] newPath, bool pathSucessfull)
	{
		if (pathSucessfull)
		{
			path = newPath;
			targetIndex = 0;
			StopCoroutine(FollowPath());
			StartCoroutine(FollowPath());
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
					PathRequestManager.ClearRequests();
					currentlyOnPath = false;
					Debug.Log("Reached Destination");

					if (stateManager.currentState == "move towards")
					{
						stateVariables.callRequestMade = false;
						stateVariables.movingTowardsPlayer = false;
					}

					stateVariables.desintationReached = true;

					yield break;
				}
				currentWaypoint = path[targetIndex];
			}

			transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, stateVariables.walkSpeed * Time.deltaTime);

			if (currentWaypoint.x < transform.position.x)
			{
				sprite.flipX = true;
			}
			else
			{
				sprite.flipX = false;
			}

			yield return null;
		}
	}

	public void OnDrawGizmos()
	{
		if (path != null)
		{
			for (int i = targetIndex; i < path.Length; i++)
			{
				if (stateManager.currentState == "roam")
				{
					Gizmos.color = Color.black;
				}
				else
				{
					Gizmos.color = Color.blue;
				}
				Gizmos.DrawCube(path[i], (Vector3.one) / 2);

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

    #endregion
}
