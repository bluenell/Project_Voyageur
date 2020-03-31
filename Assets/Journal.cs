using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Journal : MonoBehaviour
{
    public GameObject journalUI;
    public GameObject[] journalPages;
    public PlayerController playerController;

    bool paused;
    int pageIndex;

    float nextSwitchTime;
    float switchRate = 2f;

    private void Start()
    {
        journalUI.SetActive(false);
        paused = false;
    }

    private void Update()
    {
        if (Time.time >= nextSwitchTime)
        {
            if (Input.GetButtonDown("Button Start") || Input.GetButtonDown("Button Select") || Input.GetKeyDown(KeyCode.Escape))
            {
                if (paused)
                {
                    playerController.enabled = true;
                    journalUI.SetActive(false);
                    //Time.timeScale = 1f;
                    paused = false;
                    // close the journal
                }
                else
                {
                    playerController.enabled = false;
                    journalUI.SetActive(true);
                    //Time.timeScale = 0f;
                    paused = true;

                    journalPages[pageIndex].SetActive(true);
                }
            }

            if (paused)
            {
                if (Input.GetAxis("Horizontal") > 0)
                {
                    if (pageIndex >= journalPages.Length)
                    {
                        pageIndex = journalPages.Length - 1;
                    }
                    else
                    {
                        pageIndex++;
                    }
                   

                    for (int i = 0; i < journalPages.Length; i++)
                    {
                        journalPages[i].SetActive(false);
                    }                                  

                    journalPages[pageIndex].SetActive(true);
                    nextSwitchTime = Time.time + 1f / switchRate;
                }
                if (Input.GetAxis("Horizontal") < 0)
                {
                    if (pageIndex == 0)
                    {
                        pageIndex = 0;
                    }
                    else
                    {
                        pageIndex--;
                    }
                    
                    for (int i = 0; i < journalPages.Length; i++)
                    {
                        journalPages[i].SetActive(false);
                    }

                    journalPages[pageIndex].SetActive(true);
                    nextSwitchTime = Time.time + 1f / switchRate;

                }

            }

        }
    }


}
