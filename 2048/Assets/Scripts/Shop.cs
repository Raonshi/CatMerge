using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Text point;

    void Start()
    {
        point.text = GameManager.Singleton.totalPoint.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickBuy(int id)
    {
        switch(id)
        {
            case 1:
                GameManager.Singleton.totalPoint -= 5;
                break;
            case 2:
                GameManager.Singleton.totalPoint -= 10;
                break;
        }
    }
}
