using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Text point;
    public Text cash;

    // Update is called once per frame
    void Update()
    {
        point.text = "포인트 : " + GameManager.Singleton.totalPoint;
        cash.text = "캐쉬 : " + GameManager.Singleton.totalCash;
    }

    public void OnClickBuy(ShopItem item)
    {
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Click"));

        if (item.type == ShopItem.Type.Cash)
        {
            Debug.Log("현금을 결제하였습니다 : " + item.price + "원 결제!=============");
            GameManager.Singleton.totalCash += item.value;
        }
        else if(item.type == ShopItem.Type.Point)
        {
            if (GameManager.Singleton.totalCash < item.price)
            {
                Main.instance.notEnoughCash.SetActive(true);
                return;
            }
            GameManager.Singleton.totalCash -= item.price;
            GameManager.Singleton.totalPoint += item.value;
        }
    }


    public void OnClickClose()
    {
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Click"));
        gameObject.SetActive(false);
    }
}
