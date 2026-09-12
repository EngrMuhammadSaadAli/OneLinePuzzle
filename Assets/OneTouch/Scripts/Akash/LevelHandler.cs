using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelHandler : MonoBehaviour
{
    public static LevelHandler Instance;

    public GameObject LevelPanel;
    public GameObject StagePanel;

    [Space(10)]
    public ScrollRect levelScrollRect;
    public ScrollRect basicLevelScrollRect;
    public ScrollRect luminousLevelScrollRect;
    public ScrollRect stageScrollRect;

    [Space(10)]
    public Transform BasicLevelParent;
    public Transform LuminousLevelParent;
    public Transform StageParent;

    [Space(10)]
    public LevelPrefab levelPrefab;
    public StagePrefab stagePrefab;

    [Space(10)]
    public RectTransform nextButton;

    [Space(10)]
    [Header("Top Bar")]
    public Image topBarImage;
    public Image levelStarBGImage;
    public Text levelNameText;
    public Text levelStarText;
    public Image backbuttonImage;

    [Space(10)]
    [Header("Level Slider")]
    public GameObject level1;
    public GameObject level2;

    [Space(10)]
    [Header("Level Colors")]
    public Color luminousLevelColor;
    public List<Color> BasiclevelColors = new List<Color>();

    List<Level> levels;
    List<GameObject> levelObjectList = new List<GameObject>();
    List<GameObject> levelStagesList = new List<GameObject>();

    Color currentStageColor;
    string currentStageName;
    string currentStarInStage;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void OnDisable()
    {
        foreach (var level in levelObjectList)
        {
            Destroy(level);
        }

        levelObjectList.Clear();

        foreach (var level in levelStagesList)
        {
            Destroy(level);
        }

        levelStagesList.Clear();
    }

    void Start()
    {
        ScreenFading.Instance.FadeOut();
        LevelPanel.SetActive(true);
        StagePanel.SetActive(false);
        SetupScrollView();
        GenerateLevels();
    }

    void GenerateLevels()
    {
        levels = new List<Level>();

        foreach (LevelType level in Enum.GetValues(typeof(LevelType)))
        {
            levels.AddRange(LevelDataHandler.Instance.levelData.GetLevelsBytype(level));

            for (int i = 0; i < levels.Count; i++)
            {
                LevelPrefab go = Instantiate(levelPrefab);
                levelObjectList.Add(go.gameObject);

                switch (level)
                {
                    case LevelType.Basic:
                        go.imageColor = BasiclevelColors[i];
                        go.gameObject.transform.SetParent(BasicLevelParent);
                        go.gameObject.transform.localScale = Vector2.one;
                        break;
                    case LevelType.Luminous:
                        go.imageColor = luminousLevelColor;
                        go.gameObject.transform.SetParent(LuminousLevelParent);
                        go.gameObject.transform.localScale = Vector2.one;
                        break;
                }

                go.name = levels[i].Levelname;
                go.levelName = levels[i].Levelname;
                go.isLevelUnlocked = levels[i].isLevelUnlock;
                go.totalStages = levels[i].TotalStages;
                go.totalStagesUnlocked = LevelDataHandler.Instance.levelData.GetTotalStagesUnlockInLevel(levels[i]);
                go.SetupLevelData();
            }

            levels.Clear();
        }
    }

    void GenerateStagesForLevel(string levelName)
    {
        Level level = LevelDataHandler.Instance.levelData.GetLevelByName(levelName);

        for (int i = 0; i < level.stages.Count; i++)
        {
            StagePrefab go = Instantiate(stagePrefab);
            go.name = (i + 1).ToString();
            go.transform.SetParent(StageParent, false);
            levelStagesList.Add(go.gameObject);

            go.imageColor = currentStageColor;
            go.IsGotStar = level.stages[i].isGotStar;
            go.IsStageUnlock = level.stages[i].isStageUnlock;
            go.StageIndex = (level.StageStartIndex + i);
            go.StageNumber = i;
            go.StageLevel = level.LevelNumber;
            go.levelName = level.Levelname;
            go.HasHint = level.stages[i].hasHint;
            go.SetStageData();
        }
    }

    #region UI

    /// <summary>
    /// When Scroll Changed.
    /// </summary>
    /// <param name="pos">Position.</param>
    public void OnLevelScrollValueChanged(Vector2 pos)
    {
        if (pos.x < 0.3f)
        {
            level1.SetActive(true);
            level2.SetActive(false);
            levelNameText.text = LevelType.Basic.ToString().ToUpper();
            levelStarText.text = LevelDataHandler.Instance.levelData.TotalUnLockStagesInLevelsOfType(LevelType.Basic).ToString() + "/" + LevelDataHandler.Instance.levelData.TotalStagesInLevelsOfType(LevelType.Basic);
            ChangeNextButtonPosition(1f, 180, -20);//1
        }
        else if (pos.x > 0.7f)
        {
            level1.SetActive(false);
            level2.SetActive(true);
            levelNameText.text = LevelType.Luminous.ToString().ToUpper();
            levelStarText.text = LevelDataHandler.Instance.levelData.TotalUnLockStagesInLevelsOfType(LevelType.Luminous).ToString() + "/" + LevelDataHandler.Instance.levelData.TotalStagesInLevelsOfType(LevelType.Luminous);
            ChangeNextButtonPosition(0f, 0, 20);//0
        }
    }

    public void OnLevelClicked(LevelPrefab obj)
    {
        if (!obj.isLevelUnlocked)
        {
            return;
        }
        currentStageColor = obj.imageColor;
        currentStageName = obj.levelName;
        currentStarInStage = obj.totalStagesUnlocked.ToString() + "/" + obj.totalStages.ToString();

        LevelPanel.SetActive(false);
        StagePanel.SetActive(true);
        OnTopBarChanged();
        GenerateStagesForLevel(obj.gameObject.name);
    }

    public void OnStageClicked(int level, int stageNumber, int stageLevel, string levelName)
    {
        GameData.getInstance().currentLevel = level;
        GameData.getInstance().currentStage = stageNumber;
        GameData.getInstance().stageLevel = stageLevel;
        GameData.getInstance().currentlevelName = levelName;
        GameData.getInstance().currentColor = currentStageColor;

        if (ScreenFading.Instance)
        {
            ScreenFading.Instance.FadeIn(() => SceneManager.LoadScene("Game"));
        }
    }

    public void OnCloseButtonClicked()
    {
        if (AudioManager.Instance)
        {
            AudioManager.Instance.ButtonClick();
        }

        if (LevelPanel.activeInHierarchy)
        {
            ScreenFading.Instance.FadeIn(() => SceneManager.LoadScene("MainMenu"));
        }
        else if (StagePanel.activeInHierarchy)
        {
            BackToLevelPanel();
        }
    }

    void OnTopBarChanged()
    {
        if (LevelPanel.activeInHierarchy)
        {
            topBarImage.color = Color.white;
            levelStarBGImage.color = Color.white;
            backbuttonImage.transform.GetChild(0).gameObject.SetActive(false);
            backbuttonImage.transform.GetChild(1).gameObject.SetActive(true);
            OnLevelScrollValueChanged(Vector2.zero);
        }
        else if (StagePanel.activeInHierarchy)
        {
            levelStarBGImage.color = currentStageColor;
            topBarImage.color = currentStageColor;
            levelNameText.text = currentStageName;
            levelStarText.text = currentStarInStage;
            backbuttonImage.transform.GetChild(0).gameObject.SetActive(true);
            backbuttonImage.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    void BackToLevelPanel()
    {
        foreach (var level in levelStagesList)
        {
            Destroy(level);
        }

        levelStagesList.Clear();

        StagePanel.SetActive(false);
        LevelPanel.SetActive(true);
        OnTopBarChanged();
        SetupScrollView();
    }

    void SetupScrollView()
    {
        if (LevelPanel.activeInHierarchy)
        {
            OnLevelScrollValueChanged(Vector2.zero);
            levelScrollRect.normalizedPosition = Vector2.zero;
            basicLevelScrollRect.verticalNormalizedPosition = 1f;
            luminousLevelScrollRect.verticalNormalizedPosition = 1f;
        }
        else if (StagePanel.activeInHierarchy)
        {
            stageScrollRect.verticalNormalizedPosition = 1f;
        }
    }

    bool OnNext, OnPrevious;

    public void OnNextButtonClicked()
    {
        if (AudioManager.Instance)
        {
            AudioManager.Instance.ButtonClick();
        }

        if (levelScrollRect.normalizedPosition.x <= 0.1f)
        {
            OnNext = true;
            OnPrevious = false;
        }
        else if (levelScrollRect.normalizedPosition.x >= 0.9f)
        {
            OnPrevious = true;
            OnNext = false;
        }
    }

    void ChangeNextButtonPosition(float xPos, float rot, float anchPos)
    {
        nextButton.anchoredPosition = new Vector2(anchPos, 0f);
        nextButton.anchorMin = new Vector2(xPos, 0.5f);
        nextButton.anchorMax = new Vector2(xPos, 0.5f);
        nextButton.pivot = new Vector2(0.5f, 0.5f);
        nextButton.rotation = Quaternion.Euler(new Vector3(0, 0, rot));
    }

    void Update()
    {
        if (OnNext)
        {
            levelScrollRect.horizontalNormalizedPosition += Mathf.Lerp(0, 1, Time.deltaTime * 5f);

            if (levelScrollRect.horizontalNormalizedPosition >= 0.9f)
            {
                levelScrollRect.horizontalNormalizedPosition = 1f;
                ChangeNextButtonPosition(0f, 180, 20);//0
                OnNext = false;
            }
        }

        if (OnPrevious)
        {
            levelScrollRect.horizontalNormalizedPosition -= Mathf.Lerp(0, 1, Time.deltaTime * 5f);

            if (levelScrollRect.horizontalNormalizedPosition <= 0.1f)
            {
                levelScrollRect.horizontalNormalizedPosition = 0f;
                ChangeNextButtonPosition(1f, 0, -20);//1
                OnPrevious = false;
            }
        }
    }

    #endregion /UI
}
