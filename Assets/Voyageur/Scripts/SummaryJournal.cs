using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryJournal : MonoBehaviour
{
	public bool timerStarted;
	public float timer;

	public TMPro.TextMeshProUGUI[] stats;


	public GameObject journalUI;
	public GameObject[] journalPages;
	public GameObject[] interactions;
	public GameObject[] fishes;
	List<Fish> fishCaught;
	public PlayerController playerController;
	public PlayerSoundManager sounds;
	PlayerInventory inventory;
	GameManager gm;

	int pageIndex;
	int interactionIndex;
	int fishIndex;

	public int timesFetchPlayed;
	int totalFishCaught;

	private void Awake()
	{
		timerStarted = true;

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

	private void Update()
	{
		if (timerStarted)
		{
			timer += Time.deltaTime;
			UpdateGUI();
		}

		if (gm.endGame)
		{
			if (Input.GetButtonDown("InventoryRight") || (Input.GetKeyDown(KeyCode.RightArrow)) || (Input.GetKeyDown(KeyCode.D)))
			{

				if (pageIndex == journalPages.Length - 1)
				{
					pageIndex = journalPages.Length - 1;
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
			int newIndex = GetIndexOfFishCaughtInList(fish);
			fishes[newIndex].transform.GetChild(1).transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = fish.timesCaught.ToString();

		}

		totalFishCaught++;

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

	int GetIndexOfFishCaughtInList(Fish fish)
	{
		return fishCaught.IndexOf(fish);
	}

	void UpdateGUI()
	{
		float minutes = Mathf.FloorToInt(timer / 60);
		float seconds = Mathf.FloorToInt(timer - minutes * 60);
		string gameTime = string.Format("{0:00}:{1:00}", minutes, seconds);

		stats[0].text = gameTime;
		stats[1].text = interactionIndex.ToString();
		stats[2].text = timesFetchPlayed.ToString();
		stats[3].text = totalFishCaught.ToString();

	}
}
