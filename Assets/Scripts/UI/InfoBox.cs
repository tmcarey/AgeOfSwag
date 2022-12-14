using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoBox : MonoBehaviour
{
    private bool isOpen = false;
    
    public TextMeshProUGUI title;
    public TextMeshProUGUI employerTitle;

    public GameObject storageUIPrefab;

    public RectTransform storageListParent;

    private Transform target;

    private Storage _selectedStorage;

    private void Awake()
    {
        gameObject.SetActive(isOpen);
    }

    public void SelectCitizen(Citizen citizen)
    {
        isOpen = true;
        
        gameObject.SetActive(true);
        
        target = citizen.transform;
        
        title.text = "Citizen";

        employerTitle.text = citizen.employment.GetComponent<Structure>().structureEntry.name;

        Storage storage = citizen.GetComponentInParent<Storage>();
        if (storage)
        {
            _selectedStorage = storage;
            storage.OnInputUpdated += OnStorageUpdate;
            RenderStorage(storage);
        }

        isOpen = true;
    }

    private void RenderStorage(Storage storage)
    {
        foreach (Transform t in storageListParent)
        {
            Destroy(t.gameObject);
        }

        storage.OnOutputUpdated += OnStorageUpdate;
        if (storage)
        {
            foreach(var item in storage.GetOutputStorage())
            {
                if(item.Value.FreeAmount > 0)
                {
                    var storageUI = Instantiate(storageUIPrefab, storageListParent);
                    storageUI.GetComponent<StorageUIEntry>().Initialize(item.Key, item.Value.FreeAmount);
                }
            }
        }
    }

    public void OnStorageUpdate(Storage.StorageEntry entry)
    {
        RenderStorage(_selectedStorage);
    }

    public void Close()
    {
        isOpen = false;
        gameObject.SetActive(false);
        if(_selectedStorage)
            _selectedStorage.OnInputUpdated -= OnStorageUpdate;
    }

    private void Update()
    {
        if (isOpen)
        {
            transform.position = Camera.main.WorldToScreenPoint(target.position);
        }
    }
}
