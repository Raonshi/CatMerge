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

    public bool isSpawn;

    public List<GameObject> catList = new List<GameObject>();
    
    void Start()
    {
        isSpawn = false;

        if(!PlayerPrefs.HasKey("townLevel"))
        {
            townLevel = 1;
            PlayerPrefs.SetInt("townLevel", townLevel);
        }

        townLevel = PlayerPrefs.GetInt("townLevel");

        town.sprite = Resources.Load<Sprite>("Images/Towns/Level" + townLevel);
    }

    // Update is called once per frame
    void Update()
    {
        CatSpawn();
        if (townLevel == 5)
        {
            priceText.text = "마을강화\n최대";
            buyButton.interactable = false;
            return;
        }

        buyButton.interactable = true;
        priceText.text = "마을강화" + "\n-" + price + "point";

        if(townLevel == 1)
        {
            price = 10;
        }
        else if (townLevel == 2)
        {
            price = 30;
        }
        else if (townLevel == 3)
        {
            price = 50;
        }
        else if (townLevel == 4)
        {
            price = 100;
        }
    }


    public void CatSpawn()
    {
        switch(townLevel)
        {
            case 1:
                catList.Clear();

                if(isSpawn == false)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/Town/2"), GameObject.Find("Canvas/BackGround/CatTown/Image").transform);
                        obj.name = "Cat";
                    }
                    isSpawn = true;
                }
                break;

            case 2:
                catList.Clear();

                if (isSpawn == false)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/Town/4"), GameObject.Find("Canvas/BackGround/CatTown/Image").transform);
                        obj.name = "Cat";
                    }
                    isSpawn = true;
                }
                break;

            case 3:
                catList.Clear();

                if (isSpawn == false)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/Town/8"), GameObject.Find("Canvas/BackGround/CatTown/Image").transform);
                        obj.name = "Cat";
                    }
                    isSpawn = true;
                }
                break;

            case 4:
                catList.Clear();

                if (isSpawn == false)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/Town/16"), GameObject.Find("Canvas/BackGround/CatTown/Image").transform);
                        obj.name = "Cat";
                    }
                    isSpawn = true;
                }
                break;

            case 5:
                catList.Clear();

                if (isSpawn == false)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/Town/32"), GameObject.Find("Canvas/BackGround/CatTown/Image").transform);
                        obj.name = "Cat";
                    }
                    isSpawn = true;
                }
                break;
        }
    }


    public void OnClickBuy()
    {
        int point = PlayerPrefs.GetInt("point");
        //PlayerPrefs.SetInt("point", point - price);

        isSpawn = false;

        GameObject[] array = GameObject.FindGameObjectsWithTag("Cat");
        for (int i = 0; i < array.Length; i++)
        {
            Destroy(array[i]);
        }


        townLevel++;
        PlayerPrefs.SetInt("townLevel", townLevel);
        town.sprite = Resources.Load<Sprite>("Images/Towns/Level" + townLevel);
    }
}
