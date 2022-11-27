using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class RobberBehaviour : BTAgent
{
    public GameObject diamond;
    public GameObject van;
    public GameObject BackDoor;
    public GameObject FrontDoor;

    [Range(0, 1000)] public int money = 800;

    new void Start()
    {
        base.Start();
        Sequence steal = new("Steal Something");
        Leaf hasGotMoney = new("Has Hot Money", HasMoney);
        Leaf goToBackDoor = new("Go To Back Door", GoToBackDoor);
        Leaf goToDiamond = new("Go To Diamond",GoToDiamond);
        Leaf goToFrontDoor = new("Go To Front Door", GoToFrontDoor);
        Leaf goToVan = new("Go to Van",GoToVan);

        Inverter invertMoney = new("Invert Money");
        invertMoney.AddChild(hasGotMoney);

        Selector OpenDoor = new("Open Door");
        OpenDoor.AddChild(goToFrontDoor);
        OpenDoor.AddChild(goToBackDoor);

        steal.AddChild(invertMoney);
        steal.AddChild(OpenDoor);
        steal.AddChild(goToDiamond);
        steal.AddChild(goToVan);
        tree.AddChild(steal);
    }

    public Node.Status HasMoney()
    {
        return money < 500 ? Node.Status.FAILURE : Node.Status.SUCCESS;
    }

    public Node.Status GoToBackDoor()
    {
        return GoToDoor(BackDoor);
    }
    public Node.Status GoToFrontDoor()
    {
        return GoToDoor(FrontDoor);
    }
    public Node.Status GoToDiamond()
    {
        Node.Status s = GoToLocation(diamond.transform.position);
        if(s==Node.Status.SUCCESS)
        {
            diamond.transform.SetParent(transform);

            return Node.Status.SUCCESS;
        }

        return s;
    }

    public Node.Status GoToVan()
    {
        Node.Status s = GoToLocation(van.transform.position);
        if(s== Node.Status.SUCCESS)
        {
            diamond.transform.SetParent(van.transform);
            money += 200;
            return Node.Status.SUCCESS;
        }

        return s;

    }

    public Node.Status GoToDoor(GameObject door)
    {
        Node.Status s = GoToLocation(door.transform.position);

        if(s == Node.Status.SUCCESS)
        {
            if(!door.GetComponent<Lock>().isLocked)
            {
                door.SetActive(false);
                return Node.Status.SUCCESS;
            }
            return Node.Status.FAILURE;
        }

        return s;
    }

    private void Update()
    {
        if (treeStatus != Node.Status.SUCCESS)
            treeStatus = tree.Process();
    }
}
