using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialResources : MonoBehaviour
{
    private Storage _storage;
    
    public List<Economy.ResourceStorageEntry> initialResources;

    private void Awake()
    {
        _storage = GetComponent<Storage>();
    }

    private void Start()
    {
        foreach(Economy.ResourceStorageEntry entry in initialResources)
        {
            _storage.CreateResource(entry.resource, entry.amount);
        }
    }
}
