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
	public bool inTutorial;

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
		inFetch = false;
		inTutorial = true;
	}

	private void Update()
	{
		if (!inFetch && !inTutorial)
		{
			if (!stateVariables.movingTowardsPlayer && stateVariables.distFromPlayer >= playerController.armsReach)
			{
				stateVariables.movingTowardsPlayer = false;
				currentState = "roam";
				SwitchState();
			}

			if (stateVariables.callRequestMade)
			{
				currentState = "move towards";
				SwitchState();

				if (stateVariables.desintationReached)
				{
					stateVariables.movingTowardsPlayer = false;
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
		}
		else if (inFetch)
		{
			currentState = "fetch";
			SwitchState();
		}
		else if (inTutorial)
		{
			currentState = "launch";
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

			case "launch":
				stateActions.Launch();
				break;

			case "wait":
				stateActions.Wait();
				break;

			case "move towards":
				stateActions.MoveTowards();
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

