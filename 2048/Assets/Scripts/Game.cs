using UnityEditor;
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

    public Text scoreText;
    public int score;
    public int count;
    int k;

    public bool isMove, isStop;
    Vector2 startPos, endPos, gap;


    public int screenWidth;



    public static Game instance;

    private void Awake()
    {
        instance = this;

        screenWidth = Screen.width;
        Screen.SetResolution(screenWidth, (screenWidth * 16) / 9, true);
    }

    void Start()
    {
        score = 0;
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
        if(gameOver.activeSelf == true)
        {
            return;
        }

        if(count == 16)
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
                EditorApplication.isPlaying = false;
            }
#endif
            Touch();
        }

        if(isStop == true)
        {
            if(k != 0)
            {
                GenerateNumber();
            }
            k = 0;
            isStop = false;
        }
       
        InitCombine();
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

        count++;
        int x, y;

        while(true)
        {
            x = Random.Range(0, size);
            y = Random.Range(0, size);

            if(slotArray[x, y] == null)
            {
                break;
            }
        }
        GameObject obj;

        if(score > 5000)
        {
            obj = n[1];
        }
        else
        {
            obj = n[0];
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
        //가로 조합 체크
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

        //세로 조합 체크
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
        if (slotArray[x2, y2] == null && slotArray[x1, y1] != null)
        {
            slotArray[x1, y1].GetComponent<Slot>().Move(x2, y2, false);
            slotArray[x2, y2] = slotArray[x1, y1];
            slotArray[x1, y1] = null;

            k++;
        }
        else if (slotArray[x2, y2] != null && slotArray[x1, y1] != null && slotArray[x1, y1].GetComponent<Slot>().num == slotArray[x2, y2].GetComponent<Slot>().num && slotArray[x1, y1].GetComponent<Slot>().isCombine == false && slotArray[x2, y2].GetComponent<Slot>().isCombine == false)
        {
            int i;
            isMove = true;

            for (i = 0; i <= n.Length; i++)
            {
                if (slotArray[x2, y2].name == n[i].name)
                {
                    break;
                }
            }

            slotArray[x1, y1].GetComponent<Slot>().Move(x2, y2, true);

            Destroy(slotArray[x2, y2]);
            count--;

            slotArray[x1, y1] = null;

            slotArray[x2, y2] = Instantiate(n[i+1], GameObject.Find("Canvas/TileSet").transform);
            slotArray[x2, y2].gameObject.name = n[i+1].name;

            slotArray[x2, y2].transform.localPosition = new Vector2((x2 * 320) - xPos, (y2 * 320) - yPos);
            slotArray[x2, y2].transform.rotation = Quaternion.identity;

            slotArray[x2, y2].GetComponent<Slot>().isCombine = true;
            score += (int)Mathf.Pow(2, i + 2);

            k++;
        }
    }


    public void OnClickExit()
    {
        close.SetActive(true);
    }
}

