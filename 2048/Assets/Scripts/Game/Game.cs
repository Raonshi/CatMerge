using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Game : MonoBehaviour
{
    //고양이 타일
    public GameObject[,] slotArray;

    //타일 사이즈
    public int size = 4;

    //슬롯 기준점
    public float xPos;
    public float yPos;

    //현재 점수
    public Text scoreText;
    public int score;

    //최고 점수
    public Text bestText;

    //왕고양이 포인트, 시간
    public int point;           //1게임에서 획득한 총 포인트
    public int recoveryTime;    //왕고양이 제거시 얻는 시간

    //상태값
    public bool isMove;     //타일이 움직이는 동안 true
    public bool isStop;     //타일이 정지하는 순간 true, 이후 바로 false
    public bool isOver;     //게임 오버시 고양이 타일 이동 금지
    public bool isClose;    //튜토리얼 창 닫을 때 고양이 움직이지 못하도록 하기 위함. 
    public bool isHalf;     //남은 시간 절반

    //터치 좌표
    public Vector2 startPos, endPos, gap;

    //게임 플레이 제한시간
    public float maxTime;
    public float time;
    public Slider timeSlider;
    public Text timeText;

    //생존 시간
    public float lifeTime;

    //노말, 하드모드
    public float hardTime;
    public int blockCount;

    //UI
    public GameObject retry;
    public GameObject notEnoughPoint;
    public GameObject gameOver;
    public GameObject close;
    public GameObject tutorial0, tutorial1, tutorial2, tutorial3;
    public GameObject numEnable;
    public GameObject halfTime;
    public GameObject optionPanel;

    //기타
    public int count;  //현재 생성되어 있는 타일의 수
    public int k;      //타일 움직임 감지용 변수

    //싱글턴
    public static Game instance;




    private void Awake()
    {
        instance = this;

        isMove = false;
        isStop = false;


        //시간 초기화
        maxTime = 90;
        time = maxTime;
        lifeTime = 0;

        //매 게임마다 현재 점수 초기화
        score = 0;
    }

    void Start()
    {
        //게임 씬 bgm재생
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/BGM_Game"));

        isOver = false;
        isHalf = true;

        //숫자 보기 안보기 토글값 불러옴
        if(GameManager.Singleton.isNum == true)
        {
            numEnable.GetComponent<Toggle>().isOn = true;
        }
        else
        {
            numEnable.GetComponent<Toggle>().isOn = false;
        }

        //게임 시작하자마자 시간을 풀충전해야함
        timeSlider.maxValue = maxTime;
        timeSlider.value = maxTime;
        timeText.text = Convert.ToInt32(timeSlider.value).ToString();

        //알림창 전부 false
        retry.SetActive(false);
        notEnoughPoint.SetActive(false);
        gameOver.SetActive(false);
        close.SetActive(false);

        //옵션창 false
        optionPanel.SetActive(false);

        //튜토리얼 창 false
        tutorial1.SetActive(false);
        tutorial2.SetActive(false);
        tutorial3.SetActive(false);

        //고양이 타일 배열의 크기 지정
        slotArray = new GameObject[size, size];

        //고양이 타일 2개 랜덤 생성
        Spawner.instance.TileSpawn();
        Spawner.instance.TileSpawn();

        if (GameManager.Singleton.tutorial0 == true)
        {
            tutorial0.SetActive(true);
        }

    }

    private void Update()
    {
        //패널 화면이 없으면 시간이 흘러간다.
        if (GameObject.FindGameObjectsWithTag("InfoPanel").Length == 0)
        {
            time -= Time.deltaTime;
        }
        else if(GameObject.FindGameObjectsWithTag("InfoPanel").Length != 0)
        {
            isMove = true;
            return;
        }

        //게임 내에 존재하는 고양이의 수를 센다.
        count = GameObject.Find("Canvas/TileSet").transform.childCount;

        //제한시간 동안
        if (time > 0)
        {
            lifeTime += Time.deltaTime;
            hardTime += Time.deltaTime;

            //1초 동안 시간 절반 지남음을 표시
            if(time >= (maxTime * 0.5f) - 1 && time <= maxTime * 0.5f && isHalf == true)
            {
                isHalf = false;
                halfTime.SetActive(true);
            }
        }
        //제한시간이 다 되면
        else if (time <= 0)
        {
            time = 0;
            gameOver.SetActive(true);
        }

        //hardTime이 20초 될때마다 블럭을 막는다.
        if(hardTime >= 20 && (GameManager.Singleton.difficulty == GameManager.Difficulty.Normal || GameManager.Singleton.difficulty == GameManager.Difficulty.Hard))
        {
            Spawner.instance.BlockSpawn();
            hardTime = 0;
        }

        timeSlider.value = time;
        timeText.text = Convert.ToInt32(timeSlider.value).ToString();

        //점수를 체크한다.
        ScoreCheck();
        
        //이동 중이 아닌 경우
        if(isMove == false)
        {
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
            //이동되거나 결합된 타일을 확인.
            if (k != 0)
            {
                Spawner.instance.TileSpawn();
            }

            //고양이 수가 16이상이면 타일을 검사한다.
            if (count >= 16)
            {
                TileCheck();
            }
            k = 0;
            isStop = false;

            //고양이 숫자 표시
            EnableNum();
        }
       
        //결합된 고양이의 불 값을 변경
        InitCombine();


        //현재 점수가 최고점보다 높을 경우에 최고점이 실시간으로 올라간다.
        if(score > GameManager.Singleton.best)
        {
            GameManager.Singleton.best = score;
        }
        bestText.text = GameManager.Singleton.best.ToString();
    }


    #region 기능
    //고양이 숫자 보기 / 안보기 -> true : 숫자 안보임      false : 숫자 보임
    public void EnableNum()
    {

        if (GameManager.Singleton.isNum == false)
        {
            GameObject tileSet = GameObject.Find("Canvas/TileSet");
            int tmp = tileSet.transform.childCount;
            for (int i = 0; i < tmp; i++)
            {
                Slot slot = tileSet.transform.GetChild(i).GetComponent<Slot>();
                if(slot == null)
                {
                    continue;
                }
                slot.isNum = true;
            }
        }
        else
        {
            GameObject tileSet = GameObject.Find("Canvas/TileSet");
            int tmp = tileSet.transform.childCount;
            for (int i = 0; i < tmp; i++)
            {
                if(tileSet.transform.GetChild(i).name == "TimeRecovery")
                {
                    continue;
                }
                Slot slot = tileSet.transform.GetChild(i).GetComponent<Slot>();
                if (slot == null)
                {
                    continue;
                }
                slot.isNum = false;
            }
        }
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

        //결합 가능한 타일이 없으면 리트라이 패널 출력
        if (i == 0)
        {
            retry.SetActive(true);
        }
    }

    //터치 조작
    public void Touch()
    {
        //터치 시작점 좌표
        if (Input.GetMouseButtonDown(0))
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
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickExit();
        }

        //좌표사이의 거리 절대값
        float x = Mathf.Abs(gap.x);
        float y = Mathf.Abs(gap.y);

        //right, left
        if (x > y)
        {
            if (gap.x > 150)
            {
                Move("right");
            }
            else if (gap.x < -150)
            {
                Move("left");
            }
            isStop = true;
        }
        //up, down
        else if (x < y)
        {
            if (gap.y > 150)
            {
                Move("up");
            }
            else if (gap.y < -150)
            {
                Move("down");
            }
            isStop = true;
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
            if(slotArray[x1, y1].name == "Block")
            {
                return;
            }
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
            int i = 1;

            //만약 두 타일 모두 특수타일이면 이동 될 좌표의 타일 결합 상태를 true로 한다.
            if (slotArray[x1, y1].name == "Multiple")
            {
                if(slotArray[x2, y2].name == "Division" || slotArray[x2, y2].name == "Multiple" || slotArray[x2, y2].name == "Block")
                {
                    slotArray[x2, y2].GetComponent<Slot>().isCombine = true;
                    return;
                }
                i = slotArray[x2, y2].GetComponent<Slot>().num;
            }
            else if (slotArray[x2, y2].name == "Multiple")
            {
                if(slotArray[x1, y1].name == "Division" || slotArray[x1, y1].name == "Multiple" || slotArray[x1, y1].name == "Block")
                {
                    slotArray[x2, y2].GetComponent<Slot>().isCombine = true;
                    return;
                }
                i = slotArray[x1, y1].GetComponent<Slot>().num;
            }

            isMove = true;

            //타일 프리팹 생성 >> 특수타일은 결합으로 생성되지 않으므로 Tile로 고정
            GameObject obj = Resources.Load("Prefabs/Tiles/Tile") as GameObject;

            //화면상 이동 >> 이동 후 두 좌표의 오브젝트를 제거
            slotArray[x1, y1].GetComponent<Slot>().Move(x2, y2, true);

            slotArray[x1, y1] = null;
            count--;

            if (i * 2 >= 64)
            {
                score += ((i * 2) * 10) + Convert.ToInt32((i * 2) * 10 * GameManager.Singleton.scoreRate);

                if (time > maxTime - recoveryTime)
                {
                    time = maxTime;
                }
                else
                {
                    time += recoveryTime;
                }

                GameObject timeRecovery = Instantiate(Resources.Load<GameObject>("Prefabs/TimeRecovery"));
                timeRecovery.name = "TimeRecovery";
                timeRecovery.transform.SetParent(GameObject.Find("Canvas").transform);
                timeRecovery.GetComponent<Animator>().SetBool("Create", true);

                SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_TimeRecovery"));

                point++;

                StartCoroutine(Complete64(slotArray[x2, y2].transform.localPosition = new Vector2((x2 * 270) - xPos, (y2 * 270) - yPos)));
                DestroyImmediate(slotArray[x2, y2]);
            }
            else
            {
                Destroy(slotArray[x2, y2]);
                //게임오브젝트를 생성
                slotArray[x2, y2] = Instantiate(obj, GameObject.Find("Canvas/TileSet").transform);
                slotArray[x2, y2].gameObject.name = obj.name;

                //생성되는 게임 오브젝트는 기존 타일의 진화형태이다.
                slotArray[x2, y2].GetComponent<Slot>().num = i * 2;

                //스프라이트 변경
                slotArray[x2, y2].GetComponent<Slot>().image.sprite = Resources.Load<Sprite>("Images/Cats/" + slotArray[x2, y2].GetComponent<Slot>().num);

                //위치 및 로테이션 초기화
                slotArray[x2, y2].transform.localPosition = new Vector2((x2 * 270) - xPos, (y2 * 270) - yPos);
                slotArray[x2, y2].transform.rotation = Quaternion.identity;

                slotArray[x2, y2].GetComponent<Slot>().isCombine = true;
                slotArray[x2, y2].GetComponent<Slot>().anim.SetBool("isCombine", true);

                score += (slotArray[x2, y2].GetComponent<Slot>().num * 10) + Convert.ToInt32((slotArray[x2, y2].GetComponent<Slot>().num * 10 * GameManager.Singleton.scoreRate));
            }
            k++;
            
        }
        //나눗셈 결합
        else if (slotArray[x2, y2] != null && slotArray[x1, y1] != null && (slotArray[x1, y1].name == "Division" || slotArray[x2, y2].name == "Division") &&
            slotArray[x1, y1].GetComponent<Slot>().isCombine == false && slotArray[x2, y2].GetComponent<Slot>().isCombine == false)
        {
            int i = 1;

            if (slotArray[x1, y1].name == "Division")
            {
                if(slotArray[x2, y2].name == "Multiple" || slotArray[x2, y2].name == "Division" || slotArray[x2, y2].name == "Block" || slotArray[x2, y2].GetComponent<Slot>().num == 2)
                {
                    slotArray[x2, y2].GetComponent<Slot>().isCombine = true;
                    return;
                }
                i = slotArray[x2, y2].GetComponent<Slot>().num;
            }
            else if (slotArray[x2, y2].name == "Division")
            {
                if (slotArray[x1, y1].name == "Multiple" || slotArray[x1, y1].name == "Division" || slotArray[x1, y1].name == "Block" || slotArray[x1, y1].GetComponent<Slot>().num == 2)
                {
                    slotArray[x2, y2].GetComponent<Slot>().isCombine = true;
                    return;
                }
                i = slotArray[x1, y1].GetComponent<Slot>().num;
            }

            isMove = true;

            GameObject obj = Resources.Load("Prefabs/Tiles/Tile") as GameObject;

            slotArray[x1, y1].GetComponent<Slot>().Move(x2, y2, true);

            Destroy(slotArray[x2, y2]);
            count--;

            slotArray[x1, y1] = null;

            slotArray[x2, y2] = Instantiate(obj, GameObject.Find("Canvas/TileSet").transform);
            slotArray[x2, y2].gameObject.name = obj.name;
            slotArray[x2, y2].GetComponent<Slot>().num = Convert.ToInt32(i * 0.5f);

            slotArray[x2, y2].GetComponent<Slot>().image.sprite = Resources.Load<Sprite>("Images/Cats/" + slotArray[x2, y2].GetComponent<Slot>().num);

            slotArray[x2, y2].transform.localPosition = new Vector2((x2 * 270) - xPos, (y2 * 270) - yPos);
            slotArray[x2, y2].transform.rotation = Quaternion.identity;

            slotArray[x2, y2].GetComponent<Slot>().isCombine = true;
            slotArray[x2, y2].GetComponent<Slot>().anim.SetBool("isCombine", true);
            k++;
        }
        //일반 결합
        else if (slotArray[x2, y2] != null && slotArray[x1, y1] != null && slotArray[x1, y1].GetComponent<Slot>().num == slotArray[x2, y2].GetComponent<Slot>().num &&
            slotArray[x1, y1].GetComponent<Slot>().num != 1 && slotArray[x2, y2].GetComponent<Slot>().num != 1 &&
            slotArray[x1, y1].GetComponent<Slot>().isCombine == false && slotArray[x2, y2].GetComponent<Slot>().isCombine == false)
        {
            isMove = true;

            GameObject obj = Resources.Load("Prefabs/Tiles/Tile") as GameObject;
            int i, j;

            i = slotArray[x1, y1].GetComponent<Slot>().num;
            j = slotArray[x2, y2].GetComponent<Slot>().num;

            slotArray[x1, y1].GetComponent<Slot>().Move(x2, y2, true);

            slotArray[x1, y1] = null;
            count--;

            if(i + j >= 64)
            {
                score += ((i + j) * 10) + Convert.ToInt32((i + j) * 10 * GameManager.Singleton.scoreRate);

                if (time > maxTime - recoveryTime)
                {
                    time = maxTime;
                }
                else
                {
                    time += recoveryTime;
                }

                GameObject timeRecovery = Instantiate(Resources.Load<GameObject>("Prefabs/TimeRecovery"));
                timeRecovery.name = "TimeRecovery";
                timeRecovery.transform.SetParent(GameObject.Find("Canvas").transform);
                timeRecovery.GetComponent<Animator>().SetBool("Create", true);

                SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_TimeRecovery"));

                point++;

                StartCoroutine(Complete64(slotArray[x2, y2].transform.localPosition = new Vector2((x2 * 270) - xPos, (y2 * 270) - yPos)));
                DestroyImmediate(slotArray[x2, y2]);
            }
            else
            {
                Destroy(slotArray[x2, y2]);

                slotArray[x2, y2] = Instantiate(obj, GameObject.Find("Canvas/TileSet").transform);
                slotArray[x2, y2].gameObject.name = obj.name;
                slotArray[x2, y2].GetComponent<Slot>().num = j + i;

                slotArray[x2, y2].GetComponent<Slot>().image.sprite = Resources.Load<Sprite>("Images/Cats/" + slotArray[x2, y2].GetComponent<Slot>().num);

                slotArray[x2, y2].transform.localPosition = new Vector2((x2 * 270) - xPos, (y2 * 270) - yPos);
                slotArray[x2, y2].transform.rotation = Quaternion.identity;

                slotArray[x2, y2].GetComponent<Slot>().isCombine = true;
                slotArray[x2, y2].GetComponent<Slot>().anim.SetBool("isCombine", true);

                score += (slotArray[x2, y2].GetComponent<Slot>().num * 10) + Convert.ToInt32((slotArray[x2, y2].GetComponent<Slot>().num * 10 * GameManager.Singleton.scoreRate));
            }
            k++;
        }
        //특수블럭 간 결합 못하도록 함
        else if (slotArray[x2, y2] != null && slotArray[x1, y1] != null && slotArray[x1, y1].GetComponent<Slot>().num == 1 && slotArray[x2, y2].GetComponent<Slot>().num == 1 && slotArray[x1, y1].GetComponent<Slot>().isCombine == false && slotArray[x2, y2].GetComponent<Slot>().isCombine == false)
        {
            if (slotArray[x1, y1].name == "Block" || slotArray[x2, y2].name == "Block")
            {
                k++;
                return;
            }
            slotArray[x1, y1].GetComponent<Slot>().Move(x2, y2, false);
            slotArray[x2, y2] = slotArray[x1, y1];
            slotArray[x1, y1] = null;
        }
    }
    #endregion

    public void OnClickNumber()
    {
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Click"));

        if (numEnable.GetComponent<Toggle>().isOn == true)
        {
            GameManager.Singleton.isNum = true;
        }
        else
        {
            GameManager.Singleton.isNum = false;
        }
        EnableNum();
    }


    public void OnClickExit()
    {
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Click"));

        close.SetActive(true);
    }

    public void OnClickOption()
    {
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Popup"));

        optionPanel.SetActive(true);
    }


    #region 코루틴

    IEnumerator Complete64(Vector2 position)
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/CompleteEffect"), GameObject.Find("Canvas").transform);
        obj.name = "Tile";
        obj.transform.localPosition = position;
        obj.transform.rotation = Quaternion.identity;

        yield return new WaitForSeconds(1.5f);

        Destroy(obj);
    }

    #endregion
}

