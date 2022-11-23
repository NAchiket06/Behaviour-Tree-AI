using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    public Sequence(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        Status childstatus = children[currentChild].Process();
        if(childstatus == Status.RUNNING) return Status.RUNNING;
        else if(childstatus == Status.FAILURE)
        {
            //Debug.Log("Child status returned failure");
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
