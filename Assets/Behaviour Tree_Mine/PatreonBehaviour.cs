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

    public bool ticket = false;

    public override void Start()
    {
        base.Start();
        RSelector selectObejctToView = new("Select Art To View");
        for (int i = 0; i < art.Length; i++)
        {
            Leaf goToArt = new("Go to " + art[i].name, i, GoToArt);
            selectObejctToView.AddChild(goToArt);
        }

        Leaf goToFrontDoor = new("Go To Front Door", GoToFrontDoor);
        Leaf goToHome = new("Go To Home", GoToHome);
        Leaf isBored = new("Is Bored?", IsBored);
        Leaf isOpen = new("Is Open?", IsOpen);

        Sequence viewArt = new("View Art");
        viewArt.AddChild(isOpen);
        viewArt.AddChild(isBored);
        viewArt.AddChild(goToFrontDoor);

        Leaf noTicket = new("Wait for Ticket", NoTicket);
        Leaf isWaiting = new("Waiting for Worker", IsWaiting);

        BehaviourTree waitForTicket = new();
        waitForTicket.AddChild(noTicket);

        Loop getTicket = new("Ticket", waitForTicket);
        getTicket.AddChild(isWaiting);

        viewArt.AddChild(getTicket);

        BehaviourTree whileBored = new();
        whileBored.AddChild(isBored);

        Loop LookAtPaintings = new("Look", whileBored);
        LookAtPaintings.AddChild(selectObejctToView);

        viewArt.AddChild(LookAtPaintings);
        viewArt.AddChild(goToHome);

        BehaviourTree galleryOpenCondition = new();
        galleryOpenCondition.AddChild(isOpen);
        DepSequence bePatreon = new("Be an Art Patreon",galleryOpenCondition, agent);
        bePatreon.AddChild(viewArt);


        Selector viewArtWithFallback = new("View Art with fallback");
        viewArtWithFallback.AddChild(bePatreon);
        viewArtWithFallback.AddChild(goToHome);

        tree.AddChild(viewArtWithFallback);

        StartCoroutine(IncreaseBoredom());
    }

    IEnumerator IncreaseBoredom()
    {
        while(true)
        {
            boredom = Mathf.Clamp(boredom + 20, 0, 1000);
            yield return new WaitForSeconds(Random.Range(5f,10f));
        }
    }

    public Node.Status GoToArt(int i)
    {
        if (!art[i].activeInHierarchy) return Node.Status.FAILURE;
        Node.Status s = GoToLocation(art[i].transform.position);
        if(s == Node.Status.SUCCESS)
        {
            boredom = Mathf.Clamp(boredom - 40, 0, 1000);
        }
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
        ticket = false;
        return s;
    }


    public Node.Status IsBored()
    {
        if (boredom <= 100) return Node.Status.FAILURE;
        else return Node.Status.SUCCESS;
    }

    public Node.Status NoTicket()
    {
        if (ticket || IsOpen() == Node.Status.FAILURE) return Node.Status.FAILURE;
        return Node.Status.SUCCESS;
    }

    public Node.Status IsWaiting()
    {
        if (Blackboard.Instance.RegisterPatreon(this.gameObject))
        {
            return Node.Status.SUCCESS;
        }
        else return Node.Status.FAILURE;
    }


}
