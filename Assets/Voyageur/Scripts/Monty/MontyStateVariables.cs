using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MontyStateVariables : MonoBehaviour
{

	public float distFromPlayer;
	public bool playerMoving;
	public int sitWaitTime;
	public float animationDelay;

	GameObject player;

	private void Start()
	{
		player = GameObject.Find("Player");
	}
	private void Update()
	{
		distFromPlayer = CalculateDistance();
		playerMoving = GetPlayerMoving();
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

	public IEnumerator DelayAnimation()
	{
		yield return new WaitForSeconds(animationDelay);
	}





}
