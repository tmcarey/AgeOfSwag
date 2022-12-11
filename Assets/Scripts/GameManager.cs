using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameConfigScriptableObject gameConfig;
    public Vector3 spawnPoint;

    private void Start()
    {
        GameObject spawnedStructures = Instantiate(gameConfig.initialStructures);
        foreach (Transform t in spawnedStructures.transform)
        {
            t.SetParent(null);
        }
    }
}
