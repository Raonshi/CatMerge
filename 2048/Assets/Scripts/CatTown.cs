using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class CatTown : MonoBehaviour
{
    public Image town;
    public int townLevel;

    public Button buyButton;

    public Text priceText;
    public int price;

    public Text scoreRateText;
    public float scoreRate;

    public bool isSpawn;

    public GameObject info;
    public Text infoText;

    public List<GameObject> catList = new List<GameObject>();

    public GameObject[] catArray;
    public int catCount;
    public int maxCount;
    public Text cat;

    
    void Start()
    {
        isSpawn = false;

        if(!PlayerPrefs.HasKey("townLevel"))
        {
            townLevel = 1;
            PlayerPrefs.SetInt("townLevel", townLevel);
        }
        townLevel = PlayerPrefs.GetInt("townLevel");


        if (!PlayerPrefs.HasKey("maxCount"))
        {
            maxCount = 5;
            PlayerPrefs.SetInt("maxCount", maxCount);
        }
        maxCount = PlayerPrefs.GetInt("maxCount");

        if (!PlayerPrefs.HasKey("catCount"))
        {
            catCount = 0;
            PlayerPrefs.SetInt("catCount", catCount);
        }
        catCount = PlayerPrefs.GetInt("catCount");

        town.sprite = Resources.Load<Sprite>("Images/Towns/Level" + townLevel);

        CatSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (townLevel < 5 && GameObject.Find("Main").GetComponent<Main>().close.activeSelf == false && GameObject.Find("Main").GetComponent<Main>().tutorial.activeSelf == false)
        {
            buyButton.interactable = true;
        }
        priceText.text = "마을강화" + "\n-" + price + "point";

        if(townLevel == 1)
        {
            price = 10;
            maxCount = 5;
        }
        else if (townLevel == 2)
        {
            price = 30;
            maxCount = 6;
        }
        else if (townLevel == 3)
        {
            price = 50;
            maxCount = 7;
        }
        else if (townLevel == 4)
        {
            price = 100;
            maxCount = 8;
        }
        else if (townLevel == 5)
        {
            priceText.text = "마을강화\n최대";
            buyButton.interactable = false;
            maxCount = 9;
        }

        //점수 배율 조정
        townLevel = PlayerPrefs.GetInt("townLevel");
        if (townLevel == 1)
        {
            scoreRate = 0;
        }
        else
        {
            scoreRate = (townLevel * 0.25f);
        }

        scoreRateText.text = "점수배율 : 1 + " + scoreRate + " 배";
        cat.text = "고양이 : " + catCount + " / " + maxCount;
    }


    public void CatSpawn()
    {
        catList.Clear();

        if (PlayerPrefs.HasKey("catCount"))
        {
            catCount = PlayerPrefs.GetInt("catCount");
            if (isSpawn == false)
            {
                for (int i = 0; i < catCount; i++)
                {
                    GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/Town/" + Mathf.Pow(2, townLevel)), GameObject.Find("Canvas/BackGround/CatTown/Image").transform);
                    obj.name = "Cat";
                    catList.Add(obj);
                }
                isSpawn = true;
            }
        }
    }


    public void OnClickBuy()
    {
        int point = PlayerPrefs.GetInt("point");
        
        if(point < price)
        {
            info.SetActive(true);
            infoText.text = "포인트가 부족합니다.\n게임 플레이를 통해 포인트를 획득할 수 있습니다.";
            return;
        }

        PlayerPrefs.SetInt("point", point - price);

        isSpawn = false;

        GameObject[] array = GameObject.FindGameObjectsWithTag("Cat");
        for (int i = 0; i < array.Length; i++)
        {
            Destroy(array[i]);
        }


        townLevel++;
        PlayerPrefs.SetInt("townLevel", townLevel);

        town.sprite = Resources.Load<Sprite>("Images/Towns/Level" + townLevel);

        CatSpawn();
    }

    public void OnClickSummon()
    {
        if(catCount < maxCount)
        {
            int point = PlayerPrefs.GetInt("point");

            if (point < 5)
            {
                info.SetActive(true);
                infoText.text = "포인트가 부족합니다.\n게임 플레이를 통해 포인트를 획득할 수 있습니다.";
                return;
            }

            PlayerPrefs.SetInt("point", point - 5);

            GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/Town/" + Mathf.Pow(2, townLevel)), GameObject.Find("Canvas/BackGround/CatTown/Image").transform);
            obj.name = "Cat";

            catCount++;
            PlayerPrefs.SetInt("catCount", catCount);
        }
        else
        {
            info.SetActive(true);
            infoText.text = "최대 소환 한도입니다.";
            return;
        }
    }
}