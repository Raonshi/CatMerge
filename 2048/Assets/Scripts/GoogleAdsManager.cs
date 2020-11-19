using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleAdsManager : MonoBehaviour
{

    private BannerView bannerView;

    // Start is called before the first frame update
    void Start()
    {
        //appId
        string appId = "ca-app-pub-1569961743545752~6108665085";

        MobileAds.Initialize((iniStatue) => { RequestBanner(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void RequestBanner()
    {
        //test adUnitId
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";

        //real adUnitId
        //string adUnitId = "ca-app-pub-1569961743545752/4795583415";

        this.bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);

        AdRequest request = new AdRequest.Builder().Build();

        this.bannerView.LoadAd(request);
    }
}
