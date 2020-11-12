using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Info : MonoBehaviour
{
    public Text message;
    public List<GameObject> button = new List<GameObject>();
    InputField nicknameInput;

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

            case "FullCount":
                message.text = string.Format("최대 소환 한도입니다.");
                break;

            case "GoToMain":
                message.text = string.Format("메인메뉴로 돌아갑니다.");
                break;

            case "GiftInfo":
                message.text = string.Format("고양이가 선물을 찾아왔습니다!");
                break;

            case "CanNotUseItem":
                message.text = string.Format("고양이를 움직여 빈 공간을 만든 후\n아이템을 사용하세요.");
                break;

            case "ItemEmpty":
                message.text = string.Format("아이템을 모두 소진하였습니다");
                break;

            case "Retry":
                button.Clear();
                button.Add(gameObject.transform.Find("Yes").gameObject);
                button.Add(gameObject.transform.Find("No").gameObject);

                Game.instance.isOver = true;
                Game.instance.isClose = true;

                message.text = string.Format("이동 가능한 타일이 없습니다.\n5포인트를 사용하여 타일 1개를 삭제할 수 있습니다.\n(현재 보유 포인트 : {0})\n<color=red>점수는 반영되지 않습니다</color>", GameManager.Singleton.totalPoint);
                break;

            case "Input":
                message.text = string.Format("닉네임을 입력해주세요.");
                nicknameInput = gameObject.transform.Find("InputField").GetComponent<InputField>();
                break;
        }
    }

    public void OnClickYes()
    {
        switch (gameObject.name)
        {
            case "GameClose":
                SaveManager.Singleton.SaveUserJson();
                //EditorApplication.isPlaying = false;
                Application.Quit();
                break;

            case "GoToMain":
                SceneManager.LoadScene("Loading", LoadSceneMode.Additive);
                StartCoroutine(GoToMain());
                break;

            case "Retry":
                if (GameManager.Singleton.totalPoint < 5)
                {
                    Game.instance.notEnoughPoint.SetActive(true);
                    return;
                }

                Game.instance.isOver = false;
                GameManager.Singleton.totalPoint -= 5;
                GameObject obj = Game.instance.slotArray[UnityEngine.Random.Range(0, Game.instance.size), UnityEngine.Random.Range(0, Game.instance.size)];
                Destroy(obj);

                gameObject.SetActive(false);
                break;
        }
    }
    public void OnClickNo()
    {
        
        switch (gameObject.name)
        {
            case "Retry":
                Game.instance.gameOver.SetActive(true);
                gameObject.SetActive(false);
                break;

            case "GoToMain":
                Game.instance.isMove = false;
                gameObject.SetActive(false);
                break;

            default:
                gameObject.SetActive(false);
                break;
        }
    }

    public void OnClickCheck()
    {
        Debug.Log("준비중인 기능 : 닉네임 중복검사");
    }

    public void OnClickApply()
    {
        GameManager.Singleton.nickname = nicknameInput.text;
        SaveManager.Singleton.SaveUserJson();

        transform.parent.gameObject.SetActive(false);
    }

    IEnumerator GoToMain()
    {
        yield return new WaitForSeconds(3.0f);

        TimeManager.Singleton.time = TimeSpan.FromSeconds(Game.instance.lifeTime);
        SaveManager.Singleton.SaveUserJson();
        SceneManager.LoadScene("MainMenu");
    }

}
