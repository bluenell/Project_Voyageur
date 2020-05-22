using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientFish : MonoBehaviour
{

	Animator anim;
	float timer;
	bool generated;
	int random;
	int fish;

	[SerializeField] Vector2 spawnRate;


	private void Awake()
	{
		anim = transform.GetChild(0).GetComponent<Animator>();
	}

	private void Update()
	{
		timer += Time.deltaTime;

		if (!generated)
		{
			fish = Random.Range(0,2);
			random = Mathf.FloorToInt(Random.Range(spawnRate.x, spawnRate.y));
			generated = true;
		}

		if (timer > random)
		{
			if (fish == 0)
			{
				anim.SetTrigger("play");
			}
			else
			{
				anim.SetTrigger("play2");
			}

			timer = 0;
			fish = 0;
		}

	}
	

}
