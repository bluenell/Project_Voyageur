using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventsHandler : MonoBehaviour
{
	TransitionHandler TransitionHandler;
	PlayerController playerController;

	private void Awake()
	{
		TransitionHandler = GameObject.Find("Transition Handler").GetComponent<TransitionHandler>();
		playerController = GameObject.Find("Player").GetComponent<PlayerController>();
	}

	public void Transition()
	{
		TransitionHandler.Beach();
	}

	/*
	public void RevealCanoe()
	{
		playerController.canoe.SetActive(true);
	}
	*/
}
