using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCall : MonoBehaviour
{
    public FindPlayer findplayer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            findplayer.requestMade = true;
        }
        else
        {
            findplayer.requestMade = false;
        }
    }


}
