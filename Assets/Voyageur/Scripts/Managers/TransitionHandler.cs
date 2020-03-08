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

	private void Awake()
	{
		playerController = player.GetComponent<PlayerController>();
		paddleScript = canoeAIO.GetComponent<CanoePaddle>();
		cameraHandler = GameObject.Find("Camera Manager").GetComponent<CameraHandler>();
	}


	public void Beach()
	{
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
		//hide player
		//hide monty
		//hide canoe object
		//show canoe all in one at the new location
		pathwayCollider.SetActive(false);
		cameraHandler.SwitchToCanoe();
	}

}
