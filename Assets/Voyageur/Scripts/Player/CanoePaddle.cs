using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanoePaddle : MonoBehaviour
{
	Rigidbody2D rb;
	River river;
	Animator anim;

	Vector2 movement;

	[Header("Canoe Speed Variables")]
	public float paddleForce;
	public float paddleRate;
	public float nextPaddleTime;

	public bool canPaddle;
	public bool beached;
	bool stickReset;



	// Start is called before the first frame update
	void Start()
	{
		beached = false;
		canPaddle = true;
		rb = GetComponent<Rigidbody2D>();
		anim = transform.GetChild(0).GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{		
		if (beached)
		{
			BeachCanoe();
		}
		else
		{
				movement = new Vector2(river.riverCurrent * Time.deltaTime, 0);
				transform.Translate(movement);

			if (Time.time > nextPaddleTime)
			{
				if (Input.GetAxis("Horizontal") > 0 && stickReset)
				{
					stickReset = false;
					Paddle();
					nextPaddleTime = Time.time + 1f / paddleRate;
				}
			}		
		}

		if (Input.GetAxis("Horizontal") == 0)
		{
			stickReset = true;
		}
	}


	void Paddle()
	{
		if (canPaddle)
		{
			anim.SetTrigger("Paddle");
		}
	}


	public void AddPaddleForce()
	{
		rb.AddForce(new Vector2(paddleForce, 0));
	}

	void DisableMovement()
	{
		beached = true;
		canPaddle = false;
	}

	void BeachCanoe()
	{
		//Debug.Log("Beach Canoe");
		DisableMovement();
		beached = true;


		//Debug.Log("Tes");

		if (Input.GetAxis("Horizontal") > 0)
		{
			//Debug.Log("Beach Animation");
			anim.SetTrigger("Beach");
		}

	}



	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "River")
		{
			//Debug.Log("River");
			river = collision.gameObject.GetComponent<River>();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "BeachPoint")
		{
			beached = true;
		}

		if (collision.gameObject.tag == "BeachDisableInput")
		{
			canPaddle = false;
			anim.SetBool("isPreaching", true);
		}
	}



}
