using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerLighting : MonoBehaviour
{

	SpriteRenderer sprite;
	public SpriteRenderer playerSprite;

	private void Awake()
	{
		sprite = GetComponent<SpriteRenderer>();

	}


	private void Update()
	{
		if (sprite.sortingOrder > playerSprite.sortingOrder)
		{
			sprite.sortingLayerName = "Ignore Fire Light";
		}
		else
		{
			sprite.sortingLayerName = "Default";
		}
	}


}
