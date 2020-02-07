using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MontyStateManager : MonoBehaviour
{
	string currentState;
	float counter;
	
	MontyStateActions stateActions;
	MontyStateVariables stateVariables;

	private void Start()
	{
		stateActions = GetComponent<MontyStateActions>();
		stateVariables = GetComponent<MontyStateVariables>();
		
	}

	private void Update()
	{

		if (stateVariables.playerMoving)
		{
			currentState = "follow";
			SwitchState();
			counter = 0;
		}

		if (!stateVariables.playerMoving && counter < stateVariables.sitWaitTime)
		{
			currentState = "idle";
			SwitchState();

			counter += Time.deltaTime;

			if (counter >= stateVariables.sitWaitTime)
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
