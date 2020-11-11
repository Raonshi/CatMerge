using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public string closeString;
    public string openString;

    //메인화면에서 벗어나는 시간
    public DateTime closeTime;
    
    public DateTime openTime;
    public TimeSpan time;

    public bool isNew;

    //메인화면 고양이 선물 시간 정보
    public List<float> giftTime = new List<float>();



    private static TimeManager instance;

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

    //타임 매니저 초기화 및 마지막 종료 시간을 불러옴
    public void InitTimeManager()
    {
        Debug.Log("======================TimeManger loaded======================");

        DontDestroyOnLoad(gameObject);

        SaveManager.Singleton.LoadTimeJson();

        if(isNew == true)
        {
            closeTime = DateTime.Now;
        }

        time = DateTime.Now - closeTime;
        closeString = closeTime.ToString();
    }

    private void Update()
    {
        SaveManager.Singleton.SaveTimeJson();
    }
}
