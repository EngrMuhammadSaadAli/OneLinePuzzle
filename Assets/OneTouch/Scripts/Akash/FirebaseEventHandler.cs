public static class FirebaseEventHandler
{
    public static void LoginEvent()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventLogin);
    }

    public static void PostScoreEvent(int level, string levelname)
    {
        Firebase.Analytics.Parameter[] LevelUpParameters = {
            new Firebase.Analytics.Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterLevel, level),
            new Firebase.Analytics.Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterLevelName, levelname),
        };
        Firebase.Analytics.FirebaseAnalytics.LogEvent(
          Firebase.Analytics.FirebaseAnalytics.EventLevelUp,
          LevelUpParameters);
    }

    public static void PurchaseEvent(string purchaseName)
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent(
            "Purchase",
            Firebase.Analytics.FirebaseAnalytics.ParameterItemName,
            purchaseName);
    }
}
