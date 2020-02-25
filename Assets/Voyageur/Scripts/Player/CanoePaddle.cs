using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanoePaddle : MonoBehaviour
{


	Rigidbody2D rb;
	River river;
	Animator anim;

	Vector2 movement;
	//Vector2 momementum = new Vector2();

	[Header("Canoe Speed Variables")]
	public float paddleForce;

	public float speed;

	float tempTime = 0;

	float counter = 6;
	int countMax = 3;
	bool canPaddle;
	bool beached;

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
			Paddle();
		}

		
	}


	void Paddle()
	{ 
		if (!beached)
		{
			movement = new Vector2(river.riverCurrent * Time.deltaTime, 0);
			transform.Translate(movement);
		}

		if (Input.GetButtonDown("Button B") && canPaddle)
		{
			counter = Time.time - tempTime;
			if (counter < 1.2)
			{
				return;
			}
			tempTime = Time.time;

			anim.SetTrigger("Paddle");

			StartCoroutine(AddPaddleForce());
		}
	}


	IEnumerator AddPaddleForce()
	{
		yield return new WaitForSeconds(0.6f);
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
