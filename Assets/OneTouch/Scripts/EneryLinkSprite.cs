using UnityEngine;

public class EneryLinkSprite : MonoBehaviour
{

    // Use this for initialization
    public int state;
    SpriteRenderer image;

    void Start()
    {
        image = GetComponent<SpriteRenderer>();
    }


    /// <summary>
    /// Changes the state when link a line
    /// </summary>
    /// <param name="state_">State.</param>
    public void changeState(int state_)
    {
        state = state_;
        Color tcolor = image.color;
        switch (state)
        {
            case 0:
                LeanTween.color(gameObject, new Color(1, 1, 1, 1), 0.3f);
                break;
            case 1:
                //LeanTween.color(gameObject, new Color(0.2f, 0.6f, 0.8f, 1), 0.3f);
                LeanTween.color(gameObject, GameData.getInstance().currentColor, 0.3f);
                break;
            case 2:
                GameData.getInstance().isfail = true;

                if (AudioManager.Instance)
                {
                    AudioManager.Instance.WrongNode();
                }

                LeanTween.color(gameObject, new Color(1, 0, 0, 1), 0.3f);
                break;
        }
    }
}
