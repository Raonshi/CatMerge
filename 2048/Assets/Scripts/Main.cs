using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    //메인화면 알림창
    public GameObject close;
    public GameObject notEnoughPoint;
    public GameObject fullCount;
    public GameObject giftInfo;
    public GameObject nicknamePanel;

    //메인화면 버튼
    public GameObject startButton;
    public GameObject summonButton;
    public GameObject townUpgradeButton;

    //닉네임
    public Text nicknameText;

    //포인트
    public Text point;

    //최초 게임시작 유도
    public GameObject tutorial;

    //도움말
    public GameObject help;

    //선물 알림창 활성여부
    public bool isGift;

    //마을 화면 관련
    public Image town;
    public List<GameObject> catList = new List<GameObject>();
    public int maxCount;
    public Text catCount;

    //고양이 소환
    public int catPrice;
    public Text catPriceText;

    //마을 강화
    public bool townUpgrade;
    public int townUpgradePrice;
    public Text townUpgradePriceText;

    //점수 배율
    public Text scoreRateText;
    public float scoreRate;


    //싱글턴
    public static Main instance;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;

        isGift = true;
        townUpgrade = false;

        nicknamePanel.SetActive(false);
        close.SetActive(false);
        tutorial.SetActive(false);
        help.SetActive(false);

        if(GameObject.Find("GameManager") == false)
        {
            GameManager.Singleton.InitGameManager();
        }
    }

    void Start()
    {
        if(GameManager.Singleton.isNew == true)
        {
            nicknamePanel.SetActive(true);
        }
        else if(GameManager.Singleton.isHelp == true && GameManager.Singleton.isNew == false)
        {
            help.SetActive(true);
            GameManager.Singleton.isHelp = false;
        }

        nicknameText.text = "닉네임 : " + GameManager.Singleton.nickname;

        UpdateCatTown();
        CatSpawn(GameManager.Singleton.catCount, false);
    }

    // Update is called once per frame
    void Update()
    {
        //텍스트 수치 표시
        point.text = "포인트 : " + GameManager.Singleton.totalPoint;
        scoreRateText.text = "점수배율 : 1 + " + GameManager.Singleton.scoreRate + " 배";
        catCount.text = "고양이 : " + GameManager.Singleton.catCount + " / " + maxCount;

        //마을 강화 가격
        townUpgradePrice = GameManager.Singleton.townLevel * 15;
        townUpgradePriceText.text = "마을 강화\n" + townUpgradePrice.ToString() + "포인트";

        //고양이 소환 가격
        catPrice = (GameManager.Singleton.townLevel * 2) + 3;
        catPriceText.text = catPrice.ToString() + "포인트";


        if (GameObject.Find("Gift") == true && isGift == true)
        {
            giftInfo.SetActive(true);
            isGift = false;
        }


        //게임 최초 실행 시 튜토리얼 보여줌
        if (GameManager.Singleton.isNew == true)
        {
            tutorial.SetActive(true);
            //게임시작 버튼 유도

            summonButton.GetComponent<Button>().interactable = false;
            townUpgradeButton.GetComponent<Button>().interactable = false;

            return;
        }

        //게임종료 알림창과 포인트 부족 알림창은 활성화 시 다른 버튼을 누를 수 없다.
        if(close.activeSelf == true || notEnoughPoint.activeSelf == true)
        {
            startButton.GetComponent<Button>().interactable = false;
            summonButton.GetComponent<Button>().interactable = false;
            townUpgradeButton.GetComponent<Button>().interactable = false;
            return;
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            close.SetActive(!close.activeSelf);

        }

        startButton.GetComponent<Button>().interactable = true;
        summonButton.GetComponent<Button>().interactable = true;
        townUpgradeButton.GetComponent<Button>().interactable = true;
    }


    //고양이 마을 정보 갱신
    //실행조건 : 마을 업그레이드할 때, 게임 시작할때
    //마을 이미지 교체, 고양이 이미지 교체
    public void UpdateCatTown()
    {
        //마을 화면 및 최대 고양이 수 초기화
        maxCount = GameManager.Singleton.townLevel + 4;
        town.sprite = Resources.Load<Sprite>("Images/Towns/Level" + GameManager.Singleton.townLevel);


        //마을 업그레이드 할때는 이미지만 교체
        //Main스크립트 실행시엔 기존 고양이 삭제 안함.
        if(townUpgrade == true)
        {
            for (int i = 0; i < catList.Count; i++)
            {
                catList[i].GetComponent<Cat>().image.sprite = Resources.Load<Sprite>("Images/Cats/" + Mathf.Pow(2, GameManager.Singleton.townLevel));
            }
        }
    }

    //고양이 생성
    //실행 조건 : 게임 시작할 때, 고양이 구매 버튼을 눌렀을 때
    public void CatSpawn(int count, bool isNew)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/Town/Cat"), GameObject.Find("Canvas/BackGround/CatTown/Image").transform);
            obj.name = "Cat";
            obj.GetComponent<Cat>().image.sprite = Resources.Load<Sprite>("Images/Cats/" + Mathf.Pow(2, GameManager.Singleton.townLevel));
            obj.GetComponent<Cat>().isNew = isNew;

            if (TimeManager.Singleton.giftTime.Count != 0)
            {
                obj.GetComponent<Cat>().giftTime = TimeManager.Singleton.giftTime[i];
            }

            catList.Add(obj);
        }

        townUpgrade = false;
    }


    #region 버튼
    public void OnClickStart()
    {
        if(GameManager.Singleton.isNew == true)
        {
            tutorial.SetActive(false);
            GameManager.Singleton.isNew = false;
        }


        //고양이 선물시간 계산을 위해 게임 시작 전 현재 시간을 저장
        TimeManager.Singleton.closeTime = DateTime.Now;

        //각 고양이들의 선물 시간을 리스트에 저장
        TimeManager.Singleton.giftTime.Clear();

        for(int i = 0; i < GameObject.FindGameObjectsWithTag("Cat").Length; i++)
        {
            GameObject obj = GameObject.FindGameObjectsWithTag("Cat")[i];
            TimeManager.Singleton.giftTime.Add(obj.GetComponent<Cat>().giftTime);
        }



        SceneManager.LoadScene("Game");
    }


    public void OnClickClose(GameObject obj)
    {
        obj.SetActive(false);
    }


    public void OnClickShop()
    {
        TimeManager.Singleton.closeTime = DateTime.Now;
        TimeManager.Singleton.giftTime.Clear();

        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Cat").Length; i++)
        {
            GameObject obj = GameObject.FindGameObjectsWithTag("Cat")[i];
            TimeManager.Singleton.giftTime.Add(obj.GetComponent<Cat>().giftTime);
        }

        SceneManager.LoadScene("Shop");
    }


    public void OnClickTownUpgrade()
    {
        if (GameManager.Singleton.totalPoint < townUpgradePrice)
        {
            notEnoughPoint.SetActive(true);
            return;
        }

        GameManager.Singleton.totalPoint -= townUpgradePrice;
        GameManager.Singleton.townLevel++;

        townUpgrade = true;
        UpdateCatTown();

        SaveManager.Singleton.SaveUserJson();
    }

    public void OnClickSummonCat()
    {
        if (GameManager.Singleton.catCount < maxCount)
        {
            if (GameManager.Singleton.totalPoint < catPrice)
            {
                notEnoughPoint.SetActive(true);
                return;
            }
            GameManager.Singleton.totalPoint -= catPrice;

            CatSpawn(1, true);

            GameManager.Singleton.catCount++;
        }
        else
        {
            fullCount.SetActive(true);
            return;
        }

        SaveManager.Singleton.SaveUserJson();
    }

    
    public void OnClickRanking()
    {
        TimeManager.Singleton.closeTime = DateTime.Now;
        TimeManager.Singleton.giftTime.Clear();

        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Cat").Length; i++)
        {
            GameObject obj = GameObject.FindGameObjectsWithTag("Cat")[i];
            TimeManager.Singleton.giftTime.Add(obj.GetComponent<Cat>().giftTime);
        }

        SaveManager.Singleton.SaveUserJson();

        SceneManager.LoadScene("Ranking");
    }

    #endregion
}
