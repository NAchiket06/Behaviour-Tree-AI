using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : Node
{
    public Inverter(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        Status childstatus = children[0].Process();
        if(childstatus == Status.RUNNING) return Status.RUNNING;
        else if(childstatus == Status.FAILURE)
        {
            return Status.SUCCESS;
        }

        return Status.FAILURE;
    }
}
