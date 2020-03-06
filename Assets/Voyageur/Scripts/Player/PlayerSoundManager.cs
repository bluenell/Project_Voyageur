using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{

	[Header("Volume Settings")]
	[Range(0, 2)]
	public float paddleVolume;

	[Range(0, 2)]
	public float footStepsVolume;

	[Range(0, 2)]
	public float footStepsInWaterVolume;

	[Range(0, 10)]
	public float torchClickVolume;

	[FMODUnity.EventRef]
	public string[] soundEvents;


	FMOD.Studio.EventInstance playerState;

	public void PlayFootsteps()
	{
		playerState = FMODUnity.RuntimeManager.CreateInstance(soundEvents[0]);
		playerState.setVolume(footStepsVolume);
		playerState.start();
	}

	public void Paddle()
	{
		playerState = FMODUnity.RuntimeManager.CreateInstance(soundEvents[1]);
		playerState.setVolume(paddleVolume);
		playerState.start();
	}

	public void PlayFootstepsInWater()
	{
		playerState = FMODUnity.RuntimeManager.CreateInstance(soundEvents[2]);
		playerState.setVolume(footStepsInWaterVolume);
		playerState.start();
	}

	public void PlayTorchClickOn()
	{
		playerState = FMODUnity.RuntimeManager.CreateInstance(soundEvents[3]);
		playerState.setVolume(torchClickVolume);
		playerState.start();
	}

	public void PlayWhistle()
	{
		playerState = FMODUnity.RuntimeManager.CreateInstance(soundEvents[6]);
		playerState.start();
	}

	public void PlayTorchClickOff()
	{
		playerState = FMODUnity.RuntimeManager.CreateInstance(soundEvents[4]);
		playerState.setVolume(torchClickVolume);
		playerState.start();
	}

	public void PlayDragCanoe()
	{
		playerState = FMODUnity.RuntimeManager.CreateInstance(soundEvents[5]);
		playerState.start();
	}
}
