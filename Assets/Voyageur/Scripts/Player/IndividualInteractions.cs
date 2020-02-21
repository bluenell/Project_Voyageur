using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualInteractions : MonoBehaviour
{
	public GameObject[] animals;



	InteractionsManager manager;

	private void Start()
	{
		manager = GameObject.Find("Player").GetComponent<InteractionsManager>();
	}

	Animator anim;

	#region InputInteractions
	void Sleep()
	{
		//check time
		//play animations
		//play sounds
		//advance time to 06:00
	}

	void Eat()
	{
		 //check fish
		 //play animation
		 //stamina = max
	}

	void Harmonica()
	{
		//play clip at index
		//advance index
		//play animation
		//exit


	}

	void PetDog()
	{
		//play animation
		//play sound
	}

	void Chop()
	{
		//play animation
		//play sound
		//increase counter
		//if counter = max, destroy object

	}

	void Collect()
	{
		//play animation
		//play sound
		//increase counter
		//if counter = max, destroy object
		//add to inventory

	}

	void LightFire()
	{
		//check if wood
		//play animation
		//play sound
		//create fire object
		
	}

	void InvestigateMine()
	{
		//play animation
	}

	#endregion 

	public void Squirrel()
	{
		manager.interaction.gameObject.transform.GetChild(0).gameObject.SetActive(true);

		anim = manager.interaction.gameObject.transform.GetChild(0).GetComponent<Animator>() ;
		
		anim.SetTrigger("Play");



	}

	public void BlueJay()
	{
		anim = manager.interaction.gameObject.transform.GetChild(0).GetComponent<Animator>();

		anim.SetTrigger("Play");
	}


	public void Fetch()
	{
		Debug.Log("Interation with Fetch");
	}




}
