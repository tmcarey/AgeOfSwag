using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employment : MonoBehaviour
{
    public List<Citizen> employees;
    public int employmentCapacity;
    
    public event Action<Citizen> OnEmployeeAdded;
    public event Action<Citizen> OnEmployeeRemoved;
    
    public void AddEmployee(Citizen citizen)
    {
        if (employees.Count >= employmentCapacity)
        {
            Debug.Log("No more room for employees");
            return;
        }
        employees.Add(citizen);
        citizen.employment = this;
        OnEmployeeAdded.Invoke(citizen);
    }

    public void RemoveEmployee(Citizen citizen)
    {
        employees.Remove(citizen);
        OnEmployeeRemoved.Invoke(citizen);
    }
}