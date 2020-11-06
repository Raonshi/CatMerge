using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Text point;
    public Text item1Count;
    public Text item2Count;

    public GameObject goToMain;
    public GameObject notEnoughPoint;

    void Start()
    {
        goToMain.SetActive(false);
        notEnoughPoint.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        point.text = "포인트 : " + GameManager.Singleton.totalPoint.ToString();
        item1Count.text = "보유수량 : " + GameManager.Singleton.item1Count.ToString();
        item2Count.text = "보유수량 : " + GameManager.Singleton.item2Count.ToString();
    }

    public void OnClickBuy(int id)
    {
        switch(id)
        {
            case 1:
                if(GameManager.Singleton.totalPoint < 5)
                {
                    notEnoughPoint.SetActive(true);
                    return;
                }

                GameManager.Singleton.item1Count++;
                GameManager.Singleton.totalPoint -= 5;
                break;

            case 2:
                if (GameManager.Singleton.totalPoint < 10)
                {
                    notEnoughPoint.SetActive(true);
                    return;
                }

                GameManager.Singleton.item2Count++;
                GameManager.Singleton.totalPoint -= 10;
                break;
        }
        SaveManager.Singleton.SaveUserJson();
        SaveManager.Singleton.SaveItemJson();
    }


    public void OnClickClose()
    {
        goToMain.SetActive(true);
    }
}
