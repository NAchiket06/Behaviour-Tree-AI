using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Blackboard : MonoBehaviour
{

    public float timeOfDay;
    [SerializeField] private TextMeshProUGUI clock;

    public GameObject patreon;

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
            print("updating clock now");
            timeOfDay = (timeOfDay+1) % 23;
            clock.text = timeOfDay.ToString() + ":00";
        }
    }

    public GameObject RegisterPatreon(GameObject p)
    {
        if (patreon == null)
            patreon = p;

        return patreon;
    }

    public void DeRegisterPatreon()
    {
        patreon = null;
    }


}
