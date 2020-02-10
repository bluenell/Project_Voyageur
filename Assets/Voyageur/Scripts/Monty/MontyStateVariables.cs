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
	public float montySpeed;

	
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





}
