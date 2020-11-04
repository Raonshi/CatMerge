using System;
using UnityEngine;
using UnityEngine.UI;


public class Game : MonoBehaviour
{
    //고양이 타일
    public GameObject[,] slotArray;

    //타일 사이즈
    int size = 4;

    //슬롯 기준점
    public float xPos;
    public float yPos;

    //점수 배율 조정
    public int townLevel;   //마을 레벨
    public float scoreRate;   //마을 레벨에 따른 점수 배율

    //현재 점수
    public Text scoreText;
    public int score;

    //최고 점수
    public int best;
    public Text bestText;

    //왕고양이 포인트
    public int point;

    //상태값
    public bool isMove;     //타일이 움직이는 동안 true
    public bool isStop;     //타일이 정지하는 순간 true, 이후 바로 false
    public bool isOver;     //게임 오버시 고양이 타일 이동 금지
    public bool isClose;    //튜토리얼 창 닫을 때 고양이 움직이지 못하도록 하기 위함. 

    //터치 좌표
    Vector2 startPos, endPos, gap;

    //게임 플레이 제한시간
    float maxTime;
    float time;
    public Slider timeSlider;
    public Text timeText;

    //생존 시간
    public float lifeTime;

    //해상도
    public int screenWidth;

    //UI
    public GameObject gameOver;
    public GameObject close;
    public GameObject tutorial1, tutorial2, tutorial3, tutorial4;

    //기타
    int count;  //현재 생성되어 있는 타일의 수
    int k;      //타일 움직임 감지용 변수

    //싱글턴
    public static Game instance;

    private void Awake()
    {
        instance = this;

        //시간 초기화
        maxTime = 90;
        time = maxTime;
        lifeTime = 0;

        //점수 배율 조정
        townLevel = PlayerPrefs.GetInt("townLevel");
        if(townLevel == 1)
        {
            scoreRate = 0;
        }
        else
        {
            scoreRate = (townLevel * 0.25f);
        }

        //매 게임마다 현재 점수 초기화
        score = 0;

        //해상도 조절
        screenWidth = Screen.width;
        Screen.SetResolution(screenWidth, (screenWidth * 16) / 9, true);
    }

    void Start()
    {
        isOver = false;

        timeSlider.maxValue = maxTime;

        //매 게임마다 최고 점수를 불러옴
        best = PlayerPrefs.GetInt("bestScore");

        //알림창 전부 false
        gameOver.SetActive(false);
        close.SetActive(false);
        tutorial1.SetActive(false);
        tutorial2.SetActive(false);
        tutorial3.SetActive(false);
        tutorial4.SetActive(false);

        //고양이 타일 배열의 크기 지정
        slotArray = new GameObject[size, size];

        //고양이 타일 2개 랜덤 생성
        GenerateNumber();
        GenerateNumber();

    }

    private void Update()
    {
        //게임 오버창이 나타나면 고양이를 이동시킬 수 없다.
        if (isOver == true)
        {
            isMove = true;
            return;
        }

        //튜토리얼 알림창이 화면에 없다면 시간이 흘러간다.
        if (GameObject.FindGameObjectsWithTag("Tutorial").Length == 0)
        {
            time -= Time.deltaTime;
        }
        timeSlider.value = time;
        timeText.text = timeSlider.value.ToString();

        //제한시간 동안
        if(time > 0)
        {
            lifeTime += Time.deltaTime;
        }
        //제한시간이 다 되면
        else if(time <= 0)
        {
            time = 0;
            gameOver.SetActive(true);
        }

        //게임 내에 존재하는 고양이의 수를 센다.
        count = GameObject.FindGameObjectsWithTag("Tile").Length + GameObject.FindGameObjectsWithTag("Multiple").Length + GameObject.FindGameObjectsWithTag("Division").Length;

        //점수를 체크한다.
        ScoreCheck();
        
        //이동 중이 아닌 경우
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

        //튜토리얼 창을 닫았을 경우
        if (isClose == true)
        {
            isClose = false;
            isMove = false;
        }

        //이동을 멈춘경우
        if (isStop == true)
        {
            //고양이 수가 16이상이면 타일을 검사한다.
            if (count >= 16)
            {
                TileCheck();
            }

            //이동되거나 결합된 타일을 확인.
            if (k != 0)
            {
                GenerateNumber();
            }
            k = 0;
            isStop = false;
        }
       
        //결합된 고양이의 불 값을 변경
        InitCombine();


        //현재 점수가 최고점보다 높을 경우에 최고점이 실시간으로 올라간다.
        if(score > best)
        {
            best = score;
        }
        bestText.text = best.ToString();

    }

    //결합된 고양이의 불 값을 변경
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

    //새로운 고양이를 생성
    public void GenerateNumber()
    {
        //고양이 타일이 16이상이면 생성하지 않는다.
        if(count >= 16)
        {
            return;
        }

        int x;  //타일의 x좌표
        int y;  //타일의 y좌표
        int i;  //생성될 고양이 타일의 확률

        //타일의 빈공간 검사
        while(true)
        {
            x = UnityEngine.Random.Range(0, size);
            y = UnityEngine.Random.Range(0, size);

            if(slotArray[x, y] == null)
            {
                break;
            }
        }

        //확률 랜덤
        i = UnityEngine.Random.Range(0, 100);

        GameObject obj;

        //x2고양이 생성
        if (i >= 0 && i < 10)
        {
            //곱하기 생성
            obj = Resources.Load("Prefabs/Multiple") as GameObject;
            slotArray[x, y] = Instantiate(obj, GameObject.Find("Canvas/TileSet").transform);
            slotArray[x, y].GetComponent<Slot>().anim.SetBool("isNew", true);
            slotArray[x, y].GetComponent<Slot>().image.sprite = Resources.Load<Sprite>("Images/Cats/Multiple");

            //곱하기 타일 최초 생성이면 튜토리얼 창 활성화
            if(!PlayerPrefs.HasKey("destroy" + tutorial2.name))
            {
                tutorial2.SetActive(true);
            }
            else
            {
                tutorial2.SetActive(false);
            }
        }

        //%2고양이 생성
        else if (i >= 10 && i < 20)
        {
            //나누기 생성
            obj = Resources.Load("Prefabs/Division") as GameObject;
            slotArray[x, y] = Instantiate(obj, GameObject.Find("Canvas/TileSet").transform);
            slotArray[x, y].GetComponent<Slot>().anim.SetBool("isNew", true);
            slotArray[x, y].GetComponent<Slot>().image.sprite = Resources.Load<Sprite>("Images/Cats/Division");

            //나누기 타일 최초 생성이면 튜토리얼 창 활성화
            if (!PlayerPrefs.HasKey("destroy" + tutorial3.name))
            {
                tutorial3.SetActive(true);
            }
            else
            {
                tutorial3.SetActive(false);
            }
        }
        //일반 고양이 생성
        else
        {
            obj = Resources.Load("Prefabs/Tile") as GameObject;
            slotArray[x, y] = Instantiate(obj, GameObject.Find("Canvas/TileSet").transform);
            slotArray[x, y].GetComponent<Slot>().anim.SetBool("isNew", true);
            slotArray[x, y].GetComponent<Slot>().image.sprite = Resources.Load<Sprite>("Images/Cats/2");

            //숫자 타일 최초 생성이면 튜토리얼 창 활성화
            if (!PlayerPrefs.HasKey("destroy" + tutorial1.name))
            {
                tutorial1.SetActive(true);
            }
            else
            {
                tutorial1.SetActive(false);
            }
        }

        slotArray[x, y].gameObject.name = obj.name;
        slotArray[x, y].transform.localPosition = new Vector2((x * 320) - xPos, (y * 320) - yPos);
        slotArray[x, y].transform.rotation = Quaternion.identity;
    }

    //점수 체크
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

    //타일 검사
    public void TileCheck()
    {
        int i = 0;      //결합 가능한 타일 검사

        //열 검사
        for(int x = 0; x < size; x++)
        {
            for(int y = 0; y < size - 1; y++)
            {
                Slot a = slotArray[x, y].GetComponent<Slot>();
                Slot b = slotArray[x, y + 1].GetComponent<Slot>();

                if(a.gameObject.name == "Tile")
                {
                    //b가 숫자이고 a와 같은 수 일 경우
                    if((b.gameObject.name == "Tile" && a.num == b.num) || (b.gameObject.name == "Multiple") || (b.gameObject.name == "Division" && a.num > 2))
                    {
                        i++;
                    }
                }
                else if(a.gameObject.name == "Multiple")
                {
                    if(b.gameObject.name == "Tile")
                    {
                        i++;
                    }
                }
                else if(a.gameObject.name == "Division")
                {
                    if(b.num > 2)
                    {
                        i++;
                    }
                }
            }
        }

        //행 검사
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size - 1; x++)
            {
                Slot a = slotArray[x, y].GetComponent<Slot>();
                Slot b = slotArray[x + 1, y].GetComponent<Slot>();

                if (a.gameObject.name == "Tile")
                {
                    //b가 숫자이고 a와 같은 수 일 경우
                    if ((b.gameObject.name == "Tile" && a.num == b.num) || (b.gameObject.name == "Multiple") || (b.gameObject.name == "Division" && a.num > 2))
                    {
                        i++;
                    }
                }
                else if (a.gameObject.name == "Multiple")
                {
                    if (b.gameObject.name == "Tile")
                    {
                        i++;
                    }
                }
                else if (a.gameObject.name == "Division")
                {
                    if (b.num > 2)
                    {
                        i++;
                    }
                }
            }
        }

        //결합 가능한 타일이 없으면 게임 종료
        if (i == 0)
        {
            gameOver.SetActive(true);
        }
    }

    //터치 조작
    public void Touch()
    {
        //터치 시작점 좌표
        if(Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
        }
        //터치 종료점 좌표
        else if (Input.GetMouseButtonUp(0))
        {
            endPos = Input.mousePosition;
            gap = endPos - startPos;
        }
        //안드로이드 뒤로가기 버튼
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickExit();
        }

        //좌표사이의 거리 절대값
        float x = Mathf.Abs(gap.x);
        float y = Mathf.Abs(gap.y);

        //right, left
        if(x > y)
        {
            if (gap.x > 150)
            {
                Move("right");
                isStop = true;
            }
            else if (gap.x < -150)
            {
                Move("left");
                isStop = true;
            }
        }
        //up, down
        else if(x < y)
        {
            if (gap.y > 150)
            {
                Move("up");
                isStop = true;
            }
            else if (gap.y < -150)
            {
                Move("down");
                isStop = true;
            }
        }
    }


    //타일 이동 명령
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

        //초기화
        gap = Vector2.zero;
        isMove = false;
    }

    //타일 이동 및 결합
    public void MoveOrCombine(int x1, int y1, int x2, int y2)
    {
        //일반 이동
        if (slotArray[x2, y2] == null && slotArray[x1, y1] != null)
        {
            slotArray[x1, y1].GetComponent<Slot>().Move(x2, y2, false);
            slotArray[x2, y2] = slotArray[x1, y1];
            slotArray[x1, y1] = null;

            k++;
        }
        //곱셈 결합
        //조건 : 두 타일이 모두 값이 존재한다 && 둘 중 하나의 타일만 특수 타일이다 && 두 타일 모두 이번 이동에서 결합된 타일이 아니다
        else if (slotArray[x2, y2] != null && slotArray[x1, y1] != null && (slotArray[x1, y1].name == "Multiple" || slotArray[x2, y2].name == "Multiple") &&
            slotArray[x1, y1].GetComponent<Slot>().isCombine == false && slotArray[x2, y2].GetComponent<Slot>().isCombine == false)
        {
            //만약 두 타일 모두 특수타일이면 이동 될 좌표의 타일 결합 상태를 true로 한다.
            if (slotArray[x1, y1].name == "Multiple" && (slotArray[x2, y2].name == "Division" || slotArray[x2, y2].name == "Multiple"))
            {
                slotArray[x2, y2].GetComponent<Slot>().isCombine = true;
                return;
            }
            else if (slotArray[x2, y2].name == "Multiple" && (slotArray[x1, y1].name == "Division" || slotArray[x1, y1].name == "Multiple"))
            {
                slotArray[x2, y2].GetComponent<Slot>().isCombine = true;
                return;
            }

            isMove = true;

            //타일 프리팹 생성 >> 특수타일은 결합으로 생성되지 않으므로 Tile로 고정
            GameObject obj = Resources.Load("Prefabs/Tile") as GameObject;
            
            //변수 i에 이동될 타일의 num값을 입력
            int i = 1;
            if (slotArray[x1, y1].name == "Multiple")
            {
                i = slotArray[x2, y2].GetComponent<Slot>().num;
            }
            else if (slotArray[x2, y2].name == "Multiple")
            {
                i = slotArray[x1, y1].GetComponent<Slot>().num;
            }

            //화면상 이동 >> 이동 후 두 좌표의 오브젝트를 제거
            slotArray[x1, y1].GetComponent<Slot>().Move(x2, y2, true);
            Destroy(slotArray[x2, y2]);
            count--;
            slotArray[x1, y1] = null;

            //게임오브젝트를 생성
            slotArray[x2, y2] = Instantiate(obj, GameObject.Find("Canvas/TileSet").transform);
            slotArray[x2, y2].gameObject.name = obj.name;

            //생성되는 게임 오브젝트는 기존 타일의 진화형태이다.
            slotArray[x2, y2].GetComponent<Slot>().num = i * 2;

            //스프라이트 변경
            slotArray[x2, y2].GetComponent<Slot>().image.sprite = Resources.Load<Sprite>("Images/Cats/" + slotArray[x2, y2].GetComponent<Slot>().num);
            
            //위치 및 로테이션 초기화
            slotArray[x2, y2].transform.localPosition = new Vector2((x2 * 320) - xPos, (y2 * 320) - yPos);
            slotArray[x2, y2].transform.rotation = Quaternion.identity;

            slotArray[x2, y2].GetComponent<Slot>().isCombine = true;
            slotArray[x2, y2].GetComponent<Slot>().anim.SetBool("isCombine", true);

            score += (slotArray[x2, y2].GetComponent<Slot>().num * 10) + Convert.ToInt32((slotArray[x2, y2].GetComponent<Slot>().num * 10 * scoreRate));

            if(slotArray[x2, y2].GetComponent<Slot>().num == 32)
            {
                if (!PlayerPrefs.HasKey("destroy" + tutorial4.name))
                {
                    tutorial4.SetActive(true);
                }
                else
                {
                    tutorial4.SetActive(false);
                }
            }
            else if (slotArray[x2, y2].GetComponent<Slot>().num == 64)
            {
                if (time > maxTime - 5)
                {
                    time = maxTime;
                }
                else
                {
                    time += 5;
                }

                Destroy(slotArray[x2, y2]);
                point++;
            }
            k++;
            
        }
        //나눗셈 결합
        else if (slotArray[x2, y2] != null && slotArray[x1, y1] != null && (slotArray[x1, y1].name == "Division" || slotArray[x2, y2].name == "Division") &&
            slotArray[x1, y1].GetComponent<Slot>().isCombine == false && slotArray[x2, y2].GetComponent<Slot>().isCombine == false)
        {
            if (slotArray[x1, y1].name == "Division" && (slotArray[x2, y2].name == "Multiple" || slotArray[x2, y2].name == "Division"))
            {
                slotArray[x2, y2].GetComponent<Slot>().isCombine = true;
                return;
            }
            else if (slotArray[x2, y2].name == "Division" && (slotArray[x1, y1].name == "Multiple" || slotArray[x1, y1].name == "Division"))
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

            slotArray[x2, y2].GetComponent<Slot>().image.sprite = Resources.Load<Sprite>("Images/Cats/" + slotArray[x2, y2].GetComponent<Slot>().num);

            slotArray[x2, y2].transform.localPosition = new Vector2((x2 * 320) - xPos, (y2 * 320) - yPos);
            slotArray[x2, y2].transform.rotation = Quaternion.identity;

            slotArray[x2, y2].GetComponent<Slot>().isCombine = true;
            slotArray[x2, y2].GetComponent<Slot>().anim.SetBool("isCombine", true);

            //score += slotArray[x2, y2].GetComponent<Slot>().num * 10;

            if (slotArray[x2, y2].GetComponent<Slot>().num == 64)
            {
                if (time > maxTime - 5)
                {
                    time = maxTime;
                }
                else
                {
                    time += 5;
                }

                Destroy(slotArray[x2, y2]);
                point++;
            }
            k++;
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

            slotArray[x2, y2].GetComponent<Slot>().image.sprite = Resources.Load<Sprite>("Images/Cats/" + slotArray[x2, y2].GetComponent<Slot>().num);

            slotArray[x2, y2].transform.localPosition = new Vector2((x2 * 320) - xPos, (y2 * 320) - yPos);
            slotArray[x2, y2].transform.rotation = Quaternion.identity;

            slotArray[x2, y2].GetComponent<Slot>().isCombine = true;
            slotArray[x2, y2].GetComponent<Slot>().anim.SetBool("isCombine", true);

            score += (slotArray[x2, y2].GetComponent<Slot>().num * 10) + Convert.ToInt32((slotArray[x2, y2].GetComponent<Slot>().num * 10 * scoreRate));

            if (slotArray[x2, y2].GetComponent<Slot>().num == 32)
            {
                if (!PlayerPrefs.HasKey("destroy" + tutorial4.name))
                {
                    tutorial4.SetActive(true);
                }
                else
                {
                    tutorial4.SetActive(false);
                }
            }
            else if (slotArray[x2, y2].GetComponent<Slot>().num == 64)
            {
                if (time > maxTime - 5)
                {
                    time = maxTime;
                }
                else
                {
                    time += 5;
                }

                Destroy(slotArray[x2, y2]);
                point++;
            }
            k++;
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

