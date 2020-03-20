using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == 12)
		{
			Physics2D.IgnoreLayerCollision(16, 12);
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
