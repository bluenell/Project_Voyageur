using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanoeSortingLayer : MonoBehaviour
{
	public SpriteRenderer playerSprite, canoeSprite;


	public void UpdateSprite()
	{
		canoeSprite.sortingOrder = playerSprite.sortingOrder;
		playerSprite.sortingOrder = canoeSprite.sortingOrder + 1;
	}
}
