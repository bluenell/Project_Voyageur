using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{

	string activeCamera;

	public GameObject playerCam, canoeCam, montyCam;
	MontyStateVariables montyStateVariables;
	PlayerController playerController;


	private void Start()
	{
		montyStateVariables = GameObject.Find("Monty").GetComponent<MontyStateVariables>();
		playerController = GameObject.Find("Player").GetComponent<PlayerController>();	

	}



	
	public void SwitchCam()
	{

	}




}
