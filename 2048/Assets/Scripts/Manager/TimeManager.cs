using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public string closeString;

    public DateTime closeTime;

    //1회 충전시간
    public float charge;
    public TimeSpan chargeTime;

    public float remainTime;
    
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
            charge = 300;
        }
        else
        {
            TimeSpan span = (DateTime.Now - closeTime);
            GameManager.Singleton.life += Convert.ToInt32(Mathf.Floor((float)span.TotalSeconds / 300));
            charge = remainTime - (Mathf.Floor((float)span.TotalSeconds) % 300);
        }

        time = DateTime.Now - closeTime;
        closeString = closeTime.ToString();
    }

    private void Update()
    {
        remainTime = charge;
        SaveManager.Singleton.SaveTimeJson();

        
        if (GameManager.Singleton.life < 5 + Convert.ToInt32(Mathf.Floor((GameManager.Singleton.townLevel - 1) * 0.5f)))
        {
            charge -= Time.deltaTime;
            chargeTime = TimeSpan.FromMinutes(charge);

            if (charge <= 0)
            {
                GameManager.Singleton.life++;
                charge = 300;
            }
        }
    }
}
