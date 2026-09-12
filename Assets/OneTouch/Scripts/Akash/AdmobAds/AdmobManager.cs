using UnityEngine;
using GoogleMobileAds.Api;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class AdmobManager : MonoBehaviour
{
    public static AdmobManager Instance;

    public bool Test;
    public string testintertitialIdAndroid;
    public string testintertitialIdIOS;
    [Space(10)]
    public string testrewardIdAndroid;
    public string testrewardIdIOS;
    [Space(10)]
    public string testBannerIdAndroid;
    public string testBannerIdIOS;

    [Space(20)]
    public string intertitialIdAndroid;
    public string intertitialIdIOS;

    [Space(10)]
    public string rewardIdAndroid;
    public string rewardIdIOS;

    [Space(10)]
    public string bannerIdAndroid;
    public string bannerIdIOS;


    InterstitialAd interstitial;
    RewardBasedVideoAd rewardBasedVideo;
    BannerView bannerView;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    void Start()
    {
#if UNITY_ANDROID
        string appId = "ca-app-pub-5331758561510209~6124159495";
#elif UNITY_IPHONE
        string appId = "ca-app-pub-5331758561510209~5190351770 ";
#else
            string appId = "unexpected_platform";
#endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);

        this.rewardBasedVideo = RewardBasedVideoAd.Instance;
        rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;

        RequestInterstitial();
        RequestRewardedVideo();
    }

    public void RequestBannerAd()
    {
        string adUnitId;

        if (GameData.getInstance().IsAdsAvailable)
        {
            if (!Test)
            {
#if UNITY_ANDROID
                adUnitId = bannerIdAndroid;
#elif UNITY_IPHONE
                adUnitId = bannerIdIOS;
#else
         adUnitId = "unexpected_platform";
#endif
            }
            else
            {
#if UNITY_ANDROID
                adUnitId = testBannerIdAndroid;
#elif UNITY_IPHONE
                adUnitId = testBannerIdIOS;
#else
                adUnitId = "unexpected_platform";
#endif
            }

            bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the interstitial with the request.
            bannerView.LoadAd(request);
        }
    }

    private void RequestInterstitial()
    {
        string adUnitId;

        if (GameData.getInstance().IsAdsAvailable)
        {
            if (!Test)
            {
#if UNITY_ANDROID
                adUnitId = intertitialIdAndroid;
#elif UNITY_IPHONE
                adUnitId = intertitialIdIOS;
#else
         adUnitId = "unexpected_platform";
#endif
            }
            else
            {
#if UNITY_ANDROID
                adUnitId = testintertitialIdAndroid;
#elif UNITY_IPHONE
                adUnitId = testintertitialIdIOS;
#else
                adUnitId = "unexpected_platform";
#endif
            }

            // Initialize an InterstitialAd.
            interstitial = new InterstitialAd(adUnitId);
            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the interstitial with the request.
            interstitial.LoadAd(request);
        }
    }

    private void RequestRewardedVideo()
    {
        string adUnitId;
        if (!Test)
        {
#if UNITY_ANDROID
            adUnitId = rewardIdAndroid;
#elif UNITY_IPHONE
            adUnitId = rewardIdIOS;
#else
             adUnitId = "unexpected_platform";
#endif
        }
        else
        {
#if UNITY_ANDROID
            adUnitId = testrewardIdAndroid;
#elif UNITY_IPHONE
            adUnitId = testrewardIdIOS;
#else
             adUnitId = "unexpected_platform";
#endif
        }

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded video ad with the request.
        this.rewardBasedVideo.LoadAd(request, adUnitId);
    }


    public void ShowInterstitialAd()
    {
        if (GameData.getInstance().IsAdsAvailable)
        {
            if (interstitial.IsLoaded())
            {
                interstitial.Show();
            }
            else
            {
                RequestInterstitial();
            }
        }
    }

    public void ShowRewardAds()
    {
        if (rewardBasedVideo.IsLoaded())
        {
            rewardBasedVideo.Show();
        }
        else
        {
            RequestRewardedVideo();
        }
    }


    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        GameData.getInstance().tipRemain += 1;
        if (MainMenuPanel.Instance)
        {
            MainMenuPanel.Instance.IAPPanel.ShowHintNumber();
        }
        if (SceneManager.GetActiveScene().name == "Game")
        {
            GameData.getInstance().level.RefreshHintView();
        }
    }

    public void ShowBannerAds()
    {
        if (bannerView != null)
        {
            bannerView.Show();
        }
        else
        {
            RequestBannerAd();
        }
    }

    public void HideBannerAds()
    {
        if (bannerView != null)
        {
            bannerView.Hide();
        }
    }
}
