using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintShower : MonoBehaviour
{
    public TextMesh textMesh;

    public string hintString;

    public void StartShowingHints()
    {
        CancelInvoke();
        InvokeRepeating("StartCoroutineHints", 0, hintString.Length);
    }

    bool isStarted;

    void StartCoroutineHints()
    {
        if (hintString.Length > 1)
        {
            string[] s = hintString.Split(',');

            if (gameObject.activeInHierarchy)
            {
                if (!isStarted)
                {
                    StartCoroutine(ShowHints(s));
                }
            }
        }
        else
        {
            CancelInvoke();
            textMesh.text = hintString;
        }
    }

    IEnumerator ShowHints(string[] hints)
    {
        isStarted = true;
        for (int i = 0; i < hints.Length; i++)
        {
            textMesh.text = hints[i];
            yield return new WaitForSeconds(1);
        }
        isStarted = false;
    }
}
