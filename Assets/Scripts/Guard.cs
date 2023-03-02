using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode]
public class Guard : MonoBehaviour
{
    public bool hasPoints;
    [ShowIf("hasPoints")]
    public List<Transform> guardPoints = new List<Transform>();
    [FoldoutGroup("References")] public NavMeshAgent navMeshAgent;
    [FoldoutGroup("References")] public int pointCount;

    void Start()
    {
        if (hasPoints)
            UpdatePath();
    }
    void Update()
    {
        if(hasPoints)
            if (navMeshAgent.hasPath)
                if (navMeshAgent.remainingDistance < 1)
                    UpdatePath();

    }

    void UpdatePath()
    {
        if (guardPoints.Count != 0)
        {
            if (pointCount == guardPoints.Count)
                pointCount = 0;
            navMeshAgent.SetDestination(guardPoints[pointCount].position);
            pointCount++;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (hasPoints)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < guardPoints.Count - 1; i++)
            {
                Gizmos.DrawLine(guardPoints[i].position, guardPoints[i + 1].position);
                Gizmos.DrawCube(guardPoints[i].position, new Vector3(0.25f, 0.5f, 0.25f));
            }
            Gizmos.DrawCube(guardPoints[guardPoints.Count - 1].position, new Vector3(0.25f, 0.5f, 0.25f));
        }
    }

    /*private void OnEnable()
    {
        int i = 1;
        foreach (Transform point in guardPoints)
        {
            point.SetParent(transform);
            point.name = "Point " + i.ToString();
            i++;
        }
    }*/

}
