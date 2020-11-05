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

        //PlayerPrefs의 날짜 문자열을 DateTime형식으로 변환
        if (PlayerPrefs.HasKey("closeTime"))
        {
            closeString = PlayerPrefs.GetString("closeTime");
        }
        else
        {

            closeString = DateTime.Now.ToString();
            PlayerPrefs.SetString("closeTime", closeString);
        }

        closeTime = Convert.ToDateTime(closeString);
        openTime = DateTime.Now;

        openString = DateTime.Now.ToString();


        if (closeTime < openTime)
        {
            time = openTime - closeTime;
        }
        else
        {
            closeTime = openTime;
        }
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
