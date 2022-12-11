using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour
{
    public GameObject resourceUIPrefab;
    
    public void OnNewResource(Economy.ResourceStorageEntry entry)
    {
        GameObject resourceUI = Instantiate(resourceUIPrefab, transform);
        resourceUI.GetComponentInChildren<Image>().sprite = entry.resource.icon;
        resourceUI.GetComponentInChildren<TextMeshProUGUI>().text = entry.amount.ToString();   
        Debug.Log("New Resource: Wood");
    }
}
