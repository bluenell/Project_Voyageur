using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{

	public GameObject activeCamera;
	public GameObject playerCam, canoeCam, montyCam, altPlayerCam;
	MontyStateVariables montyStateVariables;
	PlayerController playerController;



	float screenY;

	private void Start()
	{
		montyStateVariables = GameObject.Find("Monty").GetComponent<MontyStateVariables>();
		playerController = GameObject.Find("Player").GetComponent<PlayerController>();	

	}
	
	public void SwitchToMonty()
	{
		playerCam.SetActive(false);
		canoeCam.SetActive(false);
		montyCam.SetActive(true);

		activeCamera = montyCam;
	}

	public void SwitchToPlayer()
	{
		playerCam.SetActive(true);
		canoeCam.SetActive(false);
		montyCam.SetActive(false);
		altPlayerCam.GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 0;

		activeCamera = playerCam;
	}

	public void SwitchToCanoe()
	{
		playerCam.SetActive(false);
		canoeCam.SetActive(true);
		montyCam.SetActive(false);

		activeCamera = canoeCam;
	}

	public void SwitchToAlt()
	{
		altPlayerCam.GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 2;
		activeCamera = altPlayerCam;
	}





}
