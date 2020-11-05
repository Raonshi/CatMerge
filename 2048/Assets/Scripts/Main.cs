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
    public bool isNew;
    public GameObject tutorial;

    //도움말
    public bool isHelp;
    public GameObject help;

    //선물 알림창 활성여부
    public bool isGift;

    // Start is called before the first frame update
    private void Awake()
    {
        isHelp = false;
        isGift = true;

        close.SetActive(false);
        tutorial.SetActive(false);
        help.SetActive(false);

        GameManager.Singleton.InitGameManager();
    }

    void Start()
    {
        if (!PlayerPrefs.HasKey("point"))
        {
            PlayerPrefs.SetInt("point", 200);
        }

        if(!PlayerPrefs.HasKey("isHelp") && PlayerPrefs.HasKey("isNew"))
        {
            help.SetActive(true);
            PlayerPrefs.SetInt("isHelp", 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        point.text = "포인트 : " + PlayerPrefs.GetInt("point");

        if (GameObject.Find("Gift") == true && isGift == true)
        {
            giftInfo.SetActive(true);
            isGift = false;
        }



        if (!PlayerPrefs.HasKey("isNew"))
        {
            isNew = true;
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
        if(isNew == true)
        {
            tutorial.SetActive(false);
            PlayerPrefs.SetInt("isNew", 1);
        }

        string closeTime = DateTime.Now.ToString();

        PlayerPrefs.SetString("closeTime", closeTime);
        SceneManager.LoadScene("Game");
    }

    public void OnClickClose(GameObject obj)
    {
        obj.SetActive(false);
    }
}
