using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public int currentIsland;
    public bool inTutorial;

    public GameObject player, monty, canoe;
    public Animator fade;
	public TransitionHandler transition;
    public bool paused;
	public bool endGame;

	public GameObject journal;
	public GameObject summaryJournal;

	private void Start()
	{
		Cursor.visible = false;
		Time.timeScale = 1;
	}

	private void Update()
    {
        if (inTutorial)
        {
            monty.GetComponent<MontyStateManager>().enabled = false;
        }
        else
        {
            monty.GetComponent<MontyStateManager>().enabled = true;
        }
    }

    public void IncreaseIsland()
    {
        currentIsland++;
    }

    public int GetCurrentIsland()
    {
        return currentIsland;
    }

    public void Fade()
    {
        fade.SetTrigger("fade in");
        StartCoroutine(Delay());

    }


	public void HardReset()
	{
        PlayerController playerController = player.GetComponent<PlayerController>();

		ResumeGame();

		canoe.transform.position = transition.playerSpawnPoints[GetCurrentIsland() - 1].transform.GetChild(0).transform.position;
		player.transform.position = transition.playerSpawnPoints[GetCurrentIsland() - 1].transform.GetChild(1).transform.position;
		monty.transform.position = transition.playerSpawnPoints[GetCurrentIsland() - 1].transform.GetChild(2).transform.position;
		player.transform.GetChild(0).GetComponent<Animator>().SetTrigger("hardreset");


        playerController.enabled = true;
		playerController.currentInventoryIndex = 0;
		playerController.usingAxe = false;
		playerController.usingRod = false;
		playerController.usingHands = false;
        playerController.interactionType = null;
        playerController.isMoving = false;
		StartCoroutine(playerController.EnablePlayerInput(0));
		playerController.targetFound = false;

        player.GetComponent<InteractionsManager>().interaction = null;

		monty.GetComponent<MontyStateActions>().currentlyOnPath = false;
		monty.GetComponent<MontyStateManager>().inFetch = false;

		monty.GetComponent<MontyStateManager>().currentState = "roam";
		monty.GetComponent<MontyStateManager>().SwitchState();





	}

	public void EndGame()
	{
		endGame = true;
		journal.SetActive(false);
		summaryJournal.SetActive(true);
		PauseGame();
	}

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(2);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
		paused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
		paused = false;

    }

}
