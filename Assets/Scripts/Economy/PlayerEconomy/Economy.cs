using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Economy : Singleton<Economy>
{
    private Dictionary<ResourceScriptableObject, ResourceStorageEntry> _resourceStorage = new Dictionary<ResourceScriptableObject, ResourceStorageEntry>();
    
    public UnityEvent<ResourceStorageEntry> onResourceAdded;

    [Serializable]
    public class ResourceStorageEntry
    {
        public ResourceScriptableObject resource;
        public int amount;    
        
        [HideInInspector]
        public UnityEvent<ResourceScriptableObject, int> onResourceUpdated;
    }

    private void Awake()
    {
        Instance = this;
    }
    
    public void AddResource(ResourceScriptableObject resource, int amount)
    {
        if (_resourceStorage.ContainsKey(resource))
        {
            _resourceStorage[resource].amount += amount;
            _resourceStorage[resource].onResourceUpdated?.Invoke(resource, _resourceStorage[resource].amount);
        }
        else
        {
            ResourceStorageEntry entry = new ResourceStorageEntry();
            entry.resource = resource;
            entry.amount = amount;
            _resourceStorage.Add(resource, entry);
            _resourceStorage[resource].onResourceUpdated?.Invoke(resource, _resourceStorage[resource].amount);
            onResourceAdded.Invoke(entry);
        }
    }
}
