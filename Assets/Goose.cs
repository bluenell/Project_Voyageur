using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goose : MonoBehaviour
{
    public GameObject[] geese;

   
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Canoe")
        {
            for(int i =0; i < geese.Length; i++)
            {   
                geese[i].SetActive(true);
            }   
        }
    }

}
