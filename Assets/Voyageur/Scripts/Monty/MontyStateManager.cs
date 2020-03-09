using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MontyStateManager : MonoBehaviour
{
	public string currentState;
	float counter;
	int rand;
	bool movingToPlayer;
	bool sitting;
	public bool inFetch;

	MontyStateActions stateActions;
	MontyStateVariables stateVariables;
	PlayerController playerController;
	PlayerSoundManager playerSoundManager;


	private void Start()
	{
		stateActions = GetComponent<MontyStateActions>();
		stateVariables = GetComponent<MontyStateVariables>();
		playerController = GameObject.Find("Player").GetComponent<PlayerController>();
		playerSoundManager = GameObject.Find("Player").GetComponent<PlayerSoundManager>();
	}

	private void FixedUpdate()
	{
		if (!inFetch)
		{
			if (!movingToPlayer && stateVariables.distFromPlayer >= playerController.armsReach)
			{
				movingToPlayer = false;
				currentState = "roam";
				SwitchState();
			}
		}
	}
	private void Update()
	{
		if (!inFetch)
		{
			if (Input.GetButtonDown("Button A"))
			{
				playerSoundManager.PlayWhistle();
				movingToPlayer = true;
			}

			if (movingToPlayer)
			{

				currentState = "move towards";
				SwitchState();

				if (stateVariables.distFromPlayer <= playerController.armsReach)
				{
					movingToPlayer = false;
					currentState = "wait";
					SwitchState();

				}

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

			if (Input.GetButton("Button A") && currentState != "move towards")
			{
				movingToPlayer = true;
				currentState = "follow";
				SwitchState();
			}
		}
		else
		{
			currentState = "fetch";
			SwitchState();
		}
	}


	public void SwitchState()
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

