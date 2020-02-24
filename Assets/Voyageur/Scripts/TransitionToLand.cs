using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionToLand : MonoBehaviour
{
	TransitionHandler TransitionHandler;

	private void Awake()
	{
		TransitionHandler = GameObject.Find("Transition Handler").GetComponent<TransitionHandler>();
	}

	public void Transition()
	{
		TransitionHandler.Beach();
	}
}
