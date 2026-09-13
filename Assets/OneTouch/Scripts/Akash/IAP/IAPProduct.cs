using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IAPProduct : MonoBehaviour
{
    public Text priceText;
    public Image purchaseImage;
    public string productId;

    void OnEnable()
    {
        if (!string.IsNullOrEmpty(productId))
        {
          //  priceText.text = IAPManager.Instance.GetPrice(productId);
        }

        CheckForPurchase();
    }

    public void CheckForPurchase()
    {
        if (productId == "complete_pack")
        {
            //if (IAPManager.Instance.IsAlreadyPurchased(productId) || GameData.getInstance().UnlockCompletePack)
            //{
            //    GameData.getInstance().UnlockCompletePack = true;
            //    GameData.getInstance().RemoveAds = true;
            //    priceText.gameObject.SetActive(false);
            //    if (purchaseImage != null)
            //        purchaseImage.gameObject.SetActive(true);
            //    GetComponent<Button>().interactable = false;
            //}
        }

        if (productId == "remove_ads")
        {
            //if (IAPManager.Instance.IsAlreadyPurchased(productId) || GameData.getInstance().RemoveAds)
            //{
            //    priceText.gameObject.SetActive(false);
            //    if (purchaseImage != null)
            //        purchaseImage.gameObject.SetActive(true);
            //    GetComponent<Button>().interactable = false;
            //}
        }
    }

    public void OnBuy()
    {
        if (AudioManager.Instance)
        {
            AudioManager.Instance.ButtonClick();
        }
        if (!string.IsNullOrEmpty(productId))
        {
            // IAPManager.Instance.BuyProductItem(productId);
        }
    }
}
