using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Society : Singleton<Society>
{
    public List<StructureScriptableObject> availableStructures;

    private Economy _economy;

    private readonly List<Citizen> _population = new List<Citizen>();
    private readonly List<Employment> _employmentList = new List<Employment>();
    private readonly List<Housing> _housingList = new List<Housing>();

    public void RegisterHousing(Housing housing)
    {
        _housingList.Add(housing);
    }

    public void RegisterEmployment(Employment employment)
    {
        _employmentList.Add(employment);
    }
    
    public void AssignJob(Citizen citizen)
    {
        if (citizen.employment)
            return;
            
        foreach(Employment employment in _employmentList)
        {
            if (employment.AssignCitizen(citizen))
                return;
        }
    }
    
    public void AssignHousing(Citizen citizen)
    {
        if (citizen.housing)
            return;
            
        foreach(Housing housing in _housingList)
        {
            if (housing.AssignCitizen(citizen))
                return;
        }
    }
    
    public void RegisterNewCitizen(Citizen citizen)
    {
        _population.Add(citizen);
    }

    public float workStartTime;
    public float workEndTime;
    
    void Awake()
    {
        Instance = this;
        _economy = GetComponent<Economy>();
    }

    private void Update()
    {
        if (_working && !IsWorkTime())
        {
            _working = false;
            EndWorkDay();
        }
        else if (!_working && IsWorkTime())
        {
            _working = true;
            StartCoroutine(StartWorkDay());
        }
    }

    private IEnumerator StartWorkDay()
    {
        int citizensPerFrame = Mathf.Max(1, _population.Count / 20);
        int citizenIdx = 0;
        while (true)
        {
            for (int i = citizenIdx; i < Math.Min(citizenIdx + citizensPerFrame, _population.Count); i++)
            {
                _population[i].StartWork();
            }
            citizenIdx += citizensPerFrame;
            if(citizenIdx > _population.Count)
            {
                break;
            }
            yield return new WaitForSeconds(0.05f); 
        }
    }
    
    public void EndWorkDay()
    {
        foreach(Employment employment in _employmentList)
        {
            employment.EndWork();
        }
    }

    private bool _working;
    private bool IsWorkTime()
    {
        return GameManager.Instance.currentTime.time >= workStartTime && GameManager.Instance.currentTime.time <= workEndTime;
    }

    public Economy GetEconomy()
    {
        return _economy;
    }
}
