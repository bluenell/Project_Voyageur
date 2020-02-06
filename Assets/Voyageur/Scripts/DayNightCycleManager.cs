using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNightCycleManager : MonoBehaviour
{
	[Range(0, 24)]
	public int colourArrayIndex;
	public Camera camera;

	UnityEngine.Experimental.Rendering.Universal.Light2D globalLight;
	public Color[] colours;

	//public float transitionSpeed = 50f;

	public float timeScale = 0.8f;
	float lerpValue;
	bool isChanging;

	//public Text timeText;

	

    // Start is called before the first frame update
    void Start()
    {
		
		globalLight = GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
		globalLight.color = colours[colourArrayIndex];
		
    }

    [ExecuteInEditMode]
	// Update is called once per frame
    void FixedUpdate()
    {
		/*
		if (Input.GetKeyDown("space") && !isChanging)
		{
			isChanging = true;
		}
		if (isChanging)
		{
			ChangeColour();
		}
		*/

		ChangeColour();

		//timeText.text = (colourArrayIndex.ToString() + ":00");


		//globalLight.color = Color.Lerp(colours[colourArrayIndex], colours[colourArrayIndex + 1], transitionSpeed * Time.deltaTime);
		//colourArrayIndex++;

		/*
		for (int i = 0; i < colours.Length; i++)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				globalLight.color = Color.Lerp(colours[i], colours[i + 1], transitionSpeed * Time.deltaTime);

			}

			
		}
		*/
	    	
		//globalLight.color = colours[colourArrayIndex];
	}

	
	
	void ChangeColour()
	{
		Debug.Log("Changing");
		//globalLight.color = Color.Lerp(colours[colourArrayIndex], colours[colourArrayIndex + 1], transitionSpeed * Time.deltaTime);

		if (lerpValue >= 1f)
		{
			colourArrayIndex++;
			globalLight.color = colours[colourArrayIndex % colours.Length];
			isChanging = false;
			lerpValue = 0f;
		}
		else
		{
			lerpValue += timeScale * Time.deltaTime;
			int firstColor = colourArrayIndex % colours.Length;
			int secondColor = (colourArrayIndex + 1) % colours.Length;
			globalLight.color = Color.Lerp(colours[firstColor], colours[secondColor], lerpValue);

			camera.backgroundColor = Color.Lerp(colours[firstColor], colours[secondColor], lerpValue);

		}


		if (colourArrayIndex >= colours.Length-1)
		{
			colourArrayIndex = 0;
		}
	}
	
}
