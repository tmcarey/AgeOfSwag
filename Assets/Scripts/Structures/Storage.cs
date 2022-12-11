using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Storage : MonoBehaviour
{
    private Dictionary<ResourceScriptableObject, Economy.ResourceStorageEntry> _resourceStorage = new Dictionary<ResourceScriptableObject, Economy.ResourceStorageEntry>();

    public float volumeCapacity;

    public UnityEvent<Economy.ResourceStorageEntry> onStorageUpdated;

    public List<Economy.ResourceStorageEntry> initialResources;

    public void AddResource(ResourceScriptableObject resource, int amount)
    {
        if (_resourceStorage.ContainsKey(resource))
        {
            _resourceStorage[resource].amount += amount;
            _resourceStorage[resource].onResourceUpdated?.Invoke(resource, _resourceStorage[resource].amount);
        }
        else
        {
            Economy.ResourceStorageEntry entry = new Economy.ResourceStorageEntry();
            entry.resource = resource;
            entry.amount = amount;
            _resourceStorage.Add(resource, entry);
            _resourceStorage[resource].onResourceUpdated?.Invoke(resource, _resourceStorage[resource].amount);
        }
        Economy.Instance.AddResource(resource, amount);
    }

    private void Start()
    {
        foreach(Economy.ResourceStorageEntry entry in initialResources)
        {
            AddResource(entry.resource, entry.amount);
        }
    }
}
