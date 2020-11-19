using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    //메인화면 버튼
    public GameObject startButton;


    public static Title instance;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("GameManager") == false)
        {
            GameManager.Singleton.InitGameManager();
        }

        //메인 씬 bgm재생
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/BGM_Main"));
    }

    public void OnClickStart()
    {
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Click"));

        GameManager.Singleton.LoadNextScene("Main");
    }
}
