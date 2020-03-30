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
	MontyStateVariables montyStateVariables;

	public GameObject[] pathwayColliders;
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
		montyStateVariables = monty.GetComponent<MontyStateVariables>();
		montyStateVariables = monty.GetComponent<MontyStateVariables>();
	}


	public void Beach()
	{
		canoe.transform.position = playerSpawnPoints[gm.GetCurrentIsland()].transform.GetChild(0).transform.position;
		player.transform.position = playerSpawnPoints[gm.GetCurrentIsland()].transform.GetChild(1).transform.position;
		monty.transform.position = playerSpawnPoints[gm.GetCurrentIsland()].transform.GetChild(2).transform.position;


		canoeAIO.SetActive(false);
		canoe.SetActive(true);
		player.SetActive(true);
		monty.SetActive(true);
		layerManager.SetActive(true);
		interactionsManager.SetActive(true);
		cameraHandler.SwitchToPlayer();
		pathwayColliders[0].SetActive(true);

		spritesManager.SetActive(true);

		montyStateVariables.grid.CreateGrid();


	}

	public void PreLaunch()
	{
		canoeAIO.SetActive(true);
		player.SetActive(false);
		monty.SetActive(false);
		canoe.SetActive(false);		

		canoeAIO.GetComponent<CanoePaddle>().launched = true;
		canoeAIO.GetComponent<Rigidbody2D>().simulated = false;

		canoeAIO.transform.position = canoe.transform.position;
	}

	public void Launch()
	{
		// Increasing Current Island Count
		gm.IncreaseIsland();
		Debug.Log("Transitioning to island: " + gm.GetCurrentIsland());

		// Enabling & Disabling Monty, Player, Canoe Single and their depencies
		canoeAIO.transform.position = canoeSpawnPoints[gm.GetCurrentIsland()-1].transform.position;
		canoeAIO.GetComponent<CanoePaddle>().beached = false;
		canoeAIO.GetComponent<CanoePaddle>().launched = false;
		canoeAIO.GetComponent<CanoePaddle>().canPaddle = true;
		canoeAIO.GetComponent<Rigidbody2D>().simulated = true;

		layerManager.SetActive(false);
		interactionsManager.SetActive(false);
		pathwayColliders[0].SetActive(false);
		spritesManager.SetActive(true);

		//Setting all other pathfinding managers to inactive
		for (int i = 0; i < pathfindingManagers.Length; i++)
		{
			pathfindingManagers[i].SetActive(false);
		}

		// Activating the next islands pathfinding and clearing any existing path requests
		pathfindingManagers[gm.GetCurrentIsland()].SetActive(true);
		PathRequestManager.ClearRequests();
		monty.GetComponent<MontyStateActions>().StopAllCoroutines();
		montyStateVariables.grid = pathfindingManagers[gm.GetCurrentIsland()].GetComponent<MyGrid>();
		
		

		//hide monty
		//hide canoe object
		//show canoe all in one at the new location
		cameraHandler.SwitchToCanoe();

	}

}
