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

        Selector beWorker = new("Be A Worker");
        beWorker.AddChild(goToPatreon);
        beWorker.AddChild(goToOffice);

        tree.AddChild(beWorker);

    }

    public Node.Status GoToPatreon()
    {
        if (Blackboard.Instance.patreon == null) return Node.Status.FAILURE;

        Node.Status s = GoToLocation(Blackboard.Instance.patreon.transform.position);
        if(s == Node.Status.SUCCESS)
        {
            Blackboard.Instance.patreon.GetComponent<PatreonBehaviour>().ticket = true;
            Blackboard.Instance.DeRegisterPatreon();
        }

        return s;
    }

    public Node.Status GoToOffice()
    {
        Node.Status s = GoToLocation(office.transform.position);
        return s;
    }
}
