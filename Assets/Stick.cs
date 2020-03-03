using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
	public float range;

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(transform.position, range);
	}



}
