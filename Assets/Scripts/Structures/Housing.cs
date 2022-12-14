using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Housing : MonoBehaviour
{
    private Structure _structure;
    public int capacity;
    private int _currentPopulation;
    
    private void Awake()
    {
        _structure = GetComponent<Structure>();
    }

    public Structure GetStructure()
    {
        return _structure;
    }

    public void Initialize(Society society)
    {
        society.RegisterHousing(this);
    }
    
    public bool AssignCitizen(Citizen citizen)
    {
        if (_currentPopulation >= capacity)
        {
            return false;
        }
        _currentPopulation++;
        citizen.housing = this;
        return true;
    }
}
