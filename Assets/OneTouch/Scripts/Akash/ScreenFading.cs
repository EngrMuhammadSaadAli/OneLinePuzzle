using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenFading : MonoBehaviour
{
    public static ScreenFading Instance;
    Image _fadeImage;

    public Image FadeImage
    {
        get
        {
            if (_fadeImage == null)
            {
                _fadeImage = GameObject.Find("FadeImage").GetComponent<Image>();
            }
            return _fadeImage;
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        FadeImage.gameObject.SetActive(false);
    }

    public void FadeOut()
    {
        FadeImage.gameObject.SetActive(true);
        FadeImage.color = Color.black;
        LeanTween.alpha(FadeImage.GetComponent<RectTransform>(), 0, 1f).setOnComplete(() =>
        {
            FadeImage.gameObject.SetActive(false);
        });
    }

    public void FadeIn(System.Action onCompleted)
    {
        FadeImage.gameObject.SetActive(true);
        FadeImage.color = new Color(0f, 0f, 0f, 0f);

        LeanTween.alpha(FadeImage.GetComponent<RectTransform>(), 1f, 1f).setOnComplete(() =>
        {
            var handler = onCompleted;
            if (handler != null)
            {
                handler.Invoke();
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1;

            if (SceneManager.GetActiveScene().name == "Game")
            {
                FadeIn(() => SceneManager.LoadScene("LevelMenu"));
            }
            else if (SceneManager.GetActiveScene().name == "LevelMenu")
            {
                FadeIn(() => SceneManager.LoadScene("MainMenu"));
            }
            else if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                FadeIn(() => Application.Quit());
            }
        }
    }
}
