using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionPanel : MonoBehaviour
{
    private HUDController _hudController;
    
    public GameObject structureUIPrefab;

    private void Awake()
    {
        _hudController = GetComponentInParent<HUDController>();
        foreach (StructureScriptableObject structure in Society.Instance.availableStructures)
        {
            GameObject structureUI = Instantiate(structureUIPrefab, transform);
            structureUI.GetComponent<StructureConstructionUI>().Initialize(structure, _hudController);
        }
    }
    
}
