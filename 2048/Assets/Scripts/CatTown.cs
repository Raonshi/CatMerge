using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class CatTown : MonoBehaviour
{
    //마을 화면
    public Image town;
    public List<GameObject> catList = new List<GameObject>();
    public int maxCount;
    public Text cat;

    //마을 강화
    public int townUpgradePrice;
    public Text townUpgradePriceText;
    public Button townUpgrade;

    //점수 배율
    public Text scoreRateText;
    public float scoreRate;

    public bool isSpawn;

    //포인트 부족 알림창
    public GameObject info;
    public Text infoText;

    
    void Start()
    {
        isSpawn = false;


        if (!PlayerPrefs.HasKey("maxCount"))
        {
            maxCount = 5;
            PlayerPrefs.SetInt("maxCount", maxCount);
        }
        maxCount = PlayerPrefs.GetInt("maxCount");

        town.sprite = Resources.Load<Sprite>("Images/Towns/Level" + GameManager.Singleton.townLevel);

        CatSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Singleton.townLevel < 5 && GameObject.Find("Main").GetComponent<Main>().close.activeSelf == false && GameObject.Find("Main").GetComponent<Main>().tutorial.activeSelf == false)
        {
            townUpgrade.interactable = true;
        }
        townUpgradePriceText.text = "마을강화" + "\n-" + townUpgradePrice + "point";

        if(GameManager.Singleton.townLevel == 1)
        {
            townUpgradePrice = 10;
            maxCount = 5;
        }
        else if (GameManager.Singleton.townLevel == 2)
        {
            townUpgradePrice = 30;
            maxCount = 6;
        }
        else if (GameManager.Singleton.townLevel == 3)
        {
            townUpgradePrice = 50;
            maxCount = 7;
        }
        else if (GameManager.Singleton.townLevel == 4)
        {
            townUpgradePrice = 100;
            maxCount = 8;
        }
        else if (GameManager.Singleton.townLevel == 5)
        {
            townUpgradePriceText.text = "마을강화\n최대";
            townUpgrade.interactable = false;
            maxCount = 9;
        }

        scoreRateText.text = "점수배율 : 1 + " + GameManager.Singleton.scoreRate + " 배";
        cat.text = "고양이 : " + GameManager.Singleton.catCount + " / " + maxCount;
    }


    public void CatSpawn()
    {
        catList.Clear();

        if (isSpawn == false)
        {
            for (int i = 0; i < GameManager.Singleton.catCount; i++)
            {
                GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/Town/Cat"), GameObject.Find("Canvas/BackGround/CatTown/Image").transform);
                obj.name = "Cat";
                obj.GetComponent<Cat>().image.sprite = Resources.Load<Sprite>("Images/Cats/" + Mathf.Pow(2, GameManager.Singleton.townLevel));
                catList.Add(obj);
            }
            isSpawn = true;
        }
    }


    public void OnClickBuy()
    {
        if(GameManager.Singleton.totalPoint < townUpgradePrice)
        {
            info.SetActive(true);
            infoText.text = "포인트가 부족합니다.\n게임 플레이를 통해 포인트를 획득할 수 있습니다.";
            return;
        }

        GameManager.Singleton.totalPoint -= townUpgradePrice;

        isSpawn = false;

        GameObject[] array = GameObject.FindGameObjectsWithTag("Cat");
        for (int i = 0; i < array.Length; i++)
        {
            Destroy(array[i]);
        }


        GameManager.Singleton.townLevel++;

        town.sprite = Resources.Load<Sprite>("Images/Towns/Level" + GameManager.Singleton.townLevel);

        CatSpawn();

        SaveManager.Singleton.SaveUserJson();
    }

    public void OnClickSummon()
    {
        if(GameManager.Singleton.catCount < maxCount)
        {
            

            if (GameManager.Singleton.totalPoint < 5)
            {
                info.SetActive(true);
                infoText.text = "포인트가 부족합니다.\n게임 플레이를 통해 포인트를 획득할 수 있습니다.";
                return;
            }

            GameManager.Singleton.totalPoint -= 5;

            GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/Town/Cat"), GameObject.Find("Canvas/BackGround/CatTown/Image").transform);
            obj.name = "Cat";
            obj.GetComponent<Cat>().image.sprite = Resources.Load<Sprite>("Images/Cats/"+Mathf.Pow(2,GameManager.Singleton.townLevel));
            catList.Add(obj);

            GameManager.Singleton.catCount++;
        }
        else
        {
            info.SetActive(true);
            infoText.text = "최대 소환 한도입니다.";
            return;
        }

        SaveManager.Singleton.SaveUserJson();
    }
}