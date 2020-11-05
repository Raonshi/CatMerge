using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //사용자 정보 : user.json에 저장
    public int townLevel;       //마을 레벨
    public float scoreRate;     //마을 레벨에 따른 점수 배율

    public int best;            //플레이어 최고점
    public int point;           //플레이어가 보유한 포인트

    public bool isNew;          //최초 플레이 유무

    //해상도
    public int screenWidth;

    private static GameManager instance;

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

    public void InitGameManager()
    {
        Debug.Log("======================GameManger loaded======================");

        DontDestroyOnLoad(gameObject);

        TimeManager.Singleton.InitTimeManager();
        SaveManager.Singleton.InitSaveManager();


        //해상도 조절
        screenWidth = Screen.width;
        Screen.SetResolution(screenWidth, (screenWidth * 16) / 9, true);
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
