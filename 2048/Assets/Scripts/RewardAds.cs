using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

public class RewardAds : MonoBehaviour
{
    public RewardedAd rewardedAd;

    public static RewardAds instance;



    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetNewAds();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetNewAds()
    {
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";


        this.rewardedAd = new RewardedAd(adUnitId);

        // 광고 로드 완료시
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // 광고 로드 실패 시
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // 광고 실행중일때(경우에 따라서 게임 루프를 중지하는게 좋다)
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // 광고 표시 실패할 경우
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // 광고 시청 후 보상을 받을 때 실행
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // 닫기아이콘/뒤로가기 버튼 사용하여 광고를 닫을 때 실행
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }


    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdClosed event received");
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        if(SceneManager.GetActiveScene().name == "Game")
        {
            string type = args.Type;
            double amount = args.Amount * 0.2f;

            Debug.Log("amount : " + Convert.ToInt32(amount));

            GameManager.Singleton.totalPoint -= 5;

            for (int i = 0; i < Convert.ToInt32(amount); i++)
            {
                GameObject obj;
                while (true)
                {
                    obj = Game.instance.slotArray[UnityEngine.Random.Range(0, Game.instance.size), UnityEngine.Random.Range(0, Game.instance.size)];
                    if (obj.name != "Block")
                    {
                        break;
                    }
                }
                Destroy(obj);
            }

            MonoBehaviour.print(
    "HandleRewardedAdRewarded event received for "
                + amount.ToString() + " " + type);
        }
        else if(SceneManager.GetActiveScene().name == "Main")
        {
            string type = args.Type;
            double amount = args.Amount * 0.2f;

            GameManager.Singleton.totalPoint += GameManager.Singleton.townLevel * Convert.ToInt32(amount);
            SaveManager.Singleton.SaveUserJson();

            MonoBehaviour.print(
    "HandleRewardedAdRewarded event received for "
                + amount.ToString() + " " + type);
        }
    }
}
