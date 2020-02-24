using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionHandler : MonoBehaviour
{

	CanoePaddle paddleScript;
	PlayerController playerController;

	GameObject player;
	GameObject canoe;

	private void Awake()
	{
		player = GameObject.Find("Player");
		playerController = player.GetComponent<PlayerController>();

		canoe = GameObject.Find("Canoe");
		paddleScript = canoe.GetComponent<CanoePaddle>();
	}


	public void Beach()
	{
		//hide canoe all in one
		//show player
		//show monty
		//canoe object
	}

	public void Launch()
	{
		//hide player
		//hide monty
		//hide canoe object
		//show canoe all in one at the new location
	}

}
