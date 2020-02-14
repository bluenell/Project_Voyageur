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

	public float speed;

	float tempTime = 0;

	float counter = 6;
	int countMax = 3;
	bool canPaddle;

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = transform.GetChild(0).GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		transform.Translate(new Vector2(river.riverCurrent * Time.deltaTime, 0));

		


		if (Input.GetButtonDown("Button B"))
		{
			counter = Time.time - tempTime;
			if (counter < 1.2)
			{
				return;
			}
			tempTime = Time.time;
			Paddle();
		}

		//Float();
	}

	

	void Paddle()
	{
		speed = maxSpeed;

		rb.AddForce(new Vector2(150f,0));
		Debug.Log(counter);

	}
	void Float()
	{
		

	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "River")
		{
			//Debug.Log("River");
			river = collision.gameObject.GetComponent<River>();
		}
	}

}
