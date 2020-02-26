using UnityEngine;
using UnityEditor;

public class SwitchPlayer : EditorWindow
{
	
	public static GameObject player;
	public static GameObject canoe;
	public static GameObject canoeAIO;
	public static GameObject monty;
	public static GameObject layerManager;
	public static GameObject interactionsManager;

	public static GameObject canoeCam;
	public static GameObject playerCam;


	private void OnEnable()
	{
		player = ((GameObject)GameObject.Find("Player"));
		canoe = ((GameObject)GameObject.Find("CanoeSingle"));
		canoeAIO = ((GameObject)GameObject.Find("Canoe AIO"));
		monty = ((GameObject)GameObject.Find("Monty"));
		layerManager = ((GameObject)GameObject.Find("LayerManager"));
		interactionsManager = ((GameObject)GameObject.Find("InteractionsManager"));
		canoeCam = ((GameObject)GameObject.Find("CanoeCam"));
		playerCam = ((GameObject)GameObject.Find("PlayerCam"));

	}

	[MenuItem("Testing/Switch to Player")]
	static void SwitchToPlayer()
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
	[MenuItem("Testing/Switch to Canoe")]
	static void SwitchToCanoe()
	{
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
