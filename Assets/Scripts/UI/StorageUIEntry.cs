using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StorageUIEntry : MonoBehaviour
{

    public TextMeshProUGUI amountText;
    public Image iconImage;
    
    public void Initialize(ResourceScriptableObject resource, int amount)
    {
        iconImage.sprite = resource.icon;
        amountText.text = amount.ToString();
    }

    public void UpdateAmount(int amt)
    {
        amountText.text = amt.ToString();
    }
}
