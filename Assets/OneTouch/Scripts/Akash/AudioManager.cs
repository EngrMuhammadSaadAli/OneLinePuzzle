using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource bgMusicSource;
    public AudioSource soundSource;

    public AudioClip buttonclick;
    public AudioClip gameOver;
    public AudioClip gameWin;
    public AudioClip hint;
    public AudioClip wrongNode;
    public AudioClip panelOpen;
    public AudioClip touchNode;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        if (!PlayerPrefs.HasKey("sound"))
        {
            IsSound = true;
        }
        else
        {
            int sound = PlayerPrefs.GetInt("sound", 1);
            if (sound == 1)
            {
                IsSound = true;
            }
            else
            {
                IsSound = false;
            }
        }
    }

    public bool IsSound
    {
        get
        {
            int sound = PlayerPrefs.GetInt("sound", 1);
            return sound == 1;
        }
        set
        {
            if (value)
            {
                soundSource.volume = 1;
                bgMusicSource.volume = 1;
                PlayerPrefs.SetInt("sound", 1);
            }
            else
            {
                soundSource.volume = 0;
                bgMusicSource.volume = 0;
                PlayerPrefs.SetInt("sound", 0);
            }
        }
    }

    public void ButtonClick()
    {
        if (IsSound)
        {
            soundSource.PlayOneShot(buttonclick);
        }
    }

    public void Gameover()
    {
        if (IsSound)
        {
            soundSource.PlayOneShot(gameOver);
        }
    }

    public void GameWin()
    {
        if (IsSound)
        {
            soundSource.PlayOneShot(gameWin);
        }
    }

    public void Hint()
    {
        if (IsSound)
        {
            soundSource.PlayOneShot(hint);
        }
    }

    public void WrongNode()
    {
        if (IsSound)
        {
            soundSource.PlayOneShot(wrongNode);
        }
    }

    public void PanelOpen()
    {
        if (IsSound)
        {
            soundSource.PlayOneShot(panelOpen);
        }
    }

    public void TouchNode()
    {
        if (IsSound)
        {
            soundSource.PlayOneShot(touchNode);
        }
    }
}
