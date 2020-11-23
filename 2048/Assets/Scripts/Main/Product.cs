using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Product : MonoBehaviour
{
    public Text text;
    public int price;


    // Start is called before the first frame update
    void Start()
    {
        price = System.Convert.ToInt32(gameObject.name);
        price = System.Convert.ToInt32(price * 0.5f);
        text.text = string.Format("구매\n- {0} 포인트", price);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CatSpawn()
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/Town/Cat"), GameObject.Find("Canvas/BackGround/CatTown/Image").transform);
        obj.name = "Cat";
        obj.GetComponent<Cat>().image.sprite = Resources.Load<Sprite>("Images/Cats/" + gameObject.name);
        obj.GetComponent<Cat>().isNew = true;

        Main.instance.catList.Add(obj);
    }

    public void OnClickBuy()
    {
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Click"));
        if (GameManager.Singleton.catCount < Main.instance.maxCount)
        {
            if (GameManager.Singleton.totalPoint < price)
            {
                Main.instance.notEnoughPoint.SetActive(true);
                return;
            }
            GameManager.Singleton.totalPoint -= price;

            CatSpawn();

            SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_CatSummon"));

            GameManager.Singleton.catCount++;
        }
        else
        {
            Main.instance.fullCount.SetActive(true);
            return;
        }

        SaveManager.Singleton.SaveUserJson();
    }
}
