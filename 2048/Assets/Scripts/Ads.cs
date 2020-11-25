using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class Ads : MonoBehaviour
{
    private BannerView bannerView;

    // Use this for initialization
    void Start()
    {
        RequestBanner();
    }

    private void RequestBanner()
    {
        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        //Test
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
        
        //Real
        //string adUnitId = "ca-app-pub-1569961743545752/4795583415";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up banner ad before creating a new one.
        if (this.bannerView != null)
        {
            this.bannerView.Destroy();
        }

        AdSize adaptiveSize =
                AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

        this.bannerView = new BannerView(adUnitId, adaptiveSize, AdPosition.Bottom);

        // Register for ad events.
        //앱 실행 시 광고 로드
        this.bannerView.OnAdLoaded += this.HandleAdLoaded;
        //광고 로드에 실패했을 경우
        this.bannerView.OnAdFailedToLoad += this.HandleAdFailedToLoad;
        //광고 클릭 시 광고 URL 열기
        this.bannerView.OnAdOpening += this.HandleAdOpened;
        //URL화면에서 다시 앱으로 돌아오면 호출됨.
        this.bannerView.OnAdClosed += this.HandleAdClosed;
        //URL이 호출되고 앱이 백그라운드로 실행되면 호출됨
        this.bannerView.OnAdLeavingApplication += this.HandleAdLeftApplication;

        AdRequest adRequest = new AdRequest.Builder()
            .AddTestDevice(AdRequest.TestDeviceSimulator)
            .AddTestDevice("03207E138A9B5E8CB3BD14BA72CA86BC")
            .Build();

        // Load a banner ad.
        this.bannerView.LoadAd(adRequest);
    }

    #region Banner callback handlers

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
        MonoBehaviour.print(String.Format("Ad Height: {0}, width: {1}",
            this.bannerView.GetHeightInPixels(),
            this.bannerView.GetWidthInPixels()));
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
                "HandleFailedToReceiveAd event received with message: " + args.Message);
    }

    public void HandleAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }

    public void HandleAdLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeftApplication event received");
    }

    #endregion
}
