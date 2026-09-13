using UnityEngine;
using UnityEngine.UI;

public class IAPPanel : MonoBehaviour
{
    public Text HintNumber;
    public IAPProduct removeAds;
    public IAPProduct unlockPack;
    public Button restoreButton;

    void OnEnable()
    {
        ShowHintNumber();
        restoreButton.gameObject.SetActive(false);
#if UNITY_IOS
        restoreButton.gameObject.SetActive(true);
        restoreButton.onClick.AddListener(() =>
        {
            RestorePurchase();
        });
#endif
    }

    void RestorePurchase()
    {
        //if (IAPManager.Instance)
        //{
        //    IAPManager.Instance.RestorePurchases();
        //}
    }

    public void ShowHintNumber()
    {
        HintNumber.text = GameData.getInstance().tipRemain.ToString();
    }

    public void OnShowRewardedAds()
    {
        if (AudioManager.Instance)
        {
            AudioManager.Instance.ButtonClick();
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            if (GameData.getInstance().GetSDKLevel() < 26)
            {
                AdmobManager.Instance.ShowRewardAds();
            }
        }
        else
        {
            AdmobManager.Instance.ShowRewardAds();
        }
    }

    public void checkForPurchased()
    {
        removeAds.CheckForPurchase();
        unlockPack.CheckForPurchase();
    }
}
