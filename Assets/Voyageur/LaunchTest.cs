using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchTest : MonoBehaviour
{
	public Rigidbody ball;
	public Transform goal;

	public float h = 4.06f;
	public float g = -9.87f;

	private void Start()
	{
		ball.useGravity = false;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Launch();
		}
	}

	void Launch()
	{
		Physics.gravity = Vector3.up * g;
		ball.useGravity = true;
		ball.velocity = CalculateLaunchVelocity();
		Debug.Log(CalculateLaunchVelocity());
	}

	Vector3 CalculateLaunchVelocity()
	{
		float displacementY = goal.position.y - ball.position.y;
		Vector2 displacementX = new Vector2(goal.position.x - ball.position.x, 0);

		Vector2 velocityY = Vector2.up * Mathf.Sqrt(-2 * (g * h));
		Vector2 velocityX = displacementX / (Mathf.Sqrt(-2 * h / g) + Mathf.Sqrt(2 * (displacementY - h) / g));

		return velocityX + velocityY;
	}
}