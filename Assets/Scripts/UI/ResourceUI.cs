using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour
{
    public GameObject resourceUIPrefab;
    public Economy localEconomy;
    
    public void OnNewResource(Economy.ResourceStorageEntry entry)
    {
        GameObject resourceUI = Instantiate(resourceUIPrefab, transform);
        StorageUIEntry uiEntry = resourceUI.GetComponent<StorageUIEntry>();
        uiEntry.Initialize(entry.resource, entry.amount);
        
        ResourceScriptableObject resourceObj = entry.resource;
        
        localEconomy.OnResourceValueUpdated += (((resource, amt) =>
        {
            if (resource == resourceObj)
            {
                uiEntry.UpdateAmount(amt);
            }
        }));
        
        Debug.Log("New Resource: " + entry.resource.name);
    }
}
