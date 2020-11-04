using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public GameObject close;

    public GameObject startButton;
    public GameObject summonButton;
    public GameObject townUpgradeButton;
    public GameObject info;

    public Text point;

    // Start is called before the first frame update
    private void Awake()
    {
        close.SetActive(false);
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
        SceneManager.LoadScene("Game");
    }
}
