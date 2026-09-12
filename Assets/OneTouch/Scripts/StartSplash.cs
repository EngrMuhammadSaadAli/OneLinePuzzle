using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSplash : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("mainMenu");

    }


}
