using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftPanel : MonoBehaviour
{
    public Text giftText;

    public void ShowGiftText(string name)
    {
        giftText.text = name;
    }
}
