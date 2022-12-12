using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Structure : MonoBehaviour
{
    private Economy _economy;
    
    public Economy Economy
    {
        get => _economy;
    }

    public void Initialize(Economy economy)
    {
        this._economy = economy;
    }
    
    public StructureScriptableObject structure;

    public Vector3 GetCenterOffset()
    {
        return new Vector3(structure.size.x * 2, 0, structure.size.y * 2);
    }

}
