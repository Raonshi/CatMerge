using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 게임 내에서 타일의 생성을 담당한다.
/// </summary>
public class Spawner : MonoBehaviour
{
    public static Spawner instance;

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// 현재 게임에서 존재하는 타일이 16개 미만일 경우 실행되며 타일이 없는 랜덤좌표에 2, 곱하기, 나누기 타일 중 하나를 생성한다.
    /// </summary>
    public void TileSpawn()
    {
        if (Game.instance.count >= 16)
        {
            return;
        }

        int x, y, i;

        while (true)
        {
            x = Random.Range(0, Game.instance.size);
            y = Random.Range(0, Game.instance.size);

            if (Game.instance.slotArray[x, y] == null)
            {
                break;
            }
        }
        i = Random.Range(0, 100);

        GameObject obj;


        //x2고양이 생성
        if ((i >= 0 && i < 5) && Game.instance.score >= 5000)
        {
            //곱하기 생성
            obj = Resources.Load("Prefabs/Tiles/Multiple") as GameObject;
            Game.instance.slotArray[x, y] = Instantiate(obj, GameObject.Find("Canvas/TileSet").transform);
            Game.instance.slotArray[x, y].GetComponent<Slot>().anim.SetBool("isNew", true);
            Game.instance.slotArray[x, y].GetComponent<Slot>().image.sprite = Resources.Load<Sprite>("Images/Cats/Multiple");

            //곱하기 타일 최초 생성이면 튜토리얼 창 활성화
            if (GameManager.Singleton.tutorial2 == true)
            {
                Game.instance.tutorial2.SetActive(true);
            }
        }

        //%2고양이 생성
        else if ((i >= 5 && i < 10) && Game.instance.score >= 10000)
        {
            //나누기 생성
            obj = Resources.Load("Prefabs/Tiles/Division") as GameObject;
            Game.instance.slotArray[x, y] = Instantiate(obj, GameObject.Find("Canvas/TileSet").transform);
            Game.instance.slotArray[x, y].GetComponent<Slot>().anim.SetBool("isNew", true);
            Game.instance.slotArray[x, y].GetComponent<Slot>().image.sprite = Resources.Load<Sprite>("Images/Cats/Division");

            //나누기 타일 최초 생성이면 튜토리얼 창 활성화
            if (GameManager.Singleton.tutorial3 == true)
            {
                Game.instance.tutorial3.SetActive(true);
            }
        }
        //일반 고양이 생성
        else
        {
            obj = Resources.Load("Prefabs/Tiles/Tile") as GameObject;
            Game.instance.slotArray[x, y] = Instantiate(obj, GameObject.Find("Canvas/TileSet").transform);
            Game.instance.slotArray[x, y].GetComponent<Slot>().anim.SetBool("isNew", true);
            Game.instance.slotArray[x, y].GetComponent<Slot>().image.sprite = Resources.Load<Sprite>("Images/Cats/2");

            //숫자 타일 최초 생성이면 튜토리얼 창 활성화
            if (GameManager.Singleton.tutorial1 == true)
            {
                Game.instance.tutorial1.SetActive(true);
            }
        }

        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Generate"));

        Game.instance.slotArray[x, y].gameObject.name = obj.name;
        Game.instance.slotArray[x, y].transform.localPosition = new Vector2((x * Game.instance.slotGap) - Game.instance.xPos, (y * Game.instance.slotGap) - Game.instance.yPos);
        Game.instance.slotArray[x, y].transform.rotation = Quaternion.identity;
    }


    /// <summary>
    /// 보통난이도일 경우 4곳의 귀퉁이에 블록 타일을 생성한다.
    /// 어려움난이도일 경우 랜덤한 위치에 블록 타일을 생성한다.
    /// </summary>
    public void BlockSpawn()
    {
        if (Game.instance.blockCount == 4)
        {
            return;
        }
        int x = 0;
        int y = 0;

        //노말모드는 블럭 모서리에만 생성
        if (GameManager.Singleton.difficulty == GameManager.Difficulty.Normal)
        {
            int[] array = new int[2] { 0, 3 };
            bool exit = false;

            //4 귀퉁이의 빈공간을 검사
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < array.Length; j++)
                {
                    x = array[i];
                    y = array[j];

                    if (Game.instance.slotArray[x, y] == null)
                    {
                        exit = true;
                        break;
                    }
                }
                //빈공간이 있다면 이중루프 탈출
                if (exit == true)
                {
                    break;
                }
            }

            //빈공간이 없을 경우
            //Block이 아닌 칸에 생성
            if (exit == false)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    for (int j = 0; j < array.Length; j++)
                    {
                        x = array[i];
                        y = array[j];

                        if (Game.instance.slotArray[x, y].name != "Block")
                        {
                            Destroy(Game.instance.slotArray[x, y]);
                            exit = true;
                            break;
                        }
                    }
                    if (exit == true)
                    {
                        break;
                    }
                }
            }

            if (exit == false)
            {
                return;
            }
            GameObject obj = Resources.Load("Prefabs/Tiles/Block") as GameObject;
            Game.instance.slotArray[x, y] = Instantiate(obj, GameObject.Find("Canvas/TileSet").transform);
            Game.instance.slotArray[x, y].GetComponent<Slot>().anim.SetBool("isNew", true);
            Game.instance.slotArray[x, y].GetComponent<Slot>().image.sprite = Resources.Load<Sprite>("Images/Cats/Block");

            Game.instance.slotArray[x, y].gameObject.name = obj.name;
            Game.instance.slotArray[x, y].transform.localPosition = new Vector2((x * Game.instance.slotGap) - Game.instance.xPos, (y * Game.instance.slotGap) - Game.instance.yPos);
            Game.instance.slotArray[x, y].transform.rotation = Quaternion.identity;

            SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Generate"));
        }
        //하드모드는 블럭 랜덤 생성
        else if (GameManager.Singleton.difficulty == GameManager.Difficulty.Hard)
        {
            List<GameObject> list = new List<GameObject>();

            while (true)
            {

                x = Random.Range(0, Game.instance.size);
                y = Random.Range(0, Game.instance.size);

                if (list.Count >= 16)
                {
                    Destroy(Game.instance.slotArray[x, y]);
                    break;
                }


                //빈 타일이 존재하거나 모든 타일의 중복검사를 끝냈을 경우
                if (Game.instance.slotArray[x, y] == null)
                {
                    break;
                }
                //slotArray[x, y]가 null이 아니라면
                else
                {
                    if (list.Count == 0)
                    {
                        list.Add(Game.instance.slotArray[x, y]);
                    }
                    else
                    {
                        bool duplicate = false;
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (Game.instance.slotArray[x, y] == list[i])
                            {
                                duplicate = true;
                                break;
                            }
                        }
                        //중복값이 아니라면 list에 추가
                        if (duplicate == false)
                        {
                            list.Add(Game.instance.slotArray[x, y]);
                        }
                    }
                }

            }
            GameObject obj = Resources.Load("Prefabs/Tiles/Block") as GameObject;
            Game.instance.slotArray[x, y] = Instantiate(obj, GameObject.Find("Canvas/TileSet").transform);
            Game.instance.slotArray[x, y].GetComponent<Slot>().anim.SetBool("isNew", true);
            Game.instance.slotArray[x, y].GetComponent<Slot>().image.sprite = Resources.Load<Sprite>("Images/Cats/Block");

            Game.instance.slotArray[x, y].gameObject.name = obj.name;
            Game.instance.slotArray[x, y].transform.localPosition = new Vector2((x * Game.instance.slotGap) - Game.instance.xPos, (y * Game.instance.slotGap) - Game.instance.yPos);
            Game.instance.slotArray[x, y].transform.rotation = Quaternion.identity;

            SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Generate"));
        }
        Game.instance.blockCount++;
    }
}

