using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MontyStateVariables : MonoBehaviour
{
	[HideInInspector]
	public bool playerFlipped;
	[HideInInspector]
	public bool playerMoving;	

	[Header("Follow")]
	public float distFromPlayer;
	public bool desintationReached;
	public float montySpeed;
	PolygonCollider2D pathwayBounds;
	
	[Header("Idle")]
	public float distanceToFollow;

	[Header("Sit")]
	public int sitWaitTime;
	public Vector2 randomWaitRange;

	GameObject player;

	private void Start()
	{
		player = GameObject.Find("Player");
	}
	private void Update()
	{
		distFromPlayer = CalculateDistance();
		playerMoving = GetPlayerMoving();
		playerFlipped = GetPlayerDirectionPositive();
		
	}

	float CalculateDistance()
	{
		distFromPlayer = Vector2.Distance(transform.position, player.transform.position);
		return distFromPlayer;
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


	public Vector2 GetRandomPointInBounds(Bounds bounds)
	{
		desintationReached = false;
		Vector2 location;
		location = new Vector2(
			Random.Range(bounds.min.x, bounds.max.x),
			Random.Range(bounds.min.y, bounds.max.y)
			);

		if (pathwayBounds.bounds.Contains(location))
		{
			Debug.Log("Location Found: " + location);
			return location;
		}
		else
		{
			Debug.Log("Location out of bounds" + location);
			return GetRandomPointInBounds(bounds);
		}

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag =="PathwayTriggerBounds")
		{
			pathwayBounds =  collision.gameObject.GetComponent<PolygonCollider2D>();
		}
	}








}
