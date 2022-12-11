using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StructureConstructionUI : MonoBehaviour
{
    public TextMeshProUGUI title;
    public Button button;
    
    public void Initialize(StructureScriptableObject structure, HUDController hudController)
    {
        title.text = structure.structureName;
        button.onClick.AddListener(() => hudController.StartConstruction(structure));
    }
}
