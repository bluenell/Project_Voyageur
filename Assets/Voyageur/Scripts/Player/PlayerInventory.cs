using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

	[Header("Tools")]
	public List<int> tools;

	public bool hasAxe, hasRod;

	[Header("Items")]
	int woodCount;
	int fishCount;
	public bool hasStick;


	public void SortInventory()
	{
		tools.Sort();
	}


	public int GetWoodCount()
	{
		return woodCount;
	}

	public void AddWood()
	{
		woodCount += 2;
	}


}
