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

    GameObject Patreon;
    public Node.Status GoToPatreon()
    {
        if (Blackboard.Instance.patreons.Count == 0) return Node.Status.FAILURE;

        Patreon = Blackboard.Instance.patreons.Pop();
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
        return s;
    }
}
