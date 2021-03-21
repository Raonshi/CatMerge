using System;
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

                message.text = string.Format("M E R G E\n\nC A T");

                break;
            case "NetworkConnect":

                message.text = string.Format("Please Connect Network");

                break;

            case "GameClose":
                if(SceneManager.GetActiveScene().name == "Title")
                {
                    message.text = string.Format("Are you sure you want to quit the game?");
                }
                else if(SceneManager.GetActiveScene().name == "Main")
                {
                    message.text = string.Format("<color=red>Cats will lose their gifts when you quit the game!</color>\n\nAre you sure you want to quit the game?");
                }
                break;
                
            case "NotEnoughLife":
                message.text = string.Format("There is not enough\n\nnumber of games available.");
                break;
                

            case "NotEnoughPoint":
                message.text = string.Format("Not enough points.\n\nYou can earn points through gameplay.");
                break;

            case "FullCount":
                message.text = string.Format("This is the maximum summon limit.");
                break;

            case "GoToMain":
                message.text = string.Format("Return to the main menu.");
                break;

            case "GiftInfo":
                message.text = string.Format("The gift has arrived!");
                break;

            case "CanNotUseItem":
                message.text = string.Format("Move the cat to create an empty space\nbefore using the item.");
                break;

            case "ItemEmpty":
                message.text = string.Format("There is no item.");
                break;

            case "Retry":
                Game.instance.isOver = true;
                Game.instance.isClose = true;

                message.text = string.Format("There are no movable tiles.\nWould you like to use 5 points to delete 1 tile?\n(You can delete 2 tiles when you watch the ad.)\n(Current point : {0})", GameManager.Singleton.totalPoint);
                break;

            case "AdGiftPanel":
                message.text = string.Format("You can get more rewards by watching an ad\nWould you like to watch it?");
                break;

            case "DataSave":
                message.text = string.Format("SAVE Success");
                break;

            case "DataLoad":
                message.text = string.Format("LOAD Success");
                break;

            case "DataSaveFail":
                message.text = string.Format("SAVE Fail!");
                break;

            case "DataLoadFail":
                message.text = string.Format("LOAD Fail!");
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
