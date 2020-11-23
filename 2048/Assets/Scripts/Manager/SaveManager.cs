﻿using System;
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


    //세이브매니저 초기화 및 기존 유저 데이터 로드
    public void InitSaveManager()
    {
        Debug.Log("======================SaveManger loaded======================");
        DontDestroyOnLoad(gameObject);

        LoadUserJson();
        LoadTutorialJson();
        LoadOptionJson();
    }


    #region 유저 정보
    public void SaveUserJson()
    {
        UserJson user = new UserJson();
        string data;

        //json으로 변환할 객체에 데이터를 저장한다.
        user.SetData(
            GameManager.Singleton.nickname,
            GameManager.Singleton.townLevel,
            GameManager.Singleton.best,
            GameManager.Singleton.totalPoint,
            GameManager.Singleton.catCount,
            GameManager.Singleton.isNew,
            GameManager.Singleton.isNum,
            GameManager.Singleton.difficulty
            );
       
        //user객체를 json으로 변환
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

        GameManager.Singleton.nickname = user.nickname;

        GameManager.Singleton.townLevel = user.townLevel;
        GameManager.Singleton.totalPoint = user.totalPoint;
        GameManager.Singleton.catCount = user.catCount;
        GameManager.Singleton.best = user.best;

        GameManager.Singleton.isNew = user.isNew;
        GameManager.Singleton.isNum = user.isNum;
        GameManager.Singleton.difficulty = user.difficulty;
    
    }
    #endregion

    #region 종료 시간
    //실행하는 경우
    //1. 게임 종료
    //2. 메인메뉴를 벗어나는 경우
    public void SaveTimeJson()
    {
        //실행되는 시점의 시간을 저장
        DateTime time = DateTime.Now;
        TimeJson close = new TimeJson();
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
            TimeManager.Singleton.isNew = true;
            return;
        }

        TimeJson close = JsonUtility.FromJson<TimeJson>(json);

        TimeManager.Singleton.closeTime = new DateTime(close.year, close.month, close.day, close.hour, close.minute, close.second);
    }
    #endregion

    #region 튜토리얼
    public void SaveTutorialJson()
    {
        TutorialJson tutorial = new TutorialJson();
        string data;

        //json으로 변환할 객체에 데이터를 저장한다.
        tutorial.SetData(
            GameManager.Singleton.tutorial0,
            GameManager.Singleton.tutorial1,
            GameManager.Singleton.tutorial2,
            GameManager.Singleton.tutorial3,
            GameManager.Singleton.tutorial4
            );

        //user객체를 json으로 변환
        data = tutorial.Save();
        SaveData(data, "tutorial");
    }

    public void LoadTutorialJson()
    {
        string json = LoadData("tutorial");

        if (json == null)
        {
            return;
        }

        TutorialJson tutorial = JsonUtility.FromJson<TutorialJson>(json);

        GameManager.Singleton.tutorial0 = tutorial.tutorial0;
        GameManager.Singleton.tutorial1 = tutorial.tutorial1;
        GameManager.Singleton.tutorial2 = tutorial.tutorial2;
        GameManager.Singleton.tutorial3 = tutorial.tutorial3;
        GameManager.Singleton.tutorial4 = tutorial.tutorial4;
    }


    public void SaveOptionJson()
    {
        OptionJson option = new OptionJson();
        string data;

        //json으로 변환할 객체에 데이터를 저장한다.
        option.SetData(
            GameManager.Singleton.bgm,
            GameManager.Singleton.sfx
            );

        //user객체를 json으로 변환
        data = option.Save();
        SaveData(data, "option");
    }

    public void LoadOptionJson()
    {
        string json = LoadData("option");

        if (json == null)
        {
            return;
        }

        OptionJson option = JsonUtility.FromJson<OptionJson>(json);

        GameManager.Singleton.bgm = option.bgm;
        GameManager.Singleton.sfx = option.sfx;
    }

    #endregion





    #region 파일 저장 및 불러오기
    public void SaveData(string data, string fileName)
    {
        string path = Application.persistentDataPath + "/Save/" + fileName + ".json";

        if(Directory.Exists(Application.persistentDataPath+"/Save") == false)
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Save");
        }

        StreamWriter writer = new StreamWriter(path);

        writer.Write(data);
        writer.Flush();
        writer.Close();
    }

    public string LoadData(string fileName)
    {
        string path = Application.persistentDataPath + "/Save/" + fileName + ".json";

        if (File.Exists(path) == false)
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


#region Data -> Json
public class UserJson
{
    public string nickname;

    public int townLevel;
    public int best;
    public int totalPoint;
    public int catCount;

    public bool isNew;
    public bool isNum;

    public GameManager.Difficulty difficulty;

    public void SetData(string _nickname,  int _townLevel, int _best, int _totalPoint, int _catCount, bool _isNew, bool _isNum, GameManager.Difficulty _difficulty)
    {
        nickname = _nickname;
        townLevel = _townLevel;
        best = _best;
        totalPoint = _totalPoint;
        catCount = _catCount;

        isNew = _isNew;
        isNum = _isNum;

        difficulty = _difficulty;
        
    }

    public string Save()
    {
        return JsonUtility.ToJson(this);
    }
}

public class TimeJson
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


public class TutorialJson
{
    public bool tutorial0;
    public bool tutorial1;
    public bool tutorial2;
    public bool tutorial3;
    public bool tutorial4;

    public void SetData(bool _tutorial0, bool _tutorial1, bool _tutorial2, bool _tutorial3, bool _tutorial4)
    {
        tutorial0 = _tutorial0;
        tutorial1 = _tutorial1;
        tutorial2 = _tutorial2;
        tutorial3 = _tutorial3;
        tutorial4 = _tutorial4;
    }

    public string Save()
    {
        return JsonUtility.ToJson(this);
    }
}


public class OptionJson
{
    public float bgm;
    public float sfx;

    public void SetData(float _bgm, float _sfx)
    {
        bgm = _bgm;
        sfx = _sfx;
    }

    public string Save()
    {
        return JsonUtility.ToJson(this);
    }
}
#endregion