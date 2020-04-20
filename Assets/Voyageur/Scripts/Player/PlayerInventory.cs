using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

	[Header("Tools")]
	public List<int> tools;

	public bool hasAxe, hasRod;

	[Header("Items")]
	public bool hasWood;
	int fishCount; 
	public bool hasStick;

	public SpriteRenderer axe, rod;

	public void DisplayAxe()
	{
		axe.enabled = true;
		rod.enabled = false;
	}

	public void DisplayRod()
	{
		axe.enabled = false;
		rod.enabled = true;
	}

	public void HideAxe()
	{
		axe.enabled = false;
	}
	public void HideRod()
	{
		rod.enabled = false;
	}

	public void SortInventory()
	{
		tools.Sort();
	}



	public int GetFishCount()
	{
		return fishCount;
	}


	public void AddFish()
	{
		fishCount++;

		if (fishCount>3)
		{
			fishCount = 3;

		}

	}


}
