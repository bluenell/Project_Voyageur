using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
	[FMODUnity.EventRef]
	public string footstepsEvent = "";

	FMOD.Studio.EventInstance playerState;

	public void PlayFootsteps()
	{
		playerState = FMODUnity.RuntimeManager.CreateInstance(footstepsEvent);
		playerState.start();
	}

}
