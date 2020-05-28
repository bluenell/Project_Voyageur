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
	MontyStateManager montyStateManager;
	MontyStateActions montyStateActions;

	public GameObject[] previousIslandColliders;
	public GameObject[] pathwayColliders;
	public GameObject spritesManager;

	[Header("Spawn Points")]
	public GameObject[] playerSpawnPoints;
	public GameObject[] canoeSpawnPoints;

	public GameObject door;

	public GameObject journal;

	public GameObject[] interactions;

	private void Awake()
	{
		playerController = player.GetComponent<PlayerController>();
		paddleScript = canoeAIO.GetComponent<CanoePaddle>();
		cameraHandler = GameObject.Find("Camera Manager").GetComponent<CameraHandler>();
		gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
		montyStateVariables = monty.GetComponent<MontyStateVariables>();
		montyStateManager = monty.GetComponent<MontyStateManager>();
		montyStateActions = monty.GetComponent<MontyStateActions>();

	}


	public void Door()
	{
		Destroy(door);
		player.SetActive(true);
		monty.SetActive(true);
		layerManager.SetActive(true);
		interactionsManager.SetActive(true);
		cameraHandler.enabled = true;
		journal.SetActive(true);
		gm.paused = true;
		gm.PauseGame();


		for (int i = 0; i < interactions.Length; i++)
		{
			interactions[i].SetActive(true);
		}




	}


	public void Beach()
	{
		//canoeAIO.transform.GetChild(1).gameObject.SetActive(false);
		canoe.transform.position = playerSpawnPoints[gm.GetCurrentIsland()-1].transform.GetChild(0).transform.position;
		player.transform.position = playerSpawnPoints[gm.GetCurrentIsland()-1].transform.GetChild(1).transform.position;
		monty.transform.position = playerSpawnPoints[gm.GetCurrentIsland()-1].transform.GetChild(2).transform.position;
		StartCoroutine(playerController.EnablePlayerInput(0f));

		canoeAIO.SetActive(false);
		canoe.SetActive(true);
		player.SetActive(true);
		monty.SetActive(true);
		layerManager.SetActive(true);
		interactionsManager.SetActive(true);
		cameraHandler.SwitchToPlayer();
		pathwayColliders[gm.GetCurrentIsland() - 1].SetActive(true);

		montyStateVariables.montyReadyToGetIn = false;
		montyStateVariables.montyInCanoe = false;
		montyStateVariables.jumping = false;

		spritesManager.SetActive(true);


		montyStateManager.inFetch = false;
		montyStateManager.inTutorial = false;
		montyStateVariables.grid.CreateGrid();
		montyStateActions.currentlyOnPath = false;
		canoe.transform.GetChild(0).GetComponent<Animator>().SetTrigger("return");

		monty.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = player.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder - 1;

	}

	public void PreLaunch()
	{
		previousIslandColliders[gm.GetCurrentIsland()].SetActive(false);

		canoeAIO.SetActive(true);
		canoeAIO.transform.GetChild(1).gameObject.SetActive(true);

		canoeAIO.GetComponent<CanoePaddle>().launched = true;
		canoeAIO.GetComponent<CanoePaddle>().beached = false;
		player.SetActive(false);
		monty.SetActive(false);
		canoe.transform.GetChild(0).GetComponent<Animator>().SetTrigger("return");
		canoe.SetActive(false);		


		canoeAIO.GetComponent<Rigidbody2D>().simulated = false;

		canoeAIO.transform.position = canoe.transform.GetChild(0).transform.position;
	}

	public void Launch()
	{
		// Increasing Current Island Count
		gm.IncreaseIsland();
		//Debug.Log("Transitioning to island: " + gm.GetCurrentIsland());

		// Enabling & Disabling Monty, Player, Canoe Single and their depencies
		//canoeAIO.transform.position = canoeSpawnPoints[gm.GetCurrentIsland()-1].transform.position;
		canoeAIO.GetComponent<CanoePaddle>().beached = false;
		canoeAIO.GetComponent<CanoePaddle>().launched = false;
		player.GetComponent<PlayerController>().canLaunch = false;
		player.GetComponent<PlayerController>().interactionType = "";
		canoeAIO.GetComponent<CanoePaddle>().canPaddle = true;
		canoeAIO.GetComponent<Rigidbody2D>().simulated = true;

		layerManager.SetActive(false);
		interactionsManager.SetActive(false);
		pathwayColliders[gm.GetCurrentIsland()-1].SetActive(false);
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
