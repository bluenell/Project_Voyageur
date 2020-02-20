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
}
