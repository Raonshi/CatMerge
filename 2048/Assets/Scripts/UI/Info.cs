﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 알림창의 메시지 및 버튼별 동작
/// </summary>
public class Info : MonoBehaviour
{
    public Text message;

    private void OnEnable()
    {
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Popup"));

        switch (gameObject.name)
        {
            case "Banner":

                message.text = string.Format("CAT\n\nMERGE");

                break;
            case "NetworkConnect":

                message.text = string.Format("네트워크에 연결 후 게임을 실행해주세요");

                break;

            case "GameClose":
                if(SceneManager.GetActiveScene().name == "Title")
                {
                    message.text = string.Format("게임을 종료하시겠습니까?");
                }
                else if(SceneManager.GetActiveScene().name == "Main")
                {
                    message.text = string.Format("<color=red>게임을 종료하면 고양이가 선물을 잃어버리게 됩니다!</color>\n게임을 종료하시겠습니까?");
                }
                break;

            case "NotEnoughCash":
                message.text = string.Format("캐쉬가 부족합니다.\n\n상점으로 이동하여 캐쉬를 충전하시겠습니까?");
                break;

            case "NotEnoughLife":
                message.text = string.Format("게임 가능 횟수가 부족합니다.\n\n잠시 후 다시 게임을 시작하거나\n<color=red>5캐쉬를 사용하여 충전할 수 있습니다.</color>");

                if (GameManager.Singleton.totalCash < 5)
                {
                    GameObject.Find(gameObject.name + "/Yes").GetComponent<Button>().interactable = false;
                }
                else
                {
                    GameObject.Find(gameObject.name + "/Yes").GetComponent<Button>().interactable = true;
                }

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
                Game.instance.isOver = true;
                Game.instance.isClose = true;

                message.text = string.Format("이동 가능한 타일이 없습니다.\n5포인트를 사용하여 타일 1개를 삭제하겠습니까?\n(추가로 광고를 시청하면 타일 2개를 삭제할 수 있습니다.)\n(현재 보유 포인트 : {0})\n<color=red>점수는 반영되지 않습니다</color>", GameManager.Singleton.totalPoint);
                break;

            case "AdGiftPanel":
                message.text = string.Format("광고를 시청하면 더 많은 보상을 얻을 수 있습니다.\n시청하겠습니까?");
                break;

            case "DataSave":
                message.text = string.Format("현재까지의 기록을 저장했습니다.");
                break;

            case "DataLoad":
                message.text = string.Format("저장되어 있던 기록을 불러왔습니다.");
                break;

            case "DataSaveFail":
                message.text = string.Format("저장에 실패했습니다!");
                break;

            case "DataLoadFail":
                message.text = string.Format("불러오기에 실패했습니다!");
                break;
        }
    }

    public void OnClickYes()
    {
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Click"));

        switch (gameObject.name)
        {
            case "NetworkConnect":
                SaveManager.Singleton.SaveUserJson();
                Application.Quit();
                break;

            case "GameClose":
                SaveManager.Singleton.SaveUserJson();
                Application.Quit();
                break;

            case "GoToMain":
                TimeManager.Singleton.time = TimeSpan.FromSeconds(Game.instance.lifeTime);
                SaveManager.Singleton.SaveUserJson();
                GameManager.Singleton.LoadNextScene("Main");
                break;

            case "Retry":
                if (GameManager.Singleton.totalPoint < 5)
                {
                    Game.instance.notEnoughPoint.SetActive(true);
                    return;
                }

                Game.instance.isOver = false;
                GameManager.Singleton.totalPoint -= 5;
                GameObject obj;
                while (true)
                {
                    obj = Game.instance.slotArray[UnityEngine.Random.Range(0, Game.instance.size), UnityEngine.Random.Range(0, Game.instance.size)];
                    if(obj.name != "Block")
                    {
                        break;
                    }
                }
                Destroy(obj);

                gameObject.SetActive(false);
                break;

            case "AdGiftPanel":


                SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_TimeRecovery"));

                if(RewardAds.instance.rewardedAd.IsLoaded() == true)
                {
                    RewardAds.instance.rewardedAd.Show();
                }

                RewardAds.instance.SetNewAds();

                gameObject.SetActive(false);

                break;

            case "NotEnoughCash":
                Main.instance.shopPanel.SetActive(true);
                break;

            case "NotEnoughLife":
                GameManager.Singleton.totalCash -= 5;
                GameManager.Singleton.life = 5;
                gameObject.SetActive(false);
                break;
        }
    }
    public void OnClickNo()
    {
        SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_Click"));

        switch (gameObject.name)
        {
            case "Retry":
                Game.instance.gameOver.SetActive(true);
                gameObject.SetActive(false);
                break;

            case "GoToMain":
                Game.instance.isClose = true;
                gameObject.SetActive(false);
                break;

            case "AdGiftPanel":

                SoundManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX_TimeRecovery"));

                GameManager.Singleton.totalPoint += GameManager.Singleton.townLevel;
                SaveManager.Singleton.SaveUserJson();

                gameObject.SetActive(false);

                break;

            default:
                gameObject.SetActive(false);
                break;
        }
    }

    public void OnClickAds()
    {
        if (GameManager.Singleton.totalPoint < 5)
        {
            Game.instance.notEnoughPoint.SetActive(true);
            return;
        }


        if (RewardAds.instance.rewardedAd.IsLoaded() == true)
        {
            RewardAds.instance.rewardedAd.Show();
        }

        RewardAds.instance.SetNewAds();

        gameObject.SetActive(false);
    }

}
