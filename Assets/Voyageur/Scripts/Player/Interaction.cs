using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
	public string interactionName;
	public bool complete;
	public bool isInteractable;

	public void MarkAsComplete()
	{
		//Debug.Log(name + " is complete");
		complete = true;

	}
}
