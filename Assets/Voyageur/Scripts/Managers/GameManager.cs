using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public int currentIsland;
    public bool inTutorial;

    public GameObject player, monty;
    public Animator fade;
    public bool paused;

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

	public void EndGame()
	{
		StartCoroutine(Delay());
	}

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(2);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;

    }

}
