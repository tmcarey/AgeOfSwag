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
        resourceUI.GetComponentInChildren<Image>().sprite = entry.resource.icon;
        TextMeshProUGUI text = resourceUI.GetComponentInChildren<TextMeshProUGUI>();
        ResourceScriptableObject resourceObj = entry.resource;
        
        text.text = entry.amount.ToString();   
        
        localEconomy.OnResourceValueUpdated += (((resource, amt) =>
        {
            if (resource == resourceObj)
            {
                text.text = amt.ToString();   
            }
        }));
        
        Debug.Log("New Resource: Wood");
    }
}
