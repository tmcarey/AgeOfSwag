using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logistics : MonoBehaviour
{
    private Employment _employment;
    private Structure _structure;

    public GameObject transporterPrefab;

    private void Awake()
    {
        _employment = GetComponent<Employment>();
        _structure = GetComponent<Structure>();
        _employment.OnEmployeeAdded += OnEmployeeAdded;
        _employment.OnEmployeeRemoved += OnEmployeeRemoved;
    }
    
    private void OnEmployeeAdded(Citizen citizen)
    {
        Destroy(citizen.GetComponent<UnityEngine.AI.NavMeshAgent>());
        LogisticsTransporter transporterObject =
            Instantiate(transporterPrefab, citizen.transform.position, Quaternion.identity)
                .GetComponent<LogisticsTransporter>();
        citizen.transform.SetParent(transporterObject.transform);
        citizen.transform.localPosition = Vector3.zero;
        citizen.transform.localRotation = Quaternion.identity;
        
        transporterObject.Initialize(_structure.Economy);
        _structure.Economy.AddLogisticsTransporter(transporterObject);
    }
    
    private void OnEmployeeRemoved(Citizen citizen)
    {
        //TODO
    }
}
