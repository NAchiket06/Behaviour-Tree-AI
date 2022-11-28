using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class RobberBehaviour : BTAgent
{
    public GameObject diamond;
    public GameObject painting;
    public GameObject van;
    public GameObject BackDoor;
    public GameObject FrontDoor;

    GameObject pickup;

    [Range(0, 1000)] public int money = 800;

    new void Start()
    {
        base.Start();
        Sequence steal = new("Steal Something");
        Leaf hasGotMoney = new("Has Hot Money", HasMoney);
        Leaf goToBackDoor = new("Go To Back Door", GoToBackDoor);
        Leaf goToDiamond = new("Go To Diamond",GoToDiamond,2);
        Leaf goToFrontDoor = new("Go To Front Door", GoToFrontDoor);
        Leaf goToVan = new("Go to Van",GoToVan);
        Leaf goToPainting = new("Go To Painting", GoToPainting,1);

        Inverter invertMoney = new("Invert Money");
        invertMoney.AddChild(hasGotMoney);

        Selector OpenDoor = new("Open Door");
        OpenDoor.AddChild(goToFrontDoor);
        OpenDoor.AddChild(goToBackDoor);

        PSelector selectObjectToSteal = new("Select Object To Steal");
        selectObjectToSteal.AddChild(goToDiamond);
        selectObjectToSteal.AddChild(goToPainting);

        steal.AddChild(invertMoney);
        steal.AddChild(OpenDoor);
        steal.AddChild(selectObjectToSteal);
        steal.AddChild(goToVan);
        steal.AddChild(selectObjectToSteal);
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
        if (!diamond.activeInHierarchy) return Node.Status.FAILURE;
        Node.Status s = GoToLocation(diamond.transform.position);
        if(s==Node.Status.SUCCESS)
        {
            diamond.transform.SetParent(transform);
            pickup = diamond;
            return Node.Status.SUCCESS;
        }

        return s;
    }
    public Node.Status GoToPainting()
    {
        if (!painting.activeInHierarchy) return Node.Status.FAILURE;
        Node.Status s = GoToLocation(painting.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            painting.transform.SetParent(transform);
            pickup = painting;
            return Node.Status.SUCCESS;
        }

        return s;
    }
    public Node.Status GoToVan()
    {
        Node.Status s = GoToLocation(van.transform.position);
        if(s== Node.Status.SUCCESS)
        {
            pickup.transform.SetParent(van.transform);
            pickup.SetActive(false);
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
                door.GetComponent<NavMeshObstacle>().enabled = false;
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
