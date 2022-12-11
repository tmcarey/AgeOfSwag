using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // TODO this is mad slow and causes freezing
                T[] assets = Resources.LoadAll<T>("");
                if (assets == null || assets.Length < 1)
                {
                    Debug.LogError("No singleton was found.");
                }
                else if(assets.Length > 1)
                {
                    Debug.LogWarning("too many found");
                }
                instance = assets[0];
            }
            return instance;
        }
    }
}

