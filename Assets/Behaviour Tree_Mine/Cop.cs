using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cop : BTAgent
{
    [SerializeField] GameObject[] patrolPoints;

    public GameObject robber;

    public override void Start()
    {
        base.Start();
        Sequence selectPatrolPoint = new("Select Patrol Point");
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            Leaf pp = new("Go to " + patrolPoints[i].name, i, GoToPoint); ;
            selectPatrolPoint.AddChild(pp);
        }

        Sequence chaseRobber = new("Chase Robber");
        Leaf canSee = new("Can See Robber", CanSeeRobber);
        Leaf chase = new("Chase Robber", ChaseRobber);

        chaseRobber.AddChild(canSee);
        chaseRobber.AddChild(chase);

        Inverter cantSeeRobber = new("Cant See Robber");
        cantSeeRobber.AddChild(canSee);

        BehaviourTree patrolConditions = new();
        Sequence condition = new("Patrol Conditions");
        condition.AddChild(cantSeeRobber);
        patrolConditions.AddChild(condition);

        DepSequence patrol = new("Patrol", patrolConditions, agent);
        patrol.AddChild(selectPatrolPoint);

        Selector beCop = new("Be a Cop");
        beCop.AddChild(patrol);
        beCop.AddChild(chaseRobber);


        tree.AddChild(beCop);
    }

    public Node.Status GoToPoint(int i)
    {
        Node.Status s = GoToLocation(patrolPoints[i].transform.position);
        return s;
    }

    public Node.Status CanSeeRobber()
    {
        return CanSee(robber.transform.position, "Robber", 5, 60);
    }

    Vector3 rl;
    public Node.Status ChaseRobber()
    {
        float chaseDistance = 10;
        if(state == ActionState.IDLE)
        {
            rl = this.transform.position - ((transform.position - robber.transform.position).normalized * chaseDistance);
        }

        return GoToLocation(rl);
    }
}
