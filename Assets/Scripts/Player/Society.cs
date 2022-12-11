using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Society : Singleton<Society>
{
    public List<StructureScriptableObject> availableStructures;
    
    void Awake()
    {
        Instance = this;
    }
}
