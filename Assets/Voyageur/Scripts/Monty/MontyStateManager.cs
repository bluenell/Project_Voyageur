using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MontyStateManager : MonoBehaviour
{
	string currentState;
	
	MontyStateActions stateActions;
	MontyStateVariables stateVariables;

	float counter;

	private void Start()
	{
		stateActions = GetComponent<MontyStateActions>();
		stateVariables = GetComponent<MontyStateVariables>();
		
	}

	private void Update()
	{

		if (stateVariables.playerMoving)
		{
			counter = 0;
			currentState = "follow";
			SwitchState();
		}


		if (!stateVariables.playerMoving)
		{
			currentState = "idle";

			if (currentState == "idle")
			{
				counter += Time.deltaTime;

				if (counter >= stateVariables.sitWaitTime)
				{
					currentState = "sit";
					counter = 0;
					SwitchState();
				}
			}
			if (currentState == "sit")
			{
				currentState = "sit";
				SwitchState();
			}
		}
	}	
		
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
