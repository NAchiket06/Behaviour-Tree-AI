using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : Node
{
    public BehaviourTree()
    {
        name = "Tree";
    }

    public BehaviourTree(string n)
    {
        name = n;
    }

    struct NodeLevel
    {
        public int level;
        public Node node;
    }

    public void PrintTree()
    {
        string treePrintout = "";
        Stack <NodeLevel> nodeStack = new();
        Node currentNode = this;
        nodeStack.Push(new NodeLevel { level = 0, node = currentNode});
        while (nodeStack.Count > 0)
        {
            NodeLevel NextNode = nodeStack.Pop();
            treePrintout += new string('-',NextNode.level) + NextNode.node.name + "\n";
            foreach (var item in NextNode.node.children)
            {
                nodeStack.Push(new NodeLevel { level = NextNode.level + 1, node = item});
            }
        }
        Debug.Log(treePrintout);
    }

}
