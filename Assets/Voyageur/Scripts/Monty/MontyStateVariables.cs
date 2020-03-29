using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MontyStateVariables : MonoBehaviour
{
	[HideInInspector]
	public bool playerFlipped;
	[HideInInspector]
	public bool playerMoving;	
	[HideInInspector ]
	public float distFromPlayer;
	[HideInInspector]
	public bool desintationReached;

	[Header("Generic Variables")]
	public float walkSpeed;
	public float runSpeed;
	public float distanceToFollow;



	[Header("Pathfinding")]
	CircleCollider2D rangeToIgnore;
	public MyGrid grid;

	public bool movingTowardsPlayer;
	public bool callRequestMade;


	[Header("Waiting")]
	public int sitWaitTime;
	public Vector2 randomWaitRange;

	GameObject player;
	InteractionsManager interactionsManager;
	GameManager gameManager;

	[Header("Fetch Variables")]

	Rigidbody2D stickRb;
	Transform stickThrowTarget;
	public float throwHeight;
	public float throwGravity;
	public bool montyHasStick = false;
	public bool playerHasStick = false;
	public bool stickThrown = false;
	public bool montyReturningStick = false;
	public bool waitedAtStick = false;



	private void Start()
	{
		player = GameObject.Find("Player");
		interactionsManager = player.GetComponent<InteractionsManager>();
		rangeToIgnore = transform.GetChild(1).GetComponent<CircleCollider2D>();
		gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
		grid = GameObject.Find("Pathfinding Grids").transform.GetChild(gameManager.GetCurrentIsland()).GetComponent<MyGrid>();

	}
	private void Update()
	{
		distFromPlayer = CalculateDistance();
		playerMoving = GetPlayerMoving();
		playerFlipped = GetPlayerDirectionPositive();
		//grid = pathFindingManagers[gameManager.GetCurrentIsland()].GetComponent<MyGrid>();
	}

    #region Player_Calculations

    public float CalculateDistance()
	{
		distFromPlayer = Vector2.Distance(transform.position, player.transform.position);
		return distFromPlayer;
	}

	public float GetPlayerDistanceFromStick()
	{
		return Vector2.Distance(player.transform.position, GetFetchStick().transform.position);
	}
	bool GetPlayerMoving()
	{
		if (player.GetComponent<PlayerController>().isMoving)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	bool GetPlayerDirectionPositive()
	{
		if (player.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX == true)
		{
			return false;
		}
		else
		{
			return true;
		}
	}

    #endregion

	#region Pathfinding

	public Vector3 GetRandomPointInBounds(Bounds bounds)
	{
		desintationReached = false;
		Vector2 location;
		location = new Vector2(
			Random.Range(bounds.min.x, bounds.max.x),
			Random.Range(bounds.min.y, bounds.max.y)
			);

		if (CheckIfPointInCollider(location))
		{
			//Debug.DrawLine(transform.position, location,color.green,20f);
			return new Vector3(location.x, location.y, 0);
		}
		else
		{
			Debug.DrawLine(transform.position, location, Color.red, 0.1f);
			//Debug.Log("Invalid Location:" + location);
			return GetRandomPointInBounds(bounds);
		}

	}

	public bool CheckIfPointInCollider(Vector2 location)
	{
		Node targetNode = grid.NodeFromWorldPoint(new Vector3(location.x, location.y, 0));

		if (targetNode.walkable && !rangeToIgnore.OverlapPoint(location))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	#endregion

	#region Fetch

	public Transform GetFetchStartingPoint()
	{
		return interactionsManager.interaction.gameObject.transform.GetChild(0).transform;
	}

	public Transform GetFetchStick()
	{
		return interactionsManager.interaction.gameObject.transform.GetChild(1);
	}

	public Transform GetThrowTarget()
	{

		return interactionsManager.interaction.gameObject.transform.GetChild(2);
	}

	public bool GetPlayerNearStick()
	{
		if (Vector3.Distance(player.transform.position, GetFetchStick().position) <= GetFetchStick().GetComponent<Stick>().range)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public Transform GetStickSpawnLocation()
	{
		return interactionsManager.interaction.gameObject.transform.GetChild(3);
	}

	public GameObject GetFetchZoneExits(int childIndex)
	{
		return interactionsManager.interaction.gameObject.transform.GetChild(childIndex).gameObject;
	}

	
	public Vector2 CalculateThrowVelocity()
	{

		float displacementY = GetThrowTarget().position.y - GetFetchStick().position.y;
		Vector2 displacementX = new Vector2(GetThrowTarget().position.x - GetFetchStick().position.x, 0);

		Vector2 velocityY = Vector2.up * Mathf.Sqrt(-2 * (throwGravity * throwHeight));
		Vector2 velocityX = displacementX / (Mathf.Sqrt(-2 * throwHeight / throwGravity) + Mathf.Sqrt(2 * (displacementY - throwHeight) / throwGravity));

		return velocityX + velocityY;
	}
	#endregion

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, distanceToFollow);
	}

}
