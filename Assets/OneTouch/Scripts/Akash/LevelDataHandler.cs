using UnityEngine;
using System.IO;

public class LevelDataHandler : MonoBehaviour
{
    public static LevelDataHandler Instance;
    public LevelData levelData;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            levelData = LoadData();
        }
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveData();
        }
    }

    void OnApplicationQuit()
    {
        SaveData();
    }


    void SaveData()
    {
        string json = JsonUtility.ToJson(levelData);
        File.WriteAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + "Data.txt", json);
    }

    LevelData LoadData()
    {
        LevelData data = null;
        if (File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + "Data.txt"))
        {
            data = ScriptableObject.CreateInstance<LevelData>();
            string json = File.ReadAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + "Data.txt");
            JsonUtility.FromJsonOverwrite(json, data);
        }
        else
        {
            data = Resources.Load<LevelData>("LevelData/LevelData");
        }
        return data;
    }
}
