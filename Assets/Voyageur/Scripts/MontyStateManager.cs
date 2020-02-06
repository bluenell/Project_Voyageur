using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MontyStateManager : MonoBehaviour
{
	string currentState;
	
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
		}

		if (!stateVariables.playerMoving)
		{
			currentState = "idle";
			SwitchState();
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

			case "dig":
				stateActions.Dig();
				break;

			case "fetch":
				stateActions.Fetch();
				break;

			


		}
	}

}
