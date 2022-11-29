using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PSelector : Node
{
    Node[] nodeArray;
    bool ordered = false;
    public PSelector(string n)
    {
        name = n;
    }

    void OrderNodes()
    {
        nodeArray = children.ToArray();
        Sort(nodeArray, 0, children.Count - 1);
        children = new List<Node>(nodeArray);

        StringBuilder s = new();

        foreach(Node node in children)
        {
             s.Append(node.name + " ");
        }

        Debug.Log(s.ToString());
    }

    public override Status Process()
    {
        if(!ordered)
        {
            OrderNodes();
            ordered = true;
        }
        Status childStatus = children[currentChild].Process();
        if (childStatus == Status.RUNNING) return Status.RUNNING;
        if (childStatus == Status.SUCCESS)
        {
            children[currentChild].SortOrder = 1;
            ordered = false;
            currentChild = 0;
            return Status.SUCCESS;
        }
        else
        {
            children[currentChild].SortOrder = 10;
        }

        currentChild++;
        if (currentChild >= children.Count)
        {
            ordered = false;
            currentChild = 0;
            return Status.FAILURE;
        }

        return Status.RUNNING;
    }

    //QuickSort
    //Adapted from: https://exceptionnotfound.net/quick-sort-csharp-the-sorting-algorithm-family-reunion/
    int Partition(Node[] array, int low,
                                int high)
    {
        Node pivot = array[high];

        int lowIndex = (low - 1);

        //2. Reorder the collection.
        for (int j = low; j < high; j++)
        {
            if (array[j].SortOrder <= pivot.SortOrder)
            {
                lowIndex++;

                Node temp = array[lowIndex];
                array[lowIndex] = array[j];
                array[j] = temp;
            }
        }

        Node temp1 = array[lowIndex + 1];
        array[lowIndex + 1] = array[high];
        array[high] = temp1;

        return lowIndex + 1;
    }

    void Sort(Node[] array, int low, int high)
    {
        if (low < high)
        {
            int partitionIndex = Partition(array, low, high);
            Sort(array, low, partitionIndex - 1);
            Sort(array, partitionIndex + 1, high);
        }
    }
}
