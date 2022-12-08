using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BTAgent : MonoBehaviour
{
    public BehaviourTree tree;
    public NavMeshAgent agent;

    public enum ActionState { IDLE, WORKING };
    public ActionState state = ActionState.IDLE;
    public Node.Status treeStatus = Node.Status.RUNNING;
    public float distanceToTarget;

    WaitForSeconds waitForSeconds;
    Vector3 rememberedLocation;
    
    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        tree = new();
        waitForSeconds = new(Random.Range(0.1f, 1f));
        StartCoroutine(Behave());
    }

    public Node.Status CanSee(Vector3 target, string tag, float distance, float maxAngle)
    {
        Vector3 directionToTarget = target - transform.position;
        float angle = Vector3.Angle(directionToTarget, this.transform.forward);
        if(angle <= maxAngle || directionToTarget.magnitude <= distance)
        {
            RaycastHit hitInfo;
            if(Physics.Raycast(transform.position, directionToTarget, out hitInfo))
            {
                if(hitInfo.collider.gameObject.CompareTag(tag))
                {
                    Debug.Log("Can See Cop");
                    return Node.Status.SUCCESS;
                }
            }
        }
        Debug.Log("Cannot See Cop");
        return Node.Status.FAILURE; 
    }

    public Node.Status Flee(Vector3 location,float distance)
    {
        if(state == ActionState.IDLE)
        {
            rememberedLocation = transform.position + (transform.position - location).normalized * distance;
        }

        return GoToLocation(rememberedLocation);
    }

    public Node.Status GoToLocation(Vector3 destination)
    {
        distanceToTarget = Vector3.Distance(destination, transform.localPosition);
        if(state == ActionState.IDLE)
        {
            agent.SetDestination(destination);
            state = ActionState.WORKING;
        }
        else if(distanceToTarget >=3)
        {
            state = ActionState.WORKING;
            return Node.Status.FAILURE;
        }
        else if(distanceToTarget < 3)
        {
            state = ActionState.IDLE;
            return Node.Status.SUCCESS; 
        }
        return Node.Status.RUNNING;
    }

    public  IEnumerator Behave()
    {
        while(true)
        {
            treeStatus = tree.Process();
            yield return waitForSeconds;
        }
    }
}
