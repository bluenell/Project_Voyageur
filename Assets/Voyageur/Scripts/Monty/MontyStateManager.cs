using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MontyStateManager : MonoBehaviour
{
	string currentState;
	float counter;
	int rand;
	bool movingToPlayer;
	bool sitting;

	MontyStateActions stateActions;
	MontyStateVariables stateVariables;
	PlayerController playerController;


	private void Awake()
	{
		stateActions = GetComponent<MontyStateActions>();
		stateVariables = GetComponent<MontyStateVariables>();
		playerController = GameObject.Find("Player").GetComponent<PlayerController>();

	}

	private void FixedUpdate()
	{
		if (!movingToPlayer && stateVariables.distFromPlayer >= playerController.armsReach)
		{
			currentState = "roam";
			SwitchState();
		}
	}

	private void Update()
	{
		if (Input.GetButtonDown("Button A"))
		{
			movingToPlayer = true;
		}

		if (movingToPlayer)
		{
			currentState = "move towards";
			SwitchState();
		}

		if (stateVariables.distFromPlayer <= playerController.armsReach && !sitting)
		{
			movingToPlayer = false;
			currentState = "wait";
			SwitchState();
		}

		if (currentState == "wait")
		{
			counter += Time.deltaTime;

			if (counter >= stateVariables.sitWaitTime)
			{
				counter = 0;
				sitting = true;
				currentState = "sit";
				SwitchState();
			}

		}

	}

	void SwitchState()
	{
		switch (currentState)
		{
			case "roam":
				stateActions.Roam();
				break;

			case "sit":
				stateActions.Sit();
				break;

			case "fetch":
				stateActions.Fetch();
				break;

			case "wait":
				stateActions.Wait();
				break;

			case "move towards":
				stateActions.MoveTowards();
				break;

			case "follow":
				stateActions.Follow();
				break;

			case "canoe":
				stateActions.Canoe();
				break;

			default:
				stateActions.Roam();
				break;




		}
	}
}

