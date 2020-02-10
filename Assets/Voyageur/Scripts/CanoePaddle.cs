using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanoePaddle : MonoBehaviour
{

	Rigidbody2D rb;
	River river;

	public float paddleForce;

    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
	{
		if (Input.GetAxis("Horizontal") > 0)
		{
			Paddle();
		}
		else
		{
			Float();
		}
    }


	void Paddle()
	{
		rb.velocity = new Vector2(paddleForce, 0);
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
