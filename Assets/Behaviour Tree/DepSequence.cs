using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class DepSequence : Node
{
    BehaviourTree dependency;
    NavMeshAgent agent;

    public DepSequence(string n,BehaviourTree d, NavMeshAgent a)
    {
        name = n;
        dependency = d;
        agent = a;
    }

    public override Status Process()
    {
        if(dependency.Process() == Status.FAILURE)
        {
            agent.ResetPath();
            foreach(Node n in children)
            {
                n.Reset();
            }
            return Status.FAILURE;
        }

        Status childstatus = children[currentChild].Process();
        if(childstatus == Status.RUNNING) return Status.RUNNING;
        else if(childstatus == Status.FAILURE)
        {
            return childstatus;
        }

        currentChild++; 
        if (currentChild >= children.Count)
        {
            currentChild = 0;
            return Status.SUCCESS;
        }

        return Status.RUNNING;
    }
}
