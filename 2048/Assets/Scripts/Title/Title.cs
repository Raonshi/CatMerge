using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public GameObject startButton;
    public GameObject gameClose;
    public GameObject newtworkConnect;

    public static Title instance;

    // Start is called before the first frame update
    void Start()
    {
        gameClose.SetActive(false);
        newtworkConnect.SetActive(false);
        startButton.GetComponent<Button>().interactable = true;

        if (GameObject.Find("GameManager") == false)
        {
            GameManager.Singleton.InitGameManager();
        }
        //메인 씬 bgm재생
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/BGM_Main"));


        switch(Application.internetReachability)
        {
            case NetworkReachability.NotReachable:
                startButton.SetActive(false);
                newtworkConnect.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            gameClose.SetActive(true);
        }
    }

    public void OnClickStart()
    {
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Click"));

        GameManager.Singleton.LoadNextScene("Main");
    }
}