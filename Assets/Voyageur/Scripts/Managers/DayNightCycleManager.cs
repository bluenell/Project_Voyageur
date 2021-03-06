﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class DayNightCycleManager : MonoBehaviour
{
	[Range(0, 24)]// Using a range puts a slider in the inspector, making adjustments really easy and visual
	public int colourArrayIndex; //stores the current index of the colour array (essentially is the time of day)

	Camera myCamera;

	UnityEngine.Experimental.Rendering.Universal.Light2D globalLight; //The global light for the scene
	public Color[] colours; //the array of colours for the cycle. Using the Color data type allows for easy and visual colour customisation

	//public float transitionSpeed = 50f;

	public float timeScale = 0.8f; //how quickly it takes to transition from one hour to the next. A value of 1 is one change per second.
	float lerpValue;

	//public Text timeText;

	



    // Start is called before the first frame update
 [ExecuteAlways]   
 void Start()
    {
		myCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		globalLight = GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
		colourArrayIndex = 6;
		globalLight.color = colours[colourArrayIndex];
		
    }


	// Update is called once per frame
	void FixedUpdate()
	{
		ChangeColour();
	}
	
	[ExecuteAlways]
	void ChangeColour()
	{
		//Debug.Log("Changing");
		//globalLight.color = Color.Lerp(colours[colourArrayIndex], colours[colourArrayIndex + 1], transitionSpeed * Time.deltaTime);

		if (lerpValue >= 1f)
		{
			colourArrayIndex++;
			globalLight.color = colours[colourArrayIndex % colours.Length];
			lerpValue = 0f;
		}
		else
		{
			
			lerpValue += timeScale * Time.deltaTime;
			int firstColor = colourArrayIndex % colours.Length;
			int secondColor = (colourArrayIndex + 1) % colours.Length;
			globalLight.color = Color.Lerp(colours[firstColor], colours[secondColor], lerpValue);
			myCamera.backgroundColor = Color.Lerp(colours[firstColor], colours[secondColor], lerpValue);

		}

		if (colourArrayIndex >= colours.Length-1)
		{
			colourArrayIndex = 0;
		}
	}
	
}
