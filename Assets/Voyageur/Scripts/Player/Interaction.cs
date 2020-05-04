using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{

	public InteractionsManager manager;

	public string interactionName;
	public Collider2D interactionCollider;

	[SerializeField]
	bool complete;

	[SerializeField]
	bool isInteractable;
	public int requiredTool;

	bool cancelled;

	public bool forceFaceRight;

	[SerializeField]
	bool canBeCancelled;

	[Header("Journal")]
	public bool hasJournalEntry;
	public Journal journal;
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
		manager.interaction = null;
		manager.inRange = false;
		interactionCollider.enabled = false;


		if (!cancelled && hasJournalEntry)
		{
			journal.UpdateInteractionPages(journalName, journalDescription, journalImage);
		}
	}
	public void CancelInteraction()
	{
		Debug.Log(interactionName + " cancelled");
		cancelled = true;
		complete = true;
	}

	public bool GetComplete()
	{
		return complete;
	}

	public bool GetInteractable()
	{
		return isInteractable;
	}

	public bool GetCancelled()
	{
		return cancelled;
	}

	public bool GetCanBeCancelled()
	{
		return canBeCancelled;
	}

	



}
