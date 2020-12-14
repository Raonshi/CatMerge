using System;
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
            GameManager.Singleton.life,
            GameManager.Singleton.townLevel,
            GameManager.Singleton.scoreRateLevel,
            GameManager.Singleton.best,
            GameManager.Singleton.totalPoint,
            GameManager.Singleton.totalCash,
            GameManager.Singleton.catCount,
            TimeManager.Singleton.remainTime,
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

        GameManager.Singleton.life = user.life;
        GameManager.Singleton.townLevel = user.townLevel;
        GameManager.Singleton.totalPoint = user.totalPoint;
        GameManager.Singleton.totalCash = user.totalCash;
        GameManager.Singleton.scoreRateLevel = user.scoreRateLevel;
        GameManager.Singleton.catCount = user.catCount;
        GameManager.Singleton.best = user.best;

        GameManager.Singleton.isNew = user.isNew;
        GameManager.Singleton.isNum = user.isNum;
        GameManager.Singleton.difficulty = user.difficulty;

        TimeManager.Singleton.remainTime = user.remainTime;
    
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
    /// <summary>
    /// 현재 데이터를 json으로 직렬화 후 파일로 저장
    /// </summary>
    /// <param name="data">저장할 데이터 문자열</param>
    /// <param name="fileName">파일명</param>
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


    /// <summary>
    /// 파일로 저장된 json을 문자열로 불러온다.
    /// </summary>
    /// <param name="fileName">불러올 파일명</param>
    /// <returns>json 문자열 데이터</returns>
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


#region 앱 삭제 이후에도 데이터 보관
    /// <summary>
    /// 현재 저장된 json파일을 전부 별도의 저장 경로로 복사한다.
    /// </summary>
    public void SaveDataOutSide()
    {
        string[] fileNameArray = Directory.GetFiles(Application.persistentDataPath + "/Save/");
        
        string destPath = "Assets/Save/";
        //string destPath = Application.persistentDataPath + "/Save/";

        if (Directory.Exists(destPath) == false)
        {
            Directory.CreateDirectory(destPath);

            if(Directory.Exists(destPath) == false)
            {
                Main.instance.dataSaveFail.SetActive(true);
                return;
            }
        }

        for (int i = 0; i < fileNameArray.Length; i++)
        {
            string[] name = fileNameArray[i].Split('/');
            string path = destPath + name[name.Length - 1];
            File.Copy(fileNameArray[i], path, true);
        }
        Main.instance.dataSave.SetActive(true);
    }


    /// <summary>
    /// 별도의 저장 경로에 존재하는 json파일을 모두 플레이어 정보로 불러온다.
    /// </summary>
    public void LoadDataOutSide()
    {
        string destPath = "Assets/Save/";
        //string destPath = Application.persistentDataPath + "/Save/";

        if (Directory.Exists(destPath) == false || Directory.GetFiles(destPath).Length == 0)
        {
            Main.instance.dataLoadFail.SetActive(true);
            //Debug.Log("No Save File!");
            return;
        }

        string[] fileNameArray = Directory.GetFiles(destPath);


        for (int i = 0; i < fileNameArray.Length; i++)
        {
            string path = fileNameArray[i];

            if (File.Exists(path) == false)
            {
                Main.instance.dataLoadFail.SetActive(true);
                //Debug.Log("Path Not Exist!");
                return;
            }

            StreamReader reader = new StreamReader(path);

            string data = reader.ReadToEnd();

            reader.Dispose();
            reader.Close();
            if(fileNameArray[i].Contains(".meta"))
            {
                continue;
            }

            if(fileNameArray[i].Contains("user"))
            {
                UserJson user = JsonUtility.FromJson<UserJson>(data);

                GameManager.Singleton.nickname = user.nickname;

                GameManager.Singleton.life = user.life;
                GameManager.Singleton.townLevel = user.townLevel;
                GameManager.Singleton.totalPoint = user.totalPoint;
                GameManager.Singleton.totalCash = user.totalCash;
                GameManager.Singleton.scoreRateLevel = user.scoreRateLevel;
                GameManager.Singleton.catCount = user.catCount;
                GameManager.Singleton.best = user.best;

                GameManager.Singleton.isNew = user.isNew;
                GameManager.Singleton.isNum = user.isNum;
                GameManager.Singleton.difficulty = user.difficulty;

                TimeManager.Singleton.remainTime = user.remainTime;
            }
            else if(fileNameArray[i].Contains("time"))
            {
                TimeJson close = JsonUtility.FromJson<TimeJson>(data);

                TimeManager.Singleton.closeTime = new DateTime(close.year, close.month, close.day, close.hour, close.minute, close.second);
            }
            else if (fileNameArray[i].Contains("tutorial"))
            {
                TutorialJson tutorial = JsonUtility.FromJson<TutorialJson>(data);

                GameManager.Singleton.tutorial0 = tutorial.tutorial0;
                GameManager.Singleton.tutorial1 = tutorial.tutorial1;
                GameManager.Singleton.tutorial2 = tutorial.tutorial2;
                GameManager.Singleton.tutorial3 = tutorial.tutorial3;
                GameManager.Singleton.tutorial4 = tutorial.tutorial4;
            }
            else if (fileNameArray[i].Contains("option"))
            {
                OptionJson option = JsonUtility.FromJson<OptionJson>(data);

                GameManager.Singleton.bgm = option.bgm;
                GameManager.Singleton.sfx = option.sfx;
            }
            //Debug.Log(fileNameArray[i] + " Has Load!");

            Main.instance.dataLoad.SetActive(true);
        }
    }

#endregion

}




#region Data -> Json

/// <summary>
/// 플레이어 정보
/// </summary>
public class UserJson
{
    public string nickname;

    public int life;
    public int townLevel;
    public int scoreRateLevel;
    public int best;
    public int totalPoint;
    public int totalCash;
    public int catCount;
    public float remainTime;

    public bool isNew;
    public bool isNum;

    public GameManager.Difficulty difficulty;

    public void SetData(string _nickname, int _life,  int _townLevel, int _scoreRateLevel, int _best, int _totalPoint,int _totalCash,
        int _catCount, float _remainTime, bool _isNew, bool _isNum, GameManager.Difficulty _difficulty)
    {
        nickname = _nickname;
        life = _life;
        townLevel = _townLevel;
        scoreRateLevel = _scoreRateLevel;
        best = _best;
        totalPoint = _totalPoint;
        totalCash = _totalCash;
        catCount = _catCount;
        remainTime = _remainTime;

        isNew = _isNew;
        isNum = _isNum;

        difficulty = _difficulty;
        
    }

    public string Save()
    {
        return JsonUtility.ToJson(this);
    }
}

/// <summary>
/// 마지막 종료시간
/// </summary>
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

/// <summary>
/// 튜토리얼 알림창 활성/비활성 여부
/// </summary>
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

/// <summary>
/// 게임 옵션 설정값
/// </summary>
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