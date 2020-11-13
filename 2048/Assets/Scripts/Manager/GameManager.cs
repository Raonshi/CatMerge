using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //사용자 정보 : user.json에 저장
    public string nickname;     //사용자 닉네임
    public int townLevel;       //마을 레벨
    public float scoreRate;     //마을 레벨에 따른 점수 배율

    public int best;            //플레이어 최고점
    public int totalPoint;      //플레이어가 보유한 포인트

    public bool isNew;          //최초 플레이 유무
    public bool isHelp;         //첫 플레이 후 메인화면 설명

    public bool isStart;        //게임 실행 확인                                true : 게임 실행됨                         /   false : 게임 실행 후 false로 변함

    public int catCount;        //보유한 고양이 수

    //숫자 활성화 토글
    public bool isNum;      //게임 화면에서 숫자 표현 토글을 사용하기 위한 변수

    //난이도 변경 토글
    public bool isHard;

    public enum Difficulty
    {
        Easy,
        Normal,
        Hard,
    }

    public Difficulty difficulty;


    //튜토리얼
    public bool tutorial0;
    public bool tutorial1;
    public bool tutorial2;
    public bool tutorial3;

    //해상도
    public int screenWidth;

    private static GameManager instance;

    //싱글턴 객체 생성
    public static GameManager Singleton
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = GameObject.Find("GameManager");

                if (obj == null)
                {
                    obj = new GameObject("GameManager");
                    obj.AddComponent<GameManager>();
                }

                instance = obj.GetComponent<GameManager>();
            }
            return instance;
        }
    }

    //게임 매니저 초기화
    public void InitGameManager()
    {
        Debug.Log("======================GameManger loaded======================");

        DontDestroyOnLoad(gameObject);

        //해상도 조절
        screenWidth = Screen.width;
        Screen.SetResolution(screenWidth, (screenWidth * 16) / 9, true);

        
        
        //각종 매니저 생성 및 초기화
        SaveManager.Singleton.InitSaveManager();
        TimeManager.Singleton.InitTimeManager();

        isStart = true;
    }


    void Start()
    {
        //최초 실행인 경우
        if(isNew == true)
        {
            nickname = "Cat";
            townLevel = 1;
            totalPoint = 100;
            best = 0;
            catCount = 0;

            tutorial0 = true;
            tutorial1 = true;
            tutorial2 = true;
            tutorial3 = true;

            isNum = false;
            isHard = false;

            SaveManager.Singleton.SaveUserJson();
            SaveManager.Singleton.SaveTutorialJson();
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScoreRate();

        SaveManager.Singleton.SaveUserJson();
    }

    public void UpdateScoreRate()
    {
        if (townLevel == 1)
        {
            scoreRate = 0;
        }
        else
        {
            scoreRate = (townLevel * 0.25f);
        }
    }


    public void Loading()
    {
        SceneManager.LoadScene("Loading", LoadSceneMode.Additive);
    }
}
