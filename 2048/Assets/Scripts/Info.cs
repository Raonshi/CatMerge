using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Info : MonoBehaviour
{
    //public GameObject Yes;
    //public GameObject No;
    public Text message;

    public List<GameObject> buttonList = new List<GameObject>();

    private void OnEnable()
    {
        switch (gameObject.name)
        {
            case "GameClose":
                message.text = string.Format("<color=red>게임을 종료하면 고양이가 선물을 잃어버리게 됩니다!</color>\n게임을 종료하시겠습니까?");
                break;

            case "NotEnoughPoint":
                message.text = string.Format("포인트가 부족합니다.\n\n게임 플레이를 통해 포인트를 획득할 수 있습니다.");
                break;

            case "GoToMain":
                message.text = string.Format("메인메뉴로 돌아갑니다.");
                break;

            case "GiftInfo":
                message.text = string.Format("고양이가 선물을 찾아왔습니다!");
                break;
        }
    }

    public void OnClickYes()
    {
        switch (gameObject.name)
        {
            case "GameClose":
                string closeTime = DateTime.Now.ToString();
                PlayerPrefs.SetString("closeTime", closeTime);

                //EditorApplication.isPlaying = false;
                Application.Quit();
                break;

            case "GoToMain":
                SceneManager.LoadScene("MainMenu");
                break;
        }
    }
    public void OnClickNo()
    {
        gameObject.SetActive(false);
    }
}
