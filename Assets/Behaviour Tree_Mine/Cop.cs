using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cop : BTAgent
{
    [SerializeField] GameObject[] patrolPoints;

    public override void Start()
    {
        base.Start();
        Sequence selectPatrolPoint = new("Select Patrol Point");
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            Leaf pp = new("Go to " + patrolPoints[i].name, i, GoToPoint); ;
            selectPatrolPoint.AddChild(pp);
        }

        tree.AddChild(selectPatrolPoint);
    }


    public Node.Status GoToPoint(int i)
    {
        Node.Status s = GoToLocation(patrolPoints[i].transform.position);
        return s;
    }
}
