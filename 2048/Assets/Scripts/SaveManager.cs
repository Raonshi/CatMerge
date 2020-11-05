using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static SaveManager instance;

    public static SaveManager Singleton
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = GameObject.Find("SaveManager");

                if (obj == null)
                {
                    obj = new GameObject("SaveManager");
                    obj.AddComponent<SaveManager>();
                }

                instance = obj.GetComponent<SaveManager>();
            }
            return instance;
        }
    }


    public void InitSaveManager()
    {
        Debug.Log("======================SaveManger loaded======================");
        DontDestroyOnLoad(gameObject);

        LoadUserJson();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveUserJson()
    {
        UserJson user = new UserJson();
        string data;

        user.SetData(GameManager.Singleton.townLevel, GameManager.Singleton.best, GameManager.Singleton.totalPoint,GameManager.Singleton.catCount, GameManager.Singleton.isNew, GameManager.Singleton.isHelp);
       
        data = user.Save();

        SaveData(data, "user");
    }

    public void LoadUserJson()
    {
        string json = LoadData("user");

        if(json == null)
        {
            GameManager.Singleton.isNew = true;
            GameManager.Singleton.isHelp = true;
            return;
        }

        UserJson user = JsonUtility.FromJson<UserJson>(json);

        GameManager.Singleton.townLevel = user.townLevel;
        GameManager.Singleton.totalPoint = user.totalPoint;
        GameManager.Singleton.catCount = user.catCount;
        GameManager.Singleton.best = user.best;
        GameManager.Singleton.isNew = user.isNew;


        //테스트용 코드
        //Debug.Log(user.townLevel + " / " + user.best + " / " + user.point );
    }


    //실행하는 경우
    //1. 게임 종료
    //2. 메인메뉴를 벗어나는 경우
    public void SaveTimeJson()
    {
        //실행되는 시점의 시간을 저장
        DateTime time = DateTime.Now;
        CloseTime close = new CloseTime();
        string data;

        close.SetData(DateTime.Now);
        data = close.Save();

        SaveData(data, "time");
    }

    public void LoadTimeJson()
    {
        string json = LoadData("time");

        if(json == null)
        {
            return;
        }

        CloseTime close = JsonUtility.FromJson<CloseTime>(json);

        TimeManager.Singleton.closeTime = new DateTime(close.year, close.month, close.day, close.hour, close.minute, close.second);
    }




    #region 파일 저장 및 불러오기
    public void SaveData(string data, string fileName)
    {
        string path = "Assets/Json/" + fileName + ".json";

        StreamWriter writer = new StreamWriter(path);

        writer.Write(data);
        writer.Flush();
        writer.Close();
    }

    public string LoadData(string fileName)
    {
        string path = "Assets/Json/" + fileName + ".json";

        if(File.Exists(path) == false)
        {
            return null;
        }

        StreamReader reader = new StreamReader(path);

        string data = reader.ReadToEnd();
        
        reader.Dispose();
        reader.Close();

        return data;
    }
   
    #endregion
}

public class UserJson
{
    public int townLevel;
    public int best;
    public int totalPoint;
    public int catCount;
    public bool isNew;
    public bool isHelp;

    public void SetData(int _townLevel, int _best, int _totalPoint, int _catCount, bool _isNew, bool _isHelp)
    {
        townLevel = _townLevel;
        best = _best;
        totalPoint = _totalPoint;
        catCount = _catCount;
        isNew = _isNew;
        isHelp = _isHelp;
    }

    public string Save()
    {
        return JsonUtility.ToJson(this);
    }
}

public class CloseTime
{
    public int second;
    public int minute;
    public int hour;
    public int day;
    public int month;
    public int year;

    public void SetData(DateTime dateTime)
    {
        second = dateTime.Second;
        minute = dateTime.Minute;
        hour = dateTime.Hour;
        day = dateTime.Day;
        month = dateTime.Month;
        year = dateTime.Year;
    }

    public string Save()
    {
        return JsonUtility.ToJson(this);
    }
}
