using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class TransitionHandler : MonoBehaviour
{

	CanoePaddle paddleScript;
	PlayerController playerController;
	CameraHandler cameraHandler;

	public GameObject player;
	public GameObject canoe;
	public GameObject canoeAIO;
	public GameObject monty;
	public GameObject layerManager;
	public GameObject interactionsManager;


	public GameObject pathwayCollider;
	public GameObject spritesManager;

	public GameObject[] playerSpawnPoints;
	public GameObject[] canoeSpawnPoints;

	int currentIsland = 0;

	private void Awake()
	{
		playerController = player.GetComponent<PlayerController>();
		paddleScript = canoeAIO.GetComponent<CanoePaddle>();
		cameraHandler = GameObject.Find("Camera Manager").GetComponent<CameraHandler>();
	}


	public void Beach()
	{
		canoe.transform.position = playerSpawnPoints[currentIsland].transform.GetChild(0).transform.position;
		player.transform.position = playerSpawnPoints[currentIsland].transform.GetChild(1).transform.position;
		monty.transform.position = playerSpawnPoints[currentIsland].transform.GetChild(2).transform.position;


		canoeAIO.SetActive(false);
		canoe.SetActive(true);
		player.SetActive(true);
		monty.SetActive(true);
		layerManager.SetActive(true);
		interactionsManager.SetActive(true);
		cameraHandler.SwitchToPlayer();
		pathwayCollider.SetActive(true);
		spritesManager.SetActive(true);

		currentIsland++;
	}

	public void Launch()
	{
		canoeAIO.transform.position = canoeSpawnPoints[0].transform.GetChild(currentIsland).transform.position;
		canoeAIO.SetActive(true);
		canoeAIO.GetComponent<CanoePaddle>().beached = false;
		canoeAIO.GetComponent<CanoePaddle>().canPaddle = true;
		canoe.SetActive(false);
		player.SetActive(false);
		monty.SetActive(false);
		layerManager.SetActive(false);
		interactionsManager.SetActive(false);
		pathwayCollider.SetActive(false);
		spritesManager.SetActive(true);


		//hide monty
		//hide canoe object
		//show canoe all in one at the new location
		cameraHandler.SwitchToCanoe();

	}

}
