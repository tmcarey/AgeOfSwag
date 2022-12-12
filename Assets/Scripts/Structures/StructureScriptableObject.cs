using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Resource", menuName = "ScriptableObjects/StructureScriptableObject", order = 1)]
public class StructureScriptableObject : ScriptableObject
{
    public string structureName;
    
    public GameObject structurePrefab;

    public Vector2Int size;
    
    public List<Economy.ResourceStorageEntry> resourceCosts;
}
