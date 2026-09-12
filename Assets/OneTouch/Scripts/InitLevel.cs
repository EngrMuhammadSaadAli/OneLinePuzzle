using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class InitLevel : MonoBehaviour
{
    public GameObject nodeOrigin;//the origin node gameobject on the stage for duplicate
    public GameObject linkLine;// the oirigin line gameobject on the stage for duplicate
    public LineRenderer line;

    [Space(10)]
    [Header("UI Panels")]
    public GameObject gamePanel;
    public GameObject pausePanel;
    public GameObject winPanel;
    public GameObject giftPanel;
    public GameObject rateusPanel;
    public GameObject IAPPanel;

    [Space(10)]
    [Header("HintText")]
    public Text hintText;

    [Space(10)]
    [Header("LevelText")]
    public Text levelText;

    [Space(10)]
    [Header("WinPanel")]
    public Text keepUpText;

    [Space(10)]
    [Header("GiftPanel")]
    public Text giftUpText;
    public Button giftOkButton;

    [Space(10)]
    [Header("Color")]
    public Image topBarBGImage;
    public Image retryButton;
    public Image hintButton;
    public Image pauseButton;

    void Awake()
    {
        initData();
        refreshView();
        ScreenFading.Instance.FadeOut();

        if (AdmobManager.Instance)
        {
            AdmobManager.Instance.ShowBannerAds();
        }
    }


    /// <summary>
    /// Refreshs the view.
    /// </summary>
    public void refreshView()
    {
        hintText.text = GameData.getInstance().tipRemain.ToString();
        levelText.text = "LEVEL " + GameData.getInstance().stageLevel + " " + "-" + " " + (GameData.getInstance().currentStage + 1);
        topBarBGImage.color = GameData.getInstance().currentColor;
        retryButton.color = GameData.getInstance().currentColor;
        hintButton.color = GameData.getInstance().currentColor;
        pauseButton.color = GameData.getInstance().currentColor;
    }



    List<GameObject> nodes;//store all nodes

    SimpleJSON.JSONNode levelData;
    public string[] lvAnswerData;//the list of solve sequences

    /// <summary>
    /// parase the data get from level data to build the level.
    /// </summary>
    void initData()
    {
        if (nodes == null)
        {
            nodes = new List<GameObject>();
        }
        else
        {
            clearGame();
        }

        GameData.getInstance().resetData();
        GameData.getInstance().level = this;

        string tData = Datas.Instance.getData()[GameData.getInstance().currentLevel];

        levelData = SimpleJSON.JSONArray.Parse(tData);
        initLevelData();
    }


    void initLevelData()
    {
        lvAnswerData = new string[levelData[2].Count];

        for (int i = 0; i < levelData[2].Count; i++)
        {
            lvAnswerData[i] = levelData[2][i];
        }

        string tno = "";//0;
        float tx = 0;
        float ty = 0;


        GameData.getInstance().nLink = lvAnswerData.Length - 1;


        float zoom = Screen.height / 550f;//for resolution


        for (int i = 0; i < levelData[0].Count; i++)
        {
            for (int j = 0; j < levelData[0][i].Count; j++)
            {

                if (j == 0)
                {
                    if (levelData[0][i][j] == "" || levelData[1][i][j] == 0)
                        continue;
                    tno = (i + 1).ToString();
                }
                else if (j == 1)
                {
                    tx = levelData[0][i][j];
                }
                else
                {
                    ty = levelData[0][i][j];
                }
                if (j % 3 == 2)
                {
                    //GameObject tnode = Instantiate(nodeOrigin, nodeOrigin.transform.parent) as GameObject;
                    GameObject tnode = Instantiate(nodeOrigin, transform) as GameObject;
                    //tnode.transform.position = new Vector3(Screen.width / 2 - tx - 4630 * zoom, Screen.height / 2 + ty - 2120 * zoom, 0);
                    //Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3((Screen.width / 2) + (tx - 160), (Screen.height / 2) + (ty - 160), 0));
                    Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3((Screen.width / 2) + (tx - 160), (Screen.height / 2) + (ty - 200), 0));
                    pos.z = 0;
                    tnode.transform.localPosition = pos * zoom;
                    tnode.transform.localScale = new Vector3(5, 5, 0);
                    tnode.name = "node_" + tno;

                    //add reference for kill;
                    nodes.Add(tnode);
                    ty = levelData[0][i][j] * zoom;
                }
            }

        }

        //link map;
        string linka = ""; string linkb = "";

        for (int i = 0; i < levelData[1].Count; i++)
        {
            for (int j = 0; j < levelData[1][i].Count; j++)
            {
                if (j == 0)
                {
                    if (levelData[1][i][j] == "" || levelData[1][i][j] == 0)
                        continue;
                    linka = levelData[1][i][j];

                }
                else if (j == 1)
                {
                    linkb = levelData[1][i][j];
                }
                if (j == 1)
                {

                    GameObject obja = GameObject.Find("node_" + linka);
                    GameObject objb = GameObject.Find("node_" + linkb);

                    Vector3 tlinkLinePos = (obja.transform.localPosition + objb.transform.localPosition) / 2;
                    //				dfControl tlinkline =  dfpanel_.AddPrefab(linkLine);
                    //GameObject tlinkline = Instantiate(linkLine, linkLine.transform.parent) as GameObject;
                    GameObject tlinkline = Instantiate(linkLine, transform) as GameObject;

                    tlinkline.transform.position = tlinkLinePos;

                    int tlinka = int.Parse(linka);
                    int tlinkb = int.Parse(linkb);

                    tlinkline.name = "linkLine" + "_" + Mathf.Min(tlinka, tlinkb) + "_" + Mathf.Max(tlinka, tlinkb);

                    GameObject tChild = tlinkline.transform.Find("mc").gameObject;

                    float tdis = Vector3.Distance(obja.transform.localPosition, objb.transform.localPosition);

                    tChild.transform.localScale = new Vector3(tdis + 1.85f, 2f, 0);



                    float teng = Mathf.Atan2((obja.transform.position.y - objb.transform.position.y), (obja.transform.position.x - objb.transform.position.x)) * Mathf.Rad2Deg;
                    tlinkline.transform.Find("mc").Rotate(new Vector3(0, 0, teng));
                    tlinkline.transform.SetAsFirstSibling();
                    //add reference for kill;
                    nodes.Add(tlinkline);
                }
            }
        }

        if (GameData.getInstance().currentLevel > 260)
        {
            if (transform.localRotation.z <= 0)
            {
                transform.Rotate(new Vector3(0, 0, 180));
            }
        }
        else
        {
            transform.Rotate(Vector3.zero);
        }

        if (GameData.getInstance().AdsCounter == 3)
        {
            if (AdmobManager.Instance)
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    if (GameData.getInstance().GetSDKLevel() < 26)
                    {
                        AdmobManager.Instance.ShowInterstitialAd();
                    }
                }
                else
                {
                    AdmobManager.Instance.ShowInterstitialAd();
                }
            }
            GameData.getInstance().AdsCounter = 0;
        }
    }

    /// <summary>
    /// Clears the game.
    /// </summary>
    void clearGame()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);

        foreach (GameObject tnode in nodes)
        {
            DestroyImmediate(tnode.gameObject);
        }
        nodes.Clear();
        EneryNodeSprite.currentNode = null;
        totalHintShow = 0;
    }


    /// <summary>
    /// when game wins.
    /// </summary>
    public void gameWin()
    {
        GameData.getInstance().isWin = true;
        LevelDataHandler.Instance.levelData.UnlockStageInLevel(GameData.getInstance().currentlevelName, GameData.getInstance().currentStage);
        keepUpText.text = GameData.getInstance().keepUp[Random.Range(0, GameData.getInstance().keepUp.Length - 1)];

        rateusCounter++;

        if (GameData.getInstance().hasHint)
        {
            OnGetHint();
        }
        else
        {
            StartCoroutine(WinGame(0.3f));
        }

        if (LeaderBoard.Instance)
        {
            LeaderBoard.Instance.ReportScore();
        }


    }

    int rateusCounter;

    IEnumerator WinGame(float time)
    {
        if (rateusCounter == 5 && !GameData.getInstance().IsRated)
        {
            ShowRateUs(true);
            rateusCounter = 0;
            yield break;
        }

        yield return new WaitForSeconds(time);

        if (AudioManager.Instance)
        {
            AudioManager.Instance.GameWin();
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        winPanel.transform.GetChild(0).transform.localScale = Vector3.zero;

        LeanTween.scale(winPanel.transform.GetChild(0).gameObject, Vector3.one, 0.2f).setOnStart(() =>
        {
            winPanel.SetActive(true);
        }).setEase(LeanTweenType.easeInQuad);
    }

    void ShowRateUs(bool isShow)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (isShow)
        {
            rateusPanel.transform.GetChild(0).transform.localScale = Vector3.zero;
            LeanTween.scale(rateusPanel.transform.GetChild(0).gameObject, Vector3.one, 0.2f).setOnStart(() =>
            {
                rateusPanel.SetActive(isShow);
            }).setEase(LeanTweenType.easeInQuad);
        }
        else
        {
            rateusPanel.transform.GetChild(0).transform.localScale = Vector3.one;
            LeanTween.scale(rateusPanel.transform.GetChild(0).gameObject, Vector3.zero, 0.2f).setOnStart(() =>
            {
                rateusPanel.SetActive(isShow);
            }).setEase(LeanTweenType.easeInQuad);
        }
    }

    string androidStoreLink = "https://play.google.com/store/apps/details?id=com.linegames.oneline";

    public void OnRateusButtonClicked()
    {
        Application.OpenURL(androidStoreLink);
        GameData.getInstance().IsRated = true;
    }

    public void OnRateusCancelButtonClicked()
    {
        ShowRateUs(false);
        StartCoroutine(WinGame(0.3f));
    }


    public void OnNextStageOnWin()
    {
        if (AudioManager.Instance)
        {
            AudioManager.Instance.ButtonClick();
        }

        GameData.getInstance().AdsCounter++;

        if (GameData.getInstance().currentLevel < GameData.totalLevel - 1)
        {
            GameData.getInstance().currentLevel++;
        }
        else
        {
            GameData.getInstance().currentLevel = 0;
        }

        if (GameData.getInstance().gotoNextLevel)
        {
            OnLevelButtonClicked();
        }
        else
        {
            GameData.getInstance().currentStage++;

            if (GameData.getInstance().isWin)
            {
                Awake();
            }

        }

        winPanel.SetActive(false);
    }

    public void OnRetryClick()
    {
        if (AudioManager.Instance)
        {
            AudioManager.Instance.ButtonClick();
        }

        initData();
    }

    int totalHintShow;

    public void ShowHints()
    {
        string[] tansArr = lvAnswerData;

        if (GameData.getInstance().tipRemain > 0 && totalHintShow < (tansArr.Length - 1))
        {
            if (AudioManager.Instance)
            {
                AudioManager.Instance.Hint();
            }
            GameData.getInstance().tipRemain--;
            refreshView();
        }
        else
        {
            if (AudioManager.Instance)
            {
                AudioManager.Instance.ButtonClick();
            }

            if (GameData.getInstance().tipRemain <= 0)
            {
                if (AdmobManager.Instance)
                {
                    AdmobManager.Instance.HideBannerAds();
                }

                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
                IAPPanel.SetActive(true);
            }
            return;
        }

        totalHintShow += 3;

        if (totalHintShow >= (tansArr.Length - 1))
        {
            totalHintShow = tansArr.Length;
        }

        for (int i = 0; i < totalHintShow; i++)
        {
            GameObject tNode = GameObject.Find("node_" + tansArr[i]);
            HintShower shower = tNode.transform.Find("HintText").GetComponent<HintShower>();

            if (GameData.getInstance().currentLevel > 260)
            {
                if (tNode.transform.localRotation.z <= 0)
                {
                    tNode.transform.Rotate(new Vector3(0, 0, 180));
                }
            }
            else
            {
                tNode.transform.Rotate(Vector3.zero);
            }

            if (shower.hintString.Length == 0)
            {
                shower.hintString = (i + 1).ToString();
            }
            else
            {
                string s = (i + 1).ToString();

                if (!shower.hintString.Contains(s))
                {
                    shower.hintString += "," + (i + 1).ToString();
                }
            }

            shower.StartShowingHints();
        }
    }

    public void OnIAPPanelClose()
    {
        if (AdmobManager.Instance)
        {
            AdmobManager.Instance.ShowBannerAds();
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            IAPPanel.SetActive(false);
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void OnLevelButtonClicked()
    {
        if (AudioManager.Instance)
        {
            AudioManager.Instance.ButtonClick();
        }
        if (AdmobManager.Instance)
        {
            AdmobManager.Instance.HideBannerAds();
        }
        GameObject.Find("particle").SetActive(false);
        ScreenFading.Instance.FadeIn(() => SceneManager.LoadScene("LevelMenu"));
    }

    public void OnMenuButtonClicked()
    {
        if (AudioManager.Instance)
        {
            AudioManager.Instance.ButtonClick();
        }
        if (AdmobManager.Instance)
        {
            AdmobManager.Instance.HideBannerAds();
        }
        GameObject.Find("particle").SetActive(false);
        ScreenFading.Instance.FadeIn(() => SceneManager.LoadScene("MainMenu"));
    }

    void SetGiftPanelText()
    {
        giftUpText.text = "CONGRATULATIONS" + "\n" + "YOUR NUMBER OF HINTS" + "\n" + "INCREASED TO " + GameData.getInstance().tipRemain + ".";
    }

    public void OnPauseButtonClicked()
    {
        if (AudioManager.Instance)
        {
            AudioManager.Instance.ButtonClick();
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        pausePanel.transform.GetChild(0).transform.localScale = Vector3.zero;

        LeanTween.scale(pausePanel.transform.GetChild(0).gameObject, Vector3.one, 0.2f).setOnStart(() =>
        {
            if (AudioManager.Instance)
            {
                AudioManager.Instance.PanelOpen();
            }
            pausePanel.SetActive(true);
        }).setEase(LeanTweenType.easeInQuad);
    }

    public void OnResumeButtonClicked()
    {
        if (AudioManager.Instance)
        {
            AudioManager.Instance.ButtonClick();
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        LeanTween.scale(pausePanel.transform.GetChild(0).gameObject, Vector3.zero, 0.2f).setOnComplete(() =>
        {
            pausePanel.SetActive(false);
        }).setEase(LeanTweenType.easeInQuad);
    }

    public void OnHintOkButtonClicked()
    {
        giftPanel.transform.GetChild(0).transform.localScale = Vector3.one;

        LeanTween.scale(giftPanel.transform.GetChild(0).gameObject, Vector3.zero, 0.2f).setOnComplete(() =>
        {
            StartCoroutine(WinGame(0.1f));

            if (AudioManager.Instance)
            {
                AudioManager.Instance.ButtonClick();
            }
            giftPanel.SetActive(false);

        }).setEase(LeanTweenType.easeInQuad);
    }

    public void OnGetHint()
    {
        giftPanel.transform.GetChild(0).transform.localScale = Vector3.zero;

        LeanTween.scale(giftPanel.transform.GetChild(0).gameObject, Vector3.one, 0.2f).setOnStart(() =>
        {
            GameData.getInstance().tipRemain++;
            refreshView();

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            SetGiftPanelText();

            if (AudioManager.Instance)
            {
                AudioManager.Instance.PanelOpen();
            }

            giftOkButton.onClick.RemoveAllListeners();

            giftOkButton.onClick.AddListener(() =>
            {
                OnHintOkButtonClicked();
            });

            giftPanel.SetActive(true);
        }).setEase(LeanTweenType.easeInQuad);
    }

    public void OnHintPurchase()
    {
        giftPanel.transform.GetChild(0).transform.localScale = Vector3.zero;

        LeanTween.scale(giftPanel.transform.GetChild(0).gameObject, Vector3.one, 0.2f).setOnStart(() =>
        {
            SetGiftPanelText();
            refreshView();
            IAPPanel.GetComponent<IAPPanel>().ShowHintNumber();
            if (AudioManager.Instance)
            {
                AudioManager.Instance.PanelOpen();
            }

            giftOkButton.onClick.RemoveAllListeners();

            giftOkButton.onClick.AddListener(() =>
            {
                giftPanel.SetActive(false);
            });

            giftPanel.SetActive(true);
        }).setEase(LeanTweenType.easeInQuad);
    }

    public void RefreshHintView()
    {
        refreshView();
        IAPPanel.GetComponent<IAPPanel>().ShowHintNumber();
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

    public List<int[]> reverseList = new List<int[]>();

    public void OnReverseButtonClicked()
    {
        if (reverseList.Count > 0)
        {
            int[] nodeNames = reverseList[reverseList.Count - 1];
            //get link name
            if (nodeNames.Length == 2)
            {
                GameObject tnode0 = GameObject.Find("node_" + nodeNames[0]);
                GameObject tnode1 = GameObject.Find("node_" + nodeNames[1]);

                string tlinklineName = "linkLine" + "_" + Mathf.Min(nodeNames[0], nodeNames[1]) + "_" + Mathf.Max(nodeNames[0], nodeNames[1]);
                GameObject tLinkLine = GameObject.Find(tlinklineName);

                if (tLinkLine)
                {
                    //last node turn to blue
                    EneryLinkSprite enerylink = tLinkLine.GetComponentInChildren<EneryLinkSprite>();

                    enerylink.changeState(0);
                    //light the node only when can link a new line
                    tnode1.GetComponentInChildren<EneryNodeSprite>().changeState(0);//turn green

                    if (EneryNodeSprite.currentNode)
                    {
                        EneryNodeSprite.currentNode.gameObject.GetComponent<EneryNodeSprite>().changeState(0);
                        //active node
                        EneryNodeSprite.currentNode = tnode0.GetComponentInChildren<SpriteRenderer>();
                        line.SetPosition(0, EneryNodeSprite.currentNode.gameObject.transform.position);
                    }

                    //link a useful line

                    GameData.getInstance().nLink++;

                    reverseList.Remove(nodeNames);
                }
            }

            if (reverseList.Count < 1)
            {
                EneryNodeSprite.currentNode = null;
            }
        }
    }
}