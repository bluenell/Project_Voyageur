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
		SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    public void MainMenu()
    {
        StartCoroutine(Delay(0, 0));
    }

    IEnumerator Delay(int sceneIndex, int time)
    {
        yield return new WaitForSeconds(time);

        if (music != null)
        {
            Destroy(music);
        }

        SceneManager.LoadScene(sceneIndex);

    }
}
