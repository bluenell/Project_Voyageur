using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventsHandler : MonoBehaviour
{
	TransitionHandler TransitionHandler;
	PlayerSoundManager playerSoundManager;


	private void Awake()
	{
		TransitionHandler = GameObject.Find("Transition Handler").GetComponent<TransitionHandler>();
		
	}
	public void Transition()
	{
		TransitionHandler.Beach();
	}

	public void DestroySprite()
	{
		GameObject.Destroy(gameObject);
	}


	public void FootStepsPlayer()
	{
		playerSoundManager.PlayFootsteps();
	}

}
