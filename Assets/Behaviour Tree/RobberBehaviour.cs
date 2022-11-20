using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobberBehaviour : MonoBehaviour
{
    BehaviourTree tree;

    private void Start()
    {
        tree = new();
        Node steal = new("Steal Something");
        Node GoToDiamond = new("Go To Diamond");
        Node goToVan = new("Go to Van");

        steal.AddChild(GoToDiamond);
        steal.AddChild(goToVan);
        tree.AddChild(steal);

        tree.PrintTree();
    }
}
