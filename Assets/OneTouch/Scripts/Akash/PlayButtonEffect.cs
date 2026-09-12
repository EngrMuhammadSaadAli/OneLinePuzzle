using UnityEngine;
using UnityEngine.UI;

public class PlayButtonEffect : MonoBehaviour
{
    public Image playImage;
    float speed = 1;

    void Start()
    {
        changeSpeed();
    }

    void Update()
    {
        if (playImage.fillClockwise)
        {
            playImage.fillAmount += (Time.deltaTime / speed);
            if (playImage.fillAmount >= 0.95f)
            {
                changeSpeed();
                playImage.fillClockwise = false;
            }
        }
        else
        {
            playImage.fillAmount -= (Time.deltaTime / speed);
            if (playImage.fillAmount <= 0.05)
            {
                changeSpeed();
                playImage.fillClockwise = true;
            }
        }
    }

    void changeSpeed()
    {
        speed = 1;
    }
}
