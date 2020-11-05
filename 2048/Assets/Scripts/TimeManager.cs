using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    private static TimeManager instance;

    public string closeString;
    public string openString;
    public DateTime closeTime;
    public DateTime openTime;
    public TimeSpan time;

    public static TimeManager Singleton
    {
        get
        {
            if(instance == null)
            {
                GameObject obj = GameObject.Find("TimeManager");

                if (obj == null)
                {
                    obj = new GameObject("TimeManager");
                    obj.AddComponent<TimeManager>();
                }

                instance = obj.GetComponent<TimeManager>();
            }
            return instance;
        }
    }


    public void InitTimeManager()
    {
        Debug.Log("======================TimeManger loaded======================");

        DontDestroyOnLoad(gameObject);

        SaveManager.Singleton.LoadTimeJson();
        closeString = closeTime.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
