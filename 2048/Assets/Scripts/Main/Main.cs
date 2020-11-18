﻿using System;
using System.Collections;
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

    //메인화면 버튼
    public GameObject startButton;
    public GameObject summonButton;
    public GameObject townUpgradeButton;

    //포인트
    public Text point;

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

        close.SetActive(false);
        tutorial.SetActive(false);
        help.SetActive(false);
        tutorial4.SetActive(false);
        option.SetActive(false);
    }

    void Start()
    {
        //메인 씬 bgm재생
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/BGM_Main"));

        //게임 최초 실행 시 튜토리얼 보여줌
        if (GameManager.Singleton.isNew == true)
        {
            tutorial.SetActive(true);
            //게임시작 버튼 유도

            summonButton.GetComponent<Button>().interactable = false;
            townUpgradeButton.GetComponent<Button>().interactable = false;

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
        point.text = "포인트 : " + GameManager.Singleton.totalPoint;
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
            townUpgradeButton.GetComponent<Button>().image.color = Color.grey;
            townUpgradePriceText.text = "마을 강화(lv" + GameManager.Singleton.townLevel + ")\n" + "최대수치";
        }

        //고양이 소환 가격
        catPrice = (GameManager.Singleton.townLevel * 2) + 3;
        catPriceText.text = catPrice.ToString() + "포인트";


        if (GameObject.Find("Gift") == true && isGift == true)
        {
            giftInfo.SetActive(true);
            isGift = false;
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
        //town.sprite = Resources.Load<Sprite>("Images/Towns/Cat_Town");

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

    //고양이 생성
    //실행 조건 : 메인화면 시작할 때, 고양이 구매 버튼을 눌렀을 때
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
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Click"));

        GameManager.Singleton.isNew = false;

        GameManager.Singleton.LoadNextScene("Game");
    }


    public void OnClickClose(GameObject obj)
    {
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Click"));
        GameManager.Singleton.isHelp = false;
        obj.SetActive(false);
    }


    public void OnClickTownUpgrade()
    {
        if(GameManager.Singleton.townLevel >= 5)
        {
            SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Disable"));
            return;
        }

        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Click"));


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

    public void OnClickOption()
    {
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Click"));
        option.SetActive(true);
    }

    #endregion
}