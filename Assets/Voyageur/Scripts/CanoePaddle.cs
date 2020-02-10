using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanoePaddle : MonoBehaviour
{

	Rigidbody2D rb;
	River river;

	public float paddleForce;

	Vector2 movement;
	Vector2 momementum = new Vector2();
	public float acceleration;
	public float friction;
	public float stopValue;
	public float maxSpeed;





    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody2D>();
    }

	// Update is called once per frame
	void Update()
	{

		float xDir = Input.GetAxis("Horizontal");
		movement = new Vector2(xDir, 0);

		momementum -= (momementum.normalized * acceleration * Time.deltaTime * friction);



		if (xDir <= 0)
		{
			if (momementum.magnitude < stopValue)
			{

				momementum = new Vector2();
				Float();
			}
		}

		momementum += movement;


		if (momementum.magnitude > (maxSpeed * Time.deltaTime)) 
		{
			momementum = momementum.normalized * (maxSpeed * Time.deltaTime);
		}

		transform.Translate(momementum);

	}


	void Paddle()
	{

	}


	void Float()
	{
		rb.velocity = new Vector2(river.riverCurrent, 0);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "River")
		{
			river = collision.gameObject.GetComponent<River>();
		}
	}

}
