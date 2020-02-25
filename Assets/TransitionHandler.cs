using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionHandler : MonoBehaviour
{

	CanoePaddle paddleScript;
	PlayerController playerController;

	public GameObject player;
	public GameObject canoe;
	public GameObject canoeAIO;
	public GameObject monty;
	public GameObject layerManager;
	public GameObject interactionsManager;

	public GameObject canoeCam;
	public GameObject playerCam;

	private void Awake()
	{
		playerController = player.GetComponent<PlayerController>();
		paddleScript = canoeAIO.GetComponent<CanoePaddle>();
	}


	public void Beach()
	{
		canoeAIO.SetActive(false);
		canoe.SetActive(true);
		player.SetActive(true);
		monty.SetActive(true);
		layerManager.SetActive(true);
		interactionsManager.SetActive(true);
		canoeCam.SetActive(false);
		playerCam.SetActive(true);

	}

	public void Launch()
	{
		//hide player
		//hide monty
		//hide canoe object
		//show canoe all in one at the new location
	}

}
