using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Resource", menuName = "ScriptableObjects/ResourceLibraryScriptableObjet", order = 1)]
public class ResourceLibraryScriptableObject : ScriptableSingleton<ResourceLibraryScriptableObject>
{
    public List<ResourceScriptableObject> resources;
}
