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
	PlayerSoundManager playerSoundManager;


	private void Awake()
	{
		stateActions = GetComponent<MontyStateActions>();
		stateVariables = GetComponent<MontyStateVariables>();
		playerController = GameObject.Find("Player").GetComponent<PlayerController>();
		playerSoundManager = GameObject.Find("Player").GetComponent<PlayerSoundManager>();

	}

	private void FixedUpdate()
	{
		if (!movingToPlayer && stateVariables.distFromPlayer >= playerController.armsReach)
		{
			movingToPlayer = false;
			currentState = "roam";
			SwitchState();
		}
	}

	private void Update()
	{
		Debug.Log(currentState);

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

