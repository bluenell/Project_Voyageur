using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerMananger : MonoBehaviour
{

	PlayerController playerController;



    // Start is called before the first frame update
    void Start()
    {
		playerController = GetComponent<PlayerController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Canoe")
		{
			playerController.canPickUp = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Canoe")
		{
			playerController.canPickUp = false;
		}
	}

}
