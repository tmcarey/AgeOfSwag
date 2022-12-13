using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Structure : MonoBehaviour
{
    private Economy _economy;
    private NavMeshObstacle _obstacle;
    public StructureScriptableObject structureEntry;

    public Vector3 accessPoint;

    private Vector3 worldAccessPoint;
    
    public Economy Economy
    {
        get => _economy;
    }

    public Vector3 GetAccessPoint()
    {
        return worldAccessPoint;
    }

    public void Initialize(Economy economy)
    {
        this._economy = economy;
        worldAccessPoint = transform.TransformPoint(accessPoint);
    }
    
    public Vector3 GetCenterOffset()
    {
        return new Vector3(structureEntry.size.x * 2, 0, structureEntry.size.y * 2);
    }

    private void Awake()
    {
        _obstacle = gameObject.GetOrAddComponent<NavMeshObstacle>();
        _obstacle.carving = true;
    }

    private void Start(){
        _obstacle.center = GetCenterOffset() + new Vector3(0, 1, 0);
        _obstacle.size = new Vector3(structureEntry.size.x * 4, 2, structureEntry.size.y * 4);
    }
}
