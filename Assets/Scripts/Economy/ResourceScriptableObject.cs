using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Resource", menuName = "ScriptableObjects/ResourceScriptableObject", order = 1)]
public class ResourceScriptableObject : ScriptableObject
{
    public Sprite icon;
    public float volumePerUnit;
}
