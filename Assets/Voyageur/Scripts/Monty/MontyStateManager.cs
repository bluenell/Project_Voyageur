using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MontyStateManager : MonoBehaviour
{
	string currentState;
	float counter;
	int rand;

	MontyStateActions stateActions;
	MontyStateVariables stateVariables;


	private void Start()
	{
		stateActions = GetComponent<MontyStateActions>();
		stateVariables = GetComponent<MontyStateVariables>();

	}

	private void FixedUpdate()
	{
		//When to switch to follow
		if (stateVariables.distanceToFollow <= stateVariables.distFromPlayer)
		{
			counter = 0;
			currentState = "follow";
			SwitchState();
			counter = 0;
			rand = (int)Random.Range(stateVariables.randomWaitRange.x, stateVariables.randomWaitRange.y);
		}

		/*
		//when to switch to idle
		if (!stateVariables.playerMoving && counter < rand)
		{
			currentState = "idle";
			SwitchState();
			
			counter += Time.deltaTime;
			
			//when to switch to sit
			if (counter >= stateVariables.sitWaitTime)
			{
				currentState = "sit";
				SwitchState();

			}

		}

	*/

		void SwitchState()
		{
			switch (currentState)
			{
				case "canoe":
					stateActions.Canoe();
					break;

				case "canoe fish":
					stateActions.CanoeFish();
					break;

				case "follow":
					stateActions.Follow();
					break;


				case "idle":
					stateActions.Idle();
					break;

				case "sit":
					stateActions.Sit();
					break;

				case "fetch":
					stateActions.Fetch();
					break;

			}
		}
	}
}
