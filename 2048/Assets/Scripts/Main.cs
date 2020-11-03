using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public GameObject close;
    public GameObject rule;

    public GameObject startButton;
    public GameObject ruleButton;

    public Text point;
    // Start is called before the first frame update
    private void Awake()
    {
        close.SetActive(false);
        rule.SetActive(false);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        point.text = "POINT : " + PlayerPrefs.GetInt("point");
        if(rule.activeSelf == true || close.activeSelf == true)
        {
            startButton.GetComponent<Button>().interactable = false;
            ruleButton.GetComponent<Button>().interactable = false;
            return;
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            close.SetActive(!close.activeSelf);

        }

        startButton.GetComponent<Button>().interactable = true;
        ruleButton.GetComponent<Button>().interactable = true;
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnClickRule()
    {
        rule.SetActive(true);
    }
}
