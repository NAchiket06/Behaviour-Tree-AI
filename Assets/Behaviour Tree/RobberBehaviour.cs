using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class RobberBehaviour : MonoBehaviour
{
    BehaviourTree tree;
    public GameObject diamond;
    public GameObject van;
    public GameObject BackDoor;
    NavMeshAgent agent;
    
    public enum ActionState { IDLE, WORKING };
    public ActionState state = ActionState.IDLE;

    public Node.Status treeStatus = Node.Status.RUNNING;

    public float distanceToTarget;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        tree = new();
        Sequence steal = new("Steal Something");
        Leaf goToDoor = new("Go To Door", GoToDoor);
        Leaf goToDiamond = new("Go To Diamond",GoToDiamond);
        Leaf goToVan = new("Go to Van",GoToVan);

        steal.AddChild(goToDoor);
        steal.AddChild(goToDiamond);
        steal.AddChild(goToVan);
        tree.AddChild(steal);

    }

    public Node.Status GoToDoor()
    {
        return GoToLocation(BackDoor.transform.position);
    }
    public Node.Status GoToDiamond()
    {
        return GoToLocation(diamond.transform.position);
    }

    public Node.Status GoToVan()
    {
        return GoToLocation(van.transform.position);
    }

    Node.Status GoToLocation(Vector3 destination)
    {
        distanceToTarget = Vector3.Distance(destination, transform.localPosition);
        if(state == ActionState.IDLE)
        {
            //print("State is idle");
            agent.SetDestination(destination);
            state = ActionState.WORKING;
        }
        else if(distanceToTarget >=3)
        {
            state = ActionState.WORKING;
            //return Node.Status.FAILURE;
        }
        else if(distanceToTarget < 3)
        {
            state = ActionState.IDLE;
            return Node.Status.SUCCESS; 
        }
        //print("returnin Running");
        return Node.Status.RUNNING;
    }

    private void Update()
    {
        if(treeStatus == Node.Status.RUNNING)
            treeStatus = tree.Process();
    }
}
