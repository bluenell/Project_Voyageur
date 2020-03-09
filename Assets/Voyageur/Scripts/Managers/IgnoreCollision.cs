using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == 16)
		{
			Physics2D.IgnoreLayerCollision(12, 16);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.layer == 13)
		{
			Physics2D.IgnoreLayerCollision(13, 16);
		}
	}
}
