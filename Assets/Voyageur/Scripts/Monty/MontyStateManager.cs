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

	float random;
	float timer;
	bool generated;
	float sitTimer;


	MontyStateActions stateActions;
	MontyStateVariables stateVariables;
	public PlayerController playerController;
	public PlayerSoundManager playerSoundManager;


	private void Start()
	{
		stateActions = GetComponent<MontyStateActions>();
		stateVariables = GetComponent<MontyStateVariables>();
		inFetch = false;
	}

	private void Update()
	{
		
		timer += Time.deltaTime;

		if (!generated)
		{
			random = Random.Range(5, 15);
			generated = true;
		}

		if (timer >= random)
		{
			generated = false;
			timer = 0;
			playerSoundManager.PlayBark();
		}

	
		if (!inFetch && !inTutorial)
		{
			if (stateVariables.callRequestMade)
			{
				currentState = "move towards";				
				SwitchState();
			}

			else if (currentState == "move towards" && stateVariables.desintationReached && !stateVariables.isSitting)
			{
				currentState = "wait";
				SwitchState();
			}

			else if (currentState ==  "wait")
			{
				sitTimer += Time.deltaTime;

				if (sitTimer > Random.Range(stateVariables.randomWaitRange.x, stateVariables.randomWaitRange.y))
				{
					stateVariables.isSitting = true;
					currentState = "sit";
					SwitchState();
				}

			}
			else if (stateVariables.distFromPlayer >= stateVariables.distanceToFollow)
			{
				currentState = "roam";
				SwitchState();
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

		if (currentState != "sit")
		{
			stateVariables.isSitting = false;
		}

		if (currentState != "wait")
		{
			sitTimer = 0;
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

