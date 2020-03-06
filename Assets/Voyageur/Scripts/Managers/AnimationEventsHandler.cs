using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventsHandler : MonoBehaviour
{
	TransitionHandler TransitionHandler;
	PlayerSoundManager playerSoundManager;

	GameObject player;


	private void Awake()
	{
		player = GameObject.Find("Player");
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
		player.GetComponent<PlayerSoundManager>().PlayFootsteps();
	}

	public void PaddleSounds()
	{
		GameObject canoeAIO = GameObject.Find("Canoe AIO");
		canoeAIO.GetComponent<PlayerSoundManager>().Paddle();
	}

	public void FootstepsInWater()
	{
		GameObject canoeAIO = GameObject.Find("Canoe AIO");
		canoeAIO.GetComponent<PlayerSoundManager>().PlayFootstepsInWater();
	}

	public void FootStepsCanoe()
	{
		GameObject canoeAIO = GameObject.Find("Canoe AIO");
		canoeAIO.GetComponent<PlayerSoundManager>().PlayFootsteps();
	}

	public void DragCanoe()
	{
		GameObject canoeAIO = GameObject.Find("Canoe AIO");
		canoeAIO.GetComponent<PlayerSoundManager>().PlayDragCanoe();
	}


}
