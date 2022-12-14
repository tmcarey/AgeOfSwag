using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameConfigScriptableObject gameConfig;
    public Vector3 spawnPoint;
    public GameObject localPlayer;
    
    private Society _localSociety;

    [Serializable]
    public class AosTime
    {
        public float time = 0;
        public int day = 1;
    }

    public AosTime currentTime = new AosTime();

    private void Awake()
    {
        Instance = this;

        _localSociety = localPlayer.GetComponent<Society>();
    }

    private void Start()
    {
        GameObject spawnedStructures = Instantiate(gameConfig.initialStructures);

        foreach (Structure structure in spawnedStructures.GetComponentsInChildren<Structure>())
        {
            structure.gameObject.SendMessage("Initialize", _localSociety, SendMessageOptions.DontRequireReceiver);
        }

        foreach (Citizen citizen in spawnedStructures
                     .GetComponentsInChildren<Citizen>())
        {
            citizen.Initialize(_localSociety);
        }
        
        List<Transform> transforms = new List<Transform>();
        foreach (Transform t in spawnedStructures.transform)
        {
            transforms.Add(t);
        }
        foreach (Transform t in transforms)
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
