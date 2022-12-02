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

    public GameObject[] art;
    GameObject pickup;

    [Range(0, 1000)] public int money = 800;

    Leaf goToBackDoor;
    Leaf goToFrontDoor;
    new void Start()
    {
        base.Start();
        Sequence steal = new("Steal Something");
        Leaf hasGotMoney = new("Has Hot Money", HasMoney);
        goToBackDoor = new("Go To Back Door", GoToBackDoor);
        Leaf goToDiamond = new("Go To Diamond",GoToDiamond,2);
        goToFrontDoor = new("Go To Front Door", GoToFrontDoor);
        Leaf goToVan = new("Go to Van",GoToVan);
        Leaf goToPainting = new("Go To Painting", GoToPainting,1);

        Leaf goToArt1 = new("Go To Art 1", GoToArt1);
        Leaf goToArt2 = new("Go To Art 1", GoToArt2);
        Leaf goToArt3 = new("Go To Art 1", GoToArt3);
        Leaf goToArt4 = new("Go To Art 1", GoToArt4);
        Leaf goToArt5 = new("Go To Art 1", GoToArt5);


        Inverter invertMoney = new("Invert Money");
        invertMoney.AddChild(hasGotMoney);

        Selector OpenDoor = new("Open Door");
        OpenDoor.AddChild(goToFrontDoor);
        OpenDoor.AddChild(goToBackDoor);

        RSelector selectObjectToSteal = new("Select Object To Steal");

        for (int i = 0; i < art.Length; i++)
        {
            Leaf goToArt = new("Go to " + art[i].name, i, GoToArt);
            selectObjectToSteal.AddChild(goToArt);
        }
        selectObjectToSteal.AddChild(goToDiamond);
        //selectObjectToSteal.AddChild(goToArt1);
        //selectObjectToSteal.AddChild(goToArt2);
        //selectObjectToSteal.AddChild(goToArt3);
        //selectObjectToSteal.AddChild(goToArt4);
        //selectObjectToSteal.AddChild(goToArt5);

        steal.AddChild(invertMoney);
        steal.AddChild(OpenDoor);
        steal.AddChild(selectObjectToSteal);
        steal.AddChild(goToVan);
        steal.AddChild(selectObjectToSteal);
        steal.AddChild(goToVan);
        tree.AddChild(steal);

        tree.PrintTree();
    }

    public Node.Status HasMoney()
    {
        return money < 500 ? Node.Status.FAILURE : Node.Status.SUCCESS;
    }

    public Node.Status GoToBackDoor()
    {
        Node.Status s = GoToDoor(BackDoor);
        if (s == Node.Status.RUNNING)
        {
            goToBackDoor.SortOrder = 10;
        }
        else
        {
            goToBackDoor.SortOrder = 1;
        }

        return s;
    }
    public Node.Status GoToFrontDoor()
    {
        Node.Status s = GoToDoor(FrontDoor);
        if (s == Node.Status.RUNNING) goToFrontDoor.SortOrder = 10;
        else goToFrontDoor.SortOrder = 1;

        return s;
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

    public Node.Status GoToArt(int i)
    {
        if (!art[i].activeInHierarchy) return Node.Status.FAILURE;
        Node.Status s = GoToLocation(art[i].transform.position);
        if (s == Node.Status.SUCCESS)
        {
            art[i].transform.SetParent(transform);
            pickup = art[i];
            return Node.Status.SUCCESS;
        }

        return s;
    }
    public Node.Status GoToArt1()
    {
        if (!art[0].activeInHierarchy) return Node.Status.FAILURE;
        Node.Status s = GoToLocation(art[0].transform.position);
        if (s == Node.Status.SUCCESS)
        {
            art[0].transform.SetParent(transform);
            pickup = art[0];
            return Node.Status.SUCCESS;
        }

        return s;
    }

    public Node.Status GoToArt2()
    {
        if (!art[1].activeInHierarchy) return Node.Status.FAILURE;
        Node.Status s = GoToLocation(art[1].transform.position);
        if (s == Node.Status.SUCCESS)
        {
            art[1].transform.SetParent(transform);
            pickup = art[1];
            return Node.Status.SUCCESS;
        }

        return s;
    }

    public Node.Status GoToArt3()
    {
        if (!art[2].activeInHierarchy) return Node.Status.FAILURE;
        Node.Status s = GoToLocation(art[2].transform.position);
        if (s == Node.Status.SUCCESS)
        {
            art[2].transform.SetParent(transform);
            pickup = art[2];
            return Node.Status.SUCCESS;
        }

        return s;
    }

    public Node.Status GoToArt4()
    {
        if (!art[3].activeInHierarchy) return Node.Status.FAILURE;
        Node.Status s = GoToLocation(art[3].transform.position);
        if (s == Node.Status.SUCCESS)
        {
            art[3].transform.SetParent(transform);
            pickup = art[3];
            return Node.Status.SUCCESS;
        }

        return s;
    }

    public Node.Status GoToArt5()
    {
        if (!art[4].activeInHierarchy) return Node.Status.FAILURE;
        Node.Status s = GoToLocation(art[4].transform.position);
        if (s == Node.Status.SUCCESS)
        {
            art[4].transform.SetParent(transform);
            pickup = art[4];
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
