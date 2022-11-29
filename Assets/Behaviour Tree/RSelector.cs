using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSelector : Node
{
    bool shuffled = false;

    public RSelector(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        if(!shuffled)
        {
            children.Shuffle();
            shuffled = true;
        }

        Status childStatus = children[currentChild].Process();
        if (childStatus == Status.RUNNING) return Status.RUNNING;
        if (childStatus == Status.SUCCESS)
        {
            shuffled = false;
            currentChild = 0;
            return Status.SUCCESS;
        }

        currentChild++;
        if (currentChild >= children.Count)
        {
            shuffled = false;
            currentChild = 0;
            return Status.FAILURE;
        }

        return Status.RUNNING;
    }
}
