using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanoeZone : MonoBehaviour
{
	public float x, y;
	// Start is called before the first frame update

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(x, y, 0));
	}
}
