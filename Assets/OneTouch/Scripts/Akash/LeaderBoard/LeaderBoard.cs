#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    public static LeaderBoard Instance;
    public string IosBasicLeaderboardId;
    public string IosLuminousLeaderboardId;

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
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();

        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
#endif
        Invoke("Signin", 2f);
    }

    void Signin()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                FirebaseEventHandler.LoginEvent();
            }
        });
    }

    Level level;
    public void ReportScore()
    {
        string id = null;

        if (Social.localUser.authenticated)
        {
            level = LevelDataHandler.Instance.levelData.GetLevelByName(GameData.getInstance().currentlevelName);
#if UNITY_ANDROID

            if (level.levelType == LevelType.Basic)
            {
                id = GPGSIds.leaderboard_basic_stages;
            }
            else if (level.levelType == LevelType.Luminous)
            {
                id = GPGSIds.leaderboard_luminous_stages;
            }


#elif (UNITY_IPHONE)

            if (level.levelType == LevelType.Basic)
            {
                id = IosBasicLeaderboardId;
            }
            else if (level.levelType == LevelType.Luminous)
            {
                id = IosLuminousLeaderboardId;
            }
#endif
            if (level.levelType == LevelType.Basic)
            {
                Social.ReportScore(LevelDataHandler.Instance.levelData.TotalUnLockStagesInLevelsOfType(LevelType.Basic), id, HandleScoreReported);
            }
            else if (level.levelType == LevelType.Luminous)
            {
                Social.ReportScore(LevelDataHandler.Instance.levelData.TotalUnLockStagesInLevelsOfType(LevelType.Luminous), id, HandleScoreReported);
            }
        }
    }

    public void HandleScoreReported(bool success)
    {
        if (success)
        {
            FirebaseEventHandler.PostScoreEvent(LevelDataHandler.Instance.levelData.TotalUnLockStagesInLevelsOfType(level.levelType), level.levelType.ToString());
        }
    }

    public void ShowLeaderboard()
    {
        if (Social.localUser.authenticated)
        {
            Social.ShowLeaderboardUI();
        }
        else
        {
            Signin();
        }
    }

}
