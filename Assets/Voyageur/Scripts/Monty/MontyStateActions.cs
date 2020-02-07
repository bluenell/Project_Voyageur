using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MontyStateActions : MonoBehaviour
{
	Animator anim;
	MontyStateVariables stateVariables;

	private void Start()
	{
		stateVariables = GetComponent<MontyStateVariables>();
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
		//StartCoroutine(stateVariables.DelayAnimation());
		Debug.Log("Monty is following");
		anim.SetBool("isMoving", true);
		anim.SetBool("isSitting", false);



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
