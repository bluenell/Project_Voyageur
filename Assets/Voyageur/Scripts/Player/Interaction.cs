using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
	public string interactionName;
	public bool complete;

	public void MarkAsComplete()
	{
		//Debug.Log(name + " is complete");
		complete = true;

	}
}
