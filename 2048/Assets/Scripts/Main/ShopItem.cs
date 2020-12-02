using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public enum Type
    {
        Cash,
        Point,
    }
    public Type type;

    public int price;
    public int value;

    public Image image;


    private void OnEnable()
    {
        InitShopItem();
    }


    public void InitShopItem()
    {
        switch(type)
        {
            case Type.Cash:
                image.sprite = Resources.Load<Sprite>("Images/UI/Cash");
                break;

            case Type.Point:
                image.sprite = Resources.Load<Sprite>("Images/UI/Point");
                break;
        }
    }
}
