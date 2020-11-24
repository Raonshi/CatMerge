using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    public GameObject startButton;
    public GameObject gameClose;

    public static Title instance;

    // Start is called before the first frame update
    void Start()
    {
        gameClose.SetActive(false);

        if (GameObject.Find("GameManager") == false)
        {
            GameManager.Singleton.InitGameManager();
        }
        //메인 씬 bgm재생
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/BGM_Main"));
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