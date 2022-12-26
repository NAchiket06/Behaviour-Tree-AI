using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Blackboard : MonoBehaviour
{

    public float timeOfDay;
    [SerializeField] private TextMeshProUGUI clock;

    public Stack<GameObject> patreons = new();

    public int OpenTime = 9;
    public int CloseTime = 21;  

    static Blackboard instance;
    public static Blackboard Instance
    {
        get 
        { 
            if (!instance)
            {
                Blackboard[] blackboards = GameObject.FindObjectsOfType<Blackboard>();
                if (blackboards != null)
                {
                    if (blackboards.Length == 1)
                    {
                        instance = blackboards[0];
                        return instance;
                    }
                }
                GameObject go = new("Blackboard",typeof(Blackboard));
                instance = go.GetComponent<Blackboard>();
                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }

        set
        {
            instance = value as Blackboard;
        }
    }


    private void Start()
    {
        StartCoroutine(UpdateClock());
    }

    IEnumerator UpdateClock()
    {
        while(true)
        {
            yield return new WaitForSeconds(5f);
            timeOfDay = (timeOfDay+1) % 23;
            if (timeOfDay == CloseTime)
                patreons.Clear();
            clock.text = timeOfDay.ToString() + ":00";
        }
    }

    public bool RegisterPatreon(GameObject p)
    {
        patreons.Push(p);

        return true;
    }

    public void DeRegisterPatreon()
    {
    }


}
