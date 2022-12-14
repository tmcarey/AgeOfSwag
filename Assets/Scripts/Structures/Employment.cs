using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employment : MonoBehaviour
{
    public List<Citizen> employees;
    public int employmentCapacity;

    private Structure _structure;

    private void Awake()
    {
        _structure = GetComponent<Structure>();
    }

    public void Initialize(Society society)
    {
        society.RegisterEmployment(this);
    }
    
    public Structure GetStructure()
    {
        return _structure;
    }

    public event Action<Citizen> OnEmployeeAdded;
    public event Action<Citizen> OnEmployeeRemoved;
    
    public event Action OnWorkEnded;

    public void EndWork()
    {
        OnWorkEnded?.Invoke();
        foreach(Citizen citizen in employees)
        {
            if(!citizen.dontImmediatelyEndWork)
                citizen.EndWorkDay();
        }
    }

    public bool AssignCitizen(Citizen citizen)
    {
        if(employees.Count >= employmentCapacity)
        {
            return false;
        }
        
        employees.Add(citizen);
        citizen.employment = this;
        return true;
    }
    
    public void EmployeeStarted(Citizen citizen)
    {
        OnEmployeeAdded?.Invoke(citizen);
    }

    public void EmployeeEnded(Citizen citizen)
    {
        employees.Remove(citizen);
        OnEmployeeRemoved?.Invoke(citizen);
    }
}