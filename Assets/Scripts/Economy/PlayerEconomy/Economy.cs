using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Economy : MonoBehaviour
{
    private readonly Dictionary<ResourceScriptableObject, ResourceStorageEntry> _resourceStorage = new Dictionary<ResourceScriptableObject, ResourceStorageEntry>();

    private readonly Dictionary<ResourceScriptableObject, HashSet<Storage>> _storageMap =
        new Dictionary<ResourceScriptableObject, HashSet<Storage>>();
    
    public UnityEvent<ResourceStorageEntry> onResourceAdded;
    
    public UnityEvent<LogisticsRequest> onNewLogisticsRequest;

    private Queue<ConstructionProject> _constructionQueue = new Queue<ConstructionProject>();

    private readonly Queue<LogisticsRequest> _logisticsQueue = new Queue<LogisticsRequest>();
    
    private readonly HashSet<LogisticsTransporter> _freeTransporters = new HashSet<LogisticsTransporter>();
    
    private List<StorageClaim> _claims = new List<StorageClaim>();
    
    public void RegisterSource(ResourceScriptableObject resource, Storage storage)
    {
        if (!_storageMap.ContainsKey(resource))
        {
            _storageMap[resource] = new HashSet<Storage>();
        }

        _storageMap[resource].Add(storage);
    }
    
    public void DeregisterSource(ResourceScriptableObject resource, Storage storage)
    {
        if (!_storageMap.ContainsKey(resource))
        {
            return;
        }

        _storageMap[resource].Remove(storage);
    }
    

    public class LogisticsAssignment
    {
        public List<StorageClaim> Claims;
        public Storage Target;
        public int Total;
        public ResourceScriptableObject Resource;
    }

    public class StorageClaim
    {
        public Storage Storage;
        public int Amount;
    }

    public void AddLogisticsTransporter(LogisticsTransporter transporter)
    {
        _freeTransporters.Add(transporter);
    }
    
    /*
    public void RemoveLogisticsTransporter(NavMeshAgent agent)
    {
        foreach(LogisticsTransporter transporter in transporters)
        {
            if(transporter.agent == agent)
            {
                transporters.Remove(transporter);
                break;
            }
        }
    }
    */

    public class LogisticsRequest
    {
        public List<ResourceStorageEntry> Resources;
        public Storage Target;
    }
    
    [Serializable]
    public class ResourceStorageEntry
    {
        public ResourceScriptableObject resource;
        public int amount;
        
        [HideInInspector]
        public UnityEvent<ResourceScriptableObject, int> onResourceUpdated;
    }

    public void SubmitLogisticsRequest(LogisticsRequest request)
    {
        _logisticsQueue.Enqueue(request);
        onNewLogisticsRequest.Invoke(request);
    }
    
    public void UpdateResource(ResourceScriptableObject resource, int amount)
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

    private void Start()
    {
        StartCoroutine(LogisticsAssignmentRoutine());
    }
    
    public void OnTransporterFinished(LogisticsTransporter transporter)
    {
        _freeTransporters.Add(transporter);
    }
    
    IEnumerator LogisticsAssignmentRoutine()
    {
        while (true)
        {
            if (_freeTransporters.Count > 0)
            {
                if (_logisticsQueue.Count > 0)
                {
                    LogisticsRequest request = _logisticsQueue.Peek();
                    if (request.Resources.Count == 0)
                    {
                        throw new Exception("Logistics request has no resources");   
                    }
                    LogisticsTransporter transporter = _freeTransporters.First();
                    ResourceStorageEntry entry = request.Resources[0];
                    
                    _claims.Clear();
                    
                    var maxAmount = (int)Mathf.Min(entry.amount, transporter.storage.volumeCapacity / entry.resource.volumePerUnit);
                    var currentAmount = 0;
                    
                    foreach(var storage in _storageMap[entry.resource])
                    {
                        var nextCurrentAmount = currentAmount + storage.GetOutputResourceCount(entry.resource);
                        if(nextCurrentAmount >= maxAmount)
                        {
                            _claims.Add(new StorageClaim() {Storage = storage, Amount = maxAmount - currentAmount });       
                            currentAmount = maxAmount;
                            break;
                        }
                        else
                        {
                            currentAmount = nextCurrentAmount;
                            _claims.Add(new StorageClaim(){ Storage = storage, Amount = storage.GetOutputResourceCount(entry.resource) });
                        }
                    }

                    if (currentAmount == maxAmount)
                    {
                        _freeTransporters.Remove(transporter);
                        
                        foreach(StorageClaim claim in _claims)
                        {
                            claim.Storage.ClaimResource(entry.resource, claim.Amount);
                        }

                        transporter.SetAssignment(new LogisticsAssignment()
                        {
                            Claims = _claims, Target = request.Target, Total = currentAmount, Resource = entry.resource
                        });
                        
                        entry.amount -= currentAmount;
                        if (entry.amount <= 0)
                        {
                            request.Resources.RemoveAt(0);
                            if (request.Resources.Count == 0)
                            {
                                _logisticsQueue.Dequeue();
                            }
                        }
                    }
                    
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

}
