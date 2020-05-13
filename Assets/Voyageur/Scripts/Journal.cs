using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Journal : MonoBehaviour
{
    public GameObject journalUI; 
    public GameObject[] journalPages;
    public GameObject[] interactions;
	public GameObject[] fishes;
	List<Fish> fishCaught;
    public GameObject[] lines;
    public PlayerController playerController;
    public PlayerSoundManager sounds;
    PlayerInventory inventory;
    GameManager gm;

    bool paused;
    int pageIndex;
    int interactionIndex;
	int fishIndex;

    float nextSwitchTime;
    float switchRate = 2f;

    private void Start()
    {
		fishCaught = new List<Fish>();
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        sounds = playerController.gameObject.GetComponent<PlayerSoundManager>();
        inventory = playerController.gameObject.GetComponent<PlayerInventory>();

        interactionIndex = 0;
		fishIndex = 0;
        journalUI.SetActive(false);
        gm.paused = false;

        for (int i = 0; i < interactions.Length; i++)
        {
            interactions[i].SetActive(false);
        }

		for (int i = 0; i < fishes.Length; i++)
		{
			fishes[i].SetActive(false);
		}

    }


	public void HardReset()
	{
		journalUI.SetActive(false);
		gm.HardReset();
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
			if (pageIndex == 6)
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
            if (Input.GetButtonDown("InventoryRight") || (Input.GetKeyDown(KeyCode.RightArrow)) || (Input.GetKeyDown(KeyCode.D)))
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
            if (Input.GetButtonDown("InventoryLeft") || (Input.GetKeyDown(KeyCode.LeftArrow)) || (Input.GetKeyDown(KeyCode.A)))
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

	public void UpdateFishPages(Fish fish)
	{
		if (!CheckIfExists(fish))
		{
			fishCaught.Add(fish);
			fishes[fishIndex].SetActive(true);
			fishes[fishIndex].GetComponent<Image>().sprite = fish.image;

			fishes[fishIndex].transform.GetChild(0).transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = fish.fishName;
			fishes[fishIndex].transform.GetChild(0).transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = fish.fishDesc;
			fishes[fishIndex].transform.GetChild(0).transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = fish.fishSize;
			fishes[fishIndex].transform.GetChild(1).transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = fish.timesCaught.ToString();
			fishIndex++;
		}
		else
		{
			fishes[fishIndex].transform.GetChild(1).transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = fish.timesCaught.ToString();

		}
	}

	bool CheckIfExists(Fish fish)
	{
		if (fishCaught.Contains(fish))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

}
