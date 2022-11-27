using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BTAgent : MonoBehaviour
{
    public BehaviourTree tree;
    public NavMeshAgent agent;

    public enum ActionState { IDLE, WORKING };
    public ActionState state = ActionState.IDLE;
    public Node.Status treeStatus = Node.Status.RUNNING;
    public float distanceToTarget;

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        tree = new();
    }

    public Node.Status GoToLocation(Vector3 destination)
    {
        distanceToTarget = Vector3.Distance(destination, transform.localPosition);
        if(state == ActionState.IDLE)
        {
            //print("State is idle");
            agent.SetDestination(destination);
            state = ActionState.WORKING;
        }
        else if(distanceToTarget >=3)
        {
            state = ActionState.WORKING;
            return Node.Status.FAILURE;
        }
        else if(distanceToTarget < 3)
        {
            state = ActionState.IDLE;
            return Node.Status.SUCCESS; 
        }
        return Node.Status.RUNNING;
    }

    private void Update()
    {
        if(treeStatus != Node.Status.SUCCESS)
            treeStatus = tree.Process();
    }
}
