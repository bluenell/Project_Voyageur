using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{

	[Header("Volume Settings")]


	[FMODUnity.EventRef]
	public string[] soundEvents;


	Rigidbody2D montyRb;


	FMOD.Studio.EventInstance playerState;
	FMOD.Studio.EventInstance montyState;

	void Start()
	{
		montyRb = GameObject.Find("MontyChild").GetComponent<Rigidbody2D>();
	}


	void Update()
	{

		montyState.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(montyRb.gameObject, montyRb));
	}


	public void PlayBark()
	{
		montyState = FMODUnity.RuntimeManager.CreateInstance(soundEvents[18]);
		montyState.start();
	}

	public void PlayMontyFootSteps()
	{
		montyState = FMODUnity.RuntimeManager.CreateInstance(soundEvents[17]);
		montyState.start();
	}

	public void PlayPickUpCanoe()
	{
		playerState = FMODUnity.RuntimeManager.CreateInstance(soundEvents[11]);
		playerState.start();
	}
	public void PlayPutDownCanoe()
	{
		playerState = FMODUnity.RuntimeManager.CreateInstance(soundEvents[12]);
		playerState.start();
	}


	public void PlayFootsteps()
	{
		playerState = FMODUnity.RuntimeManager.CreateInstance(soundEvents[0]);
		playerState.start();
	}

	public void Paddle()
	{
		playerState = FMODUnity.RuntimeManager.CreateInstance(soundEvents[1]);
		playerState.start();
	}

	public void PlayTorchClickOn()
	{
		playerState = FMODUnity.RuntimeManager.CreateInstance(soundEvents[3]);
		playerState.start();
	}
	public void PlayTorchClickOff()
	{
		playerState = FMODUnity.RuntimeManager.CreateInstance(soundEvents[8]);
		playerState.start();
	}

	public void PlayExitCanoe()
	{
		playerState = FMODUnity.RuntimeManager.CreateInstance(soundEvents[2]);
		playerState.start();
	}

	public void PlayDragCanoe()
	{
		playerState = FMODUnity.RuntimeManager.CreateInstance(soundEvents[4]);
		playerState.start();
	}

	public void PlayItemSwitch()
	{
		playerState = FMODUnity.RuntimeManager.CreateInstance(soundEvents[6]);
		playerState.start();
	}

	public void PlayWhistle()
	{
		playerState = FMODUnity.RuntimeManager.CreateInstance(soundEvents[7]);
		playerState.start();
	}

	public void PlayTreeChop(int count)
	{
		playerState = FMODUnity.RuntimeManager.CreateInstance(soundEvents[count]);
		playerState.start();
	}


	public void PlayDoorShut()
	{
		playerState = FMODUnity.RuntimeManager.CreateInstance(soundEvents[13]);
		playerState.start();
	}

	public void PlayDoorOpen()
	{
		playerState = FMODUnity.RuntimeManager.CreateInstance(soundEvents[14]);
		playerState.start();

	}

	public void PlayPageTurn()
	{
		playerState = FMODUnity.RuntimeManager.CreateInstance(soundEvents[15]);
		playerState.start();

	}

	public void PlayPushCanoe()
	{
		playerState = FMODUnity.RuntimeManager.CreateInstance(soundEvents[16]);
		playerState.start();
	}
}
