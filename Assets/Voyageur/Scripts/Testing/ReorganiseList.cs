using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReorganiseList : MonoBehaviour
{

    public Dictionary<int,string> tools;
    public bool axeAdded, rodAddded;


    void Awake()
    {
        tools = new Dictionary<int, string>();

        tools.Add(4,"camera");
        tools.Add(3,"torch");
        tools.Add(0,"bare");


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            tools.Add(1,"axe");
            Reorganise(tools);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            tools.Add(2, "rod");
        }

    }


    Dictionary<int, string> Reorganise(Dictionary<int, string> listToReorganise)
    {              



        return listToReorganise;
    }

}
