using UnityEngine;
using UnityEditor;

public class SwitchPlayer : Editor
{
	
	public static GameObject player;
	public static GameObject canoe;
	public static GameObject canoeAIO;
	public static GameObject monty;
	public static GameObject layerManager;
	public static GameObject interactionsManager;
	public static GameObject canoeCam;
	public static GameObject playerCam;






	[MenuItem("Testing/Switch to Player")]
	static void SwitchToPlayer()
	{
		player = GameObject.Find("Player").gameObject;
		canoe = GameObject.Find("CanoeSingle").gameObject;
		canoeAIO = GameObject.Find("Canoe AIO").gameObject;
		monty = GameObject.Find("Monty").gameObject;
		layerManager = GameObject.Find("LayerManager").gameObject;
		interactionsManager = GameObject.Find("InteractionsManager").gameObject;
		canoeCam = GameObject.Find("CanoeCam").gameObject;

		canoeAIO.SetActive(false);
		canoe.SetActive(true);
		player.SetActive(true);
		monty.SetActive(true);
		layerManager.SetActive(true);
		interactionsManager.SetActive(true);
		canoeCam.SetActive(false);
		playerCam.SetActive(true);
	}

	[MenuItem("Testing/Switch to Canoe")]
	static void SwitchToCanoe()
	{
		player = GameObject.Find("Player").gameObject;
		canoe = GameObject.Find("CanoeSingle").gameObject;
		canoeAIO = GameObject.Find("Canoe AIO").gameObject;
		monty = GameObject.Find("Monty").gameObject;
		layerManager = GameObject.Find("LayerManager").gameObject;
		interactionsManager = GameObject.Find("InteractionsManager").gameObject;
		canoeCam = GameObject.Find("CanoeCam").gameObject;

		canoeAIO.SetActive(true);
		canoe.SetActive(false);
		player.SetActive(false);
		monty.SetActive(false);
		layerManager.SetActive(false);
		interactionsManager.SetActive(false);
		canoeCam.SetActive(true);
		playerCam.SetActive(false);

	}

}
