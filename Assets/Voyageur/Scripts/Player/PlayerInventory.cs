using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

	DayNightCycleManager nightCycle;

	private void Start()
	{
		nightCycle = GameObject.Find("Global Light (Sun)").GetComponent<DayNightCycleManager>();
	}

	

	[Header("Tools")]
	public List<string> tools;

	[Header("Items")]
	public bool hasWood;
	public bool hasFish; 
	public bool hasStick;

	

	

}
