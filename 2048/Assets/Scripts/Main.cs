using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    //메인화면 알림창
    public GameObject close;
    public GameObject info;
    public GameObject giftInfo;

    //메인화면 버튼
    public GameObject startButton;
    public GameObject summonButton;
    public GameObject townUpgradeButton;

    //포인트
    public Text point;

    //최초 게임시작 유도
    public GameObject tutorial;

    //도움말
    public GameObject help;

    //선물 알림창 활성여부
    public bool isGift;

    public static Main instance;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;

        isGift = true;

        close.SetActive(false);
        tutorial.SetActive(false);
        help.SetActive(false);

        if(GameObject.Find("GameManager") == false)
        {
            GameManager.Singleton.InitGameManager();
        }
    }

    void Start()
    {
        if(GameManager.Singleton.isHelp == true && GameManager.Singleton.isNew == false)
        {
            help.SetActive(true);
            GameManager.Singleton.isHelp = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        point.text = "포인트 : " + GameManager.Singleton.totalPoint;

        if (GameObject.Find("Gift") == true && isGift == true)
        {
            giftInfo.SetActive(true);
            isGift = false;
        }



        if (GameManager.Singleton.isNew == true)
        {
            tutorial.SetActive(true);
            //게임시작 버튼 유도

            summonButton.GetComponent<Button>().interactable = false;
            townUpgradeButton.GetComponent<Button>().interactable = false;

            return;
        }


        if(close.activeSelf == true || info.activeSelf == true)
        {
            startButton.GetComponent<Button>().interactable = false;
            summonButton.GetComponent<Button>().interactable = false;
            townUpgradeButton.GetComponent<Button>().interactable = false;
            return;
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            close.SetActive(!close.activeSelf);

        }

        startButton.GetComponent<Button>().interactable = true;
        summonButton.GetComponent<Button>().interactable = true;
        townUpgradeButton.GetComponent<Button>().interactable = true;
    }

    public void OnClickStart()
    {
        if(GameManager.Singleton.isNew == true)
        {
            tutorial.SetActive(false);
            GameManager.Singleton.isNew = false;
        }


        //고양이 선물시간 계산을 위해 게임 시작 전 현재 시간을 저장
        TimeManager.Singleton.closeTime = DateTime.Now;

        //각 고양이들의 선물 시간을 리스트에 저장
        TimeManager.Singleton.giftTime.Clear();

        for(int i = 0; i < GameObject.FindGameObjectsWithTag("Cat").Length; i++)
        {
            GameObject obj = GameObject.FindGameObjectsWithTag("Cat")[i];
            TimeManager.Singleton.giftTime.Add(obj.GetComponent<Cat>().giftTime);
        }



        SceneManager.LoadScene("Game");
    }

    public void OnClickClose(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void OnClickShop()
    {
        TimeManager.Singleton.closeTime = DateTime.Now;

        TimeManager.Singleton.giftTime.Clear();

        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Cat").Length; i++)
        {
            GameObject obj = GameObject.FindGameObjectsWithTag("Cat")[i];
            TimeManager.Singleton.giftTime.Add(obj.GetComponent<Cat>().giftTime);
        }

        SceneManager.LoadScene("Shop");
    }
}
