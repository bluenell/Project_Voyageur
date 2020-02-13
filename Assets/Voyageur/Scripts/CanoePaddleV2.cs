using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanoePaddleV2 : MonoBehaviour
{

	bool canPaddle = true;

	float counter, x;
	public int countMax;

	public float maxSpeed,decelerationRate, friction;


	Vector2 movement, momentum;


    void Update()
    {
		x = Input.GetAxis("Horizontal");

		if (x > 0)
		{
			Paddle();
			
		}

    }



	void Paddle()
	{

		movement = new Vector2(x, 0);

		momentum += movement;


		if (x <= 0)
		{
			momentum -= ((momentum.normalized * decelerationRate * Time.deltaTime * friction));
		}

		if (momentum.magnitude > (maxSpeed * Time.deltaTime))
		{
			momentum = momentum.normalized * (maxSpeed * Time.deltaTime);
		}

		transform.Translate(momentum);

	}
}
