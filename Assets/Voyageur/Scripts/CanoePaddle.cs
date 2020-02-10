using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanoePaddle : MonoBehaviour
{

	Rigidbody2D rb;
	River river;
	Animator anim;

	Vector2 movement;
	Vector2 momementum = new Vector2();

	[Header("Canoe Speed Variables")]
	public float decelerationRate;
	public float friction;
	public float stopValue;
	public float maxSpeed;
	public float brakingMultiplier;



	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = transform.GetChild(0).GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetAxis("Horizontal") > 0)
		{
			anim.SetBool("isPaddling", true);
			anim.SetBool("isStopping", false);
		}
		else if(Input.GetAxis("Horizontal") < 0)
		{
			anim.SetBool("isStopping", true);
			anim.SetBool("isPaddling", false);
		}
		else
		{
			anim.SetBool("isStopping", false);
			anim.SetBool("isPaddling", false);
		}


		Paddle();
	}

	void Paddle()
	{

		
		float xDir = Input.GetAxis("Horizontal");

		if (xDir < 0)
		{
			xDir = 0;
			brakingMultiplier = 3f;
		}
		else
		{
			brakingMultiplier = 1f;
		}

		movement = new Vector2(xDir, 0);

		if (xDir <= 0)
		{

			momementum -= ((momementum.normalized * decelerationRate * Time.deltaTime * friction) * brakingMultiplier);
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
