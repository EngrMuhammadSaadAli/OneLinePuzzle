using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData : ScriptableObject
{
    public List<Level> LevelList = new List<Level>();

    public List<Level> GetLevelsBytype(LevelType type)
    {
        List<Level> levels = new List<Level>();

        switch (type)
        {
            case LevelType.Basic:
                foreach (var level in LevelList)
                {
                    if (level.levelType == LevelType.Basic)
                    {
                        levels.Add(level);
                    }
                }
                break;
            case LevelType.Luminous:
                foreach (var level in LevelList)
                {
                    if (level.levelType == LevelType.Luminous)
                    {
                        levels.Add(level);
                    }
                }
                break;
        }

        return levels;
    }

    public bool UnlockStageInLevel(string levelName, int currentStage)
    {
        bool isUnlock = true;

        foreach (var level in LevelList)
        {
            if (level.Levelname == levelName)
            {
                if ((level.stages.Count) > currentStage)
                {
                    level.stages[currentStage].isGotStar = true;
                    level.stages[currentStage].isStageUnlock = true;
                    GameData.getInstance().hasHint = level.stages[currentStage].hasHint;
                    level.stages[currentStage].hasHint = false;

                    currentStage++;

                    if (level.stages.Count > currentStage)
                    {
                        if (!level.stages[currentStage].isStageUnlock)
                        {
                            level.stages[currentStage].isStageUnlock = true;
                        }
                    }
                    else
                    {
                        UnlockNextLevelOfType(level.levelType);
                        isUnlock = false;
                        GameData.getInstance().gotoNextLevel = true;
                        return isUnlock;
                    }
                    isUnlock = true;
                }
                else
                {
                    UnlockNextLevelOfType(level.levelType);
                    isUnlock = false;
                    GameData.getInstance().gotoNextLevel = true;
                }
            }
        }

        return isUnlock;
    }

    void UnlockNextLevelOfType(LevelType levelType)
    {
        int unlockCount = 0;
        int totalCount = 0;

        switch (levelType)
        {
            case LevelType.Basic:
                unlockCount = TotalUnlockLevelOfType(levelType);
                totalCount = TotalLevelOfType(levelType);

                if (totalCount > unlockCount)
                {
                    if (LevelList[unlockCount].levelType == LevelType.Basic)
                    {
                        LevelList[unlockCount].isLevelUnlock = true;
                    }
                }
                break;

            case LevelType.Luminous:
                unlockCount = TotalUnlockLevelOfType(levelType);
                totalCount = TotalLevelOfType(levelType);

                if (totalCount > unlockCount)
                {
                    if (LevelList[unlockCount].levelType == LevelType.Luminous)
                    {
                        LevelList[unlockCount].isLevelUnlock = true;
                    }
                }
                break;
        }
    }

    public void UnlockAllLevelsOfType(LevelType levelType)
    {
        switch (levelType)
        {
            case LevelType.Basic:
                foreach (var level in LevelList)
                {
                    if (level.levelType == LevelType.Basic)
                    {
                        level.isLevelUnlock = true;
                    }
                }
                break;
            case LevelType.Luminous:
                foreach (var level in LevelList)
                {
                    if (level.levelType == LevelType.Luminous)
                    {
                        level.isLevelUnlock = true;
                    }
                }
                break;
        }
    }

    /// <summary>
    /// Total number of the stages in levels of Leveltype.
    /// </summary>
    /// <returns>The stages in levels of type.</returns>
    /// <param name="levelType">Level type.</param>
    public int TotalStagesInLevelsOfType(LevelType levelType)
    {
        int count = 0;

        switch (levelType)
        {
            case LevelType.Basic:

                foreach (var level in LevelList)
                {
                    if (level.levelType == LevelType.Basic)
                    {
                        count += level.TotalStages;
                    }
                }
                break;
            case LevelType.Luminous:

                foreach (var level in LevelList)
                {
                    if (level.levelType == LevelType.Luminous)
                    {
                        count += level.TotalStages;
                    }
                }
                break;
        }

        return count;
    }

    /// <summary>
    /// Total number of the unlock stages in levels of in Leveltype.
    /// </summary>
    /// <returns>The un lock stages in levels of type.</returns>
    /// <param name="levelType">Level type.</param>
    public int TotalUnLockStagesInLevelsOfType(LevelType levelType)
    {
        int count = 0;

        switch (levelType)
        {
            case LevelType.Basic:

                foreach (var level in LevelList)
                {
                    if (level.levelType == LevelType.Basic)
                    {
                        for (int i = 0; i < level.stages.Count; i++)
                        {
                            if (level.stages[i].isGotStar)
                            {
                                count += 1;
                            }
                        }
                    }
                }
                break;
            case LevelType.Luminous:

                foreach (var level in LevelList)
                {
                    if (level.levelType == LevelType.Luminous)
                    {
                        for (int i = 0; i < level.stages.Count; i++)
                        {
                            if (level.stages[i].isGotStar)
                            {
                                count += 1;
                            }
                        }
                    }
                }
                break;
        }

        return count;
    }

    public int GetTotalStagesUnlockInLevel(Level level)
    {
        int count = 0;
        for (int i = 0; i < level.stages.Count; i++)
        {
            if (level.stages[i].isGotStar)
            {
                count++;
            }
        }
        return count;
    }

    /// <summary>
    /// Gets level by its name.
    /// </summary>
    /// <returns>The level by name.</returns>
    /// <param name="name">Name.</param>
    public Level GetLevelByName(string name)
    {
        Level level = null;

        foreach (var levels in LevelList)
        {
            if (levels.Levelname == name)
            {
                level = levels;
            }
        }

        return level;
    }

    /// <summary>
    /// Totals of the unlock level of levelType.
    /// </summary>
    /// <returns>The unlock level of type.</returns>
    /// <param name="levelType">Level type.</param>
    int TotalUnlockLevelOfType(LevelType levelType)
    {
        int count = 0;

        switch (levelType)
        {
            case LevelType.Basic:

                foreach (var level in LevelList)
                {
                    if (level.isLevelUnlock && level.levelType == LevelType.Basic)
                    {
                        count++;
                    }
                }
                break;
            case LevelType.Luminous:

                foreach (var level in LevelList)
                {
                    if (!level.isLevelUnlock && level.levelType == LevelType.Luminous)
                    {
                        count++;
                    }
                }
                break;
        }

        return count;
    }

    int TotalLevelOfType(LevelType levelType)
    {
        int count = 0;

        switch (levelType)
        {
            case LevelType.Basic:

                foreach (var level in LevelList)
                {
                    if (level.levelType == LevelType.Basic)
                    {
                        count++;
                    }
                }
                break;
            case LevelType.Luminous:

                foreach (var level in LevelList)
                {
                    if (level.levelType == LevelType.Luminous)
                    {
                        count++;
                    }
                }
                break;
        }

        return count;
    }
}

[System.Serializable]
public class Level
{
    public string Levelname;
    public int LevelNumber;
    public bool isLevelUnlock;
    public LevelType levelType;
    public int TotalStages = 50;
    public int StageStartIndex;
    public List<Stage> stages = new List<Stage>(50);
}

[System.Serializable]
public class Stage
{
    public bool isGotStar;
    public bool isStageUnlock;
    public bool hasHint;
}

[System.Serializable]
public enum LevelType
{
    Basic,
    Luminous
}
