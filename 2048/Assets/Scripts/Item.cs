using System;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public GameObject canNotUseItem;
    public GameObject itemEmpty;

    public int itemCount;
    public Text itemText;


    private void Awake()
    {
        canNotUseItem.SetActive(false);
        itemEmpty.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.name == "Item1")
        {
            itemText.text = GameManager.Singleton.item1Count.ToString();
        }
        else if (gameObject.name == "Item2")
        {
            itemText.text = GameManager.Singleton.item2Count.ToString();
        }
    }

    //츄르 아이템 사용(고양이 타일 1개 랜덤 제거)
    //1. 왕고양이 타일 제거시 : 포인트 증가
    //2. 그 외 고양이 타일 제거시 : 포인트 증가 안함 / 점수만 증가
    //3. 특수 타일 제거 시 : 아무런 이득이 없음.
    public void OnClickItem1()
    {
        if (GameManager.Singleton.item1Count == 0)
        {
            itemEmpty.SetActive(true);
            return;
        }
        //EraseCat(1, 2);
        CreateCat(1);
    }

    //참치캔 사용(고양이 타일 최대 5개 제거)
    //1. 왕고양이 타일 제거시 : 포인트 증가
    //2. 그 외 고양이 타일 제거시 : 포인트 증가 안함 / 점수만 증가
    //3. 특수 타일 제거 시 : 아무런 이득이 없음.
    public void OnClickItem2()
    {
        if(GameManager.Singleton.item2Count == 0)
        {
            itemEmpty.SetActive(true);
            return;
        }

        //EraseCat(5, 6);
        CreateCat(3);
    }

    /// <summary>
    /// 고양이 생성
    /// </summary>
    /// <param name="count">생성할 마리 수</param>
    public void CreateCat(int count)
    {
        int tmp = GameObject.FindGameObjectsWithTag("Tile").Length + GameObject.FindGameObjectsWithTag("Multiple").Length + GameObject.FindGameObjectsWithTag("Division").Length;
        if (tmp >= 16)
        {
            canNotUseItem.SetActive(true);
            return;
        }

        if (gameObject.name == "Item1")
        {
            GameManager.Singleton.item1Count--;
        }
        else if (gameObject.name == "Item2")
        {
            GameManager.Singleton.item2Count--;
        }

        for (int i = 0; i < count; i++)
        {
            tmp = GameObject.FindGameObjectsWithTag("Tile").Length + GameObject.FindGameObjectsWithTag("Multiple").Length + GameObject.FindGameObjectsWithTag("Division").Length;
            GameObject obj;

            if (tmp >= 16)
            {
                return;
            }

            int x;  //타일의 x좌표
            int y;  //타일의 y좌표
            int rate;  //생성될 고양이 타일의 확률

            //타일의 빈공간 검사
            while (true)
            {
                x = UnityEngine.Random.Range(0, Game.instance.size);
                y = UnityEngine.Random.Range(0, Game.instance.size);

                if (Game.instance.slotArray[x, y] == null)
                {
                    break;
                }
            }

            rate = UnityEngine.Random.Range(1, 6);

            obj = Resources.Load("Prefabs/Tile") as GameObject;
            Game.instance.slotArray[x, y] = Instantiate(obj, GameObject.Find("Canvas/TileSet").transform);
            Game.instance.slotArray[x, y].GetComponent<Slot>().anim.SetBool("isNew", true);
            Game.instance.slotArray[x, y].GetComponent<Slot>().image.sprite = Resources.Load<Sprite>("Images/Cats/" + Convert.ToInt32(Mathf.Pow(2, rate)));

            Game.instance.slotArray[x, y].gameObject.name = obj.name;
            Game.instance.slotArray[x, y].GetComponent<Slot>().num = Convert.ToInt32(Mathf.Pow(2,rate));

            Game.instance.slotArray[x, y].transform.localPosition = new Vector2((x * 270) - Game.instance.xPos, (y * 270) - Game.instance.yPos);
            Game.instance.slotArray[x, y].transform.rotation = Quaternion.identity;
        }

        SaveManager.Singleton.SaveItemJson();
    }
}
