using System;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public bool logisticsAccessible = true;
    
    private readonly Dictionary<ResourceScriptableObject, StorageEntry> _storageInput = new Dictionary<ResourceScriptableObject, StorageEntry>();

    private Dictionary<ResourceScriptableObject, StorageEntry> _storageOutput;
    
    [SerializeField] private bool inputIsOutput = false;

    public float volumeCapacity;

    public event Action<StorageEntry> OnInputUpdated;
    public event Action<StorageEntry> OnOutputUpdated;

    
    public class StorageEntry
    {
        public ResourceScriptableObject Resource;
        public int FreeAmount;
        public int ClaimedAmount;
    }

    private Structure _structure;

    private void Awake()
    {
        _structure = GetComponent<Structure>();
        if (inputIsOutput)
        {
            _storageOutput = _storageInput;
        }
        else
        {
            _storageOutput = new Dictionary<ResourceScriptableObject, StorageEntry>();
        }
    }

    public void CreateResource(ResourceScriptableObject resource, int amount)
    {
        if (_storageOutput.ContainsKey(resource))
        {
            _storageOutput[resource].FreeAmount += amount;
        }
        else
        {
            StorageEntry entry = new StorageEntry();
            entry.Resource = resource;
            entry.FreeAmount = amount;
            entry.ClaimedAmount = 0;
            _storageOutput.Add(resource, entry);
            if (logisticsAccessible)
            {
                _structure.Economy.RegisterSource(resource, this);
            }
        }

        OnOutputUpdated?.Invoke(_storageOutput[resource]);
        _structure.Economy.UpdateResource(resource, amount);
    }

    public void ClaimResource(ResourceScriptableObject resource, int amount)
    {
        if (!_storageOutput.ContainsKey(resource))
        {
            Debug.LogError("Tried to claim resource which is not in storage");
            return;
        }
        
        _storageOutput[resource].FreeAmount -= amount;
        _storageOutput[resource].ClaimedAmount += amount;
        
        if(_storageOutput[resource].FreeAmount == 0)
        {
            _structure.Economy.DeregisterSource(resource, this);
        }
    }

    public void ConsumeResource(ResourceScriptableObject resource, int amount)
    {
        if (!_storageInput.ContainsKey(resource))
        {
            Debug.LogError("Tried to consume resource which is not in storage");
            return;
        }
        else
        {
            _storageInput[resource].FreeAmount -= amount;
            if (_storageInput[resource].FreeAmount < 0)
            {
                Debug.LogError("Tried to consume more resource than is in storage");
                _storageInput[resource].FreeAmount = 0;
            }
        }
        
        OnInputUpdated?.Invoke(_storageInput[resource]);
        _structure.Economy.UpdateResource(resource, -amount);
    }

    public int GetInputResourceCount(ResourceScriptableObject resource)
    {
        if (!_storageInput.ContainsKey(resource))
        {
            return 0;
        }
        
        return _storageInput[resource].FreeAmount;
    }
    
    public int GetOutputResourceCount(ResourceScriptableObject resource)
    {
        if (!_storageOutput.ContainsKey(resource))
        {
            return 0;
        }
        
        return _storageOutput[resource].FreeAmount;
    }

    public void TransferResource(Storage target, ResourceScriptableObject resource, int amount)
    {
        if (!_storageOutput.ContainsKey(resource))
        {
            Debug.LogError("Tried to transfer resource which is not in storage");
            return;
        }
        else
        {
            if (inputIsOutput)
            {
                _storageOutput[resource].FreeAmount -= amount;
                if (_storageOutput[resource].FreeAmount < 0)
                {
                    Debug.LogError("Tried to transfer more resource than is in storage");
                    _storageOutput[resource].FreeAmount = 0;
                }
            }
            else
            {
                _storageOutput[resource].ClaimedAmount -= amount;
                if (_storageOutput[resource].ClaimedAmount < 0)
                {
                    Debug.LogError("Tried to transfer more resource than is in storage");
                    _storageOutput[resource].ClaimedAmount = 0;
                }
            }
        }
        
        OnOutputUpdated?.Invoke(_storageOutput[resource]);
        
        if (target._storageInput.ContainsKey(resource))
        {
            target._storageInput[resource].FreeAmount += amount;
        }
        else
        {
            StorageEntry entry = new StorageEntry();
            entry.Resource = resource;
            entry.FreeAmount = amount;
            target._storageInput.Add(resource, entry);
            if (target.inputIsOutput && target.logisticsAccessible)
            {
                _structure.Economy.RegisterSource(resource, this);
            }
        }
        
        target.OnInputUpdated?.Invoke(target._storageInput[resource]);
    }
}
