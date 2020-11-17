using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Difficult : MonoBehaviour
{
    public Text text;
    public Image buttonImage;
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Singleton.townLevel < 3)
        {
            
            buttonImage.color = Color.grey;
            text.fontSize = 40;
            text.text = "마을 Lv3 이상\n해금";
        }
        else
        {
            Color color;
            text.fontSize = 70;
            switch (GameManager.Singleton.difficulty)
            {
                case GameManager.Difficulty.Easy:
                    ColorUtility.TryParseHtmlString("#98E05A", out color);
                    buttonImage.color = color;
                    text.text = "쉬움";
                    break;

                case GameManager.Difficulty.Normal:
                    ColorUtility.TryParseHtmlString("#FA8748", out color);
                    buttonImage.color = color;
                    text.text = "보통";
                    break;

                case GameManager.Difficulty.Hard:
                    ColorUtility.TryParseHtmlString("#FF4B38", out color);
                    buttonImage.color = color;
                    text.text = "어려움";
                    break;
            }
        }
    }


    public void OnClickDifficulty()
    {
        if(GameManager.Singleton.townLevel < 3)
        {
            SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Disable"));

            return;
        }

        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Click"));
        switch (GameManager.Singleton.difficulty)
        {
            case GameManager.Difficulty.Easy:
                GameManager.Singleton.difficulty = GameManager.Difficulty.Normal ;
                break;

            case GameManager.Difficulty.Normal:
                GameManager.Singleton.difficulty = GameManager.Difficulty.Hard;
                break;

            case GameManager.Difficulty.Hard:
                GameManager.Singleton.difficulty = GameManager.Difficulty.Easy;
                break;
        }
    
    }
}
