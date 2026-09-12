using UnityEngine;
using UnityEngine.UI;

public class LevelPrefab : MonoBehaviour
{
    public Image bgImage;
    public Text nameText;
    public Text startCountText;
    public Image downStarImage;
    public Image middleStarImage;
    public Image lockImage;
    public Image statCountBG;


    [HideInInspector]
    public Color imageColor = Color.white;
    [HideInInspector]
    public string levelName;
    [HideInInspector]
    public bool isLevelUnlocked;
    [HideInInspector]
    public int totalStages;
    [HideInInspector]
    public int totalStagesUnlocked;

    public void SetupLevelData()
    {
        bgImage.color = imageColor;
        statCountBG.color = imageColor;
        nameText.text = levelName.ToUpper();

        if (isLevelUnlocked)
        {
            if (totalStages == totalStagesUnlocked)
            {
                startCountText.gameObject.SetActive(false);
                downStarImage.gameObject.SetActive(false);
                middleStarImage.gameObject.SetActive(true);
                lockImage.gameObject.SetActive(false);
            }
            else
            {
                startCountText.gameObject.SetActive(true);
                downStarImage.gameObject.SetActive(true);
                middleStarImage.gameObject.SetActive(false);
                lockImage.gameObject.SetActive(false);

                startCountText.text = totalStagesUnlocked.ToString() + "/" + totalStages.ToString();
            }
        }
        else
        {
            startCountText.gameObject.SetActive(false);
            downStarImage.gameObject.SetActive(false);
            middleStarImage.gameObject.SetActive(false);
            lockImage.gameObject.SetActive(true);
        }
    }

    public void OnLevelClicked()
    {
        if (AudioManager.Instance)
        {
            AudioManager.Instance.ButtonClick();
        }

        if (LevelHandler.Instance != null)
        {
            LevelHandler.Instance.OnLevelClicked(this);
        }
    }
}