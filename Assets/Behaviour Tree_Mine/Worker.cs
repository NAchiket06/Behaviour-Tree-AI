using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : BTAgent
{
    public GameObject office;

    public override void Start()
    {
        base.Start();
        Leaf goToPatreon = new("Go To Patreon", GoToPatreon);
        Leaf goToOffice = new("Go To Office", GoToOffice);
        Leaf allocatePatreon = new("Allocate Patreo", AllocatePatreon);

        Sequence getPatreon = new("Get Patreon");
        getPatreon.AddChild(allocatePatreon);
        getPatreon.AddChild(goToPatreon);

        Selector beWorker = new("Be A Worker");
        beWorker.AddChild(getPatreon);
        beWorker.AddChild(goToOffice);

        tree.AddChild(beWorker);

    }

    GameObject Patreon;

    public Node.Status PatreonWaiting()
    {
        if (Patreon.GetComponent<PatreonBehaviour>().isWaiting) return Node.Status.SUCCESS;
        return Node.Status.FAILURE;
    }
    public Node.Status AllocatePatreon()
    {
        if (Blackboard.Instance.patreons.Count == 0) return Node.Status.FAILURE;

        Patreon = Blackboard.Instance.patreons.Pop();
        if (Patreon == null) return Node.Status.FAILURE;
        return Node.Status.SUCCESS;
    }
    public Node.Status GoToPatreon()
    {
        if (Patreon == null) return Node.Status.FAILURE;

        Node.Status s = GoToLocation(Patreon.transform.position);
        if(s == Node.Status.SUCCESS)
        {
            Patreon.GetComponent<PatreonBehaviour>().ticket = true;
            Patreon = null;
        }

        return s;
    }

    public Node.Status GoToOffice()
    {
        Node.Status s = GoToLocation(office.transform.position);
        Patreon = null;
        return s;
    }
}
