using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{


	public string interactionName;
	public bool complete;
	public bool isInteractable;
	public Sprite journalImage;
	public string journalName;

	[TextArea]
	public string journalDescription;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space)) 
		{
			MarkAsComplete();
		}
	}


	public void MarkAsComplete()
	{
		//Debug.Log(name + " is complete");
		complete = true;

		GameObject.Find("Canvas").GetComponent<Journal>().UpdateInteractionPages(journalName, journalDescription, journalImage);

	}



}
