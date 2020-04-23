﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour
{
    public Animator a;
    public GameObject music;



    public void PlayGame()
    {
        a.SetTrigger("play");
        StartCoroutine(Delay(1, 1));
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    public void MainMenu()
    {
        a.SetTrigger("play");
        StartCoroutine(Delay(0, 1));
    }

    IEnumerator Delay(int sceneIndex, int time)
    {
        yield return new WaitForSeconds(1);

        if (music != null)
        {
            Destroy(music);
        }

        SceneManager.LoadScene(time);

    }
}