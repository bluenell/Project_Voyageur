using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistantGeese : MonoBehaviour
{

	Animator anim;

	private void Awake()
	{
		anim = transform.GetChild(0).GetComponent<Animator>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		
		if (collision.gameObject.tag == "Canoe")
		{
			anim.SetTrigger("play");
		}
		
	}

}
