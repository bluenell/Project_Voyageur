using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
	public float range;
	Rigidbody2D rb;
	MontyStateVariables stateVariables;
	MontyStateManager stateManager;
	public bool hitGround = false;

	private void Start()
	{
		stateVariables = GameObject.Find("Monty").GetComponent<MontyStateVariables>();
		stateManager = GameObject.Find("Monty").GetComponent<MontyStateManager>();
		rb = GetComponent<Rigidbody2D>();
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(transform.position, range);
	}


	private void FixedUpdate()
	{
		if (stateManager.currentState == "fetch")
		{
			if (stateVariables.stickThrown)
			{
				rb.AddTorque(-rb.velocity.x/2);
				if (transform.position.y <= stateVariables.GetThrowTarget().position.y)
				{
					Debug.Log("hit ground");
					hitGround = true;

					rb.velocity = new Vector3();
					rb.gravityScale = 0;
					rb.angularVelocity = 0;
					rb.freezeRotation = true;
				}
			}

			if (transform.position.x == stateVariables.GetFetchStartingPoint().x && transform.position.y == stateVariables.GetFetchStartingPoint().y)
			{
				Debug.Log("hit ground");
				hitGround = true;

				rb.velocity = Vector3.zero;
				rb.gravityScale = 0;
				rb.angularVelocity = 0;
				rb.freezeRotation = true;

			}
		}
		
		
	}


}
