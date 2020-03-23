using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingTest : MonoBehaviour
{
    MontyStateVariables variables;
    public BoxCollider2D followTargetCollider;
    public PolygonCollider2D pathwayBounds;

    Vector3[] path;
    int targetIndex;

    private void Start()
    {
        variables = GetComponent<MontyStateVariables>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RequestPath();
        }
    }


    void RequestPath()
    {
        Vector3 target = variables.GetRandomPointInBounds(pathwayBounds.bounds);

        PathRequestManager.RequestPath(transform.position, target, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSucessfull)
    {
        if (pathSucessfull)
        {
            path = newPath;
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
                variables.desintationReached = true;
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, variables.walkSpeed * Time.deltaTime);
            yield return null;
        }
    }
    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

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
