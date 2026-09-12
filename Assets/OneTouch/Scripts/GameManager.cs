using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using System.Collections.Generic;

/// <summary>
/// The main controller singleton class
/// </summary>
public class GameManager
{

    /// <summary>
    /// not used yet.
    /// </summary>
    /// <returns>The object by name.</returns>
    /// <param name="objname">Objname.</param>
    public GameObject getObjectByName(string objname)
    {
        GameObject rtnObj = null;
        foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject)))
        {
            if (obj.name == objname)
            {
                rtnObj = obj;
            }
        }
        return rtnObj;
    }



    public static GameManager instance;

    public static GameManager getInstance()
    {
        if (instance == null)
        {
            instance = new GameManager();
        }
        return instance;
    }
}
