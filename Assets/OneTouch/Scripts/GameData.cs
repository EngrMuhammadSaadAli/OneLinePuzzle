using UnityEngine;

public class GameData
{
    public int nLink = 0; //check in game.When nlink = 0.All the lines linked,so win.
    public int currentLevel = 0;//currect level....i.e 50,51,52....499,500

    public int currentStage;//Current Stage
    public int stageLevel;//Current Stage Level i.e 1,2,3....
    public string currentlevelName;//Level 1....Level 2 etc...
    public bool gotoNextLevel;
    public bool hasHint;

    public Color currentColor;

    public int AdsCounter;

    public bool IsRated
    {
        get
        {
            return PlayerPrefs.GetInt("RateUs", 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("RateUs", value ? 1 : 0);
        }
    }

    public bool UnlockCompletePack
    {
        get
        {
            return PlayerPrefs.GetInt("UnlockPack", 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("UnlockPack", value ? 1 : 0);
        }
    }

    public bool RemoveAds
    {
        get
        {
            return PlayerPrefs.GetInt("RemoveAds", 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("RemoveAds", value ? 1 : 0);
        }
    }


    public int tipRemain
    {
        get
        {
            return PlayerPrefs.GetInt("tipRemain", 3);
        }
        set
        {
            PlayerPrefs.SetInt("tipRemain", value);
        }
    }

    public bool IsAdsAvailable
    {
        get
        {
            return PlayerPrefs.GetInt("Ads", 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("Ads", value ? 1 : 0);
        }
    }

    public string[] keepUp = { "Great!", "Awesome!", "Brilliant!", "Marvelous!", "Wonderful!", "Fantastic!", "Amazing!" };

    public InitLevel level;
    public static int totalLevel = 500;//total levels

    public static GameData instance;
    public static GameData getInstance()
    {
        if (instance == null)
        {
            instance = new GameData();
            //PlayerPrefs.DeleteAll();
        }
        return instance;
    }

    public bool isWin;//check if win
    public bool isfail;//whether the game failed

    /// <summary>
    /// Always uses for initial or reset to start a new level.
    /// </summary>
    public void resetData()
    {
        isWin = false;
        isfail = false;
        gotoNextLevel = false;
    }

    public int GetSDKLevel()
    {
        AndroidJavaClass build = new AndroidJavaClass("android.os.Build$VERSION");
        int sdkLevel = build.GetStatic<int>("SDK_INT");
        return sdkLevel;
    }
}
