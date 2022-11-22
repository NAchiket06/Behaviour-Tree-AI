using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class RobberBehaviour : MonoBehaviour
{
    BehaviourTree tree;
    public GameObject diamond;
    public GameObject van;
    NavMeshAgent agent;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        tree = new();
        Node steal = new("Steal Something");
        Leaf goToDiamond = new Leaf("Go To Diamond",GoToDiamond);
        Leaf goToVan = new("Go to Van",GoToVan);

        steal.AddChild(goToDiamond);
        steal.AddChild(goToVan);
        tree.AddChild(steal);

        tree.PrintTree();
        tree.Process();
    }

    public Node.Status GoToDiamond()
    {
        agent.SetDestination(diamond.transform.position);
        return Node.Status.SUCCESS; 
    }

    public Node.Status GoToVan()
    {
        agent.SetDestination(van.transform.position);
        return Node.Status.SUCCESS;
    }

}
