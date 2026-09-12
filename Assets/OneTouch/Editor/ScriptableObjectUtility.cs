using UnityEditor;
using UnityEngine;

public class ScriptableObjectUtility
{
    [MenuItem("Assets/Create/LevelData")]
    public static void CreateMyAsset()
    {
        LevelData asset = ScriptableObject.CreateInstance<LevelData>();

        AssetDatabase.CreateAsset(asset, "Assets/OneTouch/Resources/LevelData/LevelData.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }

}
