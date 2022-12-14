using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Citizen : MonoBehaviour
{
    public Employment employment;
    public Housing housing;

    private NavMeshAgent _agent;
    
    private Structure _currentStructure;

    private bool _hasAssignedPath = false;
    
    public bool dontImmediatelyEndWork = false;

    public void StartWork()
    {
        StartCoroutine(GoToWork());
    }

    private IEnumerator GoToWork()
    {
        yield return WaitUntilEnteredStructure(employment.GetStructure());
        employment.EmployeeStarted(this);
    }

    public void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void Initialize(Society society){
        society.RegisterNewCitizen(this);
        if (!housing)
        {
            society.AssignHousing(this);
            if (housing && !employment)
            {
                society.AssignJob(this);
            }

            if (housing)
            {
                _currentStructure = housing.GetStructure();
                SetVisibility(false);
                transform.position = _currentStructure.GetAccessPoint();
            }
        }
    }

    public void EndWorkDay()
    {
        StartCoroutine(GoHome());
    }

    private IEnumerator GoHome()
    {
        yield return WaitUntilEnteredStructure(housing.GetStructure());
    }
    
    private void SetVisibility(bool active)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(active);
        }
    }
    
    private IEnumerator EnterStructure(Structure _structure)
    {
        _currentStructure = _structure;
        SetVisibility(false);
        _agent.enabled = false;
        transform.position = _currentStructure.transform.position;
        yield return new WaitForSeconds(1.0f);
    }

    private IEnumerator ExitStructure()
    {
        Vector3 exitPosition = _currentStructure.GetAccessPoint();
        _currentStructure = null;
        transform.position = exitPosition;
        SetVisibility(true);
        yield return new WaitForSeconds(1.0f);
        _agent.enabled = true;
    }

    private IEnumerator WaitUntilAtDestination(Vector3 destination)
    {
        _agent.SetDestination(destination);
        do
        {
            yield return new WaitForSeconds(0.1f);
        } while (_agent.remainingDistance > _agent.stoppingDistance);
    }
    
    public IEnumerator WaitUntilEnteredStructure(Structure structure)
    {
        while (_hasAssignedPath)
            yield return new WaitForSeconds(0.1f);
        _hasAssignedPath = true;
        if (_currentStructure)
            yield return ExitStructure();
        yield return WaitUntilAtDestination(structure.GetAccessPoint());
        yield return EnterStructure(structure);
        _hasAssignedPath = false;
    }
    
}
