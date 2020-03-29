using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPlayer : MonoBehaviour
{
    public bool requestMade;
	public GameObject player;


    Vector3[] path;
    int targetIndex;

    private void Update()
    {
        if (requestMade)
        {
			PathRequestManager.RequestPath(transform.position, player.transform.position, OnPathFound);
			requestMade = false;
        }
    }


	public void OnPathFound(Vector3[] newPath, bool pathSucessfull)
	{
		if (pathSucessfull)
		{
			path = newPath;
			targetIndex = 0;
			StopCoroutine(FollowPath());
			StartCoroutine(FollowPath());
		}
	}

	IEnumerator FollowPath()
	{
		Vector3 currentWaypoint = path[0];

		while (true)
		{
			if (transform.position == currentWaypoint)
			{
				Debug.Log("At waypoint: " + path[targetIndex]);
				targetIndex++;
				if (targetIndex >= path.Length)
				{
					PathRequestManager.ClearRequests();
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}

			transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, 5* Time.deltaTime);

		}
	}
	public void OnDrawGizmos()
	{
		if (path != null)
		{
			for (int i = targetIndex; i < path.Length; i++)
			{
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], (Vector3.one) / 2);

				if (i == targetIndex)
				{
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else
				{
					Gizmos.DrawLine(path[i - 1], path[i]);
				}
			}
		}
	}

}
