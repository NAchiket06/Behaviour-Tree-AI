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
    public GameObject FrontDoor;
    NavMeshAgent agent;
    
    public enum ActionState { IDLE, WORKING };
    ActionState state = ActionState.IDLE;

    Node.Status treeStatus = Node.Status.RUNNING;

    public float distanceToTarget;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        tree = new();
        Sequence steal = new("Steal Something");
        Leaf goToBackDoor = new("Go To Back Door", GoToBackDoor);
        Leaf goToDiamond = new("Go To Diamond",GoToDiamond);
        Leaf goToFrontDoor = new("Go To Front Door", GoToFrontDoor);
        Leaf goToVan = new("Go to Van",GoToVan);

        Selector OpenDoor = new("Open Door");
        OpenDoor.AddChild(goToFrontDoor);
        OpenDoor.AddChild(goToBackDoor);

        steal.AddChild(OpenDoor);
        steal.AddChild(goToDiamond);
        //steal.AddChild(OpenDoor);
        steal.AddChild(goToVan);
        tree.AddChild(steal);

    }

    public Node.Status GoToBackDoor()
    {
        return GoToLocation(BackDoor.transform.position);
    }
    public Node.Status GoToFrontDoor()
    {
        return GoToLocation(FrontDoor.transform.position);
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
