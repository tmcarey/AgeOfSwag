using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionProject : MonoBehaviour
{
    private Structure _structure;
    private Storage _storage;
    
    [SerializeField]
    private Transform constructionPreview;

    private void Awake()
    {
        _structure = GetComponent<Structure>();
        _storage = GetComponent<Storage>();
        _storage.OnInputUpdated += OnInputUpdated;
    }

    private void Start()
    {
        constructionPreview.transform.localScale =
            new Vector3(_structure.structure.size.x * 4, 5, _structure.structure.size.y * 4);
            
        constructionPreview.transform.localPosition = 
            new Vector3(-_structure.structure.size.x * 2, 2.5f, -_structure.structure.size.y * 2);

        List<Economy.ResourceStorageEntry> costsCopy = new List<Economy.ResourceStorageEntry>();
        foreach (Economy.ResourceStorageEntry entry in _structure.structure.resourceCosts)
        {
            costsCopy.Add(new Economy.ResourceStorageEntry() { resource = entry.resource, amount = entry.amount });
        }
        Economy.LogisticsRequest request = new Economy.LogisticsRequest() { Resources = 
             costsCopy, Target = _storage };
        _structure.Economy.SubmitLogisticsRequest(request);
    }

    private void OnInputUpdated(Storage.StorageEntry entry)
    {
        foreach (Economy.ResourceStorageEntry cost in _structure.structure.resourceCosts)
        {
            if (cost.amount > _storage.GetInputResourceCount(cost.resource))
            {
                return;
            }
        }
        
        Instantiate(_structure.structure.structurePrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}