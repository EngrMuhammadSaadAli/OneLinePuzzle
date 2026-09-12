using UnityEngine;
using UnityEngine.UI;

public class StagePrefab : MonoBehaviour
{
    public Image BgImage;
    public Text StageNumberText;
    public Image StarImage;
    public Image LockImage;
    public Image UnlockStarImage;
    public Image HintImageBG;

    [HideInInspector]
    public int StageNumber;
    [HideInInspector]
    public bool IsGotStar;
    [HideInInspector]
    public bool IsStageUnlock;
    [HideInInspector]
    public Color imageColor;
    [HideInInspector]
    public int StageIndex;
    [HideInInspector]
    public int StageLevel;
    [HideInInspector]
    public string levelName;
    [HideInInspector]
    public bool HasHint;

    public void SetStageData()
    {
        StageNumberText.text = (StageNumber + 1).ToString();

        if (IsStageUnlock)
        {
            BgImage.color = imageColor;

            if (IsGotStar)
            {
                HintImageBG.gameObject.SetActive(false);
                StarImage.gameObject.SetActive(false);
                UnlockStarImage.gameObject.SetActive(true);
                LockImage.gameObject.SetActive(false);
            }
            else
            {
                HintImageBG.gameObject.SetActive(false);
                StarImage.gameObject.SetActive(true);
                UnlockStarImage.gameObject.SetActive(false);
                LockImage.gameObject.SetActive(false);
            }
        }
        else
        {
            BgImage.color = Color.white;

            if (HasHint)
            {
                HintImageBG.color = imageColor;
                HintImageBG.gameObject.SetActive(true);
            }
            UnlockStarImage.gameObject.SetActive(false);
            StarImage.gameObject.SetActive(false);
            LockImage.gameObject.SetActive(true);
        }
    }

    public void OnStageButtonClicked()
    {
        if (AudioManager.Instance)
        {
            AudioManager.Instance.ButtonClick();
        }

        if (IsStageUnlock)
        {
            if (LevelHandler.Instance != null)
            {
                LevelHandler.Instance.OnStageClicked(StageIndex - 1, StageNumber, StageLevel, levelName);
            }
        }
    }
}
