using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    //메인화면 알림창
    public GameObject close;
    public GameObject info;

    //메인화면 버튼
    public GameObject startButton;
    public GameObject summonButton;
    public GameObject townUpgradeButton;

    //포인트
    public Text point;

    //처음 시작
    public bool isNew;
    public GameObject tutorial;

    // Start is called before the first frame update
    private void Awake()
    {
        close.SetActive(false);
        tutorial.SetActive(false);
    }

    void Start()
    {
        if(!PlayerPrefs.HasKey("point"))
        {
            PlayerPrefs.SetInt("point", 200);
        }
    }

    // Update is called once per frame
    void Update()
    {
        point.text = "포인트 : " + PlayerPrefs.GetInt("point");

        if(!PlayerPrefs.HasKey("isNew"))
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
        SceneManager.LoadScene("Game");
    }
}
