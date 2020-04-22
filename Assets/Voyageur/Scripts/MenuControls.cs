using System.Collections;
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
        StartCoroutine(Delay());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        Destroy(music);
        SceneManager.LoadScene(1);

    }
}
