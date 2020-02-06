using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MontyStateActions : MonoBehaviour
{
	Animator anim;

	private void Start()
	{
		anim = transform.GetChild(0).GetComponent<Animator>();
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
		Debug.Log("Monty is following");
		anim.SetBool("isMoving", true);


	}

	public void Dig()
	{
		Debug.Log("Monty is digging");
	}
	public void Fetch()
	{
		Debug.Log("Monty is playing fetch");
	}



}
