using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MontyStateActions : MonoBehaviour
{
	Animator anim;
	MontyStateVariables stateVariables;
	GameObject player;


	private void Start()
	{
		stateVariables = GetComponent<MontyStateVariables>();
		anim = transform.GetChild(0).GetComponent<Animator>();
		player = GameObject.Find("Player");
	}

	public void Canoe()
	{
		Debug.Log("Monty in Canoe");
	}

	public void CanoeFish()
	{
		Debug.Log("Monty Fishing");
	}

	public void Idle()
	{


	   	Debug.Log("Monty is Idle");
		anim.SetBool("isMoving", false);





	}

	public void Follow()
	{
		SpriteRenderer sprite;
		sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();


		//StartCoroutine(stateVariables.DelayAnimation());
		Debug.Log("Monty is following");
		anim.SetBool("isMoving", true);
		anim.SetBool("isSitting", false);

		if (player.transform.position.x < transform.position.x)
		{
			sprite.flipX = true;
		}
		if (player.transform.position.x > transform.position.x)
		{
			sprite.flipX = false;
		}


		transform.position = Vector2.MoveTowards(transform.position, player.transform.position, stateVariables.montySpeed * Time.deltaTime);
	}

	public void Sit()
	{
		anim.SetBool("isSitting", true);

		Debug.Log("Monty is sitting");
	}
	public void Fetch()
	{
		Debug.Log("Monty is playing fetch");
	}



}
