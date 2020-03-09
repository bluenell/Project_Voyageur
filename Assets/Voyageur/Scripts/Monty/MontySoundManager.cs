using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MontySoundManager : MonoBehaviour
{
	[FMODUnity.EventRef]
	public string[] soundEvents;

	FMOD.Studio.EventInstance montyState;

	public void PlayFootsteps()
	{
		montyState = FMODUnity.RuntimeManager.CreateInstance(soundEvents[0]);
		montyState.start();
	}
}
