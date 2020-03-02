using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
	[FMODUnity.EventRef]
	public string footstepsEvent = "";

	[FMODUnity.EventRef]
	public string paddleEvent = "";

	FMOD.Studio.EventInstance playerState;

	public void PlayFootsteps()
	{
		playerState = FMODUnity.RuntimeManager.CreateInstance(footstepsEvent);
		playerState.start();
	}

	public void Paddle()
	{
		playerState = FMODUnity.RuntimeManager.CreateInstance(paddleEvent);
		playerState.start();
	}

}
