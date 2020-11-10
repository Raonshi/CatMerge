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


    //세이브매니저 초기화 및 기존 유저 데이터 로드
    public void InitSaveManager()
    {
        Debug.Log("======================SaveManger loaded======================");
        DontDestroyOnLoad(gameObject);

        LoadUserJson();
        LoadItemJson();
        LoadRankJson();
        LoadTutorialJson();
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
        
        //json으로 변환할 객체에 데이터를 저장한다.
        user.SetData(
            GameManager.Singleton.nickname,
            GameManager.Singleton.townLevel,
            GameManager.Singleton.best,
            GameManager.Singleton.totalPoint,
            GameManager.Singleton.catCount,
            GameManager.Singleton.isNew
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
    }


    public void SaveItemJson()
    {
        ItemJson item = new ItemJson();
        string data;

        //json으로 변환할 객체에 데이터를 저장한다.
        item.SetData(GameManager.Singleton.item1Count, GameManager.Singleton.item2Count);

        //item객체를 json으로 저장
        data = item.Save();

        SaveData(data, "item");
    }

    public void LoadItemJson()
    {
        string json = LoadData("item");

        if (json == null)
        {
            GameManager.Singleton.isNew = true;
            return;
        }

        ItemJson item = JsonUtility.FromJson<ItemJson>(json);

        GameManager.Singleton.item1Count = item.item1Count;
        GameManager.Singleton.item2Count = item.item2Count;

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


    public void SaveRankJson()
    {
        //Rank디렉토리가 존재하지 않을 경우 디렉토리를 생성한다.
        if (Directory.Exists(Application.persistentDataPath + "/Save/Rank") == false)
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Save/Rank");
        }

        //Rank디렉토리의 파일 수를 count에 저장
        int count = Directory.GetFiles(Application.persistentDataPath + "/Save/Rank").Length;
        RankJson rank = new RankJson();
        string data;

        //json으로 변환할 객체에 데이터를 저장한다.
        rank.SetData(
            DateTime.Now,
            Game.instance.score,
            GameManager.Singleton.nickname
            );

        //user객체를 json으로 변환
        data = rank.Save();

        SaveData(data, "Rank/rank_" + (count + 1));
    }

    public void LoadRankJson()
    {
        string json = LoadData("Rank/rank_");

        if (json == null)
        {
            //랭킹 기록이 없다는 알림창 나와야할듯
            Debug.Log("랭킹없음");
            return;
        }

        UserJson user = JsonUtility.FromJson<UserJson>(json);

        GameManager.Singleton.townLevel = user.townLevel;
        GameManager.Singleton.totalPoint = user.totalPoint;
        GameManager.Singleton.catCount = user.catCount;

        GameManager.Singleton.best = user.best;
        GameManager.Singleton.isNew = user.isNew;
    }


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
            GameManager.Singleton.itemTutorial1,
            GameManager.Singleton.itemTutorial2
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
        GameManager.Singleton.itemTutorial1 = tutorial.itemTutorial1;
        GameManager.Singleton.itemTutorial2 = tutorial.itemTutorial2;


        //테스트용 코드
        //Debug.Log(user.townLevel + " / " + user.best + " / " + user.point );
    }






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
    public bool isHelp;

    public void SetData(string _nickname,  int _townLevel, int _best, int _totalPoint, int _catCount, bool _isNew)
    {
        nickname = _nickname;
        townLevel = _townLevel;
        best = _best;
        totalPoint = _totalPoint;
        catCount = _catCount;

        isNew = _isNew;
    }

    public string Save()
    {
        return JsonUtility.ToJson(this);
    }
}

public class ItemJson
{
    public int item1Count;
    public int item2Count;

    public void SetData(int _item1Count, int _item2Count)
    {
        item1Count = _item1Count;
        item2Count = _item2Count;
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

public class RankJson
{
    public int day;
    public int month;
    public int year;

    public int score;
    public string nickname;

    public void SetData(DateTime dateTime, int _score, string _nickname)
    {
        day = dateTime.Day;
        month = dateTime.Month;
        year = dateTime.Year;

        score = _score;
        nickname = _nickname;
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
    public bool itemTutorial1;
    public bool itemTutorial2;

    public void SetData(bool _tutorial0, bool _tutorial1, bool _tutorial2, bool _tutorial3, bool _itemTutorial1, bool _itemTutorial2)
    {
        tutorial0 = _tutorial0;
        tutorial1 = _tutorial1;
        tutorial2 = _tutorial2;
        tutorial3 = _tutorial3;
        itemTutorial1 = _itemTutorial1;
        itemTutorial2 = _itemTutorial2;
    }

    public string Save()
    {
        return JsonUtility.ToJson(this);
    }


}
#endregion