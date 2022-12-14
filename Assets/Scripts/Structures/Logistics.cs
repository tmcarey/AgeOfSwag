using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Logistics : MonoBehaviour
{
    private Employment _employment;
    private Structure _structure;

    private List<LogisticsTransporter> _logisticsTransporters =
        new List<LogisticsTransporter>();

    public GameObject transporterPrefab;

    public Vector3 GetAccessPoint()
    {
        return _structure.GetAccessPoint();
    }

    public Structure GetStructure()
    {
        return _structure;
    }

    private void Awake()
    {
        _employment = GetComponent<Employment>();
        _structure = GetComponent<Structure>();
        _employment.OnEmployeeAdded += OnEmployeeStarted;
        _employment.OnEmployeeRemoved += OnEmployeeRemoved;
        _employment.OnWorkEnded += OnWorkEnded;
    }

    public void OnWorkEnded()
    {
        foreach(LogisticsTransporter transporter in _logisticsTransporters)
        {
            if (transporter.IsTransporting)
            {
                StartCoroutine(FinishWorkRoutine(transporter));
            }
            else
            {
                EndTransporter(transporter);        
            }
        }
        _logisticsTransporters.Clear();
    }

    IEnumerator FinishWorkRoutine(LogisticsTransporter transporter)
    {
        yield return transporter.WaitForTransportFinished();
        EndTransporter(transporter);
    }

    private void EndTransporter(LogisticsTransporter transporter)
    {
        _structure.Economy.DeregisterLogisticsTransporter(transporter);
        Citizen citizen = transporter.GetComponentInParent<Citizen>();
        citizen.EndWorkDay();
        citizen.dontImmediatelyEndWork = false;
        Destroy(transporter.gameObject);
    }

    private void OnEmployeeStarted(Citizen citizen)
    {
        LogisticsTransporter transporterObject =
        Instantiate(transporterPrefab, citizen.transform).GetComponent<LogisticsTransporter>();
        
        _logisticsTransporters.Add(transporterObject);
                
        transporterObject.Initialize(_structure.Economy, this);
        _structure.Economy.RegisterLogisticsTransporter(transporterObject);
        citizen.dontImmediatelyEndWork = true;
    }

    private void OnEmployeeEnded(Citizen citizen)
    {
    }
    
    private void OnEmployeeRemoved(Citizen citizen)
    {
    }
}
