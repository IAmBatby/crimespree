using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{
    public List<Transform> guardPoints = new List<Transform>();
    public NavMeshAgent navMeshAgent;
    public int pointCount;

    void Start()
    {
        UpdatePath();
    }
    void Update()
    {
        if(navMeshAgent.hasPath)
        {
            if (navMeshAgent.remainingDistance < 1)
            {
                UpdatePath();
            }
            //Vector3 rotatePos = Vector3.RotateTowards(transform.forward, guardPoints[pointCount].position, 1f, 0f);
            //transform.Rotate(rotatePos);
        }

    }

    void UpdatePath()
    {
        if(pointCount == guardPoints.Count)
        {
            pointCount = 0;
        }
        navMeshAgent.SetDestination(guardPoints[pointCount].position);
        Debug.Log(guardPoints[pointCount].position);
        pointCount++;
    }

}
