using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDogLayerManager : MonoBehaviour
{
	//on the empty


	GameObject player, dog;

	SpriteRenderer playerSprite, dogSprite;

	public int dogMin, playerMin;

	int bottomLayer,topLayer;

    // Start is called before the first frame update
    void Start()
    {
		player = GameObject.Find("Player");
		dog = GameObject.Find("Monty");

		playerSprite = player.transform.GetChild(0).GetComponent<SpriteRenderer>();
		dogSprite = dog.transform.GetChild(0).GetComponent<SpriteRenderer>();

	}

    // Update is called once per frame
    void Update()
    {

		if ((playerSprite.sortingOrder == dogSprite.sortingOrder) || (playerSprite.sortingOrder + 1 == dogSprite.sortingOrder) || (playerSprite.sortingOrder == dogSprite.sortingOrder + 1))
		{
			if (player.transform.position.y > dog.transform.position.y)
			{
				//dog infront player
				//Debug.Log("Dog in front");

				bottomLayer = (int)((Mathf.Floor(playerSprite.sortingOrder / 3) * 3) + 1);
				topLayer = bottomLayer + 1;


				dogSprite.sortingOrder = topLayer;
				playerSprite.sortingOrder = bottomLayer;
			}
			else
			{
				//player infront dog
				//Debug.Log("Player in front");

				bottomLayer = (int)((Mathf.Floor(playerSprite.sortingOrder / 3) * 3) + 1);
				topLayer = bottomLayer + 1;
					

				dogSprite.sortingOrder = bottomLayer;
				playerSprite.sortingOrder = topLayer;
			}
		
		}


		
    }
}
