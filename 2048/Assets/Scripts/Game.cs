using System;
using UnityEngine;
using UnityEngine.UI;


public class Game : MonoBehaviour
{
    public GameObject[,] slotArray;
    public GameObject[] n;

    public GameObject gameOver;
    public GameObject close;

    public int size = 4;

    public float xPos;
    public float yPos;

    //현재 점수
    public Text scoreText;
    public int score;

    //최고 점수
    public Text bestText;
    public int best;

    public int count;
    public int point;

    public bool isMove, isStop;
    Vector2 startPos, endPos, gap;


    public float time;
    public Text timeText;


    public int screenWidth;



    public static Game instance;

    private void Awake()
    {
        instance = this;
        time = 60;

        screenWidth = Screen.width;
        Screen.SetResolution(screenWidth, (screenWidth * 16) / 9, true);
    }

    void Start()
    {
        //매 게임마다 현재 점수 초기화
        score = 0;

        //매 게임마다 최고 점수를 불러옴
        best = PlayerPrefs.GetInt("bestScore");

        gameOver.SetActive(false);
        close.SetActive(false);
        isMove = false;

        slotArray = new GameObject[size, size];

        //숫자 2개 생성(2:90% / 4:10%) -> 랜덤한 위치 / 겹치면안됨

        GenerateNumber();
        GenerateNumber();

    }

    private void Update()
    {
        time -= Time.deltaTime;
        timeText.text = Convert.ToInt32(time).ToString();

        if(time <= 0)
        {
            time = 0;
            gameOver.SetActive(true);
        }

        if(gameOver.activeSelf == true)
        {
            return;
        }

        count = GameObject.FindGameObjectsWithTag("Tile").Length;

        if (count == 16)
        {
            TileCheck();
        }
        
        ScoreCheck();
        
        if(isMove == false)
        {
#if UNITY_EDITOR
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                Move("up");
                isStop = true;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Move("down");
                isStop = true;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Move("right");
                isStop = true;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Move("left");
                isStop = true;
            }
            else if(Input.GetKeyDown(KeyCode.Escape))
            {
                OnClickExit();
            }
#endif
            Touch();
        }

        if(isStop == true)
        {

            GenerateNumber();
            isStop = false;
        }
       
        InitCombine();


        //현재 점수가 최고점보다 높을 경우에 최고점이 실시간으로 올라간다.
        if(score > best)
        {
            best = score;
        }
        bestText.text = best.ToString();

    }

    public void InitCombine()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (slotArray[x, y] == null)
                {
                    continue;
                }
                slotArray[x, y].GetComponent<Slot>().isCombine = false;
            }
        }
    }

    public void GenerateNumber()
    {
        if(count >= 16)
        {
            return;
        }

        int x, y, i;

        while(true)
        {
            x = UnityEngine.Random.Range(0, size);
            y = UnityEngine.Random.Range(0, size);

            if(slotArray[x, y] == null)
            {
                break;
            }
        }
        i = UnityEngine.Random.Range(0, 100);

        GameObject obj = Resources.Load("Prefabs/Tile") as GameObject;


        if (i >= 0 && i < 5)
        {
            //조커 생성
            obj = Resources.Load("Prefabs/Joker") as GameObject;
        }

        else if (i >= 5 && i < 15)
        {
            //곱하기 생성
            obj = Resources.Load("Prefabs/Multiple") as GameObject;
        }

        else if (i >= 15 && i < 25)
        {
            //나누기 생성
            obj = Resources.Load("Prefabs/Division") as GameObject;
        }
        

        slotArray[x, y] = Instantiate(obj, GameObject.Find("Canvas/TileSet").transform);
        slotArray[x, y].gameObject.name = obj.name;

        slotArray[x, y].transform.localPosition = new Vector2((x * 320) - xPos, (y * 320) - yPos);
        slotArray[x, y].transform.rotation = Quaternion.identity;
    }

    public void ScoreCheck()
    {
        if(score == 0)
        {
            scoreText.text = "0";
        }
        else
        {
            scoreText.text = score.ToString();
        }
    }

    public void TileCheck()
    {
        int i = 0;
        for(int x = 0; x < size; x++)
        {
            for(int y = 0; y < size - 1; y++)
            {
                if(slotArray[x,y].name == slotArray[x, y+1].name)
                {
                    i++;
                }
            }
        }

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size - 1; x++)
            {
                if (slotArray[x, y].name == slotArray[x + 1, y].name)
                {
                    i++;
                }
            }
        }

        if (i == 0)
        {
            gameOver.SetActive(true);
        }
    }


    public void Touch()
    {
        if(Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            endPos = Input.mousePosition;
            gap = endPos - startPos;
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickExit();
        }

        if(gap.x > 250)
        {
            Move("right");
            isStop = true;
        }
        else if(gap.x < -250)
        {
            Move("left");
            isStop = true;
        }
        else if(gap.y > 250)
        {
            Move("up");
            isStop = true;
        }
        else if(gap.y < -250)
        {
            Move("down");
            isStop = true;
        }
    }



    public void Move(string dir)
    {
        isMove = true;

        switch(dir)
        {
            case "up":
                for (int x = 0; x < size; x++)
                {
                    for (int loop = 0; loop < size-1; loop++)
                    {
                        for (int y = size - 1; y >= loop + 1; y--)
                        {
                            MoveOrCombine(x, y - 1, x, y);
                        }
                    }
                }
                isMove = false;
                break;

            case "down":
                for (int x = 0; x < size; x++)
                {
                    for (int loop = size - 1; loop >= 1; loop--)
                    {
                        for (int y = 0; y < loop; y++)
                        {
                            MoveOrCombine(x, y + 1, x, y);
                        }
                    }
                }
                break;

            case "right":
                for (int y = 0; y < size; y++)
                {
                    for (int loop = 0; loop < size - 1; loop++)
                    {
                        for (int x = size - 1; x >= loop + 1; x--)
                        {
                            MoveOrCombine(x - 1, y, x, y);
                        }
                    }
                }
                break;

            case "left":
                for (int y = 0; y < size; y++)
                {
                    for (int loop = size - 1; loop >= 1; loop--)
                    {
                        for (int x = 0; x < loop; x++)
                        {
                            MoveOrCombine(x + 1, y, x, y);
                        }
                    }
                }
                break;
        }
        gap = Vector2.zero;
        isMove = false;
    }

    public void MoveOrCombine(int x1, int y1, int x2, int y2)
    {
        //일반 이동
        if (slotArray[x2, y2] == null && slotArray[x1, y1] != null)
        {
            slotArray[x1, y1].GetComponent<Slot>().Move(x2, y2, false);
            slotArray[x2, y2] = slotArray[x1, y1];
            slotArray[x1, y1] = null;

            
        }
        //조커 결합 >> 조커가 (x2, y2)일 경우 해당 라인의 모든 타일이 제거되는 버그가 있음.
        else if(slotArray[x2, y2] != null && slotArray[x1, y1] != null && (slotArray[x1, y1].name == "Joker" || slotArray[x2, y2].name == "Joker") &&
            slotArray[x1, y1].GetComponent<Slot>().isCombine == false && slotArray[x2, y2].GetComponent<Slot>().isCombine == false)
        {
            if(slotArray[x1, y1].name == "Joker" && (slotArray[x2,y2].name == "Multiple" || slotArray[x2, y2].name == "Division" || slotArray[x2, y2].name == "Joker"))
            {
                return;
            }
            else if(slotArray[x2, y2].name == "Joker" && (slotArray[x1, y1].name == "Multiple" || slotArray[x1, y1].name == "Division" || slotArray[x1, y1].name == "Joker"))
            {
                return;
            }
            isMove = true;

            slotArray[x1, y1].GetComponent<Slot>().Move(x2, y2, true);

            Destroy(slotArray[x2, y2]);
            count--;

            slotArray[x1, y1] = null;

            if (time > 55)
            {
                time = 60;
            }
            else
            {
                time += 5;
            }

            score += 64;
            point++;

            
        }
        //곱셈 결합
        else if (slotArray[x2, y2] != null && slotArray[x1, y1] != null && (slotArray[x1, y1].name == "Multiple" || slotArray[x2, y2].name == "Multiple") &&
            slotArray[x1, y1].GetComponent<Slot>().isCombine == false && slotArray[x2, y2].GetComponent<Slot>().isCombine == false)
        {
            if (slotArray[x1, y1].name == "Multiple" && (slotArray[x2, y2].name == "Joker" || slotArray[x2, y2].name == "Division" || slotArray[x2, y2].name == "Multiple"))
            {
                slotArray[x2, y2].GetComponent<Slot>().isCombine = true;
                return;
            }
            else if (slotArray[x2, y2].name == "Multiple" && (slotArray[x1, y1].name == "Joker" || slotArray[x1, y1].name == "Division" || slotArray[x2, y2].name == "Multiple"))
            {
                slotArray[x2, y2].GetComponent<Slot>().isCombine = true;
                return;
            }

            isMove = true;

            GameObject obj = Resources.Load("Prefabs/Tile") as GameObject;
            int i = 1;

            if (slotArray[x1, y1].name == "Multiple")
            {
                i = slotArray[x2, y2].GetComponent<Slot>().num;
            }
            else if (slotArray[x2, y2].name == "Multiple")
            {
                i = slotArray[x1, y1].GetComponent<Slot>().num;
            }

            slotArray[x1, y1].GetComponent<Slot>().Move(x2, y2, true);

            Destroy(slotArray[x2, y2]);
            count--;

            slotArray[x1, y1] = null;

            slotArray[x2, y2] = Instantiate(obj, GameObject.Find("Canvas/TileSet").transform);
            slotArray[x2, y2].gameObject.name = obj.name;
            slotArray[x2, y2].GetComponent<Slot>().num = i * 2;

            slotArray[x2, y2].transform.localPosition = new Vector2((x2 * 320) - xPos, (y2 * 320) - yPos);
            slotArray[x2, y2].transform.rotation = Quaternion.identity;

            slotArray[x2, y2].GetComponent<Slot>().isCombine = true;

            score += slotArray[x2, y2].GetComponent<Slot>().num;

            if (slotArray[x2, y2].GetComponent<Slot>().num == 64)
            {
                if (time > 55)
                {
                    time = 60;
                }
                else
                {
                    time += 5;
                }
                
                Destroy(slotArray[x2, y2]);
                point++;
            }
            
        }
        //나눗셈 결합
        else if (slotArray[x2, y2] != null && slotArray[x1, y1] != null && (slotArray[x1, y1].name == "Division" || slotArray[x2, y2].name == "Division") &&
            slotArray[x1, y1].GetComponent<Slot>().isCombine == false && slotArray[x2, y2].GetComponent<Slot>().isCombine == false)
        {
            if (slotArray[x1, y1].name == "Division" && (slotArray[x2, y2].name == "Joker" || slotArray[x2, y2].name == "Multiple" || slotArray[x2, y2].name == "Division"))
            {
                slotArray[x2, y2].GetComponent<Slot>().isCombine = true;
                return;
            }
            else if (slotArray[x2, y2].name == "Division" && (slotArray[x1, y1].name == "Joker" || slotArray[x1, y1].name == "Multiple" || slotArray[x2, y2].name == "Division"))
            {
                slotArray[x2, y2].GetComponent<Slot>().isCombine = true;
                return;
            }

            isMove = true;

            GameObject obj = Resources.Load("Prefabs/Tile") as GameObject;
            int i = 1;

            if (slotArray[x1, y1].name == "Division")
            {
                if(slotArray[x2, y2].GetComponent<Slot>().num == 2)
                {
                    slotArray[x2, y2].GetComponent<Slot>().isCombine = true;
                    return;
                }
                i = slotArray[x2, y2].GetComponent<Slot>().num;
            }
            else if (slotArray[x2, y2].name == "Division")
            {
                if (slotArray[x1, y1].GetComponent<Slot>().num == 2)
                {
                    slotArray[x2, y2].GetComponent<Slot>().isCombine = true;
                    return;
                }
                i = slotArray[x1, y1].GetComponent<Slot>().num;
            }

            slotArray[x1, y1].GetComponent<Slot>().Move(x2, y2, true);

            Destroy(slotArray[x2, y2]);
            count--;

            slotArray[x1, y1] = null;

            slotArray[x2, y2] = Instantiate(obj, GameObject.Find("Canvas/TileSet").transform);
            slotArray[x2, y2].gameObject.name = obj.name;
            slotArray[x2, y2].GetComponent<Slot>().num = i / 2;

            slotArray[x2, y2].transform.localPosition = new Vector2((x2 * 320) - xPos, (y2 * 320) - yPos);
            slotArray[x2, y2].transform.rotation = Quaternion.identity;

            slotArray[x2, y2].GetComponent<Slot>().isCombine = true;

            score += slotArray[x2, y2].GetComponent<Slot>().num;

            if (slotArray[x2, y2].GetComponent<Slot>().num == 64)
            {
                if (time > 55)
                {
                    time = 60;
                }
                else
                {
                    time += 5;
                }

                Destroy(slotArray[x2, y2]);
                point++;
            }

        }
        //일반 결합
        else if (slotArray[x2, y2] != null && slotArray[x1, y1] != null && slotArray[x1, y1].GetComponent<Slot>().num == slotArray[x2, y2].GetComponent<Slot>().num &&
            slotArray[x1, y1].GetComponent<Slot>().num != 1 && slotArray[x2, y2].GetComponent<Slot>().num != 1 &&
            slotArray[x1, y1].GetComponent<Slot>().isCombine == false && slotArray[x2, y2].GetComponent<Slot>().isCombine == false)
        {
            isMove = true;

            GameObject obj = Resources.Load("Prefabs/Tile") as GameObject;
            int i, j;

            i = slotArray[x1, y1].GetComponent<Slot>().num;
            j = slotArray[x2, y2].GetComponent<Slot>().num;

            slotArray[x1, y1].GetComponent<Slot>().Move(x2, y2, true);

            Destroy(slotArray[x2, y2]);
            count--;

            slotArray[x1, y1] = null;

            slotArray[x2, y2] = Instantiate(obj, GameObject.Find("Canvas/TileSet").transform);
            slotArray[x2, y2].gameObject.name = obj.name;
            slotArray[x2, y2].GetComponent<Slot>().num = j + i;

            slotArray[x2, y2].transform.localPosition = new Vector2((x2 * 320) - xPos, (y2 * 320) - yPos);
            slotArray[x2, y2].transform.rotation = Quaternion.identity;

            slotArray[x2, y2].GetComponent<Slot>().isCombine = true;

            score += slotArray[x2, y2].GetComponent<Slot>().num;

            if (slotArray[x2, y2].GetComponent<Slot>().num == 64)
            {
                if (time > 55)
                {
                    time = 60;
                }
                else
                {
                    time += 5;
                }

                Destroy(slotArray[x2, y2]);
                point++;
            }
        }
        //특수블럭 간 결합 못하도록 함
        else if (slotArray[x2, y2] != null && slotArray[x1, y1] != null && slotArray[x1, y1].GetComponent<Slot>().num == 0 && slotArray[x2, y2].GetComponent<Slot>().num == 0 && slotArray[x1, y1].GetComponent<Slot>().isCombine == false && slotArray[x2, y2].GetComponent<Slot>().isCombine == false)
        {
            slotArray[x1, y1].GetComponent<Slot>().Move(x2, y2, false);
            slotArray[x2, y2] = slotArray[x1, y1];
            slotArray[x1, y1] = null;
        }
    }


    public void OnClickExit()
    {
        close.SetActive(true);
    }
}

