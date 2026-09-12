using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class MainMenuPanel : MonoBehaviour
{
    public static MainMenuPanel Instance;
    public GameObject shape;
    public Image SoundButton;
    public Sprite soundOn;
    public Sprite soundOff;
    public GameObject giftButton;
    public GiftPanel giftPanel;
    public IAPPanel IAPPanel;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        ScreenFading.Instance.FadeOut();

        if (AudioManager.Instance)
        {
            if (AudioManager.Instance.IsSound)
            {
                SoundButton.sprite = soundOn;
            }
            else
            {
                SoundButton.sprite = soundOff;
            }
        }

        string time = PlayerPrefs.GetString("giftTimer", DateTime.Now.ToString());
        DateTime giftTime = DateTime.Parse(time);

        if (giftTime <= DateTime.Now)
        {
            giftButton.SetActive(true);
        }
        else
        {
            giftButton.SetActive(false);
        }

    }

    public void OnPlayButtonClicked()
    {
        shape.SetActive(false);

        if (AudioManager.Instance)
        {
            AudioManager.Instance.ButtonClick();
        }

        ScreenFading.Instance.FadeIn(() =>
        {
            SceneManager.LoadScene("LevelMenu");
        });
    }

    public void OnSoundButtonClicked()
    {
        if (AudioManager.Instance)
        {
            AudioManager.Instance.ButtonClick();
        }

        if (AudioManager.Instance)
        {
            if (AudioManager.Instance.IsSound)
            {
                AudioManager.Instance.IsSound = false;
                SoundButton.sprite = soundOff;
            }
            else
            {
                AudioManager.Instance.IsSound = true;
                SoundButton.sprite = soundOn;
            }
        }
    }

    public void OnCartButtonClicked()
    {
        if (AudioManager.Instance)
        {
            AudioManager.Instance.ButtonClick();
        }
    }

    public void OnShareButtonClicked()
    {
        if (AudioManager.Instance)
        {
            AudioManager.Instance.ButtonClick();
        }
        string link = "";
#if UNITY_IOS
        link = "https://itunes.apple.com/us/app/1line-puzzle-mania/id1345325072?ls=1&mt=8";
#elif UNITY_ANDROID
        link = "https://play.google.com/store/apps/details?id=com.linegames.oneline";
#endif
        NativeShare.Share("Play 1 Line-Puzzle Game\n" + link, null, null, "1 Line Puzzle", "text/plain", true);
    }

    public void OnGiftButtonClicked()
    {
        if (AudioManager.Instance)
        {
            AudioManager.Instance.ButtonClick();
        }

        GameData.getInstance().tipRemain++;
        IAPPanel.ShowHintNumber();
        giftPanel.ShowGiftText("CONGRATULATIONS\n" + "YOUR DAILY NUMBER OF HINTS INCREASED TO " + GameData.getInstance().tipRemain + ".");
        giftPanel.gameObject.SetActive(true);

        giftButton.SetActive(false);
        PlayerPrefs.SetString("giftTimer", DateTime.Now.AddHours(24).ToString());
    }

    public void OnLeaderboardClicked()
    {
        if (LeaderBoard.Instance)
        {
            LeaderBoard.Instance.ShowLeaderboard();
        }
    }

    public void OnRateUsButtonClicked()
    {
        string link = "";
#if UNITY_IOS
        link = "https://itunes.apple.com/us/app/1line-puzzle-mania/id1345325072?ls=1&mt=8";
#elif UNITY_ANDROID
        link = "https://play.google.com/store/apps/details?id=com.linegames.oneline";
#endif
        Application.OpenURL(link);
    }
}
