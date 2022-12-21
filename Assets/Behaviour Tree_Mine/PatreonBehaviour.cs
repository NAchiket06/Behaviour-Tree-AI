using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatreonBehaviour : BTAgent
{
    public GameObject[] art;
    public GameObject FrontDoor;
    public GameObject home;

    [Range(0, 1000)] public int boredom = 0;


    public override void Start()
    {
        base.Start();
        RSelector selectObjectToSteal = new("Select Art To View");
        for (int i = 0; i < art.Length; i++)
        {
            Leaf goToArt = new("Go to " + art[i].name, i, GoToArt);
            selectObjectToSteal.AddChild(goToArt);
        }

        Leaf goToFrontDoor = new("Go To Front Door", GoToFrontDoor);
        Leaf goToHome = new("Go To Home", GoToHome);

        Leaf isBored = new("Is Bored ?", IsBored);
    }

    public Node.Status GoToArt(int i)
    {
        if (!art[i].activeInHierarchy) return Node.Status.FAILURE;
        Node.Status s = GoToLocation(art[i].transform.position);
        return s;
    }

    public Node.Status GoToFrontDoor()
    {
        Node.Status s = GoToDoor(FrontDoor);
        return s;
    }

    public Node.Status GoToHome()       
    {
        Node.Status s = GoToLocation(home.transform.position);
        return s;
    }


    public Node.Status IsBored()
    {
        if (boredom <= 100) return Node.Status.FAILURE;
        else return Node.Status.FAILURE;
    }

}
