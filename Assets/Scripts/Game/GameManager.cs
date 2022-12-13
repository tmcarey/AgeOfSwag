using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameConfigScriptableObject gameConfig;
    public Vector3 spawnPoint;
    public GameObject localPlayer;

    [Serializable]
    public class AOSTime
    {
        public float time = 0;
        public int day = 0;
    }

    public AOSTime currentTime = new AOSTime();

    private void Awake()
    {
        Instance = this;

        
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

    private void Update()
    {
        if (Time.timeScale > 0)
        {
            currentTime.time += Time.deltaTime / GameManager.Instance.gameConfig.secondsInDay;
            if (currentTime.time >= 1.0f)
            {
                currentTime.day++;
                currentTime.time = 0;
            }
        }
    }
}
