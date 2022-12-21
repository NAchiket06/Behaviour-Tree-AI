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
    public GameObject Cop;
    public GameObject[] art;
    GameObject pickup;

    [Range(0, 1000)] public int money = 800;

    Leaf goToBackDoor;
    Leaf goToFrontDoor;

    new void Start()
    {
        base.Start();

        Leaf hasGotMoney = new("Has Hot Money", HasMoney);

        goToBackDoor = new("Go To Back Door", GoToBackDoor);
        Leaf goToDiamond = new("Go To Diamond",GoToDiamond,2);

        goToFrontDoor = new("Go To Front Door", GoToFrontDoor);
        Leaf goToVan = new("Go to Van",GoToVan);

        Leaf goToPainting = new("Go To Painting", GoToPainting,1);

        RSelector selectObjectToSteal = new("Select Object To Steal");
        for (int i = 0; i < art.Length; i++)
        {
            Leaf goToArt = new("Go to " + art[i].name, i, GoToArt);
            selectObjectToSteal.AddChild(goToArt);
        }
        selectObjectToSteal.AddChild(goToDiamond);


        Leaf canSeeCop = new("Can See Cop", CanSeeCop);
        Leaf flee = new("Flee From Cop", FleeFromCop);

        Inverter invertMoney = new("Invert Money");
        invertMoney.AddChild(hasGotMoney);

        Inverter cantSeeCop = new("Cant See Cop");
        cantSeeCop.AddChild(canSeeCop);

        Selector OpenDoor = new("Open Door");
        OpenDoor.AddChild(goToFrontDoor);
        OpenDoor.AddChild(goToBackDoor);

        Sequence runAway = new("Run Away");
        runAway.AddChild(canSeeCop);
        runAway.AddChild(flee);


        Selector s1 = new("S1");
        s1.AddChild(invertMoney);
        Sequence s2 = new("S2");
        s2.AddChild(cantSeeCop);
        s2.AddChild(OpenDoor);
        Sequence s3 = new("S3");
        s3.AddChild(cantSeeCop);
        s3.AddChild(selectObjectToSteal);
        Sequence s4 = new("S4");
        s4.AddChild(cantSeeCop);
        s4.AddChild(goToVan);


        /*
        BehaviourTree seeCop = new("See Cop");
        seeCop.AddChild(cantSeeCop);

        DepSequence steal = new("Steal Something", seeCop, agent);

        steal.AddChild(s1);
        steal.AddChild(s2);
        steal.AddChild(s3);
        steal.AddChild(s4);

        //steal.AddChild(invertMoney);
        //steal.AddChild(OpenDoor);

        //steal.AddChild(cantSeeCop);

        //steal.AddChild(selectObjectToSteal);
        //steal.AddChild(goToVan);
        //steal.AddChild(selectObjectToSteal);
        //steal.AddChild(goToVan);


        Selector bThief = new("Be A Thief");
        bThief.AddChild(steal);
        bThief.AddChild(runAway);

        tree.AddChild(bThief);
        */

        BehaviourTree stealConditions = new();
        Sequence conditions = new("Stealing Conditions");

        conditions.AddChild(cantSeeCop);
        conditions.AddChild(invertMoney);
        stealConditions.AddChild(conditions);

        DepSequence steal = new("Steal Something", stealConditions, agent);
        //steal.AddChild(invertMoney);

        steal.AddChild(OpenDoor);
        steal.AddChild(selectObjectToSteal);
        steal.AddChild(goToVan);

        Selector stealWithFallback = new("Steal With Fallback");
        stealWithFallback.AddChild(steal);
        stealWithFallback.AddChild(goToVan);

        Selector bThief = new("Be A Thief");
        bThief.AddChild(stealWithFallback);
        bThief.AddChild(runAway);

        tree.AddChild(bThief);

        tree.PrintTree();

        StartCoroutine(DecreaseMoney());
    }

    IEnumerator DecreaseMoney()
    {
        while(true)
        {
            money = Mathf.Clamp(money - Random.Range(10, 100), 0, 1000);
            yield return new WaitForSeconds(Random.Range(3f, 10f));
        }
    }
    public Node.Status CanSeeCop()
    {
        return CanSee(Cop.transform.position, "Cop", 10, 10);
    }

    public Node.Status FleeFromCop()
    {
        return Flee(Cop.transform.position, 25);
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
    public Node.Status GoToVan()
    {
        Node.Status s = GoToLocation(van.transform.position);
        if(s== Node.Status.SUCCESS && pickup != null)
        {
            pickup = null;
            pickup.transform.SetParent(van.transform);
            pickup.SetActive(false);
            money += 200;
            return Node.Status.SUCCESS;
        }

        return s;

    }


    private void Update()
    {
        if (treeStatus != Node.Status.SUCCESS)
            treeStatus = tree.Process();
    }
}
