﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Journal : MonoBehaviour
{
    public GameObject journalUI; 
    public GameObject[] journalPages;
    public GameObject[] interactions;
    public GameObject[] lines;
    public PlayerController playerController;
    public PlayerSoundManager sounds;
    PlayerInventory inventory;
    GameManager gm;

    bool paused;
    int pageIndex;
    int interactionIndex;

    float nextSwitchTime;
    float switchRate = 2f;

    private void Start()
    {
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        sounds = playerController.gameObject.GetComponent<PlayerSoundManager>();
        inventory = playerController.gameObject.GetComponent<PlayerInventory>();

        interactionIndex = 0;
        journalUI.SetActive(false);
        gm.paused = false;

        for (int i = 0; i < interactions.Length; i++)
        {
            interactions[i].SetActive(false);
        }

    }

    private void Update()
    {

        if (inventory.hasAxe) 
        {
            lines[0].SetActive(true);
        }
        if (inventory.hasRod)
        {
            lines[1].SetActive(true);
        }


		if (gm.paused)
		{
			if (pageIndex == 4)
			{
				Cursor.visible = true;
			}
			else
			{
				Cursor.visible = false;
			}
		}
		else
		{
			Cursor.visible = false;
		}


        if (Input.GetButtonDown("Button Start") || Input.GetButtonDown("Button Select") || Input.GetKeyDown(KeyCode.Escape))
        {
            if (gm.paused)
            {
                sounds.PlayPageTurn();
                playerController.enabled = true;
                journalUI.SetActive(false);
                gm.ResumeGame();
                gm.paused = false;
                // close the journal
            }
            else
            {
                sounds.PlayPageTurn();
                playerController.enabled = false;
                journalUI.SetActive(true);
                gm.PauseGame();
                gm.paused = true;

                journalPages[pageIndex].SetActive(true);
            }
        }

        if (gm.paused)
        {
            if (Input.GetButtonDown("InventoryRight") || (Input.GetKeyDown(KeyCode.RightArrow)))
            {
               
                if (pageIndex == journalPages.Length -1)
                {
                    pageIndex = journalPages.Length-1;
                }
                else
                {
                    sounds.PlayPageTurn();
                    pageIndex++;
                }

                for (int i = 0; i < journalPages.Length; i++)
                {
                    journalPages[i].SetActive(false);
                }

                journalPages[pageIndex].SetActive(true);
            }
            if (Input.GetButtonDown("InventoryLeft") || (Input.GetKeyDown(KeyCode.LeftArrow)))
            {
               
                if (pageIndex == 0)
                {
                    pageIndex = 0;
                }
                else
                {
                    sounds.PlayPageTurn();
                    pageIndex--;
                }

                for (int i = 0; i < journalPages.Length; i++)
                {
                    journalPages[i].SetActive(false);
                }

                journalPages[pageIndex].SetActive(true);

            }

        }

    }

    public void UpdateInteractionPages(string name, string desc, Sprite sprite)
    {
        //sounds.PlayPageTurn();
        interactions[interactionIndex].SetActive(true);
        interactions[interactionIndex].GetComponent<Image>().sprite = sprite;

        interactions[interactionIndex].transform.GetChild(0).transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = name;
        interactions[interactionIndex].transform.GetChild(0).transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = desc;

        interactionIndex++;

    }

}
