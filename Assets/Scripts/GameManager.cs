using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameConfigScriptableObject gameConfig;
    public Vector3 spawnPoint;
    public GameObject localPlayer;

    private void Awake()
    {
        GameObject spawnedStructures = Instantiate(gameConfig.initialStructures);

        foreach (Structure structure in spawnedStructures.GetComponentsInChildren<Structure>())
        {
            structure.Initialize(localPlayer.GetComponent<Economy>());
        }
        
        foreach (Transform t in spawnedStructures.transform)
        {
            t.SetParent(null);
        }
    }
}
