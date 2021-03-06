﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
	public SummaryJournal summaryJournal;
	public Sprite journalImage;
	public string journalName;
	[TextArea]
	public string journalDescription;

	public void MarkAsComplete()
	{

		Debug.Log(name + " is complete");

		if (manager.interaction != null)
		{
			complete = true;
			manager.inRange = false;

			interactionCollider.enabled = false;


			manager.interaction = null;
		}

		if (hasJournalEntry)
		{
			StartCoroutine(PlaySoundWithDelay(2f));
			journal.UpdateInteractionPages(journalName, journalDescription, journalImage);
			summaryJournal.UpdateInteractionPages(journalName, journalDescription, journalImage);
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


	IEnumerator PlaySoundWithDelay(float delay)
	{
		yield return new WaitForSeconds(delay);
		manager.GetComponent<PlayerSoundManager>().PlayCameraShutter();
	}


}
