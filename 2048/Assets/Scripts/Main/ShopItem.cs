using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
