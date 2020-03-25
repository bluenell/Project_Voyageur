using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class TransitionHandler : MonoBehaviour
{


	public int currentIsland = 0;

	[Header("Components")]
	public GameObject player;
	public GameObject canoe;
	public GameObject canoeAIO;
	public GameObject monty;
	public GameObject layerManager;
	public GameObject interactionsManager;
	public GameObject[] pathfindingManagers;

	CanoePaddle paddleScript;
	PlayerController playerController;
	CameraHandler cameraHandler;
	GameManager gm;


	public GameObject pathwayCollider;
	public GameObject spritesManager;

	[Header("Spawn Points")]
	public GameObject[] playerSpawnPoints;
	public GameObject[] canoeSpawnPoints;

	

	private void Awake()
	{
		playerController = player.GetComponent<PlayerController>();
		paddleScript = canoeAIO.GetComponent<CanoePaddle>();
		cameraHandler = GameObject.Find("Camera Manager").GetComponent<CameraHandler>();
		gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
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



	}

	public void Launch()
	{
		gm.IncreaseIsland();
		Debug.Log("Transitioning to island: " + gm.GetCurrentIsland());

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

		for (int i = 0; i < pathfindingManagers.Length; i++)
		{
			pathfindingManagers[i].SetActive(false);
		}

		pathfindingManagers[gm.GetCurrentIsland()].SetActive(true);

		//hide monty
		//hide canoe object
		//show canoe all in one at the new location
		cameraHandler.SwitchToCanoe();

	}

}
