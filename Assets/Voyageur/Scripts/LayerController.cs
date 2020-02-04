using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerController : MonoBehaviour
{

	

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "TriggerTop")
		{
			transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 1;
		}

		if (other.gameObject.tag=="TriggerBottom")
		{
			transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 3;
		}
	}
}

