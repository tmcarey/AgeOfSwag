using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialEmployment : MonoBehaviour
{
    private Employment _employment;
    public List<Citizen> initialEmployees;

    private void Awake()
    {
        _employment = GetComponent<Employment>();
        foreach (var citizen in initialEmployees)
        {
            _employment.AssignCitizen(citizen);
        }
    }
    
}
