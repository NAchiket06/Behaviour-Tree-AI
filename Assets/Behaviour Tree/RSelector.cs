using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSelector : Node
{
    public RSelector(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        children.Shuffle();

        Status childStatus = children[currentChild].Process();
        if (childStatus == Status.RUNNING) return Status.RUNNING;
        if (childStatus == Status.SUCCESS)
        {
            currentChild = 0;
            return Status.SUCCESS;
        }

        currentChild++;
        if (currentChild >= children.Count)
        {
            currentChild = 0;
            return Status.FAILURE;
        }

        return Status.RUNNING;
    }
}
