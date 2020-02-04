using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

	[Header("Player Stats")]
	public float stamina;
	public float speed;

	[Header("Player Inventory")]
	public bool hasWood;
	public bool hasStick;
	public bool hasFish;
	public bool hasCanoe;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	//Controls the player movment when not holding the canoe
	void Move()
	{

	}
	//Detects input and what interacted with
	void Interact()
	{

	}
	//Handles the picking up/placing down the canoe
	void HandleCanoe()
	{

	}

}
