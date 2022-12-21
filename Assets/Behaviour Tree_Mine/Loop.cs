using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loop : Node
{
    BehaviourTree dependency;

    public Loop(string n, BehaviourTree d)
    {
        name = n;
        dependency = d;
    }

    public override Status Process()
    {
        if(dependency.Process() == Status.FAILURE)
        {
            return Status.SUCCESS;
        }
        Status childstatus = children[currentChild].Process();
        if (childstatus == Status.RUNNING) return Status.RUNNING;
        else if (childstatus == Status.FAILURE)
        {
            //currentChild = 0;
            //foreach (Node n in children)
            //    n.Reset();
            return childstatus;
        }

        currentChild++;
        if (currentChild >= children.Count)
        {
            currentChild = 0;
        }

        return Status.RUNNING;
    }
}
