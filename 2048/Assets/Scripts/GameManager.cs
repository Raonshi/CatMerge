using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public bool isStart;        //게임 실행 확인

    public int catCount;        //보유한 고양이 수
    public int item1Count;      //츄르 보유량
    public int item2Count;      //참치캔 보유량

    //랭킹 정보
    public List<RankSlot> rankList = new List<RankSlot>();

    //튜토리얼
    public bool tutorial0;
    public bool tutorial1;
    public bool tutorial2;
    public bool tutorial3;
    public bool itemTutorial1;
    public bool itemTutorial2;

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
            item1Count = 3;
            item2Count = 3;

            tutorial0 = true;
            tutorial1 = true;
            tutorial2 = true;
            tutorial3 = true;
            itemTutorial1 = true;
            itemTutorial2 = true;

            Main.instance.nicknamePanel.SetActive(true);
            SaveManager.Singleton.SaveItemJson();
            SaveManager.Singleton.SaveTutorialJson();
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScoreRate();

        //UpdateRankList();

        SaveManager.Singleton.SaveUserJson();
        SaveManager.Singleton.SaveItemJson();
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


    /*
     * 랭킹의 점수와 현재 점수 비교
     * 1. 1위부터 차례대로 비교
     * ->rankList[0] ~ rankList[14]
     * 2. 현재 점수 < rankList[n]등 점수 : rankList[n+1]등 점수와 비교
     * 3. 현재 점수 > rankList[n]등 점수 : ... / rankList[n + 2] = rankList[n + 1] / rankList[n + 1] = rankList[n] / rankList[n] = 현재 점수
     * -> List.Insert를 사용하면 될거 같음.
     * 4. 현재 점수 == rankList[n] : rankList[n] = 현재 점수
     * 5. 각 랭킹에 포함되는 항목
     * -> 날짜기록 / 점수 / 닉네임
     */
    public void CompareScore()
    {
        if(rankList.Count == 0)
        {
            //랭킹리스트가 비어있으면 랭킹저장
            SaveManager.Singleton.SaveRankJson();
        }
        else
        {
            //랭킹리스트가 비어있지 않으면 비교시작
            int tmp = Game.instance.score;
            for(int i = 0; i < rankList.Count; i++)
            {
                //tmp가 rankList[i]보다 크면 랭킹에 삽입

                //작으면 다음 반복 실행
            }
        }
    }
}
