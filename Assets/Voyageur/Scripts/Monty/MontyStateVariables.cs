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
	PolygonCollider2D pathwayBounds;
	public bool movingTowardsPlayer;
	

	public float distanceToFollow;

	public int sitWaitTime;
	public Vector2 randomWaitRange;

	GameObject player;
	InteractionsManager interactionsManager;

	Rigidbody2D stickRb;
	Transform stickThrowTarget;
	[Header("Fetch Variables")]
	public float throwHeight;
	public float throwGravity;
	[HideInInspector]
	public bool montyHasStick = false;
	[HideInInspector]
	public bool playerHasStick = false;
	[HideInInspector]
	public bool stickThrown = false;
	[HideInInspector]
	public bool montyReturningStick = false;
	[HideInInspector]
	public int throwCount;
	[HideInInspector]
	public bool waitedAtStick = false;


	private void Start()
	{
		player = GameObject.Find("Player");
		interactionsManager = player.GetComponent<InteractionsManager>();
	}
	private void Update()
	{
		distFromPlayer = CalculateDistance();
		playerMoving = GetPlayerMoving();
		playerFlipped = GetPlayerDirectionPositive();
		
	}

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

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, distanceToFollow);
	}


	#region Pathfinding

	public Vector2 GetRandomPointInBounds(Bounds bounds)
	{
		desintationReached = false;
		Vector2 location;
		location = new Vector2(
			Random.Range(bounds.min.x, bounds.max.x),
			Random.Range(bounds.min.y, bounds.max.y)
			);


		if (CheckIfPointInCollider(location))
		{
			Debug.DrawLine(transform.position, location,Color.green,20f);
			
			return location;
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
		return pathwayBounds.OverlapPoint(location);

	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "PathwayTriggerBounds")
		{
			//Debug.Log(collision.gameObject.name);
			pathwayBounds = collision.gameObject.GetComponent<PolygonCollider2D>();
		}
	}

	#endregion


	#region Fetch

	public Vector3 GetFetchStartingPoint()
	{
		return interactionsManager.interaction.gameObject.transform.GetChild(0).transform.position;
	}

	public Transform GetFetchStick()
	{
		return interactionsManager.interaction.gameObject.transform.GetChild(1);
	}

	public Transform GetThrowTarget()
	{

		return interactionsManager.interaction.gameObject.transform.GetChild(2);
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


}
