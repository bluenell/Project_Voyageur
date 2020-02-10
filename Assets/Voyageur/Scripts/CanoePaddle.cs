using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanoePaddle : MonoBehaviour
{

	Rigidbody2D rb;

	public float riverSpeed;
	public float paddleForce;

    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
	{
		rb.velocity = new Vector2(riverSpeed, 0);
	
    }


	void Paddle()
	{

	}


	void Float()
	{

	}
}
