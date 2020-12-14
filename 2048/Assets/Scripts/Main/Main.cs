using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    //메인화면 알림창
    public GameObject close;
    public GameObject notEnoughCash;
    public GameObject notEnoughLife;
    public GameObject notEnoughPoint;
    public GameObject fullCount;
    public GameObject giftInfo;
    public GameObject adGiftPanel;
    public GameObject dataSaveFail;
    public GameObject dataLoadFail;
    public GameObject dataSave;
    public GameObject dataLoad;


    //메인화면 버튼
    public GameObject startButton;
    public Text startButtonText;
    public GameObject summonButton;
    public GameObject townUpgradeButton;

    //포인트
    public Text point;
    public GameObject pointBuy;

    //캐쉬
    public Text cash;
    public GameObject cashBuy;

    //상점
    public GameObject shopPanel;

    //최초 게임시작 유도
    public GameObject tutorial;

    //도움말
    public GameObject help;
    public GameObject tutorial4;

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

    //옵션
    public GameObject option;


    //싱글턴
    public static Main instance;

    private void Awake()
    {
        instance = this;

        isGift = true;
        townUpgrade = false;

        notEnoughCash.SetActive(false);
        notEnoughLife.SetActive(false);
        notEnoughPoint.SetActive(false);
        close.SetActive(false);
        tutorial.SetActive(false);
        help.SetActive(false);
        tutorial4.SetActive(false);
        option.SetActive(false);
        adGiftPanel.SetActive(false);
        shopPanel.SetActive(false);
        dataSaveFail.SetActive(false);
        dataLoadFail.SetActive(false);
        dataSave.SetActive(false);
        dataLoad.SetActive(false);
    }

    void Start()
    {
        //메인 씬 bgm재생
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/BGM_Main"));

        //게임 최초 실행 시 튜토리얼 보여줌
        if (GameManager.Singleton.isNew == true)
        {
            tutorial.SetActive(true);
            return;
        }
        else if(GameManager.Singleton.isHelp == true && GameManager.Singleton.isNew == false)
        {
            help.SetActive(true);
            GameManager.Singleton.isHelp = false;
        }

        UpdateCatTown();
        CatSpawn(GameManager.Singleton.catCount, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Singleton.tutorial4 == true && GameManager.Singleton.townLevel >= 3 && GameManager.Singleton.isNew == false && GameManager.Singleton.isHelp == false)
        {
            tutorial4.SetActive(true);
        }

        //텍스트 수치 표시
        //게임 실행 가능 횟수가 full인 경우
        if(GameManager.Singleton.life == 5 + Convert.ToInt32(Mathf.Floor((GameManager.Singleton.townLevel - 1) * 0.5f)))
        {
            startButtonText.text = string.Format("게임시작({0} / {1})\n00:00", GameManager.Singleton.life, 5 + Convert.ToInt32(Mathf.Floor((GameManager.Singleton.townLevel - 1) * 0.5f)));
        }
        else
        {
            startButtonText.text = string.Format("게임시작({0} / {1})\n{2:00}:{3:00}", GameManager.Singleton.life, 5 + Convert.ToInt32(Mathf.Floor((GameManager.Singleton.townLevel - 1) * 0.5f)),
                TimeManager.Singleton.chargeTime.Hours, TimeManager.Singleton.chargeTime.Minutes);
        }       
        point.text = "포인트 : " + GameManager.Singleton.totalPoint;
        cash.text = "캐쉬 : " + GameManager.Singleton.totalCash;
        scoreRateText.text = "점수배율 : 1 + " + GameManager.Singleton.scoreRate + " 배";
        catCount.text = "고양이 : " + GameManager.Singleton.catCount + " / " + maxCount;

        //마을 강화 가격
        if(GameManager.Singleton.townLevel < 5)
        {
            townUpgradePrice = GameManager.Singleton.townLevel * 15;
            townUpgradePriceText.text = "마을 강화(lv" + GameManager.Singleton.townLevel + ")\n" + townUpgradePrice.ToString() + "포인트";
        }
        else
        {
            townUpgradePrice = 20 + Convert.ToInt32(GameManager.Singleton.scoreRateLevel * 1.15f);
            townUpgradePriceText.text = string.Format("점수배율증가(lv {0})\n{1}포인트", GameManager.Singleton.scoreRateLevel, townUpgradePrice);
        }

        //고양이 소환 가격
        catPrice = (GameManager.Singleton.townLevel * 2) + 3;
        catPriceText.text = catPrice.ToString() + "포인트";

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


    /// <summary>
    /// 마을 강화
    /// </summary>
    public void UpdateCatTown()
    {
        //마을 화면 및 최대 고양이 수 초기화
        maxCount = GameManager.Singleton.townLevel + 4;
        town.sprite = Resources.Load<Sprite>("Images/Towns/Level" + GameManager.Singleton.townLevel);

        //마을 업그레이드 할때는 이미지만 교체
        //Main스크립트 실행시엔 기존 고양이 삭제 안함.
        if (townUpgrade == true)
        {
            for (int i = 0; i < catList.Count; i++)
            {
                catList[i].GetComponent<Cat>().image.sprite = Resources.Load<Sprite>("Images/Cats/" + Mathf.Pow(2, GameManager.Singleton.townLevel));
            }
        }
    }

    /// <summary>
    /// 고양이 생성
    /// </summary>
    /// <param name="count">생성할 고양이 수</param>
    /// <param name="isNew">인구가 추가될 경우 true</param>
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

    /// <summary>
    /// 장면 전환 및 데이터 불러오기 실행 시 고양이 초기화
    /// </summary>
    public void CatDispose()
    {
        for(int i = 0; i < catList.Count; i++)
        {
            Destroy(catList[i]);
        }

        catList.Clear();
    }

    #region 버튼
    /// <summary>
    /// 게임 시작
    /// </summary>
    public void OnClickStart()
    {
        if(GameManager.Singleton.life == 0)
        {
            notEnoughLife.SetActive(true);
            return;
        }
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Click"));

        GameManager.Singleton.isNew = false;

        
        if (GameManager.Singleton.life == 5 + Convert.ToInt32(Mathf.Floor((GameManager.Singleton.townLevel - 1) * 0.5f)))
        {
            TimeManager.Singleton.charge = 300;
        }

        GameManager.Singleton.life--;

        GameManager.Singleton.LoadNextScene("Game");
    }

    /// <summary>
    /// 닫기 버튼
    /// </summary>
    /// <param name="obj"></param>
    public void OnClickClose(GameObject obj)
    {
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Click"));
        GameManager.Singleton.isHelp = false;
        obj.SetActive(false);
    }

    /// <summary>
    /// 고양이 마을 업그레이드 버튼 클릭. 마을이 최대레벨이면 점수배율 강화로 전환.
    /// </summary>
    public void OnClickTownUpgrade()
    {
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Click"));
        
        if (GameManager.Singleton.totalPoint < townUpgradePrice)
        {
            notEnoughPoint.SetActive(true);
            return;
        }


        if (GameManager.Singleton.townLevel >= 5)
        {
            GameManager.Singleton.totalPoint -= townUpgradePrice;
            GameManager.Singleton.scoreRateLevel++;
        }
        else
        {
            GameManager.Singleton.totalPoint -= townUpgradePrice;
            GameManager.Singleton.townLevel++;

            townUpgrade = true;
            UpdateCatTown();
        }

        SaveManager.Singleton.SaveUserJson();
    }

    /// <summary>
    /// 고양이 소환 버튼 클릭
    /// </summary>
    public void OnClickSummonCat()
    {
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Click"));
        if (GameManager.Singleton.catCount < maxCount)
        {
            if (GameManager.Singleton.totalPoint < catPrice)
            {
                notEnoughPoint.SetActive(true);
                return;
            }
            GameManager.Singleton.totalPoint -= catPrice;

            CatSpawn(1, true);

            SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_CatSummon"));

            GameManager.Singleton.catCount++;
        }
        else
        {
            fullCount.SetActive(true);
            return;
        }

        SaveManager.Singleton.SaveUserJson();
    }

    /// <summary>
    /// 옵션 버튼 클릭
    /// </summary>
    public void OnClickOption()
    {
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Click"));
        option.SetActive(true);
    }

    /// <summary>
    /// 재화표시 옆 +버튼 클릭시 상점 호출
    /// </summary>
    public void OnClickBuy()
    {
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Click"));
        shopPanel.SetActive(true);
    }


    #endregion
}