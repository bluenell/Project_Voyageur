using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int currentIsland;
    public bool inTutorial;

    public GameObject player, monty;


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
}
