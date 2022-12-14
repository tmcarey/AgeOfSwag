using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LogisticsTransporter : MonoBehaviour
{
    public Storage storage;
    
    private Economy.LogisticsAssignment _assignment;
    private Economy _economy;
    private Coroutine transportRoutine;
    
    private Logistics _logistics;
    private Citizen _citizen;
    
    public bool IsTransporting => transportRoutine != null;

    public IEnumerator WaitForTransportFinished()
    {
        while (IsTransporting)
        {
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Initialize(Economy economy, Logistics logistics)
    {
        _economy = economy;
        _logistics = logistics;
        _citizen = GetComponent<Citizen>();
    }

    public void SetAssignment(Economy.LogisticsAssignment assignment)
    {
        if (this._assignment != null)
        {
            Debug.LogError("Already has assignment");
            return;
        }
        
        this._assignment = assignment;
        if(transportRoutine != null)
            StopCoroutine(transportRoutine);
        transportRoutine = StartCoroutine(TransportRoutine());
    }

    private IEnumerator TransportRoutine()
    {
        foreach(Economy.StorageClaim claim in _assignment.Claims)
        {
            yield return _citizen.WaitUntilEnteredStructure(claim.Storage.GetStructure()); 
            yield return new WaitForSeconds(1.0f);
            claim.Storage.TransferResource(storage, this._assignment.Resource,
                claim.Amount);
        }

        yield return _citizen.WaitUntilEnteredStructure(this._assignment.Target
            .GetStructure());
            
        yield return new WaitForSeconds(1.0f);

        storage.TransferResource(this._assignment.Target, this._assignment.Resource, this._assignment.Total);
        _economy.OnTransporterFinished(this);
        this._assignment = null;

        yield return _citizen.WaitUntilEnteredStructure(_logistics.GetStructure());
    }
}
