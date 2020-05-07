using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Custom/Fish")]
public class Fish : ScriptableObject
{
	public int id;
	public Sprite image;
	public string fishName;
	public string fishSize;
	[TextArea]
	public string fishDesc;

	public int timesCaught;

	public void IncreaseTimesCaught()
	{
		Debug.Log("Increase");
		timesCaught++;
	}



}
