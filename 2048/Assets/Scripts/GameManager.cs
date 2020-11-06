using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //사용자 정보 : user.json에 저장
    public int townLevel;       //마을 레벨
    public float scoreRate;     //마을 레벨에 따른 점수 배율

    public int best;            //플레이어 최고점
    public int totalPoint;      //플레이어가 보유한 포인트

    public bool isNew;          //최초 플레이 유무
    public bool isHelp;         //첫 플레이 후 메인화면 설명

    public int catCount;        //보유한 고양이 수
    public int item1Count;      //츄르 보유량
    public int item2Count;      //참치캔 보유량


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
    }


    void Start()
    {
        //최초 실행인 경우
        if(isNew == true)
        {
            townLevel = 1;
            totalPoint = 100;
            best = 0;
            catCount = 0;
            item1Count = 3;
            item2Count = 3;

            SaveManager.Singleton.SaveUserJson();
            SaveManager.Singleton.SaveItemJson();
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScoreRate();
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
}
