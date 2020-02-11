using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
	public int index;
	public string iName;
	public bool complete;

	public void MarkAsComplete()
	{
		//Debug.Log(name + " is complete");
		complete = true;

	}
}
