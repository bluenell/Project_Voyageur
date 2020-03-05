using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetchDog : MonoBehaviour
{
	MontyStateManager stateManager;
	MontyStateVariables stateVariables;
	MontyStateActions stateActions;
	PlayerController playerController;
	GameObject monty;

	public Rigidbody2D stickRb;
	public Transform goal;

	public float throwGravity;
	public float throwHeight;

	private void Start()
	{
		monty = GameObject.Find("Monty");
		playerController = GetComponent<PlayerController>();
		stateManager = monty.GetComponent<MontyStateManager>();
		stateVariables = monty.GetComponent<MontyStateVariables>();
		stateActions = monty.GetComponent<MontyStateActions>();
	}

	private void Update()
	{
		if (Input.GetButtonDown("Button A") && stateVariables.montyHasStick && stateVariables.GetPlayerDistanceFromStick() <= stateVariables.GetFetchStick().GetComponent<Stick>().range)
		{
			Debug.Log("Player has stick");
			stateVariables.playerHasStick = true;
			stateVariables.GetFetchStick().transform.position = transform.GetChild(3).transform.position;

		}else if (Input.GetButtonDown("Button A") && stateVariables.playerHasStick)
		{
			//Debug.Log("Throw");
			playerController.DisablePlayerInput();
			ThrowStick();
		}
		

	}

	void ThrowStick()
	{
		Physics2D.gravity = Vector2.up * stateVariables.throwGravity;
		stickRb.gravityScale = 1;
		Debug.Log(CalculateThrowVelocity());
		stickRb.velocity = CalculateThrowVelocity();
	}

	public Vector2 CalculateThrowVelocity()
	{
		float displacementY = goal.position.y - stickRb.position.y;
		Vector2 displacementX = new Vector2(goal.position.x - stickRb.position.x, 0);

		Vector2 velocityY = Vector2.up * Mathf.Sqrt(-2 * (throwGravity * throwHeight));
		Vector2 velocityX = displacementX / (Mathf.Sqrt(-2 * throwHeight / throwGravity) + Mathf.Sqrt(2 * (displacementY - throwHeight) / throwGravity));

		return velocityX + velocityY;
	}



}
